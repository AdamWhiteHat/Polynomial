using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

namespace PolynomialLibrary
{
	public partial class Polynomial : IPolynomial
	{
		public class Field
		{
			public IPolynomial ModulusPolynomial { get; private set; }
			public BigInteger ModulusInteger { get; private set; }

			public Field(IPolynomial modulus, BigInteger mod)
			{
				ModulusPolynomial = modulus;
				ModulusInteger = mod;
			}

			public static IPolynomial ReduceFully(IPolynomial toReduce, IPolynomial modPoly, BigInteger modInt)
			{
				return ReduceInteger(ReducePolynomial(toReduce, modPoly), modInt);
			}

			public static IPolynomial ReducePolynomial(IPolynomial poly, IPolynomial modPoly)
			{
				int sortOrder = modPoly.CompareTo(poly);
				if (sortOrder > 0)
				{
					return poly;
				}
				else if (sortOrder == 0)
				{
					return Polynomial.Zero;
				}

				IPolynomial remainder = Polynomial.Zero;
				Polynomial.Divide(poly, modPoly, out remainder);

				return remainder;
			}

			public static IPolynomial ReduceInteger(IPolynomial poly, BigInteger modInt)
			{
				IPolynomial clone = poly.Clone();
				List<ITerm> terms = new List<ITerm>();

				foreach (ITerm term in clone.Terms)
				{
					BigInteger remainder = 0;
					BigInteger.DivRem(term.CoEfficient, modInt, out remainder);

					if (remainder.Sign == -1)
					{
						remainder = (remainder + modInt);
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

			public IPolynomial GCD(IPolynomial left, IPolynomial right)
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
					b = ReduceFully(temp, b, ModulusInteger);
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

			public IPolynomial Add(IPolynomial left, IPolynomial right)
			{
				return ReduceFully(Polynomial.Add(left, right), ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial Subtract(IPolynomial left, IPolynomial right)
			{
				return ReduceFully(Polynomial.Subtract(left, right), ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial Divide(IPolynomial left, IPolynomial right, out IPolynomial remainder)
			{
				if (left == null) throw new ArgumentNullException(nameof(left));
				if (right == null) throw new ArgumentNullException(nameof(right));
				if (right.Degree > left.Degree || right.CompareTo(left) == 1)
				{
					remainder = Polynomial.Zero; return left;
				}

				int rightDegree = right.Degree;
				int quotientDegree = (left.Degree - rightDegree) + 1;
				BigInteger leadingCoefficent = new BigInteger(right[rightDegree].ToByteArray()).Mod(ModulusInteger);

				Polynomial rem = (Polynomial)left.Clone();
				Polynomial quotient = (Polynomial)Polynomial.Zero;

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = BigInteger.Divide(rem[rightDegree + i], leadingCoefficent).Mod(ModulusInteger);
					rem[rightDegree + i] = BigInteger.Zero;

					for (int j = rightDegree + i - 1; j >= i; j--)
					{
						rem[j] = BigInteger.Subtract(rem[j], BigInteger.Multiply(quotient[i], right[j - i]).Mod(ModulusInteger)).Mod(ModulusInteger);
					}
				}

				// Remove zeros
				rem.RemoveZeros();
				quotient.RemoveZeros();

				remainder = rem;
				return ReduceFully(quotient, ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial MultiplyScalar(IPolynomial poly, BigInteger multiplier)
			{
				IPolynomial result = poly.Clone();

				foreach (ITerm term in result.Terms)
				{
					BigInteger newCoefficient = term.CoEfficient;
					if (newCoefficient != 0)
					{
						newCoefficient = (newCoefficient * multiplier);
						term.CoEfficient = (newCoefficient.Mod(ModulusInteger));
					}
				}

				return ReduceFully(result, ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial Multiply(IPolynomial left, IPolynomial right)
			{
				return ReduceFully(Polynomial.Multiply(left, right), ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial Square(IPolynomial poly)
			{
				return ReduceFully(Polynomial.Square(poly), ModulusPolynomial, ModulusInteger);
			}

			public IPolynomial Pow(IPolynomial poly, BigInteger exponent)
			{
				IPolynomial result = Polynomial.One;
				if (exponent == 0) { return result; }

				IPolynomial A = poly.Clone();

				bool[] bitArray = exponent.ConvertToBase2();

				// Remove trailing zeros ?
				if (bitArray[0] == true)
				{
					result = poly.Clone();
				}

				int i = 1;
				int t = bitArray.Length;
				while (i < t)
				{
					A = Square(A);
					if (bitArray[i] == true)
					{
						result = Multiply(A, result);
					}
					i++;
				}

				return result;
			}

			/*
			public IPolynomial PowMod(IPolynomial poly, BigInteger exponent)
			{
				IPolynomial result = poly.Clone();

				foreach (ITerm term in result.Terms)
				{
					BigInteger newCoefficient = term.CoEfficient;
					if (newCoefficient != 0)
					{
						newCoefficient = BigInteger.ModPow(newCoefficient, exponent, ModulusInteger);
						if (newCoefficient.Sign == -1)
						{
							throw new Exception("BigInteger.ModPow returned negative number");
						}
						term.CoEfficient = newCoefficient;
					}
				}

				return result;
			}
			*/

			/*
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
						total = Field.ReducePolynomial(total, mod);
					}

					counter -= 1;
				}

				return total;
			}
			*/

			public bool IsIrreducible()
			{
				IPolynomial splittingField = new Polynomial(
					new Term[] {
						new Term(  1, (int)ModulusInteger),
						new Term( -1, 1)
					});

				IPolynomial reducedField = Field.ReduceFully(splittingField, ModulusPolynomial, ModulusInteger);

				IPolynomial gcd = Polynomial.GCD(reducedField, ModulusPolynomial);
				return (gcd.CompareTo(Polynomial.One) == 0);
			}

			public bool IsIrreducible_Eisenstein()
			{
				List<BigInteger> coefficients = ModulusPolynomial.Terms.Select(t => t.CoEfficient).ToList();

				BigInteger leadingCoefficient = coefficients.Last();
				BigInteger constantCoefficient = coefficients.First();

				coefficients.Remove(leadingCoefficient);
				coefficients.Remove(constantCoefficient);

				BigInteger leadingRemainder = -1;
				BigInteger.DivRem(leadingCoefficient, ModulusInteger, out leadingRemainder);

				BigInteger constantRemainder = -1;
				BigInteger.DivRem(constantCoefficient, ModulusInteger.Square(), out constantRemainder);

				bool result = (leadingRemainder != 0); // p does not divide leading coefficient

				result &= (constantRemainder != 0);    // p^2 does not divide constant coefficient

				coefficients.Add(ModulusInteger);
				result &= (coefficients.GCD() == 1); // GCD == 1

				return result;
			}
		}
	}
}
