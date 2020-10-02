using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public partial class Polynomial
	{
		public static class Field
		{
			public static Polynomial GCD(Polynomial left, Polynomial right, BigInteger modulus)
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
					Polynomial temp = a;
					a = b;
					b = Field.ModMod(temp, b, modulus);
				}

				if (a.Degree == 0)
				{
					return Polynomial.One.Clone();
				}
				else
				{
					return a;
				}
			}

			public static Polynomial ModMod(Polynomial toReduce, Polynomial modPoly, BigInteger primeModulus)
			{
				return Field.Modulus(Field.Modulus(toReduce, modPoly), primeModulus);
			}

			public static Polynomial Modulus(Polynomial poly, Polynomial mod)
			{
				int sortOrder = mod.CompareTo(poly);
				if (sortOrder > 0)
				{
					return poly.Clone();
				}
				else if (sortOrder == 0)
				{
					return Polynomial.Zero.Clone();
				}

				Polynomial remainder;
				Polynomial.Divide(poly, mod, out remainder);

				return remainder;
			}

			public static Polynomial Modulus(Polynomial poly, BigInteger mod)
			{
				Polynomial clone = poly.Clone();
				List<Term> terms = new List<Term>();

				foreach (Term term in clone.Terms)
				{
					BigInteger remainder;
					BigInteger.DivRem(term.CoEfficient, mod, out remainder);

					if (remainder.Sign == -1)
					{
						remainder = (remainder + mod);
					}

					terms.Add(new Term(remainder, term.Exponent));
				}

				// Recalculate the degree
				Term[] termArray = terms.SkipWhile(t => t.CoEfficient.Sign == 0).ToArray();
				if (!termArray.Any())
				{
					termArray = Term.GetTerms(new BigInteger[] { 0 });
				}
				Polynomial result = new Polynomial(termArray);
				return result;
			}

			public static Polynomial Divide(Polynomial left, Polynomial right, BigInteger mod, out Polynomial remainder)
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
				BigInteger leadingCoefficent = right[rightDegree].Clone().Mod(mod);

				Polynomial rem = (Polynomial)left.Clone();
				Polynomial quotient = (Polynomial)Polynomial.Zero.Clone();

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent).Mod(mod);
					rem[rightDegree + i] = new BigInteger(0);

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

			public static Polynomial Multiply(Polynomial poly, BigInteger multiplier, BigInteger mod)
			{
				Polynomial result = poly.Clone();

				foreach (Term term in result.Terms)
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

			public static Polynomial PowMod(Polynomial poly, BigInteger exponent, BigInteger mod)
			{
				Polynomial result = poly.Clone();

				foreach (Term term in result.Terms)
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

			public static Polynomial ExponentiateMod(Polynomial startPoly, BigInteger s2, Polynomial f, BigInteger p)
			{
				Polynomial result = Polynomial.One.Clone();
				if (s2 == 0) { return result; }

				Polynomial A = startPoly.Clone();

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

			public static Polynomial ModPow(Polynomial poly, BigInteger exponent, Polynomial mod)
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

				Polynomial total = Polynomial.Square(poly);

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

			public static bool IsIrreducibleOverField(Polynomial f, BigInteger p)
			{
				Polynomial splittingField = new Polynomial(
					new Term[] {
						new Term(  1, (int)p),
						new Term( -1, 1)
					});

				Polynomial reducedField = Field.ModMod(splittingField, f, p);

				Polynomial gcd = Polynomial.GCD(reducedField, f);
				return (gcd.CompareTo(Polynomial.One) == 0);
			}

			public static bool IsIrreducibleOverP(Polynomial poly, BigInteger p)
			{
				List<BigInteger> coefficients = poly.Terms.Select(t => t.CoEfficient).ToList();

				BigInteger leadingCoefficient = coefficients.Last();
				BigInteger constantCoefficient = coefficients.First();

				coefficients.Remove(leadingCoefficient);
				coefficients.Remove(constantCoefficient);

				BigInteger leadingRemainder;
				BigInteger.DivRem(leadingCoefficient, p, out leadingRemainder);

				BigInteger constantRemainder;
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
