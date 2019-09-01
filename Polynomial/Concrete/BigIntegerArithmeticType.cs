using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class BigIntegerArithmeticType : ArithmeticType<BigIntegerArithmeticType, BigInteger>, IArithmetic<BigIntegerArithmeticType, BigInteger>
	{
		static BigIntegerArithmeticType()
		{
			Initialize();
		}

		private BigIntegerArithmeticType()
			: base()
		{
		}

		public BigIntegerArithmeticType(BigInteger value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<BigIntegerArithmeticType, BigInteger>.Instance == null)
			{
				ArithmeticType<BigIntegerArithmeticType, BigInteger>.Instance = new BigIntegerArithmeticType();
			}
		}


		private static BigIntegerArithmeticType Wrap(BigInteger value)
		{
			return new BigIntegerArithmeticType(value).Value;
		}

		private static Func<BigIntegerArithmeticType, BigIntegerArithmeticType> Wrap(Func<BigInteger, BigInteger> function)
		{
			return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> Wrap(Func<BigInteger, BigInteger, BigInteger> function)
		{
			return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override BigIntegerArithmeticType MinusOne { get { return Wrap(BigInteger.MinusOne); } }
		public override BigIntegerArithmeticType Zero { get { return Wrap(BigInteger.Zero); } }
		public override BigIntegerArithmeticType One { get { return Wrap(BigInteger.One); } }
		public override BigIntegerArithmeticType Two { get { return Wrap(BigInteger.Add(BigInteger.One, BigInteger.One)); } }

		protected override Func<BigInteger, BigIntegerArithmeticType> ConstructionMethod { get { return new Func<BigInteger, BigIntegerArithmeticType>((n) => Wrap(n)); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> AdditionMethod { get { return Wrap(BigInteger.Add); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> SubtractionMethod { get { return Wrap(BigInteger.Subtract); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> MultiplicationMethod { get { return Wrap(BigInteger.Multiply); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> DivisionMethod { get { return Wrap(BigInteger.Divide); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> ModPowMethod { get { return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType>((val, exp, mod) => Wrap(BigInteger.ModPow(val.InternalValue, exp.InternalValue, mod.InternalValue))); } }
		protected override Func<BigIntegerArithmeticType, int, BigIntegerArithmeticType> PowMethod { get { return new Func<BigIntegerArithmeticType, int, BigIntegerArithmeticType>((b, e) => Wrap(BigInteger.Pow(b.InternalValue, e))); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType> AbsMethod { get { return Wrap(BigInteger.Abs); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType> NegateMethod { get { return Wrap(BigInteger.Negate); } }
		protected override Func<string, BigIntegerArithmeticType> ParseMethod { get { return new Func<string, BigIntegerArithmeticType>((str) => Wrap(BigInteger.Parse(str))); } }
		protected override Func<BigIntegerArithmeticType, int> SignMethod { get { return new Func<BigIntegerArithmeticType, int>((bi) => bi.InternalValue.Sign); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, int> CompareMethod { get { return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType, int>((l, r) => BigInteger.Compare(l.InternalValue, r.InternalValue)); } }
		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, bool> EqualsMethod { get { return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }

		// delegate BigIntegerArithmeticType DivRemDelegate(BigIntegerArithmeticType dividend, BigIntegerArithmeticType divisor, out BigIntegerArithmeticType rem);
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }
		private BigIntegerArithmeticType DivRemFunction(BigIntegerArithmeticType dividend, BigIntegerArithmeticType divisor, out BigIntegerArithmeticType rem)
		{
			BigInteger outRemainder = -1;
			BigInteger result = BigInteger.DivRem(dividend.InternalValue, divisor.InternalValue, out outRemainder);
			rem = Wrap(outRemainder);
			return Wrap(result);
		}

		protected override Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType> ModMethod
		{
			get
			{
				return new Func<BigIntegerArithmeticType, BigIntegerArithmeticType, BigIntegerArithmeticType>(
					(input, modulus) =>
					{
						BigInteger mod = modulus.InternalValue;
						if (mod.Equals(BigInteger.Zero)) { throw new DivideByZeroException($"Parameter '{nameof(mod)}' must not be zero."); }

						BigInteger src = input.InternalValue;

						BigInteger r = (src >= mod) ? src % mod : src;
						return Wrap((r < 0) ? r + mod : r);
					}
				);
			}
		}

	}
}
