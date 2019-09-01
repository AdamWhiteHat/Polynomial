using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class DecimalArithmeticType : ArithmeticType<DecimalArithmeticType, Decimal>, IArithmetic<DecimalArithmeticType, Decimal>
	{
		static DecimalArithmeticType()
		{
			Initialize();
		}

		private DecimalArithmeticType()
			: base()
		{
		}

		public DecimalArithmeticType(Decimal value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<DecimalArithmeticType, Decimal>.Instance == null)
			{
				ArithmeticType<DecimalArithmeticType, Decimal>.Instance = new DecimalArithmeticType();
			}
		}


		private static DecimalArithmeticType Wrap(Decimal value)
		{
			return new DecimalArithmeticType(value).Value;
		}

		private static Func<DecimalArithmeticType, DecimalArithmeticType> Wrap(Func<Decimal, Decimal> function)
		{
			return new Func<DecimalArithmeticType, DecimalArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> Wrap(Func<Decimal, Decimal, Decimal> function)
		{
			return new Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override DecimalArithmeticType MinusOne { get { return Wrap(Decimal.MinusOne); } }
		public override DecimalArithmeticType Zero { get { return Wrap(Decimal.Zero); } }
		public override DecimalArithmeticType One { get { return Wrap(Decimal.One); } }
		public override DecimalArithmeticType Two { get { return Wrap(Decimal.Add(Decimal.One, Decimal.One)); } }

		protected override Func<Decimal, DecimalArithmeticType> ConstructionMethod { get { return new Func<Decimal, DecimalArithmeticType>((n) => Wrap(n)); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> AdditionMethod { get { return Wrap(Decimal.Add); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> SubtractionMethod { get { return Wrap(Decimal.Subtract); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> MultiplicationMethod { get { return Wrap(Decimal.Multiply); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> DivisionMethod { get { return Wrap(Decimal.Divide); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> ModPowMethod
		{
			get
			{
				throw new NotImplementedException();
				//return new Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType>((val, exp, mod) => Wrap(Decimal.ModPow(val.InternalValue, exp.InternalValue, mod.InternalValue)));
			}
		}
		protected override Func<DecimalArithmeticType, int, DecimalArithmeticType> PowMethod { get { return new Func<DecimalArithmeticType, int, DecimalArithmeticType>((b, e) => Wrap((decimal)Math.Pow(((double)b.InternalValue), e))); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType> AbsMethod { get { return Wrap(Math.Abs); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType> NegateMethod { get { return Wrap(Decimal.Negate); } }
		protected override Func<string, DecimalArithmeticType> ParseMethod { get { return new Func<string, DecimalArithmeticType>((str) => Wrap(Decimal.Parse(str))); } }
		protected override Func<DecimalArithmeticType, int> SignMethod { get { return new Func<DecimalArithmeticType, int>((bi) => Math.Sign(bi.InternalValue)); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, int> CompareMethod { get { return new Func<DecimalArithmeticType, DecimalArithmeticType, int>((l, r) => Decimal.Compare(l.InternalValue, r.InternalValue)); } }
		protected override Func<DecimalArithmeticType, DecimalArithmeticType, bool> EqualsMethod { get { return new Func<DecimalArithmeticType, DecimalArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }

		// delegate DecimalArithmeticType DivRemDelegate(DecimalArithmeticType dividend, DecimalArithmeticType divisor, out DecimalArithmeticType rem);
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }
		private DecimalArithmeticType DivRemFunction(DecimalArithmeticType dividend, DecimalArithmeticType divisor, out DecimalArithmeticType rem)
		{
			Decimal result = Decimal.Divide(dividend.InternalValue, divisor.InternalValue);
			Decimal outRemainder = Decimal.Remainder(dividend.InternalValue, divisor.InternalValue);
			rem = Wrap(outRemainder);
			return Wrap(result);
		}

		protected override Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType> ModMethod
		{
			get
			{
				return new Func<DecimalArithmeticType, DecimalArithmeticType, DecimalArithmeticType>(
					(input, modulus) =>
					{
						Decimal mod = modulus.InternalValue;
						if (mod.Equals(Decimal.Zero)) { throw new DivideByZeroException($"Parameter '{nameof(mod)}' must not be zero."); }

						Decimal src = input.InternalValue;

						Decimal r = (src >= mod) ? src % mod : src;
						return Wrap((r < 0) ? r + mod : r);
					}
				);
			}
		}

	}
}
