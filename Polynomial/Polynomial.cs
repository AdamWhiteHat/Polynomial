using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace ExtendedArithmetic
{
	[DataContract]
	public partial class Polynomial : ICloneable<Polynomial>, IComparable, IComparable<Polynomial>, IEquatable<Polynomial>
	{
		public static Polynomial Zero = null;
		public static Polynomial One = null;
		public static Polynomial Two = null;

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public Term[] Terms { get { return _terms.ToArray(); } }

		[DataMember(Name = "Terms")]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<Term> _terms = new List<Term>();
		public int Degree { get; set; }

		public BigInteger this[int degree]
		{
			get
			{
				Term term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(Term))
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
				Term term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(Term))
				{
					if (value != BigInteger.Zero)
					{
						Term newTerm = new Term(value, degree);
						List<Term> terms = _terms;
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

		public Polynomial() { _terms = new List<Term>(); }

		public Polynomial(Term[] terms)
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

		private void SetTerms(IEnumerable<Term> terms)
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

		private static List<Term> GetPolynomialTerms(BigInteger value, BigInteger polynomialBase, int degree)
		{
			int d = degree; // (int)Math.Truncate(BigInteger.Log(value, (double)polynomialBase)+ 1);
			BigInteger toAdd = value;
			List<Term> result = new List<Term>();
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
					BigInteger quotient = BigInteger.Divide(toAdd, placeValue);

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

		public static Polynomial FromRoots(params BigInteger[] roots)
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

		public static Polynomial Parse(string input)
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
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		public double Evaluate(double indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		public decimal Evaluate(decimal indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		public Complex Evaluate(Complex indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		public static BigInteger Evaluate(Polynomial polynomial, BigInteger indeterminateValue)
		{
			int counter = polynomial.Degree;
			BigInteger result = polynomial[counter];
			while (--counter >= 0)
			{
				result *= indeterminateValue;
				result += polynomial[counter];
			}
			return result;
		}

		public static double Evaluate(Polynomial polynomial, double indeterminateValue)
		{
			int counter = polynomial.Degree;
			double result = (double)polynomial[counter];
			while (--counter >= 0)
			{
				result *= indeterminateValue;
				result += (double)polynomial[counter];
			}
			return result;
		}

		public static decimal Evaluate(Polynomial polynomial, decimal indeterminateValue)
		{
			int counter = polynomial.Degree;
			decimal result = (decimal)polynomial[counter];
			while (--counter >= 0)
			{
				result *= indeterminateValue;
				result += (decimal)polynomial[counter];
			}
			return result;
		}

		public static Complex Evaluate(Polynomial polynomial, Complex indeterminateValue)
		{
			int counter = polynomial.Degree;
			Complex result = (Complex)polynomial[counter];
			while (--counter >= 0)
			{
				result *= indeterminateValue;
				result += (Complex)polynomial[counter];
			}
			return result;
		}

		#endregion

		#region Change Forms

		public static Polynomial GetDerivativePolynomial(Polynomial poly)
		{
			int d;
			List<Term> terms = new List<Term>();
			foreach (Term term in poly.Terms)
			{
				d = term.Exponent - 1;
				if (d < 0)
				{
					continue;
				}
				terms.Add(new Term(term.CoEfficient * term.Exponent, d));
			}

			Polynomial result = new Polynomial(terms.ToArray());
			return result;
		}

		public static Polynomial MakeMonic(Polynomial polynomial, BigInteger polynomialBase)
		{
			int deg = polynomial.Degree;
			Polynomial result = new Polynomial(polynomial.Terms.ToArray());
			if (BigInteger.Abs(result.Terms[deg].CoEfficient) > 1)
			{
				BigInteger toAdd = (result.Terms[deg].CoEfficient - 1) * polynomialBase;
				result.Terms[deg].CoEfficient = 1;
				result.Terms[deg - 1].CoEfficient += toAdd;
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(Polynomial polynomial, BigInteger polynomialBase)
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

		public static Polynomial GCD(Polynomial left, Polynomial right)
		{
			Polynomial a = left.Clone();
			Polynomial b = right.Clone();

			if (b.Degree > a.Degree)
			{
				Polynomial swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || b.Terms[0].CoEfficient == 0))
			{
				Polynomial temp = a.Clone();
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

		public static Polynomial Divide(Polynomial left, Polynomial right)
		{
			Polynomial remainder;
			return Polynomial.Divide(left, right, out remainder);
		}

		public static Polynomial Divide(Polynomial left, Polynomial right, out Polynomial remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree || right.CompareTo(left) == 1)
			{
				remainder = new Polynomial(new Term[] { new Term(new BigInteger(0), 0) });
				return left.Clone();
			}

			int rightDegree = right.Degree;
			int quotientDegree = (left.Degree - rightDegree) + 1;
			BigInteger leadingCoefficent = right[rightDegree].Clone();

			Polynomial rem = (Polynomial)left.Clone();
			Polynomial quotient = (Polynomial)new Polynomial(new Term[] { new Term(new BigInteger(0), 0) });

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

		public static Polynomial Multiply(Polynomial left, Polynomial right)
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

		public static Polynomial Product(params Polynomial[] polys)
		{
			return Product(polys.ToList());
		}

		public static Polynomial Product(IEnumerable<Polynomial> polys)
		{
			Polynomial result = null;

			foreach (Polynomial p in polys)
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

		public static Polynomial Square(Polynomial poly)
		{
			return Polynomial.Multiply(poly, poly);
		}

		public static Polynomial Pow(Polynomial poly, int exponent)
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

			Polynomial total = Polynomial.Square(poly);

			int counter = exponent - 2;
			while (counter != 0)
			{
				total = Polynomial.Multiply(total, poly);
				counter -= 1;
			}

			return total;
		}

		public static Polynomial Subtract(Polynomial left, Polynomial right)
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

			Polynomial result = new Polynomial(Term.GetTerms(terms.ToArray()));

			return result;
		}

		public static Polynomial Sum(params Polynomial[] polys)
		{
			return Sum(polys.ToList());
		}

		public static Polynomial Sum(IEnumerable<Polynomial> polys)
		{
			Polynomial result = null;

			foreach (Polynomial p in polys)
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

		public static Polynomial Add(Polynomial left, Polynomial right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			BigInteger[] terms = new BigInteger[Math.Max(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = (left[i] + right[i]);
			}

			Polynomial result = new Polynomial(Term.GetTerms(terms.ToArray()));
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

			Polynomial other = obj as Polynomial;

			if (other == null)
			{
				throw new ArgumentException();
			}

			return this.CompareTo(other);
		}

		public int CompareTo(Polynomial other)
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

		public Polynomial Clone()
		{
			var terms = _terms.Select(pt => pt.Clone()).ToArray();
			return new Polynomial(terms);
		}

		public bool Equals(Polynomial other)
		{
			return (this.CompareTo(other) == 0);
		}

		private static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		public override int GetHashCode()
		{
			int hash = this.Degree.GetHashCode();
			foreach (Term term in this.Terms)
			{
				hash = CombineHashCodes(hash, term.GetHashCode());
			}
			return hash;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }
			Polynomial otherPoly = obj as Polynomial;
			if (otherPoly == null) { return false; }
			else { return Equals(otherPoly); }
		}

		public static string FormatString(Polynomial polynomial)
		{
			List<string> stringTerms = new List<string>();
			int degree = polynomial.Terms.Length;
			while (--degree >= 0)
			{
				Term term = polynomial.Terms[degree];

				if (term.CoEfficient == 0)
				{
					if (term.Exponent == 0)
					{
						if (stringTerms.Count == 0) { stringTerms.Add("0"); }
					}
					continue;
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

		public override string ToString()
		{
			return Polynomial.FormatString(this);
		}

		#endregion

	}
}
