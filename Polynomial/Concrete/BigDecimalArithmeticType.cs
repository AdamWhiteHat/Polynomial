using System;
using System.Linq;
using System.Numerics;
using ExtendedNumerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class BigDecimalArithmeticType : ArithmeticType<BigDecimalArithmeticType, BigDecimal>, IArithmetic<BigDecimalArithmeticType, BigDecimal>
	{
		static BigDecimalArithmeticType()
		{
			Initialize();
		}

		private BigDecimalArithmeticType()
			: base()
		{
		}

		public BigDecimalArithmeticType(BigDecimal value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<BigDecimalArithmeticType, BigDecimal>.Instance == null)
			{
				ArithmeticType<BigDecimalArithmeticType, BigDecimal>.Instance = new BigDecimalArithmeticType();
			}
		}


		private static BigDecimalArithmeticType Wrap(BigDecimal value)
		{
			return new BigDecimalArithmeticType(value).Value;
		}

		private static Func<BigDecimalArithmeticType, BigDecimalArithmeticType> Wrap(Func<BigDecimal, BigDecimal> function)
		{
			return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> Wrap(Func<BigDecimal, BigDecimal, BigDecimal> function)
		{
			return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override BigDecimalArithmeticType MinusOne { get { return Wrap(BigDecimal.MinusOne); } }
		public override BigDecimalArithmeticType Zero { get { return Wrap(BigDecimal.Zero); } }
		public override BigDecimalArithmeticType One { get { return Wrap(BigDecimal.One); } }
		public override BigDecimalArithmeticType Two { get { return Wrap(BigDecimal.Add(BigDecimal.One, BigDecimal.One)); } }

		protected override Func<BigDecimal, BigDecimalArithmeticType> ConstructionMethod { get { return new Func<BigDecimal, BigDecimalArithmeticType>((n) => Wrap(n)); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> AdditionMethod { get { return Wrap(BigDecimal.Add); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> SubtractionMethod { get { return Wrap(BigDecimal.Subtract); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> MultiplicationMethod { get { return Wrap(BigDecimal.Multiply); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> DivisionMethod { get { return Wrap(BigDecimal.Divide); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> ModPowMethod { get { throw new NotImplementedException(); /* return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType>((val, exp, mod) => Wrap(BigDecimal.ModPow(val.InternalValue, exp.InternalValue, mod.InternalValue))); */ } }
		protected override Func<BigDecimalArithmeticType, int, BigDecimalArithmeticType> PowMethod { get { return new Func<BigDecimalArithmeticType, int, BigDecimalArithmeticType>((b, e) => Wrap(BigDecimal.Pow(b.InternalValue, e))); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType> AbsMethod { get { return Wrap(BigDecimal.Abs); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType> NegateMethod { get { return Wrap(new Func<BigDecimal, BigDecimal>((bi) => BigDecimal.Subtract(0, bi))); } }
		protected override Func<string, BigDecimalArithmeticType> ParseMethod { get { return new Func<string, BigDecimalArithmeticType>((str) => Wrap(BigDecimal.Parse(str))); } }
		protected override Func<BigDecimalArithmeticType, int> SignMethod { get { return new Func<BigDecimalArithmeticType, int>((bi) => bi.InternalValue.Sign); } }
		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, bool> EqualsMethod { get { return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }

		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, int> CompareMethod
		{
			get
			{
				return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType, int>(
				(l, r) =>
				{
					BigDecimal left = (BigDecimal)(l.InternalValue);
					BigDecimal right = (BigDecimal)(r.InternalValue);
					if (left == right)
					{
						return 0;
					}
					else if (left > right)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				});
			}
		}

		// delegate BigDecimalArithmeticType DivRemDelegate(BigDecimalArithmeticType dividend, BigDecimalArithmeticType divisor, out BigDecimalArithmeticType rem);
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }
		private BigDecimalArithmeticType DivRemFunction(BigDecimalArithmeticType dividend, BigDecimalArithmeticType divisor, out BigDecimalArithmeticType rem)
		{
			BigDecimal result = BigDecimal.Divide(dividend.InternalValue, divisor.InternalValue);
			BigDecimal outRemainder = BigDecimal.Mod(dividend.InternalValue, divisor.InternalValue);
			rem = Wrap(outRemainder);
			return Wrap(result);
		}

		protected override Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType> ModMethod
		{
			get
			{
				return new Func<BigDecimalArithmeticType, BigDecimalArithmeticType, BigDecimalArithmeticType>(
					(input, modulus) =>
					{
						BigDecimal mod = modulus.InternalValue;
						if (mod.Equals(BigDecimal.Zero)) { throw new DivideByZeroException($"Parameter '{nameof(mod)}' must not be zero."); }

						BigDecimal src = input.InternalValue;

						BigDecimal r = (src >= mod) ? BigDecimal.Mod(src, mod) : src;
						return Wrap((r < 0) ? r + mod : r);
					}
				);
			}
		}

	}
}
