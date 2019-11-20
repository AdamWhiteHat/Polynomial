using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public partial class Polynomial : IPolynomial
	{
		public static class Field
		{
			public static IPolynomial GCD(IPolynomial left, IPolynomial right, BigInteger modulus)
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
					b = Field.ModMod(temp, b, modulus);
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

			public static IPolynomial ModMod(IPolynomial toReduce, IPolynomial modPoly, BigInteger primeModulus)
			{
				return Field.Modulus(Field.Modulus(toReduce, modPoly), primeModulus);
			}

			public static IPolynomial Modulus(IPolynomial poly, IPolynomial mod)
			{
				int sortOrder = mod.CompareTo(poly);
				if (sortOrder > 0)
				{
					return poly;
				}
				else if (sortOrder == 0)
				{
					return Polynomial.Zero;
				}

				IPolynomial remainder = new Polynomial();
				Polynomial.Divide(poly, mod, out remainder);

				return remainder;
			}

			public static IPolynomial Modulus(IPolynomial poly, BigInteger mod)
			{
				IPolynomial clone = poly.Clone();
				List<ITerm> terms = new List<ITerm>();

				foreach (ITerm term in clone.Terms)
				{
					BigInteger remainder = new BigInteger();
					BigInteger.DivRem(term.CoEfficient, mod, out remainder);

					if (remainder.Sign == -1)
					{
						remainder = (remainder + mod);
					}

					terms.Add(new Term(remainder, term.Exponent));
				}

				// Recalculate the degree
				ITerm[] termArray = terms.SkipWhile(t => t.CoEfficient.Sign == 0).ToArray();
				if (!termArray.Any())
				{
					termArray = Term.GetTerms(new BigInteger[] { 0 });
				}
				IPolynomial result = new Polynomial(termArray);
				return result;
			}

			public static IPolynomial Divide(IPolynomial left, IPolynomial right, BigInteger mod, out IPolynomial remainder)
			{
				if (left == null) throw new ArgumentNullException(nameof(left));
				if (right == null) throw new ArgumentNullException(nameof(right));
				if (right.Degree > left.Degree || right.CompareTo(left) == 1)
				{
					remainder = Polynomial.Zero;
					return left.Clone();
				}

				int rightDegree = right.Degree;
				int quotientDegree = (left.Degree - rightDegree) + 1;
				BigInteger leadingCoefficent = new BigInteger(right[rightDegree].ToByteArray()).Mod(mod);

				Polynomial rem = (Polynomial)left.Clone();
				Polynomial quotient = (Polynomial)Polynomial.Zero;

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent).Mod(mod);
					rem[rightDegree + i] = BigInteger.Zero;

					for (int j = rightDegree + i - 1; j >= i; j--)
					{
						rem[j] = BigInteger.Subtract(rem[j], BigInteger.Multiply(quotient[i], right[j - i]).Mod(mod)).Mod(mod);
					}
				}

				// Remove zeros
				rem.RemoveZeros();
				quotient.RemoveZeros();

				remainder = rem.Clone();
				return quotient.Clone();
			}

			public static IPolynomial Multiply(IPolynomial poly, BigInteger multiplier, BigInteger mod)
			{
				IPolynomial result = poly.Clone();

				foreach (ITerm term in result.Terms)
				{
					BigInteger newCoefficient = term.CoEfficient;
					if (newCoefficient != 0)
					{
						newCoefficient = (newCoefficient * multiplier);
						term.CoEfficient = (newCoefficient.Mod(mod));
					}
				}

				return result;
			}

			public static IPolynomial PowMod(IPolynomial poly, BigInteger exponent, BigInteger mod)
			{
				IPolynomial result = poly.Clone();

				foreach (ITerm term in result.Terms)
				{
					BigInteger newCoefficient = term.CoEfficient;
					if (newCoefficient != 0)
					{
						newCoefficient = BigInteger.ModPow(newCoefficient, exponent, mod);
						if (newCoefficient.Sign == -1)
						{
							throw new Exception("BigInteger.ModPow returned negative number");
						}
						term.CoEfficient = newCoefficient;
					}
				}

				return result;
			}

			public static IPolynomial ExponentiateMod(IPolynomial startPoly, BigInteger s2, IPolynomial f, BigInteger p)
			{
				IPolynomial result = Polynomial.One;
				if (s2 == 0) { return result; }

				IPolynomial A = startPoly.Clone();

				byte[] byteArray = s2.ToByteArray();
				bool[] bitArray = new BitArray(byteArray).Cast<bool>().ToArray();

				// Remove trailing zeros ?
				if (bitArray[0] == true)
				{
					result = startPoly.Clone();
				}

				int i = 1;
				int t = bitArray.Length;
				while (i < t)
				{
					A = Field.ModMod(Polynomial.Square(A), f, p);
					if (bitArray[i] == true)
					{
						result = Field.ModMod(Polynomial.Multiply(A, result), f, p);
					}
					i++;
				}

				return result;
			}

			public static IPolynomial ModPow(IPolynomial poly, BigInteger exponent, IPolynomial mod)
			{
				if (exponent < 0)
				{
					throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
				}
				else if (exponent == 0)
				{
					return Polynomial.One;
				}
				else if (exponent == 1)
				{
					return poly.Clone();
				}
				else if (exponent == 2)
				{
					return Polynomial.Square(poly);
				}

				IPolynomial total = Polynomial.Square(poly);

				BigInteger counter = exponent - 2;
				while (counter != 0)
				{
					total = Polynomial.Multiply(poly, total);

					if (total.CompareTo(mod) < 0)
					{
						total = Field.Modulus(total, mod);
					}

					counter -= 1;
				}

				return total;
			}

			public static bool IsIrreducibleOverField(IPolynomial f, BigInteger p)
			{
				IPolynomial splittingField = new Polynomial(
					new Term[] {
						new Term(  1, (int)p),
						new Term( -1, 1)
					});

				IPolynomial reducedField = Field.ModMod(splittingField, f, p);

				IPolynomial gcd = Polynomial.GCD(reducedField, f);
				return (gcd.CompareTo(Polynomial.One) == 0);
			}

			public static bool IsIrreducibleOverP(IPolynomial poly, BigInteger p)
			{
				List<BigInteger> coefficients = poly.Terms.Select(t => t.CoEfficient).ToList();

				BigInteger leadingCoefficient = coefficients.Last();
				BigInteger constantCoefficient = coefficients.First();

				coefficients.Remove(leadingCoefficient);
				coefficients.Remove(constantCoefficient);

				BigInteger leadingRemainder = new BigInteger();
				BigInteger.DivRem(leadingCoefficient, p, out leadingRemainder);

				BigInteger constantRemainder = new BigInteger();
				BigInteger.DivRem(constantCoefficient, p.Square(), out constantRemainder);

				bool result = (leadingRemainder != 0); // p does not divide leading coefficient

				result &= (constantRemainder != 0);    // p^2 does not divide constant coefficient

				coefficients.Add(p);
				result &= (coefficients.GCD() == 1); // GCD == 1

				return result;
			}
		}
	}
}
