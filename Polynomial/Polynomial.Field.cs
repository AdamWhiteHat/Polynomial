using System;
using System.Text;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public partial class Polynomial<TAlgebra, TNumber> : IPolynomial<TAlgebra, TNumber> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		public static class Field
		{
			public static IPolynomial<TAlgebra, TNumber> GCD(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right, TAlgebra modulus)
			{
				IPolynomial<TAlgebra, TNumber> a = left.Clone();
				IPolynomial<TAlgebra, TNumber> b = right.Clone();

				if (b.Degree > a.Degree)
				{
					IPolynomial<TAlgebra, TNumber> swap = b;
					b = a;
					a = swap;
				}

				while (!(b.Terms.Length == 0 || b.Terms[0].CoEfficient.Equals(0)))
				{
					IPolynomial<TAlgebra, TNumber> temp = a;
					a = b;
					b = Field.ModMod(temp, b, modulus);
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

			public static IPolynomial<TAlgebra, TNumber> ModMod(IPolynomial<TAlgebra, TNumber> toReduce, IPolynomial<TAlgebra, TNumber> modPoly, TAlgebra primeModulus)
			{
				return Field.Modulus(Field.Modulus(toReduce, modPoly), primeModulus);
			}

			public static IPolynomial<TAlgebra, TNumber> Modulus(IPolynomial<TAlgebra, TNumber> poly, IPolynomial<TAlgebra, TNumber> mod)
			{
				int sortOrder = mod.CompareTo(poly);
				if (sortOrder > 0)
				{
					return poly;
				}
				else if (sortOrder == 0)
				{
					return Polynomial<TAlgebra, TNumber>.Zero;
				}

				IPolynomial<TAlgebra, TNumber> remainder = Polynomial<TAlgebra, TNumber>.Zero;
				Polynomial<TAlgebra, TNumber>.Divide(poly, mod, out remainder);

				return remainder;
			}

			public static IPolynomial<TAlgebra, TNumber> Modulus(IPolynomial<TAlgebra, TNumber> poly, TAlgebra mod)
			{
				IPolynomial<TAlgebra, TNumber> clone = poly.Clone();
				List<ITerm<TAlgebra, TNumber>> terms = new List<ITerm<TAlgebra, TNumber>>();

				foreach (ITerm<TAlgebra, TNumber> term in clone.Terms)
				{
					TAlgebra remainder = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;
					TAlgebra quotient = term.CoEfficient.DivRem(mod, out remainder);

					if (remainder.Sign() == -1)
					{
						remainder = remainder.Add(mod);
					}

					terms.Add((ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(remainder, term.Exponent));
				}

				// Recalculate the degree
				ITerm<TAlgebra, TNumber>[] termArray = terms.SkipWhile(t => t.CoEfficient.Sign() == 0).ToArray();
				if (!termArray.Any())
				{
					termArray = GetTerms(new TAlgebra[] { ArithmeticType<TAlgebra, TNumber>.Instance.Zero });
				}
				IPolynomial<TAlgebra, TNumber> result = new Polynomial<TAlgebra, TNumber>(termArray);
				return result;
			}

			public static IPolynomial<TAlgebra, TNumber> Divide(IPolynomial<TAlgebra, TNumber> left, IPolynomial<TAlgebra, TNumber> right, TAlgebra mod, out IPolynomial<TAlgebra, TNumber> remainder)
			{
				if (left == null) throw new ArgumentNullException(nameof(left));
				if (right == null) throw new ArgumentNullException(nameof(right));
				if (right.Degree > left.Degree || right.CompareTo(left) == 1)
				{
					remainder = Polynomial<TAlgebra, TNumber>.Zero; return left;
				}

				int rightDegree = right.Degree;
				int quotientDegree = (left.Degree - rightDegree) + 1;
				TAlgebra leadingCoefficent = right[rightDegree].Mod(mod);

				Polynomial<TAlgebra, TNumber> rem = (Polynomial<TAlgebra, TNumber>)left.Clone();
				Polynomial<TAlgebra, TNumber> quotient = (Polynomial<TAlgebra, TNumber>)Polynomial<TAlgebra, TNumber>.Zero;

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = rem[rightDegree + i].Divide(leadingCoefficent).Mod(mod);
					rem[rightDegree + i] = ArithmeticType<TAlgebra, TNumber>.Instance.Zero;

					for (int j = rightDegree + i - 1; j >= i; j--)
					{
						rem[j] = rem[j].Subtract(quotient[i].Multiply(right[j - i]).Mod(mod)).Mod(mod);
					}
				}

				// Remove zeros
				rem.RemoveZeros();
				quotient.RemoveZeros();

				remainder = rem;
				return quotient;
			}

			public static IPolynomial<TAlgebra, TNumber> Multiply(IPolynomial<TAlgebra, TNumber> poly, TAlgebra multiplier, TAlgebra mod)
			{
				IPolynomial<TAlgebra, TNumber> result = poly.Clone();

				foreach (ITerm<TAlgebra, TNumber> term in result.Terms)
				{
					TAlgebra newCoefficient = term.CoEfficient;
					if (!newCoefficient.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
					{
						newCoefficient = newCoefficient.Multiply(multiplier);
						term.CoEfficient = newCoefficient.Mod(mod);
					}
				}

				return result;
			}

			public static IPolynomial<TAlgebra, TNumber> PowMod(IPolynomial<TAlgebra, TNumber> poly, TAlgebra exponent, TAlgebra mod)
			{
				IPolynomial<TAlgebra, TNumber> result = poly.Clone();

				foreach (ITerm<TAlgebra, TNumber> term in result.Terms)
				{
					TAlgebra newCoefficient = term.CoEfficient;
					if (!newCoefficient.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
					{
						newCoefficient = newCoefficient.ModPow(exponent, mod);
						if (newCoefficient.Sign() == -1)
						{
							throw new Exception("T.ModPow returned negative number");
						}
						term.CoEfficient = newCoefficient;
					}
				}

				return result;
			}

			public static IPolynomial<TAlgebra, TNumber> ExponentiateMod(IPolynomial<TAlgebra, TNumber> startPoly, TAlgebra s2, IPolynomial<TAlgebra, TNumber> f, TAlgebra p)
			{
				IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.One;
				if (s2.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero)) { return result; }

				IPolynomial<TAlgebra, TNumber> A = startPoly.Clone();

				byte[] byteArray = BigInteger.Parse(s2.ToString()).ToByteArray(); //Encoding.ASCII.GetBytes(s2);   // BitConverter.GetBytes(s2);
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
					A = Field.ModMod(Polynomial<TAlgebra, TNumber>.Square(A), f, p);
					if (bitArray[i] == true)
					{
						result = Field.ModMod(Polynomial<TAlgebra, TNumber>.Multiply(A, result), f, p);
					}
					i++;
				}

				return result;
			}

			public static IPolynomial<TAlgebra, TNumber> ModPow(IPolynomial<TAlgebra, TNumber> poly, TAlgebra exponent, IPolynomial<TAlgebra, TNumber> mod)
			{
				if (exponent.Compare(ArithmeticType<TAlgebra, TNumber>.Instance.Zero) < 0)
				{
					throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
				}
				else if (exponent.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
				{
					return Polynomial<TAlgebra, TNumber>.One;
				}
				else if (exponent.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.One))
				{
					return poly.Clone();
				}
				else if (exponent.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Two))
				{
					return Polynomial<TAlgebra, TNumber>.Square(poly);
				}

				IPolynomial<TAlgebra, TNumber> total = Polynomial<TAlgebra, TNumber>.Square(poly);

				TAlgebra counter = exponent.Subtract(ArithmeticType<TAlgebra, TNumber>.Instance.Two);
				while (!counter.Equals(ArithmeticType<TAlgebra, TNumber>.Instance.Zero))
				{
					total = Polynomial<TAlgebra, TNumber>.Multiply(poly, total);

					if (total.CompareTo(mod) < 0)
					{
						total = Field.Modulus(total, mod);
					}

					counter = counter.Subtract(ArithmeticType<TAlgebra, TNumber>.Instance.One);
				}

				return total;
			}

			public static bool IsIrreducibleOverField(IPolynomial<TAlgebra, TNumber> f, TAlgebra p)
			{
				IPolynomial<TAlgebra, TNumber> splittingField = new Polynomial<TAlgebra, TNumber>(
					new List<ITerm<TAlgebra, TNumber>>() {
						(ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>( ArithmeticType<TAlgebra, TNumber>.Instance.One, int.Parse(p.Value.ToString())),
						(ITerm<TAlgebra, TNumber>)new Term<TAlgebra, TNumber>(ArithmeticType<TAlgebra, TNumber>.Instance.MinusOne, 1)
					}.ToArray());

				IPolynomial<TAlgebra, TNumber> reducedField = Field.ModMod(splittingField, f, p);

				IPolynomial<TAlgebra, TNumber> gcd = Polynomial<TAlgebra, TNumber>.GCD(reducedField, f);
				return (gcd.CompareTo(Polynomial<TAlgebra, TNumber>.One) == 0);
			}
		}
	}
}
