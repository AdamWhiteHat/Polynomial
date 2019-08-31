using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class BigIntegerArithmeticType<TAlgebra> : ArithmeticType<TAlgebra, BigInteger>, IArithmetic<TAlgebra, BigInteger>
		where TAlgebra : BigIntegerArithmeticType<TAlgebra>, IArithmetic<TAlgebra, BigInteger>
	{
		private BigIntegerArithmeticType()
			: base()
		{
		}

		public BigIntegerArithmeticType(BigInteger value)
			: base(value)
		{
			ArithmeticType<TAlgebra, BigInteger>.Instance = new BigIntegerArithmeticType<TAlgebra>();
		}
		private static TAlgebra Wrap(BigInteger value)
		{
			return new BigIntegerArithmeticType<TAlgebra>(value).Value;
		}

		private static Func<TAlgebra, TAlgebra> Wrap(Func<BigInteger, BigInteger> function)
		{
			return new Func<TAlgebra, TAlgebra>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<TAlgebra, TAlgebra, TAlgebra> Wrap(Func<BigInteger, BigInteger, BigInteger> function)
		{
			return new Func<TAlgebra, TAlgebra, TAlgebra>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override TAlgebra MinusOne { get { return Wrap(BigInteger.MinusOne); } }
		public override TAlgebra Zero { get { return Wrap(BigInteger.Zero); } }
		public override TAlgebra One { get { return Wrap(BigInteger.One); } }
		public override TAlgebra Two { get { return Wrap(BigInteger.Add(BigInteger.One, BigInteger.One)); } }

		protected override Func<BigInteger, TAlgebra> ConstructionMethod { get { return new Func<BigInteger, TAlgebra>((n) => Wrap(n)); } }
		protected override Func<TAlgebra, TAlgebra, TAlgebra> AdditionMethod { get { return Wrap(BigInteger.Add); } }
		protected override Func<TAlgebra, TAlgebra, TAlgebra> SubtractionMethod { get { return Wrap(BigInteger.Subtract); } }
		protected override Func<TAlgebra, TAlgebra, TAlgebra> MultiplicationMethod { get { return Wrap(BigInteger.Multiply); } }
		protected override Func<TAlgebra, TAlgebra, TAlgebra> DivisionMethod { get { return Wrap(BigInteger.Divide); } }
		protected override Func<TAlgebra, TAlgebra, TAlgebra, TAlgebra> ModPowMethod { get { return new Func<TAlgebra, TAlgebra, TAlgebra, TAlgebra>((val, exp, mod) => Wrap(BigInteger.ModPow(val.InternalValue, exp.InternalValue, mod.InternalValue))); } }
		protected override Func<TAlgebra, int, TAlgebra> PowMethod { get { return new Func<TAlgebra, int, TAlgebra>((b, e) => Wrap(BigInteger.Pow(b.InternalValue, e))); } }
		protected override Func<TAlgebra, TAlgebra> AbsMethod { get { return Wrap(BigInteger.Abs); } }
		protected override Func<TAlgebra, TAlgebra> NegateMethod { get { return Wrap(BigInteger.Negate); } }
		protected override Func<string, TAlgebra> ParseMethod { get { return new Func<string, TAlgebra>((str) => Wrap(BigInteger.Parse(str))); } }
		protected override Func<TAlgebra, int> SignMethod { get { return new Func<TAlgebra, int>((bi) => bi.InternalValue.Sign); } }
		protected override Func<TAlgebra, TAlgebra, int> CompareMethod { get { return new Func<TAlgebra, TAlgebra, int>((l, r) => BigInteger.Compare(l.InternalValue, r.InternalValue)); } }
		protected override Func<TAlgebra, TAlgebra, bool> EqualsMethod { get { return new Func<TAlgebra, TAlgebra, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }

		// delegate TAlgebra DivRemDelegate(TAlgebra dividend, TAlgebra divisor, out TAlgebra rem);
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }
		private TAlgebra DivRemFunction(TAlgebra dividend, TAlgebra divisor, out TAlgebra rem)
		{
			BigInteger outRemainder = -1;
			BigInteger result = BigInteger.DivRem(dividend.InternalValue, divisor.InternalValue, out outRemainder);
			rem = Wrap(outRemainder);
			return Wrap(result);
		}

		protected override Func<TAlgebra, TAlgebra, TAlgebra> ModMethod
		{
			get
			{
				return new Func<TAlgebra, TAlgebra, TAlgebra>(
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
