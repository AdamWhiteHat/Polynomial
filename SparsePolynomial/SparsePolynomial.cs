using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

namespace SparsePolynomialLibrary
{
	public class SparsePolynomil : IPolynomial
	{
		public static IPolynomial Zero = new SparsePolynomil(PolynomialTerm.GetTerms(new BigInteger[] { 0 }));
		public static IPolynomial One = new SparsePolynomil(PolynomialTerm.GetTerms(new BigInteger[] { 1 }));
		public static IPolynomial Two = new SparsePolynomil(PolynomialTerm.GetTerms(new BigInteger[] { 2 }));

		public ITerm[] Terms { get { return _terms.ToArray(); } }
		private List<ITerm> _terms;
		public int Degree { get; private set; }

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
						ITerm newTerm = new PolynomialTerm(value, degree);
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

		public SparsePolynomil() { }

		public SparsePolynomil(ITerm[] terms)
		{
			SetTerms(terms);
			SetDegree();
		}

		public SparsePolynomil(BigInteger n, BigInteger polynomialBase)
			: this(n, polynomialBase, (int)Math.Truncate(BigInteger.Log(n, (double)polynomialBase) + 1))
		{
		}

		public SparsePolynomil(BigInteger n, BigInteger polynomialBase, int forceDegree)
		{
			Degree = forceDegree;
			SetTerms(GetPolynomialTerms(n, polynomialBase, Degree));
		}

		private void SetTerms(IEnumerable<ITerm> terms)
		{
			_terms = terms.OrderBy(t => t.Exponent).ToList();
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
					result.Add(new PolynomialTerm(toAdd, d));
					toAdd = 0;
				}
				else if (placeValue == toAdd)
				{
					result.Add(new PolynomialTerm(1, d));
					toAdd -= placeValue;
				}
				else if (placeValue < BigInteger.Abs(toAdd))
				{
					BigInteger remainder = 0;
					BigInteger quotient = BigInteger.DivRem(toAdd, placeValue, out remainder);

					if (quotient > placeValue)
					{
						quotient = placeValue;
					}

					result.Add(new PolynomialTerm(quotient, d));
					BigInteger toSubtract = BigInteger.Multiply(quotient, placeValue);

					toAdd -= toSubtract;
				}

				d--;
			}
			return result.ToList();
		}

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

		public double EvaluateDouble(double indeterminateValue)
		{
			return EvaluateDouble(Terms, indeterminateValue);
		}

		public static double EvaluateDouble(ITerm[] terms, double x)
		{
			double result = 0;

			int d = terms.Count() - 1;
			while (d >= 0)
			{
				double placeValue = Math.Pow(x, terms[d].Exponent);

				double addValue = (double)(terms[d].CoEfficient) * placeValue;

				result += addValue;

				d--;
			}

			return result;
		}

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
				terms.Add(new PolynomialTerm(term.CoEfficient * term.Exponent, d));
			}

			IPolynomial result = new SparsePolynomil(terms.ToArray());
			return result;
		}

		public static bool IsIrreducibleOverField(IPolynomial f, BigInteger p)
		{
			IPolynomial splittingField = new SparsePolynomil(
			new PolynomialTerm[] {
			new PolynomialTerm(  1, (int)p),
			new PolynomialTerm( -1, 1)
			});

			IPolynomial reducedField = SparsePolynomil.ModMod(splittingField, f, p);

			IPolynomial gcd = SparsePolynomil.GCD(reducedField, f);
			return (gcd.CompareTo(SparsePolynomil.One) == 0);
		}

		public static bool IsIrreducibleOverP(IPolynomial poly, BigInteger p)
		{
			List<BigInteger> coefficients = poly.Terms.Select(t => t.CoEfficient).ToList();

			BigInteger leadingCoefficient = coefficients.Last();
			BigInteger constantCoefficient = coefficients.First();

			coefficients.Remove(leadingCoefficient);
			coefficients.Remove(constantCoefficient);

			BigInteger leadingRemainder = -1;
			BigInteger.DivRem(leadingCoefficient, p, out leadingRemainder);

			BigInteger constantRemainder = -1;
			BigInteger.DivRem(constantCoefficient, p.Square(), out constantRemainder);

			bool result = (leadingRemainder != 0); // p does not divide leading coefficient

			result &= (constantRemainder != 0);    // p^2 does not divide constant coefficient

			coefficients.Add(p);
			result &= (coefficients.GCD() == 1); // GCD == 1

			return result;
		}

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
				IPolynomial temp = a;
				a = b;
				b = SparsePolynomil.Mod(temp, b);
			}

			if (a.Degree == 0)
			{
				return SparsePolynomil.One;
			}
			else
			{
				return a;
			}
		}

		public static IPolynomial ModularInverse(IPolynomial poly, BigInteger mod)
		{
			return new SparsePolynomil(PolynomialTerm.GetTerms(poly.Terms.Select(trm => (mod - trm.CoEfficient).Mod(mod)).ToArray()));
		}

		public static IPolynomial ModMod(IPolynomial toReduce, IPolynomial modPoly, BigInteger modPrime)
		{
			return SparsePolynomil.Modulus(SparsePolynomil.Mod(toReduce, modPoly), modPrime);
		}

		public static IPolynomial Mod(IPolynomial poly, IPolynomial mod)
		{
			if (mod.Degree > poly.Degree)
			{
				return poly;
			}

			IPolynomial remainder = new SparsePolynomil();
			Divide(poly, mod, out remainder);

			return remainder;
		}

		public static IPolynomial Modulus(IPolynomial poly, BigInteger mod)
		{
			IPolynomial clone = poly.Clone();
			List<ITerm> terms = new List<ITerm>();

			foreach (ITerm term in clone.Terms)
			{
				BigInteger remainder = 0;
				BigInteger.DivRem(term.CoEfficient, mod, out remainder);

				if (remainder.Sign == -1)
				{
					remainder = (remainder + mod);
				}

				terms.Add(new PolynomialTerm(remainder, term.Exponent));
			}

			// Recalculate the degree
			ITerm[] termArray = terms.SkipWhile(t => t.CoEfficient.Sign == 0).ToArray();
			IPolynomial result = new SparsePolynomil(termArray);
			return result;
		}

		private static void RemoveZeros(IPolynomial polynomial)
		{
			SparsePolynomil poly = (SparsePolynomil)polynomial;
			poly._terms.RemoveAll(t => t.CoEfficient == 0);
			poly.SetDegree();
		}

		public static IPolynomial Divide(IPolynomial left, IPolynomial right)
		{
			IPolynomial remainder = new SparsePolynomil();
			return SparsePolynomil.Divide(left, right, out remainder);
		}

		public static IPolynomial Divide(IPolynomial left, IPolynomial right, out IPolynomial remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree) { throw new InvalidOperationException(); }

			int rightDegree = right.Degree;
			int quotientDegree = (left.Degree - rightDegree) + 1;
			BigInteger leadingCoefficent = new BigInteger(right[rightDegree].ToByteArray());

			IPolynomial rem = left.Clone();
			IPolynomial quotient = SparsePolynomil.Zero;

			// The leading coefficient is the only number we ever divide by
			// (so if right is monic, polynomial division does not involve division at all!)
			for (int i = quotientDegree - 1; i >= 0; i--)
			{
				quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent);
				rem[rightDegree + i] = BigInteger.Zero;

				for (int j = rightDegree + i - 1; j >= i; j--)
				{
					rem[j] = BigInteger.Subtract(rem[j], BigInteger.Multiply(quotient[i], right[j - i]));
				}
			}

			// Remove zeros
			SparsePolynomil.RemoveZeros(rem);
			SparsePolynomil.RemoveZeros(quotient);

			remainder = rem;
			return quotient;
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
			return new SparsePolynomil(PolynomialTerm.GetTerms(terms));
		}

		public static IPolynomial MultiplyMod(IPolynomial poly, BigInteger multiplier, BigInteger mod)
		{
			IPolynomial result = poly.Clone();

			foreach (ITerm term in result.Terms)
			{
				BigInteger coEfficient = term.CoEfficient;
				if (coEfficient != 0)
				{
					coEfficient = (coEfficient * multiplier);
					term.CoEfficient = (coEfficient.Mod(mod));
				}
			}

			return result;
		}

		public static IPolynomial PowMod(IPolynomial poly, BigInteger exp, BigInteger mod)
		{
			IPolynomial result = poly.Clone();

			foreach (ITerm term in result.Terms)
			{
				BigInteger value = term.CoEfficient;
				if (value != 0)
				{
					value = BigInteger.ModPow(value, exp, mod);
					if (value.Sign == -1)
					{
						throw new Exception("BigInteger.ModPow returned negative number");
					}
					term.CoEfficient = value;
				}
			}

			return result;
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
					result = p;
				}
				else
				{
					result = SparsePolynomil.Multiply(result, p);
				}
			}

			return result;
		}

		public static IPolynomial Square(IPolynomial poly)
		{
			return SparsePolynomil.Multiply(poly, poly);
		}

		public static IPolynomial Pow(IPolynomial poly, int exponent)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
			}
			else if (exponent == 0)
			{
				return new SparsePolynomil(new PolynomialTerm[] { new PolynomialTerm(1, 0) });
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}
			else if (exponent == 2)
			{
				return Square(poly);
			}

			IPolynomial total = SparsePolynomil.Square(poly);

			int counter = exponent - 2;
			while (counter != 0)
			{
				total = SparsePolynomil.Multiply(total, poly);
				counter -= 1;
			}

			return total;
		}

		public static IPolynomial ExponentiateMod(IPolynomial startPoly, BigInteger s2, IPolynomial f, BigInteger p)
		{
			IPolynomial result = SparsePolynomil.One;
			if (s2 == 0) { return result; }

			IPolynomial A = startPoly.Clone();

			byte[] byteArray = s2.ToByteArray();
			bool[] bitArray = new BitArray(byteArray).Cast<bool>().ToArray();

			// Remove trailing zeros ?
			if (bitArray[0] == true)
			{
				result = startPoly;
			}

			int i = 1;
			int t = bitArray.Length;
			while (i < t)
			{
				A = SparsePolynomil.ModMod(SparsePolynomil.Square(A), f, p);
				if (bitArray[i] == true)
				{
					result = SparsePolynomil.ModMod(SparsePolynomil.Multiply(A, result), f, p);
				}
				i++;
			}

			return result;
		}

		public static IPolynomial ModPow(IPolynomial poly, BigInteger exponent, IPolynomial modulus)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
			}
			else if (exponent == 0)
			{
				return SparsePolynomil.One;
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}
			else if (exponent == 2)
			{
				return SparsePolynomil.Square(poly);
			}

			IPolynomial total = SparsePolynomil.Square(poly);

			BigInteger counter = exponent - 2;
			while (counter != 0)
			{
				total = Multiply(poly, total);

				if (total.CompareTo(modulus) < 0)
				{
					total = SparsePolynomil.Mod(total, modulus);
				}

				counter -= 1;
			}

			return total;
		}

		public static IPolynomial Subtract(IPolynomial left, IPolynomial right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			BigInteger[] terms = new BigInteger[Math.Min(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				BigInteger l = left[i];
				BigInteger r = right[i];

				terms[i] = (l - r);
			}

			IPolynomial result = new SparsePolynomil(PolynomialTerm.GetTerms(terms.ToArray()));

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
					result = p;
				}
				else
				{
					result = SparsePolynomil.Add(result, p);
				}
			}

			return result;
		}

		public static IPolynomial Add(IPolynomial left, IPolynomial right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			BigInteger[] terms = new BigInteger[Math.Min(left.Degree, right.Degree) + 1];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = (left[i] + right[i]);
			}

			IPolynomial result = new SparsePolynomil(PolynomialTerm.GetTerms(terms.ToArray()));
			return result;
		}

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

		public static IPolynomial MakeMonic(IPolynomial polynomial, BigInteger polynomialBase)
		{
			int deg = polynomial.Degree;
			IPolynomial result = new SparsePolynomil(polynomial.Terms.ToArray());
			if (BigInteger.Abs(result.Terms[deg].CoEfficient) > 1)
			{
				BigInteger toAdd = (result.Terms[deg].CoEfficient - 1) * polynomialBase;
				result.Terms[deg].CoEfficient = 1;
				result.Terms[deg - 1].CoEfficient += toAdd;
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(IPolynomial polynomial, BigInteger polynomialBase, BigInteger maxCoefficientSize = default(BigInteger))
		{
			BigInteger maxSize = maxCoefficientSize;

			if (maxSize == default(BigInteger))
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

				if (polynomial[pos] > maxSize &&
					polynomial[pos] > polynomial[pos + 1])
				{
					BigInteger diff = polynomial[pos] - maxSize;

					BigInteger toAdd = (diff / polynomialBase) + 1;
					BigInteger toRemove = toAdd * polynomialBase;

					polynomial[pos] -= toRemove;
					polynomial[pos + 1] += toAdd;
				}

				pos++;
			}
		}

		public IPolynomial Clone()
		{
			return new SparsePolynomil(Terms.Select(pt => pt.Clone()).ToArray());
		}

		public override string ToString()
		{
			return SparsePolynomil.FormatString(this);
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
	}
}
