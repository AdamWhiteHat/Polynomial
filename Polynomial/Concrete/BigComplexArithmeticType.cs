using System;
using System.Linq;
using System.Numerics;
using ExtendedNumerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class BigComplexArithmeticType : ArithmeticType<BigComplexArithmeticType, BigComplex>, IArithmetic<BigComplexArithmeticType, BigComplex>
	{
		static BigComplexArithmeticType()
		{
			Initialize();
		}

		private BigComplexArithmeticType()
			: base()
		{
		}

		public BigComplexArithmeticType(BigComplex value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<BigComplexArithmeticType, BigComplex>.Instance == null)
			{
				ArithmeticType<BigComplexArithmeticType, BigComplex>.Instance = new BigComplexArithmeticType();
			}
		}


		private static BigComplexArithmeticType Wrap(BigComplex value)
		{
			return new BigComplexArithmeticType(value).Value;
		}

		private static Func<BigComplexArithmeticType, BigComplexArithmeticType> Wrap(Func<BigComplex, BigComplex> function)
		{
			return new Func<BigComplexArithmeticType, BigComplexArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> Wrap(Func<BigComplex, BigComplex, BigComplex> function)
		{
			return new Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override BigComplexArithmeticType MinusOne { get { return Wrap(new BigComplex(-1d, 0d)); } }
		public override BigComplexArithmeticType Zero { get { return Wrap(new BigComplex(0d, 0d)); } }
		public override BigComplexArithmeticType One { get { return Wrap(new BigComplex(1d, 0d)); } }
		public override BigComplexArithmeticType Two { get { return Wrap(new BigComplex(2d, 0d)); } }

		protected override Func<BigComplex, BigComplexArithmeticType> ConstructionMethod { get { return new Func<BigComplex, BigComplexArithmeticType>((n) => Wrap(n)); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> AdditionMethod { get { return Wrap(BigComplex.Add); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> SubtractionMethod { get { return Wrap(BigComplex.Subtract); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> MultiplicationMethod { get { return Wrap(BigComplex.Multiply); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> DivisionMethod { get { return Wrap(BigComplex.Divide); } }
		protected override Func<BigComplexArithmeticType, int, BigComplexArithmeticType> PowMethod { get { return new Func<BigComplexArithmeticType, int, BigComplexArithmeticType>((b, e) => Wrap(BigComplex.Pow(b.InternalValue, e))); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType> AbsMethod { get { return new Func<BigComplexArithmeticType, BigComplexArithmeticType>((bc) => Wrap(new BigComplex(BigComplex.Abs(bc.InternalValue)))); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType> NegateMethod { get { return Wrap(BigComplex.Negate); } }
		protected override Func<string, BigComplexArithmeticType> ParseMethod { get { return new Func<string, BigComplexArithmeticType>((str) => Wrap(new BigComplex(BigInteger.Parse(str)))); } }
		protected override Func<BigComplexArithmeticType, int> SignMethod { get { return new Func<BigComplexArithmeticType, int>((bi) => bi.InternalValue.Sign); } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, bool> EqualsMethod { get { return new Func<BigComplexArithmeticType, BigComplexArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }
		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, int> CompareMethod
		{
			get
			{
				return new Func<BigComplexArithmeticType, BigComplexArithmeticType, int>(
				(l, r) =>
				{
					BigComplex left = (BigComplex)(l.InternalValue);
					BigComplex right = (BigComplex)(r.InternalValue);
					if (left == right)
					{
						return 0;
					}
					else if (left.Real > right.Real)
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

		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> ModPowMethod { get { throw new NotImplementedException("Modulus/Remainder function not defined on BiComplex numbers."); } }
		private BigComplexArithmeticType DivRemFunction(BigComplexArithmeticType dividend, BigComplexArithmeticType divisor, out BigComplexArithmeticType rem)
		{
			throw new NotImplementedException("Modulus/Remainder function not defined on BiComplex numbers.");
		}

		protected override Func<BigComplexArithmeticType, BigComplexArithmeticType, BigComplexArithmeticType> ModMethod
		{
			get
			{
				throw new NotImplementedException("Modulus/Remainder function not defined on BiComplex numbers.");
			}
		}

		public override IArithmetic<BigComplexArithmeticType, BigComplex> Clone()
		{
			return new BigComplexArithmeticType(new BigComplex(this.InternalValue.Real.Clone(), this.InternalValue.Imaginary.Clone()));
		}
	}
}
