using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public partial class Polynomial<TAlgebra, TNumber> : IPolynomial<TAlgebra, TNumber> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{

		public static IPolynomial<TAlgebra, TNumber> MinusOne = new Polynomial<TAlgebra, TNumber>(new List<ITerm<TAlgebra, TNumber>>() { (ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.MinusOne, 0) }.ToArray());
		public static IPolynomial<TAlgebra, TNumber> Zero = new Polynomial<TAlgebra, TNumber>(new List<ITerm<TAlgebra, TNumber>>() { (ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.Zero, 0) }.ToArray());
		public static IPolynomial<TAlgebra, TNumber> One = new Polynomial<TAlgebra, TNumber>(new List<ITerm<TAlgebra, TNumber>>() { (ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.One, 0) }.ToArray());

		public ITerm<TAlgebra, TNumber>[] Terms { get { return _terms.ToArray(); } }
		private List<ITerm<TAlgebra, TNumber>> _terms;
		public int Degree { get; private set; }

		public TAlgebra this[int degree]
		{
			get
			{
				ITerm<TAlgebra, TNumber> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm<TAlgebra, TNumber>))
				{
					return ArithmeticType<TAlgebra, TNumber>.Instance.Zero;
				}
				else
				{
					return term.CoEfficient;
				}
			}
			set
			{
				ITerm<TAlgebra, TNumber> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm<TAlgebra, TNumber>))
				{
					if (!value.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
					{
						ITerm<TAlgebra, TNumber> newTerm = new Term<TAlgebra, TNumber>(value, degree);
						List<ITerm<TAlgebra, TNumber>> terms = _terms;
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

		public Polynomial()
		{
			_terms = new List<ITerm<TAlgebra, TNumber>>() { new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.Zero, 0) };
			Degree = 0;
		}

		public Polynomial(ITerm<TAlgebra, TNumber>[] terms)
		{
			SetTerms(terms);
		}

		public Polynomial(TAlgebra n, TAlgebra polynomialBase, int forceDegree)
		{
			Degree = forceDegree;
			SetTerms(GetPolynomialTerms(n, polynomialBase, Degree));
		}

		private void SetTerms(IEnumerable<ITerm<TAlgebra, TNumber>> terms)
		{
			_terms = terms.OrderBy(t => t.Exponent).ToList();
			RemoveZeros();
		}

		public static ITerm<TAlgebra, TNumber>[] GetTerms(TAlgebra[] terms)
		{
			List<ITerm<TAlgebra, TNumber>> results = new List<ITerm<TAlgebra, TNumber>>();

			int degree = 0;
			foreach (TAlgebra term in terms)
			{
				results.Add(new Term<TAlgebra, TNumber>(term, degree));

				degree += 1;
			}

			return results.ToArray();
		}

		private void RemoveZeros()
		{
			_terms.RemoveAll(t => t.CoEfficient.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero));
			if (!_terms.Any())
			{
				_terms = GetTerms(new TAlgebra[] { ArithmeticType<TAlgebra, TNumber>.Instance.Zero }).ToList();
			}
			SetDegree();
		}

		private void SetDegree()
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

		private static List<ITerm<TAlgebra, TNumber>> GetPolynomialTerms(TAlgebra value, TAlgebra polynomialBase, int degree)
		{
			int d = degree;
			TAlgebra toAdd = value;
			List<ITerm<TAlgebra, TNumber>> result = new List<ITerm<TAlgebra, TNumber>>();
			while (d >= 0 && toAdd.Compare(ArithmeticType<TAlgebra, TNumber>.Instance.Zero) > 0)
			{
				TAlgebra placeValue = polynomialBase.Pow(d);

				if (placeValue.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.One))
				{
					result.Add(new Term<TAlgebra, TNumber>(toAdd, d));
					toAdd = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;
				}
				else if (placeValue.Equals(toAdd))
				{
					result.Add(new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.One, d));
					toAdd = toAdd.Subtract(placeValue);
				}
				else if (placeValue.Compare(toAdd.Abs()) < 0)
				{
					TAlgebra remainder = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;
					TAlgebra quotient = toAdd.DivRem(placeValue, out remainder);

					if (quotient.Compare(placeValue) > 0)
					{
						quotient = placeValue;
					}

					result.Add(new Term<TAlgebra, TNumber>(quotient, d));
					TAlgebra toSubtract = quotient.Multiply(placeValue);

					toAdd = toAdd.Subtract(toSubtract);
				}

				d--;
			}
			return result.ToList();
		}

		public static IPolynomial<TAlgebra, TNumber> FromRoots(params TAlgebra[] roots)
		{
			return Polynomial<TAlgebra, TNumber>.Product(
				roots.Select(
					root => new Polynomial<TAlgebra, TNumber>(
						new Term<TAlgebra, TNumber>[]
						{
							new Term<TAlgebra, TNumber>( ArithmeticType<TAlgebra, TNumber>.Instance.One, 1),
							new Term<TAlgebra, TNumber>( root.Negate(), 0)
						}
					)
				)
			);
		}

		private static Random random = null;

		public static IPolynomial<TAlgebra, TNumber> Random(int degree, int coefficientMin, int coefficientMax)
		{
			if (random == null)
			{
				random = new System.Random();

				int cnt = 20;
				while (cnt-- > 0)
				{
					random.Next();
				}
			}

			List<Term<TAlgebra, TNumber>> terms = new List<Term<TAlgebra, TNumber>>();

			int counter = degree;
			while (counter-- > 0)
			{
				int coefficient = random.Next(coefficientMin, coefficientMax);				
				int cnt = coefficient;

				TAlgebra number = ArithmeticType<TAlgebra, TNumber>.Instance.One;
				while (--cnt > 0)
				{
					number = number.Add(ArithmeticType<TAlgebra, TNumber>.Instance.One);
				}

				Term<TAlgebra, TNumber> term = new Term<TAlgebra, TNumber>(number, counter);
				terms.Add(term);
			}

			return new Polynomial<TAlgebra, TNumber>(terms.ToArray());
		}

		public static IPolynomial<TAlgebra, TNumber> Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }
			string inputString = input.Replace(" ", "");

			List<string> stringTerms = new List<string>();

			int index = -1;
			int max = inputString.Length;
			bool insideParenthesis = false;

			string buffer = "";
			while (++index < max)
			{
				char c = inputString[index];

				if (c == '(')
				{
					insideParenthesis = true;
				}
				else if (c == ')')
				{
					insideParenthesis = false;
				}
				else if (insideParenthesis == false)
				{
					if (c == '+')
					{
						stringTerms.Add(buffer);
						buffer = "";
						continue;
					}
					else if (c == '-')
					{
						stringTerms.Add(buffer);
						buffer = "";
					}
				}

				buffer += c;
			}

			stringTerms.Add(buffer);

			if (!stringTerms.Any())
			{
				throw new FormatException("Trouble splitting polynomial into terms by: '+'");
			}

			List<ITerm<TAlgebra, TNumber>> polyTerms = new List<ITerm<TAlgebra, TNumber>>();
			foreach (string stringTerm in stringTerms)
			{
				string[] termParts = stringTerm.Split(new char[] { '*' });

				if (termParts.Count() != 2)
				{
					if (termParts.Count() != 1)
					{
						throw new FormatException($"Was expecting only 1 or 2 parts to the term. Found {termParts.Count()} instead.");
					}

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
						else
						{
							termParts = new string[] { "1", temp };
						}
					}
					else
					{
						//throw new FormatException("Trouble recognizing valid characters.");
					}
				}

				TAlgebra coefficient = ArithmeticType<TAlgebra, TNumber>.Instance.Parse(termParts[0]);

				int exponent = 0;
				if (termParts.Length == 2)
				{
					string[] variableParts = termParts[1].Split(new char[] { '^' });
					if (variableParts.Count() != 2)
					{
						if (variableParts.Count() != 1)
						{
							throw new FormatException("Trouble splitting term by: '^'");
						}

						string tmp = variableParts[0];
						if (tmp.All(c => char.IsLetter(c)))
						{
							variableParts = new string[] { tmp, "1" };
						}
					}

					if (!int.TryParse(variableParts[1], out exponent))
					{
						throw new FormatException($"Trouble parsing exponent: '{variableParts[1]}'");
					}
				}

				polyTerms.Add((ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(coefficient, exponent));
			}

			if (!polyTerms.Any())
			{
				throw new FormatException("Trouble parsing terms: None parsed!");
			}
			return new Polynomial<TAlgebra, TNumber>(polyTerms.ToArray());
		}

		#endregion

		#region Evaluate

		public TAlgebra Evaluate(TAlgebra indeterminateValue)
		{
			return Evaluate(Terms, indeterminateValue);
		}

		public static TAlgebra Evaluate(ITerm<TAlgebra, TNumber>[] terms, TAlgebra indeterminateValue)
		{
			TAlgebra result = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;
			foreach (ITerm<TAlgebra, TNumber> term in terms)
			{
				TAlgebra placeValue = indeterminateValue.Pow(term.Exponent);
				TAlgebra addValue = term.CoEfficient.Multiply(placeValue);
				result = result.Add(addValue);
			}
			return result;
		}

		#endregion

		#region Change Forms

		public static IPolynomial<TAlgebra, TNumber> GetDerivativePolynomial(IPolynomial<TAlgebra, TNumber> poly)
		{
			int d = 0;
			List<ITerm<TAlgebra, TNumber>> terms = new List<ITerm<TAlgebra, TNumber>>();
			foreach (ITerm<TAlgebra, TNumber> term in poly.Terms)
			{
				d = term.Exponent - 1;
				if (d < 0)
				{
					continue;
				}
				terms.Add((ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(term.CoEfficient.Multiply(ArithmeticType<TAlgebra, TNumber>.Instance.Parse(term.Exponent.ToString())), d));
			}

			IPolynomial<TAlgebra, TNumber> result = new Polynomial<TAlgebra, TNumber>(terms.ToArray());
			return result;
		}

		public static IPolynomial<TAlgebra, TNumber> MakeMonic(IPolynomial<TAlgebra, TNumber> polynomial, TAlgebra polynomialBase)
		{
			int deg = polynomial.Degree;
			IPolynomial<TAlgebra, TNumber> result = new Polynomial<TAlgebra, TNumber>(polynomial.Terms.ToArray());
			if (result.Terms[deg].CoEfficient.Abs().Compare(ArithmeticType<TAlgebra, TNumber>.Instance.One) > 0)
			{
				TAlgebra toAdd = result.Terms[deg].CoEfficient.Subtract(ArithmeticType<TAlgebra, TNumber>.Instance.One).Multiply(polynomialBase);
				result.Terms[deg].CoEfficient = ArithmeticType<TAlgebra, TNumber>.Instance.One;
				result.Terms[deg - 1].CoEfficient = result.Terms[deg - 1].CoEfficient.Add(toAdd);
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(IPolynomial<TAlgebra, TNumber> polynomial, TAlgebra polynomialBase, TAlgebra maxCoefficientSize = default(TAlgebra))
		{
			TAlgebra maxSize = maxCoefficientSize;

			if (maxSize.Equals(default(TAlgebra)))
			{
				maxSize = polynomialBase;
			}

			int pos = 0;
			int deg = polynomial.Degree;

			while (pos < deg)
			{
				if (pos + 1 > deg)
				{
					return;
				}

				if (polynomial.Terms[pos].CoEfficient.Compare(maxSize) > 0 &&
					polynomial.Terms[pos].CoEfficient.Compare(polynomial.Terms[pos + 1].CoEfficient) > 0)
				{
					TAlgebra diff = polynomial[pos].Subtract(maxSize);

					TAlgebra toAdd = diff.Divide(polynomialBase).Add(ArithmeticType<TAlgebra, TNumber>.Instance.One);
					TAlgebra toRemove = toAdd.Multiply(polynomialBase);

					polynomial[pos] = polynomial[pos].Subtract(toRemove);
					polynomial[pos + 1] = polynomial[pos + 1].Add(toAdd);
				}

				pos++;
			}
		}

		#endregion

		#region Arithmetic

		public static IPolynomial<TAlgebra, TNumber> GCD(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right)
		{
			IPolynomial<TAlgebra, TNumber> a = left.Clone();
			IPolynomial<TAlgebra, TNumber> b = right.Clone();

			if (b.Degree > a.Degree)
			{
				IPolynomial<TAlgebra, TNumber> swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || b.Terms[0].CoEfficient.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero)))
			{
				IPolynomial<TAlgebra, TNumber> temp = a;
				a = b;
				b = Polynomial<TAlgebra, TNumber>.Field.Modulus(temp, b);
			}

			if (a.Degree == 0)
			{
				return Polynomial<TAlgebra, TNumber>.One;
			}
			else
			{
				return a;
			}
		}

		public static IPolynomial<TAlgebra, TNumber> Divide(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right)
		{
			IPolynomial<TAlgebra, TNumber> remainder = Polynomial<TAlgebra, TNumber>.Zero;
			return Polynomial<TAlgebra, TNumber>.Divide(left, right, out remainder);
		}

		public static IPolynomial<TAlgebra, TNumber> Divide(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right, out IPolynomial<TAlgebra, TNumber> remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree || right.CompareTo(left) == 1)
			{
				remainder = Polynomial<TAlgebra, TNumber>.Zero;
				return left;
			}

			int rightDegree = right.Degree;
			int quotientDegree = (left.Degree - rightDegree) + 1;
			TAlgebra leadingCoefficent = right[rightDegree].Value;

			Polynomial<TAlgebra, TNumber> rem = (Polynomial<TAlgebra, TNumber>)left.Clone();
			Polynomial<TAlgebra, TNumber> quotient = (Polynomial<TAlgebra, TNumber>)Polynomial<TAlgebra, TNumber>.Zero;

			// The leading coefficient is the only number we ever divide by
			// (so if right is monic, polynomial division does not involve division at all!)
			for (int i = quotientDegree - 1; i >= 0; i--)
			{
				quotient[i] = rem[rightDegree + i].Divide(leadingCoefficent);
				rem[rightDegree + i] = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;

				for (int j = rightDegree + i - 1; j >= i; j--)
				{
					rem[j] = rem[j].Subtract(quotient[i].Multiply(right[j - i]));
				}
			}

			// Remove zeros
			rem.RemoveZeros();
			quotient.RemoveZeros();

			remainder = rem;
			return quotient;
		}

		public static IPolynomial<TAlgebra, TNumber> Multiply(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right)
		{
			if (left == null) { throw new ArgumentNullException(nameof(left)); }
			if (right == null) { throw new ArgumentNullException(nameof(right)); }

			TAlgebra[] terms = Enumerable.Repeat(ArithmeticType<TAlgebra, TNumber>.Instance.Zero, (left.Degree + right.Degree + 1)).ToArray();

			for (int i = 0; i <= left.Degree; i++)
			{
				for (int j = 0; j <= right.Degree; j++)
				{
					terms[(i + j)] = terms[(i + j)].Add(left[i].Multiply(right[j]));
				}
			}
			return new Polynomial<TAlgebra, TNumber>(GetTerms(terms));
		}

		public static IPolynomial<TAlgebra, TNumber> Product(params IPolynomial<TAlgebra, TNumber>[] polys)
		{
			return Product(polys.ToList());
		}

		public static IPolynomial<TAlgebra, TNumber> Product(IEnumerable<IPolynomial<TAlgebra, TNumber>> polys)
		{
			IPolynomial<TAlgebra, TNumber> result = null;

			foreach (IPolynomial<TAlgebra, TNumber> p in polys)
			{
				if (result == null)
				{
					result = p;
				}
				else
				{
					result = Polynomial<TAlgebra, TNumber>.Multiply(result, p);
				}
			}

			return result;
		}

		public static IPolynomial<TAlgebra, TNumber> Square(IPolynomial<TAlgebra, TNumber> poly)
		{
			return Polynomial<TAlgebra, TNumber>.Multiply(poly, poly);
		}

		public static IPolynomial<TAlgebra, TNumber> Pow(IPolynomial<TAlgebra, TNumber> poly, int exponent)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
			}
			else if (exponent == 0)
			{

				return new Polynomial<TAlgebra, TNumber>(new List<ITerm<TAlgebra, TNumber>>() { (ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.One, 0) }.ToArray());
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}
			else if (exponent == 2)
			{
				return Square(poly);
			}

			IPolynomial<TAlgebra, TNumber> total = Polynomial<TAlgebra, TNumber>.Square(poly);

			int counter = exponent - 2;
			while (counter != 0)
			{
				total = Polynomial<TAlgebra, TNumber>.Multiply(total, poly);
				counter -= 1;
			}

			return total;
		}

		public static IPolynomial<TAlgebra, TNumber> Subtract(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			TAlgebra[] terms = new TAlgebra[Math.Max(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				TAlgebra l = left[i];
				TAlgebra r = right[i];

				terms[i] = l.Subtract(r);
			}

			IPolynomial<TAlgebra, TNumber> result = new Polynomial<TAlgebra, TNumber>(Polynomial<TAlgebra, TNumber>.GetTerms(terms.ToArray()));

			return result;
		}

		public static IPolynomial<TAlgebra, TNumber> Sum(params IPolynomial<TAlgebra, TNumber>[] polys)
		{
			return Sum(polys.ToList());
		}

		public static IPolynomial<TAlgebra, TNumber> Sum(IEnumerable<IPolynomial<TAlgebra, TNumber>> polys)
		{
			IPolynomial<TAlgebra, TNumber> result = null;

			foreach (IPolynomial<TAlgebra, TNumber> p in polys)
			{
				if (result == null)
				{
					result = p;
				}
				else
				{
					result = Polynomial<TAlgebra, TNumber>.Add(result, p);
				}
			}

			return result;
		}

		public static IPolynomial<TAlgebra, TNumber> Add(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			TAlgebra[] terms = new TAlgebra[Math.Max(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = left[i].Add(right[i]);
			}

			IPolynomial<TAlgebra, TNumber> result = new Polynomial<TAlgebra, TNumber>(Polynomial<TAlgebra, TNumber>.GetTerms(terms.ToArray()));
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

			IPolynomial<TAlgebra, TNumber> other = obj as IPolynomial<TAlgebra, TNumber>;

			if (other == null)
			{
				throw new ArgumentException();
			}

			return this.CompareTo(other);
		}

		public int CompareTo(IPolynomial<TAlgebra, TNumber> other)
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
					TAlgebra thisCoefficient = this[counter];
					TAlgebra otherCoefficient = other[counter];

					if (thisCoefficient.Compare(otherCoefficient) < 0)
					{
						return -1;
					}
					else if (thisCoefficient.Compare(otherCoefficient) > 0)
					{
						return 1;
					}

					counter--;
				}

				return 0;
			}
		}

		public IPolynomial<TAlgebra, TNumber> Clone()
		{
			return new Polynomial<TAlgebra, TNumber>(Terms.Select(pt => ((Term<TAlgebra, TNumber>)pt).Clone()).ToArray());
		}

		public override string ToString()
		{
			return Polynomial<TAlgebra, TNumber>.FormatString(this);
		}

		public static string FormatString(IPolynomial<TAlgebra, TNumber> polynomial)
		{
			List<string> stringTerms = new List<string>();
			int degree = polynomial.Terms.Length;
			while (--degree >= 0)
			{
				string termString = "";
				ITerm<TAlgebra, TNumber> term = polynomial.Terms[degree];


				if (term.CoEfficient.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
				{
					if (term.Exponent == 0)
					{
						if (stringTerms.Count == 0) { stringTerms.Add("0"); }
					}
					continue;
				}
				else if (term.CoEfficient.Compare(ArithmeticType<TAlgebra, TNumber>.Instance.One) > 0 || term.CoEfficient.Compare(ArithmeticType<TAlgebra, TNumber>.Instance.MinusOne) < 0)
				{
					termString = $"{term.CoEfficient}";
				}

				switch (term.Exponent)
				{
					case 0:
						stringTerms.Add($"{term.CoEfficient}");
						break;

					case 1:
						if (term.CoEfficient.Equals(1)) stringTerms.Add("X");
						else if (term.CoEfficient.Equals(-1)) stringTerms.Add("-X");
						else stringTerms.Add($"{term.CoEfficient}*X");
						break;

					default:
						if (term.CoEfficient.Equals(1)) stringTerms.Add($"X^{term.Exponent}");
						else if (term.CoEfficient.Equals(-1)) stringTerms.Add($"-X^{term.Exponent}");
						else stringTerms.Add($"{term.CoEfficient}*X^{term.Exponent}");
						break;
				}
			}
			return string.Join(" + ", stringTerms).Replace("+ -", "- ");
		}

		#endregion

	}
}
