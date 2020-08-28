using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExtendedArithmetic
{
	[DataContract]
	public partial class Polynomial : IPolynomial
	{
		public static IPolynomial Zero = null;
		public static IPolynomial One = null;
		public static IPolynomial Two = null;

		public ITerm[] Terms { get { return _terms.ToArray(); } }

		[DataMember(Name = "Terms")]
		private List<ITerm> _terms = new List<ITerm>();
		public int Degree { get; set; }

		public BigInteger this[int degree]
		{
			get
			{
				ITerm term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm))
				{
					return BigInteger.Zero;
				}
				else
				{
					return term.CoEfficient;
				}
			}
			set
			{
				ITerm term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm))
				{
					if (value != BigInteger.Zero)
					{
						ITerm newTerm = new Term(value, degree);
						List<ITerm> terms = _terms;
						terms.Add(newTerm);
						SetTerms(terms);
					}
				}
				else
				{
					term.CoEfficient = value;
				}
			}
		}

		#region Constructors

		static Polynomial()
		{
			Zero = new Polynomial(Term.GetTerms(new BigInteger[] { new BigInteger(0) }));
			One = new Polynomial(Term.GetTerms(new BigInteger[] { new BigInteger(1) }));
			Two = new Polynomial(Term.GetTerms(new BigInteger[] { new BigInteger(2) }));
		}

		public Polynomial() { _terms = new List<ITerm>(); }

		public Polynomial(ITerm[] terms)
		{
			SetTerms(terms);
		}

		public Polynomial(BigInteger n, BigInteger polynomialBase)
			: this(n, polynomialBase, (int)Math.Truncate(BigInteger.Log(n, (double)polynomialBase) + 1))
		{
		}

		public Polynomial(BigInteger n, BigInteger polynomialBase, int forceDegree)
		{
			Degree = forceDegree;
			SetTerms(GetPolynomialTerms(n, polynomialBase, Degree));
		}

		private void SetTerms(IEnumerable<ITerm> terms)
		{
			_terms = terms.OrderBy(t => t.Exponent).ToList();
			RemoveZeros();
		}

		private void RemoveZeros()
		{
			_terms.RemoveAll(t => t.CoEfficient == 0);
			if (!_terms.Any())
			{
				_terms = Term.GetTerms(new BigInteger[] { 0 }).ToList();
			}
			SetDegree();
		}

		public void SetDegree()
		{
			if (_terms.Any())
			{
				Degree = _terms.Max(term => term.Exponent);
			}
			else
			{
				Degree = 0;
			}
		}

		private static List<ITerm> GetPolynomialTerms(BigInteger value, BigInteger polynomialBase, int degree)
		{
			int d = degree; // (int)Math.Truncate(BigInteger.Log(value, (double)polynomialBase)+ 1);
			BigInteger toAdd = value;
			List<ITerm> result = new List<ITerm>();
			while (d >= 0 && toAdd > 0)
			{
				BigInteger placeValue = BigInteger.Pow(polynomialBase, d);

				if (placeValue == 1)
				{
					result.Add(new Term(toAdd, d));
					toAdd = 0;
				}
				else if (placeValue == toAdd)
				{
					result.Add(new Term(1, d));
					toAdd -= placeValue;
				}
				else if (placeValue < BigInteger.Abs(toAdd))
				{
					BigInteger remainder = new BigInteger();
					BigInteger quotient = BigInteger.DivRem(toAdd, placeValue, out remainder);

					if (quotient > placeValue)
					{
						quotient = placeValue;
					}

					result.Add(new Term(quotient, d));
					BigInteger toSubtract = BigInteger.Multiply(quotient, placeValue);

					toAdd -= toSubtract;
				}

				d--;
			}
			return result.ToList();
		}

		public static IPolynomial FromRoots(params BigInteger[] roots)
		{
			return Polynomial.Product(
				roots.Select(
					root => new Polynomial(
						new Term[]
						{
						new Term( 1, 1),
						new Term( BigInteger.Negate(root), 0)
						}
					)
				)
			);
		}

		public static IPolynomial Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "").Replace("−", "-").Replace("-", "+-");
			string[] stringTerms = inputString.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

			if (!stringTerms.Any()) { throw new FormatException(); }

			List<Term> polyTerms = new List<Term>();
			foreach (string stringTerm in stringTerms)
			{
				string[] termParts = stringTerm.Split(new char[] { '*' });

				if (termParts.Count() != 2)
				{
					if (termParts.Count() != 1) { throw new FormatException(); }

					string temp = termParts[0];
					if (temp.All(c => char.IsDigit(c) || c == '-'))
					{
						termParts = new string[] { temp, "X^0" };
					}
					else if (temp.All(c => char.IsLetter(c) || c == '^' || c == '-' || char.IsDigit(c)))
					{
						if (temp.Contains("-"))
						{
							temp = temp.Replace("-", "");
							termParts = new string[] { "-1", temp };
						}
						else { termParts = new string[] { "1", temp }; }
					}
					else { throw new FormatException(); }
				}

				BigInteger coefficient = BigInteger.Parse(termParts[0]);

				string[] variableParts = termParts[1].Split(new char[] { '^' });
				if (variableParts.Count() != 2)
				{
					if (variableParts.Count() != 1) { throw new FormatException(); }

					string tmp = variableParts[0];
					if (tmp.All(c => char.IsLetter(c)))
					{
						variableParts = new string[] { tmp, "1" };
					}
				}
				int exponent = int.Parse(variableParts[1]);
				polyTerms.Add(new Term(coefficient, exponent));
			}

			if (!polyTerms.Any()) { throw new FormatException(); }
			return new Polynomial(polyTerms.ToArray());
		}

		#endregion

		#region Evaluate

		public BigInteger Evaluate(BigInteger indeterminateValue)
		{
			return Evaluate(Terms, indeterminateValue);
		}

		public static BigInteger Evaluate(ITerm[] terms, BigInteger indeterminateValue)
		{
			BigInteger result = new BigInteger(0);
			foreach (ITerm term in terms)
			{
				BigInteger placeValue = BigInteger.Pow(indeterminateValue, term.Exponent);
				BigInteger addValue = BigInteger.Multiply(term.CoEfficient, placeValue);
				result = BigInteger.Add(result, addValue);
			}
			return result;
		}

		public double Evaluate(double indeterminateValue)
		{
			return Evaluate(Terms, indeterminateValue);
		}

		public decimal Evaluate(decimal indeterminateValue)
		{
			return Evaluate(Terms, indeterminateValue);
		}

		public static double Evaluate(ITerm[] terms, double indeterminateValue)
		{
			double result = 0;

			int d = terms.Count() - 1;
			while (d >= 0)
			{
				double placeValue = Math.Pow(indeterminateValue, terms[d].Exponent);

				double addValue = (double)(terms[d].CoEfficient) * placeValue;

				result += addValue;

				d--;
			}

			return result;
		}

		public static decimal Evaluate(ITerm[] terms, decimal indeterminateValue)
		{
			decimal result = 0;

			int d = terms.Count() - 1;
			while (d >= 0)
			{
				decimal placeValue = Pow(indeterminateValue, terms[d].Exponent);
				decimal addValue = decimal.Multiply((decimal)terms[d].CoEfficient, placeValue);

				result += addValue;
				d--;
			}

			return result;
		}

		private static decimal Pow(decimal b, int exp)
		{
			if (exp < 0) throw new ArgumentOutOfRangeException(nameof(exp), "Negative exponents not supported!");

			decimal result = 1m;
			decimal multiplier = b;

			while (exp > 0)
			{
				if ((exp % 2) == 1)
				{
					result *= multiplier;
					exp -= 1;
					if (exp == 0)
					{
						break;
					}
				}

				multiplier *= multiplier;
				exp /= 2;
			}

			return result;
		}

		#endregion

		#region Change Forms

		public static IPolynomial GetDerivativePolynomial(IPolynomial poly)
		{
			int d = 0;
			List<ITerm> terms = new List<ITerm>();
			foreach (ITerm term in poly.Terms)
			{
				d = term.Exponent - 1;
				if (d < 0)
				{
					continue;
				}
				terms.Add(new Term(term.CoEfficient * term.Exponent, d));
			}

			IPolynomial result = new Polynomial(terms.ToArray());
			return result;
		}

		public static IPolynomial MakeMonic(IPolynomial polynomial, BigInteger polynomialBase)
		{
			int deg = polynomial.Degree;
			IPolynomial result = new Polynomial(polynomial.Terms.ToArray());
			if (BigInteger.Abs(result.Terms[deg].CoEfficient) > 1)
			{
				BigInteger toAdd = (result.Terms[deg].CoEfficient - 1) * polynomialBase;
				result.Terms[deg].CoEfficient = 1;
				result.Terms[deg - 1].CoEfficient += toAdd;
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(IPolynomial polynomial, BigInteger polynomialBase)
		{
			BigInteger max = polynomialBase / 2;

			int pos = 0;
			int deg = polynomial.Degree;
			while (pos <= deg)
			{
				if (polynomial[pos] > max)
				{
					polynomial[pos + 1] += 1;
					polynomial[pos] = -(polynomialBase - polynomial[pos]);
				}

				pos++;
			}
		}

		#endregion

		#region Arithmetic

		public static IPolynomial GCD(IPolynomial left, IPolynomial right)
		{
			IPolynomial a = left.Clone();
			IPolynomial b = right.Clone();

			if (b.Degree > a.Degree)
			{
				IPolynomial swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || b.Terms[0].CoEfficient == 0))
			{
				IPolynomial temp = a.Clone();
				a = b.Clone();
				b = Field.Modulus(temp, b);
			}

			if (a.Degree == 0)
			{
				return Polynomial.One;
			}
			else
			{
				return a;
			}
		}

		public static IPolynomial Divide(IPolynomial left, IPolynomial right)
		{
			IPolynomial remainder = new Polynomial();
			return Polynomial.Divide(left, right, out remainder);
		}

		public static IPolynomial Divide(IPolynomial left, IPolynomial right, out IPolynomial remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree || right.CompareTo(left) == 1)
			{
				remainder = new Polynomial(new ITerm[] { new Term(new BigInteger(0), 0) });
				return left.Clone();
			}

			int rightDegree = right.Degree;
			int quotientDegree = (left.Degree - rightDegree) + 1;
			BigInteger leadingCoefficent = right[rightDegree].Clone();

			Polynomial rem = (Polynomial)left.Clone();
			Polynomial quotient = (Polynomial)new Polynomial(new ITerm[] { new Term(new BigInteger(0), 0) });

			// The leading coefficient is the only number we ever divide by
			// (so if right is monic, polynomial division does not involve division at all!)
			for (int i = quotientDegree - 1; i >= 0; i--)
			{
				quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent);
				rem[rightDegree + i] = new BigInteger(0);

				for (int j = rightDegree + i - 1; j >= i; j--)
				{
					rem[j] = BigInteger.Subtract(rem[j], BigInteger.Multiply(quotient[i], right[j - i]));
				}
			}

			// Remove zeros
			rem.RemoveZeros();
			quotient.RemoveZeros();

			remainder = rem.Clone();
			return quotient.Clone();
		}

		public static IPolynomial Multiply(IPolynomial left, IPolynomial right)
		{
			if (left == null) { throw new ArgumentNullException(nameof(left)); }
			if (right == null) { throw new ArgumentNullException(nameof(right)); }

			BigInteger[] terms = new BigInteger[left.Degree + right.Degree + 1];

			for (int i = 0; i <= left.Degree; i++)
			{
				for (int j = 0; j <= right.Degree; j++)
				{
					terms[(i + j)] += BigInteger.Multiply(left[i], right[j]);
				}
			}
			return new Polynomial(Term.GetTerms(terms));
		}

		public static IPolynomial Product(params IPolynomial[] polys)
		{
			return Product(polys.ToList());
		}

		public static IPolynomial Product(IEnumerable<IPolynomial> polys)
		{
			IPolynomial result = null;

			foreach (IPolynomial p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = Polynomial.Multiply(result, p);
				}
			}

			return result;
		}

		public static IPolynomial Square(IPolynomial poly)
		{
			return Polynomial.Multiply(poly, poly);
		}

		public static IPolynomial Pow(IPolynomial poly, int exponent)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
			}
			else if (exponent == 0)
			{
				return new Polynomial(new Term[] { new Term(1, 0) });
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}
			else if (exponent == 2)
			{
				return Square(poly);
			}

			IPolynomial total = Polynomial.Square(poly);

			int counter = exponent - 2;
			while (counter != 0)
			{
				total = Polynomial.Multiply(total, poly);
				counter -= 1;
			}

			return total;
		}

		public static IPolynomial Subtract(IPolynomial left, IPolynomial right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			BigInteger[] terms = new BigInteger[Math.Max(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				BigInteger l = left[i];
				BigInteger r = right[i];

				terms[i] = (l - r);
			}

			IPolynomial result = new Polynomial(Term.GetTerms(terms.ToArray()));

			return result;
		}

		public static IPolynomial Sum(params IPolynomial[] polys)
		{
			return Sum(polys.ToList());
		}

		public static IPolynomial Sum(IEnumerable<IPolynomial> polys)
		{
			IPolynomial result = null;

			foreach (IPolynomial p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = Polynomial.Add(result, p);
				}
			}

			return result;
		}

		public static IPolynomial Add(IPolynomial left, IPolynomial right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			BigInteger[] terms = new BigInteger[Math.Max(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = (left[i] + right[i]);
			}

			IPolynomial result = new Polynomial(Term.GetTerms(terms.ToArray()));
			return result;
		}

		#endregion

		#region Overrides and Interface implementations		

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new NullReferenceException();
			}

			IPolynomial other = obj as IPolynomial;

			if (other == null)
			{
				throw new ArgumentException();
			}

			return this.CompareTo(other);
		}

		public int CompareTo(IPolynomial other)
		{
			if (other == null)
			{
				throw new ArgumentException();
			}

			if (other.Degree != this.Degree)
			{
				if (other.Degree > this.Degree)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				int counter = this.Degree;

				while (counter >= 0)
				{
					BigInteger thisCoefficient = this[counter];
					BigInteger otherCoefficient = other[counter];

					if (thisCoefficient < otherCoefficient)
					{
						return -1;
					}
					else if (thisCoefficient > otherCoefficient)
					{
						return 1;
					}

					counter--;
				}

				return 0;
			}
		}

		public IPolynomial Clone()
		{
			var terms = _terms.Select(pt => pt.Clone()).ToArray();
			return new Polynomial(terms);
		}

		public override string ToString()
		{
			return Polynomial.FormatString(this);
		}

		public static string FormatString(IPolynomial polynomial)
		{
			List<string> stringTerms = new List<string>();
			int degree = polynomial.Terms.Length;
			while (--degree >= 0)
			{
				string termString = "";
				ITerm term = polynomial.Terms[degree];

				if (term.CoEfficient == 0)
				{
					if (term.Exponent == 0)
					{
						if (stringTerms.Count == 0) { stringTerms.Add("0"); }
					}
					continue;
				}
				else if (term.CoEfficient > 1 || term.CoEfficient < -1)
				{
					termString = $"{term.CoEfficient}";
				}

				switch (term.Exponent)
				{
					case 0:
						stringTerms.Add($"{term.CoEfficient}");
						break;

					case 1:
						if (term.CoEfficient == 1) stringTerms.Add("X");
						else if (term.CoEfficient == -1) stringTerms.Add("-X");
						else stringTerms.Add($"{term.CoEfficient}*X");
						break;

					default:
						if (term.CoEfficient == 1) stringTerms.Add($"X^{term.Exponent}");
						else if (term.CoEfficient == -1) stringTerms.Add($"-X^{term.Exponent}");
						else stringTerms.Add($"{term.CoEfficient}*X^{term.Exponent}");
						break;
				}
			}
			return string.Join(" + ", stringTerms).Replace("+ -", "- ");
		}

		#endregion

	}
}
