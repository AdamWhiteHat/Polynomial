using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public partial class Polynomial<T>
	{
		public static class Algorithms
		{
			/// <summary>
			/// Returns a^(p-1/2) (mod p)
			/// </summary>
			public static BigInteger EulersCriterion(BigInteger a, BigInteger p)
			{
				BigInteger exponent = (p - 1) / 2;
				BigInteger pow = BigInteger.Pow(a, (int)exponent);
				BigInteger result = BigInteger.ModPow(a, exponent, p);
				return result;
			}

			/// <summary>
			///  Legendre Symbol returns 1 for a (nonzero) quadratic residue mod p, -1 for a non-quadratic residue (non-residue), or 0 on zero.
			/// </summary>
			public static int LegendreSymbol(BigInteger a, BigInteger p)
			{
				if (p < 2) throw new ArgumentOutOfRangeException("p", $"Parameter 'p' must not be < 2, but you have supplied: {p}");
				if (a == 0) return 0;
				if (a == 1) return 1;

				int result;
				if (a % 2 == 0)
				{
					result = LegendreSymbol(a / 2, p);
					if (((p * p - 1) & 8) != 0) // instead of dividing by 8, shift the mask bit
					{
						result = -result;
					}
				}
				else
				{
					result = LegendreSymbol(p % a, a);
					if (((a - 1) * (p - 1) & 4) != 0) // instead of dividing by 4, shift the mask bit
					{
						result = -result;
					}
				}
				return result;
			}

			/// <summary>
			/// Find r such that (r | m) = goal, where  (r | m) is the Legendre symbol and goal can either be 1 or -1
			/// </summary>
			public static BigInteger LegendreSymbolSearch(BigInteger start, BigInteger modulus, BigInteger goal)
			{
				if (goal != -1 && goal != 0 && goal != 1)
				{
					throw new Exception($"Parameter '{nameof(goal)}' may only be -1, 0 or 1. It was {goal}.");
				}

				BigInteger counter = start;
				do
				{
					if (LegendreSymbol(counter, modulus) == goal)
					{
						break;
					}
					counter++;
				}
				while (true);

				return counter;
			}

			/// <summary>
			/// Finds X such that X^2 ≡ n (mod p)
			/// </summary>
			public static BigInteger TonelliShanks(BigInteger n, BigInteger p)
			{
				BigInteger ls = LegendreSymbol(n, p);
				if (ls != 1) // Check that n is indeed a square
				{
					throw new ArithmeticException($"Parameter n is not a quadratic residue, mod p. Legendre symbol = {ls}");
				}

				if (p.Mod(4) == 3)
				{
					return BigInteger.ModPow(n, ((p + 1) / 4), p);
				}

				// Define odd q and s such as p-1 = q * 2^s
				// That is, split p-1 up into a product of two numbers:
				// an odd q, and some power of 2, v, such that v = 2^s.
				BigInteger q = p - 1;
				BigInteger exponent = 0;
				while (q.Mod(2) == 0)
				{
					q = q / 2;
					exponent++;
				}

				if (exponent == 0) { throw new Exception(); }
				if (exponent == 1)
				{
					throw new Exception("This case should have already been covered by the p mod 4 check above.");
					//return  BigInteger.ModPow(n, ((p+1)/4), p);
				}

				//Step 1. Select a non-square z such as (z | p) = -1 , and set c ≡ z^q
				BigInteger z = LegendreSymbolSearch(0, p, -1);
				BigInteger c = BigInteger.ModPow(z, q, p);
				BigInteger r = BigInteger.ModPow(n, ((q + 1) / 2), p);
				BigInteger t = BigInteger.ModPow(n, q, p);

				////Step 3: loop => {
				//    IF t ≡ 1 OUTUPUT r, p-r .
				//    ELSE find, by repeated squaring, the lowest i in (0<i<m) , such as t^(2^i) ≡ 1
				//    LET b ≡ c^(2^(m-i-1)), r ≡ r*b, t ≡ t*b^2 , c ≡ b^2  }        
				BigInteger a = 0, b = 0;
				BigInteger counter = 1, max = exponent;

				while (t != 1 && counter < max) // Maths.LegendreSymbol(t,p) != 1
				{
					a = BigInteger.Pow(2, (int)(max - counter - 1)); //  (max-counter-1)
					b = BigInteger.ModPow(c, a, p);
					r = BigInteger.Multiply(r, b).Mod(p);
					t = BigInteger.ModPow(t * b, 2, p);
					c = BigInteger.ModPow(b, 2, p);

					counter++;
				}

				return r;
			}

			/// <summary>
			/// Finds the modulus m such that N ≡ a[i] (mod m) for all a[i] with 0 < i < a.Length
			/// </summary>
			public static BigInteger ChineseRemainderTheorem(BigInteger[] n, BigInteger[] a)
			{
				BigInteger prod = n.Aggregate(BigInteger.One, (i, j) => i * j);
				BigInteger p;
				BigInteger sm = 0;
				for (int i = 0; i < n.Length; i++)
				{
					p = prod / n[i];
					sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
				}
				return sm % prod;
			}

			/// <summary>
			/// Returns x such that a*x ≡ 1 (mod m)
			/// </summary>
			public static BigInteger ModularMultiplicativeInverse(BigInteger a, BigInteger m)
			{
				BigInteger b = a % m;
				for (int x = 1; x < m; x++)
				{
					if ((b * x) % m == 1)
					{
						return x;
					}
				}
				return 1;
			}

			/// <summary>
			/// Euler's Totient Phi Function. Returns the number of integers less than n that are co-prime to n
			/// </summary>
			public static int EulersTotientPhi(int n)
			{
				if (n < 3) { return 1; }
				if (n == 3) { return 2; }

				int totient = n;

				if ((n & 1) == 0)
				{
					totient >>= 1;
					while (((n >>= 1) & 1) == 0)
						;
				}

				for (int i = 3; i * i <= n; i += 2)
				{
					if (n % i == 0)
					{
						totient -= totient / i;
						while ((n /= i) % i == 0)
							;
					}
				}

				if (n > 1)
				{
					totient -= totient / n;
				}

				return totient;
			}
		}
	}
}
