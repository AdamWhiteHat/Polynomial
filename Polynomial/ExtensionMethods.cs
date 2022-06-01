using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	/// <summary>
	/// Extension Methods for the <see cref="ExtendedArithmetic.Term"/> class.
	/// </summary>
	public static class TermExtensionMethods
	{
		/// <summary>
		/// Returns the coefficients from an array of <see cref="ExtendedArithmetic.Term"/>.
		/// </summary>
		public static BigInteger[] GetCoefficients(this Term[] source)
		{
			return source?.Select(trm => trm?.CoEfficient.Clone() ?? new BigInteger(0)).ToArray() ?? new BigInteger[] { new BigInteger(0) };
		}
	}

	/// <summary>
	/// Extension Methods for the <see cref="System.Numerics.BigInteger"/> class.
	/// </summary>
	public static class BigIntegerExtensionMethods
	{
		/// <summary>
		/// Returns the remainder of this instance modulus the specified number.
		/// </summary>
		public static BigInteger Mod(this BigInteger source, BigInteger mod)
		{
			if (mod.Equals(BigInteger.Zero))
			{
				throw new DivideByZeroException($"Parameter '{nameof(mod)}' must not be zero.");
			}
			BigInteger n = source.Clone();
			BigInteger r = (n >= mod) ? n % mod : n;
			return (r < 0) ? (r + mod) : r;
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public static BigInteger Clone(this BigInteger source)
		{
			return new BigInteger(source.ToByteArray());
		}

		/// <summary>
		/// Converts this instance to base 2, returning the results as an array of <see cref="bool"/>.
		/// </summary>
		public static bool[] ConvertToBase2(this BigInteger value)
		{
			byte[] byteArray = value.ToByteArray();
			bool[] bitArray = new BitArray(byteArray).Cast<bool>().ToArray();
			return bitArray;
		}

		/// <summary>
		/// Squares this instance by multiplying it with itself.
		/// </summary>
		public static BigInteger Square(this BigInteger source)
		{
			return (source * source);
		}

		/// <summary>
		/// Returns the square root of this instance.
		/// </summary>
		public static BigInteger SquareRoot(this BigInteger source)
		{
			if (source.IsZero) return new BigInteger(0);

			BigInteger n = new BigInteger(0);
			BigInteger p = new BigInteger(0);
			BigInteger low = new BigInteger(0);
			BigInteger high = BigInteger.Abs(source);

			while (high > low + 1)
			{
				n = (high + low) >> 1;
				p = n * n;

				if (source < p)
					high = n;
				else if (source > p)
					low = n;
				else
					break;
			}
			return source == p ? n : low;
		}

		/// <summary>
		/// Returns the Nth root of this instance.
		/// The root must be greater than or equal to 1.
		/// </summary>
		public static BigInteger NthRoot(this BigInteger source, int root)
		{
			BigInteger remainder;
			return source.NthRoot(root, out remainder);
		}

		/// <summary>
		/// Returns the Nth root of this instance plus the Remainder.
		// The root must be greater than or equal to 1.
		/// </summary>
		public static BigInteger NthRoot(this BigInteger source, int root, out BigInteger remainder)
		{
			if (root < 1) throw new Exception("Root must be greater than or equal to 1");
			if (source.Sign == -1) throw new Exception("Value must be a positive integer");

			if (source == BigInteger.One)
			{
				remainder = 0;
				return BigInteger.One;
			}
			if (source == BigInteger.Zero)
			{
				remainder = 0;
				return BigInteger.Zero;
			}
			if (root == 1)
			{
				remainder = 0;
				return source.Clone();
			}

			BigInteger upperbound = source.Clone();
			BigInteger lowerbound = new BigInteger(0);

			while (true)
			{
				BigInteger nval = (upperbound + lowerbound) >> 1;
				BigInteger tstsq = BigInteger.Pow(nval, root);

				if (tstsq > source) upperbound = nval;
				if (tstsq < source) lowerbound = nval;
				if (tstsq == source)
				{
					lowerbound = nval;
					break;
				}
				if (lowerbound == upperbound - 1) break;
			}
			remainder = source - BigInteger.Pow(lowerbound, root);
			return lowerbound;
		}

		/// <summary>
		/// Determines whether this instance is a perfect square number.
		/// </summary>
		public static bool IsSquare(this BigInteger source)
		{
			if (source == BigInteger.Zero)
			{
				return false;
			}

			BigInteger input = BigInteger.Abs(source);

			int base16 = (int)(input & Fifteen); // Convert to base 16 number
			if (base16 > 9)
			{
				return false; // return immediately in 6 cases out of 16.
			}

			// Squares in base 16 end in 0, 1, 4, or 9
			if (base16 != 2 && base16 != 3 && base16 != 5 && base16 != 6 && base16 != 7 && base16 != 8)
			{
				BigInteger remainder;
				input.NthRoot(2, out remainder);

				return (remainder == 0);
				// - OR -
				//return (sqrt.Square() == input);
			}
			return false;
		}
		private static readonly BigInteger Fifteen = new BigInteger(15);
	}

	/// <summary>
	/// Extension Methods for an <see cref="System.Collections.Generic.IEnumerable{T}"/> collection of the <see cref="System.Numerics.BigInteger"/> type.
	/// </summary>
	public static class IEnumerableBigIntegerExtensionMethods
	{
		/// <summary>
		/// Returns the product of this <see cref="System.Numerics.BigInteger"/> collection.
		/// </summary>
		public static BigInteger Product(this IEnumerable<int> source)
		{
			return source.Select(n => new BigInteger(n)).Aggregate((accumulator, current) => accumulator * current);
		}

		/// <summary>
		/// Returns the sum of this <see cref="System.Numerics.BigInteger"/> collection.
		/// </summary>
		public static BigInteger Sum(this IEnumerable<BigInteger> source)
		{
			return source.Aggregate((accumulator, current) => accumulator + current);
		}

		/// <summary>
		/// Returns the greatest common divisor of all the <see cref="System.Numerics.BigInteger"/> in this collection.
		/// </summary>
		public static BigInteger GCD(this IEnumerable<BigInteger> source)
		{
			return source.Aggregate(BigInteger.GreatestCommonDivisor);
		}
	}
}
