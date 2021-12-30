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
	/// <summary>
	/// A symbolic polynomial arithmetic class
	/// </summary>
	/// <seealso cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Polynomial}" />
	/// <seealso cref="System.IComparable" />
	/// <seealso cref="System.IComparable{ExtendedArithmetic.Polynomial}" />
	/// <seealso cref="System.IEquatable{ExtendedArithmetic.Polynomial}" />
	[DataContract]
	public partial class Polynomial : ICloneable<Polynomial>, IComparable, IComparable<Polynomial>, IEquatable<Polynomial>
	{
		/// <summary>
		/// A polynomial representing the zero polynomial
		/// </summary>
		public static Polynomial Zero = null;
		/// <summary>
		/// A polynomial representing the constant polynomial 1
		/// </summary>
		public static Polynomial One = null;
		/// <summary>
		/// A polynomial representing the constant polynomial 2
		/// </summary>
		public static Polynomial Two = null;

		/// <summary>
		/// A read-only array for accessing the Terms of this polynomial
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public Term[] Terms { get { return _terms.ToArray(); } }

		[DataMember(Name = "Terms")]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<Term> _terms = new List<Term>();

		/// <summary>
		/// Gets the degree of the polynomial.
		/// </summary>
		public int Degree
		{
			get
			{
				if (_terms.Any())
				{
					return _terms.Max(term => term.Exponent);
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Coefficient of the term of the specified degree.
		/// </summary>
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

		/// <summary>
		/// Returns a polynomial that has the specified integer values as roots.
		/// </summary>
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

		/// <summary>
		/// Returns a polynomial from its string representation.
		/// All multiplication symbols must be made explicit,
		/// although coefficient values of 1 and exponent values of 1 or 0 can be implicit.
		/// If you are still unsure, use the ToString function on a polynomial instance,
		/// as ToString will always produce a valid string that Parse can consume. 
		/// </summary>
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

		/// <summary>
		/// Evaluates the polynomial at the specified <see cref="BigInteger"/> indeterminate value.
		/// </summary>
		public BigInteger Evaluate(BigInteger indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		/// <summary>
		/// Evaluates the polynomial at the specified <see cref="double"/> indeterminate value.
		/// </summary>
		public double Evaluate(double indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		/// <summary>
		/// Evaluates the polynomial at the specified <see cref="decimal"/> indeterminate value.
		/// </summary>
		public decimal Evaluate(decimal indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		/// <summary>
		/// Evaluates the polynomial at the specified <see cref="Complex"/> indeterminate value.
		/// </summary>
		public Complex Evaluate(Complex indeterminateValue)
		{
			return Polynomial.Evaluate(this, indeterminateValue);
		}

		/// <summary>
		/// Evaluates the specified polynomial at the specified <see cref="BigInteger"/> indeterminate value.
		/// </summary>
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

		/// <summary>
		/// Evaluates the specified polynomial at the specified <see cref="double"/> indeterminate value.
		/// </summary>
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

		/// <summary>
		/// Evaluates the specified polynomial at the specified <see cref="decimal"/> indeterminate value.
		/// </summary>
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

		/// <summary>
		/// Evaluates the specified polynomial at the specified <see cref="Complex"/> indeterminate value.
		/// </summary>
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

		/// <summary>
		/// Like the Evaluate method, except it replace every instance of the indeterminate (X)
		/// with the specified Polynomial, and returns the resulting (usually large) Polynomial
		/// </summary>
		public Polynomial FunctionalComposition(Polynomial indeterminateValue)
		{
			List<Term> terms = this.Terms.ToList();
			List<Polynomial> composedTerms = new List<Polynomial>();

			foreach (Term trm in terms)
			{
				Polynomial constant = new Polynomial(new Term[] { new Term(trm.CoEfficient, 0) });
				Polynomial composed = Polynomial.Multiply(constant, Polynomial.Pow(indeterminateValue, trm.Exponent));
				composedTerms.Add(composed);
			}

			Polynomial result = Polynomial.Sum(composedTerms);
			return result;
		}

		#endregion

		#region Change Forms

		/// <summary>
		/// Factors the specified polynomial.
		/// </summary>
		public static List<Polynomial> Factor(Polynomial polynomial)
		{
			List<Polynomial> results = new List<Polynomial>();

			Polynomial remainingPoly = polynomial.Clone();

			var coefficients = remainingPoly.Terms.Select(trm => trm.CoEfficient);
			var gcd = coefficients.Aggregate(BigInteger.GreatestCommonDivisor);

			if (gcd > 1)
			{
				Polynomial gcdPoly = Parse(gcd.ToString());
				results.Add(gcdPoly);
				remainingPoly = Divide(remainingPoly, gcdPoly);
			}

			var leading = remainingPoly.Terms.Last().CoEfficient;
			var constant = remainingPoly.Terms.First().CoEfficient;

			// +/- constant
			//     --------
			//	   leading

			if (leading == 0)
			{
				throw new Exception("Leading coefficient is zero!?");
			}

			var constantDivisors = GetAllDivisors(constant);
			var leadingDivisors = GetAllDivisors(leading);

			constantDivisors.AddRange(constantDivisors.ToList().Select(n => BigInteger.Negate(n)));
			if (leadingDivisors.Count > 1)
			{
				leadingDivisors.AddRange(leadingDivisors.ToList().Select(n => BigInteger.Negate(n)));
			}

			foreach (var denominator in leadingDivisors)
			{
				foreach (var numerator in constantDivisors)
				{
					double rational = (double)numerator / (double)denominator;

					if (remainingPoly.Evaluate(rational) == 0)
					{
						int negatedNumerator = (int)BigInteger.Negate(numerator);

						string sign = (Math.Sign(negatedNumerator) == -1) ? "-" : "+";

						string denomString = (denominator == 1) ? "" : $"{denominator}*";

						string factorString = $"{denomString}X {sign} {Math.Abs(negatedNumerator)}";

						Polynomial factor = Parse(factorString);
						results.Add(factor);

						remainingPoly = Divide(remainingPoly, factor);

						if (remainingPoly == Polynomial.One)
						{
							return results;
						}
					}
				}
			}

			if (remainingPoly != Polynomial.One)
			{
				results.Add(remainingPoly);
			}

			return results;
		}

		private static List<BigInteger> GetAllDivisors(BigInteger value)
		{
			BigInteger n = value;

			if (BigInteger.Abs(n) == 1)
			{
				return new List<BigInteger> { n };
			}

			List<BigInteger> results = new List<BigInteger>();
			if (n.Sign == -1)
			{
				results.Add(-1);
				n = n * BigInteger.MinusOne;
			}
			for (BigInteger i = 1; i * i < n; i++)
			{
				if (n % i == 0)
				{
					results.Add(i);
				}
			}
			for (BigInteger i = n.SquareRoot(); i >= 1; i--)
			{
				if (n % i == 0)
				{
					results.Add(n / i);
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the first derivative of this polynomial.
		/// </summary>
		public static Polynomial GetDerivativePolynomial(Polynomial polynomial)
		{
			int d;
			List<Term> terms = new List<Term>();
			foreach (Term term in polynomial.Terms)
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

		/// <summary>
		/// Finds the indefinite integral of the polynomial.
		/// </summary>
		/// <param name="c">The constant.</param>
		/// <returns>The indefinite integral.</returns>
		public Polynomial IndefiniteIntegral(BigInteger c)
		{
			List<Term> terms = new List<Term>();
			terms.Add(new Term(c, 0));
			BigInteger[] coefficients = new BigInteger[this.Degree + 2];
			coefficients[0] = c;

			for (int i = 0; i <= this.Degree; i++)
			{
				coefficients[i + 1] = this[i] / (i + 1);
				terms.Add(new Term(this[i] / (i + 1), i + 1));
			}

			return new Polynomial(terms.ToArray());
		}

		/// <summary>
		/// Gets the reciprocal polynomial.
		/// </summary>
		public static Polynomial GetReciprocalPolynomial(Polynomial polynomial)
		{
			List<Term> termsList = new List<Term>();

			int exponentIndex = 0;
			while (exponentIndex <= polynomial.Degree)
			{
				Term term = polynomial.Terms.Where(trm => trm.Exponent == exponentIndex).FirstOrDefault();
				if (term == null)
				{
					term = new Term(0, exponentIndex);
				}
				termsList.Add(term);
				exponentIndex++;
			}

			List<Term> newTerms = new List<Term>();

			exponentIndex = 0;
			int coefficientIndex = polynomial.Degree;
			while (coefficientIndex >= 0)
			{
				var coeff = polynomial[coefficientIndex];
				var exp = termsList[exponentIndex].Exponent;

				Term term = new Term(coeff, exp);
				newTerms.Add(term);

				exponentIndex++;
				coefficientIndex--;
			}

			return new Polynomial(newTerms.ToArray());
		}

		/// <summary>
		/// Makes the specified polynomial monic in the specified base (indeterminant).
		/// </summary>
		public static Polynomial MakeMonic(Polynomial polynomial, BigInteger polynomialBase)
		{
			int deg = polynomial.Degree;
			Polynomial result = polynomial.Clone();
			if (BigInteger.Abs(result.Terms[deg].CoEfficient) > 1)
			{
				BigInteger toAdd = (result.Terms[deg].CoEfficient - 1) * polynomialBase;
				result.Terms[deg].CoEfficient = 1;
				result.Terms[deg - 1].CoEfficient += toAdd;
			}
			return result;
		}

		/// <summary>
		/// Reduces the coefficient of a term if it is greater than half of the polynomial base by increasing the coefficient of the next higher degree term.
		/// </summary>
		public static Polynomial MakeCoefficientsSmaller(Polynomial polynomial, BigInteger polynomialBase)
		{
			BigInteger max = polynomialBase / 2;

			Polynomial result = polynomial.Clone();

			int pos = 0;
			int deg = result.Degree;
			while (pos <= deg)
			{
				if (result[pos] > max)
				{
					result[pos + 1] += 1;
					result[pos] = -(polynomialBase - result[pos]);
				}

				pos++;
			}

			return result.Clone();
		}

		#endregion

		#region Arithmetic

		/// <summary>
		/// Returns the Greatest Common Divisor of the two specified polynomials.
		/// </summary>
		public static Polynomial GCD(Polynomial left, Polynomial right)
		{
			List<Polynomial> leftFactors = Polynomial.Factor(left);
			List<Polynomial> rightFactors = Polynomial.Factor(right);

			Polynomial result = Polynomial.One;
			foreach (var factor in leftFactors)
			{
				if (rightFactors.Contains(factor))
				{
					result = Multiply(result, factor);
				}
			}

			return result;
		}

		/// <summary>
		/// Divides one polynomial by another.
		/// </summary>
		public static Polynomial Divide(Polynomial left, Polynomial right)
		{
			Polynomial remainder;
			return Polynomial.Divide(left, right, out remainder);
		}

		/// <summary>
		/// Divides one polynomial by another, returning the quotient and the remainder.
		/// </summary>
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

		/// <summary>
		/// Returns the product of two polynomials.
		/// </summary>
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

		/// <summary>
		/// Returns the product of an array of polynomials.
		/// </summary>
		public static Polynomial Product(params Polynomial[] polys)
		{
			return Product(polys.ToList());
		}

		/// <summary>
		/// Returns the product of an Enumerable collection of polynomials.
		/// </summary>
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

		/// <summary>
		/// Returns the specified polynomial multiplied with itself.
		/// </summary>
		public static Polynomial Square(Polynomial polynomial)
		{
			return Polynomial.Multiply(polynomial, polynomial);
		}

		/// <summary>
		/// Raises the specified polynomial by the specified power.
		/// </summary>
		public static Polynomial Pow(Polynomial polynomial, int exponent)
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
				return polynomial.Clone();
			}
			else if (exponent == 2)
			{
				return Square(polynomial);
			}

			Polynomial total = Polynomial.Square(polynomial);

			int counter = exponent - 2;
			while (counter != 0)
			{
				total = Polynomial.Multiply(total, polynomial);
				counter -= 1;
			}

			return total;
		}

		/// <summary>
		/// Subtracts one polynomial from another.
		/// </summary>
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

		/// <summary>
		/// Returns the sum of an array of polynomials.
		/// </summary>
		public static Polynomial Sum(params Polynomial[] polys)
		{
			return Sum(polys.ToList());
		}

		/// <summary>
		/// Returns the sum of an Enumerable collection of polynomials.
		/// </summary>
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

		/// <summary>
		/// Adds the two specified polynomials.
		/// </summary>
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

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
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

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
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

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public Polynomial Clone()
		{
			var terms = _terms.Select(pt => pt.Clone()).ToArray();
			return new Polynomial(terms);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		public bool Equals(Polynomial other)
		{
			return (this.CompareTo(other) == 0);
		}

		private static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			int hash = this.Degree.GetHashCode();
			foreach (Term term in this.Terms)
			{
				hash = CombineHashCodes(hash, term.GetHashCode());
			}
			return hash;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }
			Polynomial otherPoly = obj as Polynomial;
			if (otherPoly == null) { return false; }
			else { return Equals(otherPoly); }
		}

		private static string FormatString(Polynomial polynomial)
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

		/// <summary>
		/// Converts this polynomial instance to string.
		/// </summary>
		public override string ToString()
		{
			return Polynomial.FormatString(this);
		}

		#endregion

	}
}
