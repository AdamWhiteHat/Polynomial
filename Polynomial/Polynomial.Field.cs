using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

namespace PolynomialLibrary
{
	public partial class Polynomial<T> : IPolynomial<T>
	{
		public static class Field<T>
		{
			public static IPolynomial<T> GCD(IPolynomial<T> left, IPolynomial<T> right, T modulus)
			{
				IPolynomial<T> a = left.Clone();
				IPolynomial<T> b = right.Clone();

				if (b.Degree > a.Degree)
				{
					IPolynomial<T> swap = b;
					b = a;
					a = swap;
				}

				while (!GenericArithmetic<T>.Equal(GenericArithmetic<T>.Convert(b.Terms.Length), GenericArithmetic<T>.Zero) || GenericArithmetic<T>.Equal(b.Terms[0].CoEfficient, GenericArithmetic<T>.Zero))
				{
					IPolynomial<T> temp = a;
					a = b;
					b = Field<T>.ModMod(temp, b, modulus);
				}

				if (a.Degree == 0)
				{
					return Polynomial<T>.One;
				}
				else
				{
					return a;
				}
			}

			public static IPolynomial<T> ModMod(IPolynomial<T> toReduce, IPolynomial<T> modPoly, T primeModulus)
			{
				return Field<T>.Modulus(Field<T>.Modulus(toReduce, modPoly), primeModulus);
			}

			public static IPolynomial<T> Modulus(IPolynomial<T> poly, IPolynomial<T> mod)
			{
				int sortOrder = mod.CompareTo(poly);
				if (sortOrder > 0)
				{
					return poly;
				}
				else if (sortOrder == 0)
				{
					return Polynomial<T>.Zero;
				}

				IPolynomial<T> remainder = Polynomial<T>.Zero;
				IPolynomial<T> quotient = Polynomial<T>.Divide(poly, mod, out remainder);

				return remainder;
			}

			public static IPolynomial<T> Modulus(IPolynomial<T> poly, T mod)
			{
				IPolynomial<T> clone = poly.Clone();
				List<ITerm<T>> terms = new List<ITerm<T>>();

				foreach (ITerm<T> term in clone.Terms)
				{
					T remainder = GenericArithmetic<T>.Zero;
					GenericArithmetic<T>.DivRem(term.CoEfficient, mod, out remainder);

					if (GenericArithmetic<T>.Equal(GenericArithmetic<T>.Sign(remainder), GenericArithmetic<T>.MinusOne))
					{
						remainder = GenericArithmetic<T>.Add(remainder, mod);
					}

					terms.Add(new Term<T>(remainder, term.Exponent));
				}

				// Recalculate the degree
				ITerm<T>[] termArray = terms.SkipWhile(t => GenericArithmetic<T>.Equal(GenericArithmetic<T>.Sign(t.CoEfficient), GenericArithmetic<T>.Zero)).ToArray();
				if (!termArray.Any())
				{
					termArray = Term<T>.GetTerms(new T[] { GenericArithmetic<T>.Zero });
				}
				IPolynomial<T> result = new Polynomial<T>(termArray);
				return result;
			}

			public static IPolynomial<T> Divide(IPolynomial<T> left, IPolynomial<T> right, T mod, out IPolynomial<T> remainder)
			{
				if (left == null) throw new ArgumentNullException(nameof(left));
				if (right == null) throw new ArgumentNullException(nameof(right));
				if (right.Degree > left.Degree || right.CompareTo(left) == 1)
				{
					remainder = Polynomial<T>.Zero;
					return left;
				}

				int rightDegree = right.Degree;
				int quotientDegree = left.Degree - rightDegree + 1;
				T leadingCoefficent = GenericArithmetic<T>.Modulo(GenericArithmetic<T>.Clone(right[rightDegree]), mod);

				Polynomial<T> rem = (Polynomial<T>)left.Clone();
				Polynomial<T> quotient = (Polynomial<T>)Polynomial<T>.Zero;

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = GenericArithmetic<T>.Modulo(GenericArithmetic<T>.Divide(rem[rightDegree + i], leadingCoefficent), mod);
					rem[rightDegree + i] = GenericArithmetic<T>.Zero;

					for (int j = (rightDegree + i) - 1; j >= i; j--)
					{
						rem[j] = GenericArithmetic<T>.Modulo(GenericArithmetic<T>.Subtract(rem[j], GenericArithmetic<T>.Modulo(GenericArithmetic<T>.Multiply(quotient[i], right[j - i]), mod)), mod);
					}
				}

				// Remove zeros
				rem.RemoveZeros();
				quotient.RemoveZeros();

				remainder = rem;
				return quotient;
			}

			public static IPolynomial<T> Multiply(IPolynomial<T> poly, T multiplier, T mod)
			{
				IPolynomial<T> result = poly.Clone();

				foreach (ITerm<T> term in result.Terms)
				{
					T newCoefficient = term.CoEfficient;
					if (GenericArithmetic<T>.NotEqual(newCoefficient, GenericArithmetic<T>.Zero))
					{
						newCoefficient = GenericArithmetic<T>.Multiply(newCoefficient, multiplier);
						term.CoEfficient = GenericArithmetic<T>.Modulo(newCoefficient, mod);
					}
				}

				return result;
			}

			public static IPolynomial<T> PowMod(IPolynomial<T> poly, int exponent, T mod)
			{
				IPolynomial<T> result = poly.Clone();

				foreach (ITerm<T> term in result.Terms)
				{
					T newCoefficient = term.CoEfficient;
					if (GenericArithmetic<T>.NotEqual(newCoefficient, GenericArithmetic<T>.Zero))
					{
						newCoefficient = GenericArithmetic<T>.ModPow(newCoefficient, exponent, mod);
						if (GenericArithmetic<T>.Equal(GenericArithmetic<T>.Sign(newCoefficient), GenericArithmetic<T>.MinusOne))
						{
							throw new Exception("T.ModPow returned negative number");
						}
						term.CoEfficient = newCoefficient;
					}
				}

				return result;
			}

			public static IPolynomial<T> ExponentiateMod(IPolynomial<T> startPoly, T s2, IPolynomial<T> f, T p)
			{
				IPolynomial<T> result = Polynomial<T>.One;
				if (GenericArithmetic<T>.Equal(s2, GenericArithmetic<T>.Zero)) { return result; }

				IPolynomial<T> A = startPoly.Clone();

				byte[] byteArray = GenericArithmetic<T>.ToBytes(s2);
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
					A = Field<T>.ModMod(Polynomial<T>.Square(A), f, p);
					if (bitArray[i] == true)
					{
						result = Field<T>.ModMod(Polynomial<T>.Multiply(A, result), f, p);
					}
					i++;
				}

				return result;
			}

			public static IPolynomial<T> ModPow(IPolynomial<T> poly, T exponent, IPolynomial<T> mod)
			{
				if (GenericArithmetic<T>.LessThan(exponent, GenericArithmetic<T>.Zero))
				{
					throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
				}
				else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.Zero))
				{
					return Polynomial<T>.One;
				}
				else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.One))
				{
					return poly.Clone();
				}
				else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.Two))
				{
					return Polynomial<T>.Square(poly);
				}

				IPolynomial<T> total = Polynomial<T>.Square(poly);

				T counter = GenericArithmetic<T>.Subtract(exponent, GenericArithmetic<T>.Two);
				while (GenericArithmetic<T>.NotEqual(counter, GenericArithmetic<T>.Zero))
				{
					total = Polynomial<T>.Multiply(poly, total);

					if (total.CompareTo(mod) < 0)
					{
						total = Field<T>.Modulus(total, mod);
					}

					counter = GenericArithmetic<T>.Decrement(counter);
				}

				return total;
			}

			public static bool IsIrreducibleOverField(IPolynomial<T> f, T p)
			{
				IPolynomial<T> splittingField = new Polynomial<T>(
					new Term<T>[] {
						new Term<T>(GenericArithmetic<T>.One, GenericArithmetic<int>.Convert<T>(p)),
						new Term<T>(GenericArithmetic<T>.MinusOne,1)
					});

				IPolynomial<T> reducedField = Field<T>.ModMod(splittingField, f, p);

				IPolynomial<T> gcd = Polynomial<T>.GCD(reducedField, f);
				return (gcd.CompareTo(Polynomial<T>.One) == 0);
			}

			public static bool IsIrreducibleOverP(IPolynomial<T> poly, T p)
			{
				List<T> coefficients = poly.Terms.Select(t => t.CoEfficient).ToList();

				T leadingCoefficient = coefficients.Last();
				T constantCoefficient = coefficients.First();

				coefficients.Remove(leadingCoefficient);
				coefficients.Remove(constantCoefficient);

				T leadingRemainder = GenericArithmetic<T>.MinusOne;
				GenericArithmetic<T>.DivRem(leadingCoefficient, p, out leadingRemainder);

				T constantRemainder = GenericArithmetic<T>.MinusOne;
				GenericArithmetic<T>.DivRem(constantCoefficient, GenericArithmetic<T>.Multiply(p, p), out constantRemainder);

				bool result = GenericArithmetic<T>.NotEqual(leadingRemainder, GenericArithmetic<T>.Zero); // p does not divide leading coefficient

				result &= GenericArithmetic<T>.NotEqual(constantRemainder, GenericArithmetic<T>.Zero);    // p^2 does not divide constant coefficient

				coefficients.Add(p);
				result &= GenericArithmetic<T>.Equal(GenericArithmetic<T>.GCD(coefficients), GenericArithmetic<T>.One); // GCD == 1

				return result;
			}
		}
	}
}
