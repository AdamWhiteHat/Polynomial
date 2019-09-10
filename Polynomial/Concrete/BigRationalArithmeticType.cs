using System;
using System.Linq;
using System.Numerics;
using ExtendedNumerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class BigRationalArithmeticType : ArithmeticType<BigRationalArithmeticType, BigRational>, IArithmetic<BigRationalArithmeticType, BigRational>
	{
		static BigRationalArithmeticType()
		{
			Initialize();
		}

		private BigRationalArithmeticType()
			: base()
		{
		}

		public BigRationalArithmeticType(BigRational value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<BigRationalArithmeticType, BigRational>.Instance == null)
			{
				ArithmeticType<BigRationalArithmeticType, BigRational>.Instance = new BigRationalArithmeticType();
			}
		}


		private static BigRationalArithmeticType Wrap(BigRational value)
		{
			return new BigRationalArithmeticType(value).Value;
		}

		private static Func<BigRationalArithmeticType, BigRationalArithmeticType> Wrap(Func<BigRational, BigRational> function)
		{
			return new Func<BigRationalArithmeticType, BigRationalArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> Wrap(Func<BigRational, BigRational, BigRational> function)
		{
			return new Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override BigRationalArithmeticType MinusOne { get { return Wrap(new BigRational(new BigInteger(-1))); } }
		public override BigRationalArithmeticType Zero { get { return Wrap(new BigRational(new BigInteger(0))); } }
		public override BigRationalArithmeticType One { get { return Wrap(new BigRational(new BigInteger(1))); } }
		public override BigRationalArithmeticType Two { get { return Wrap(new BigRational(new BigInteger(2))); } }

		protected override Func<BigRational, BigRationalArithmeticType> ConstructionMethod { get { return new Func<BigRational, BigRationalArithmeticType>((n) => Wrap(n)); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> AdditionMethod { get { return Wrap(BigRational.Add); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> SubtractionMethod { get { return Wrap(BigRational.Subtract); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> MultiplicationMethod { get { return Wrap(BigRational.Multiply); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> DivisionMethod { get { return Wrap(BigRational.Divide); } }
		protected override Func<BigRationalArithmeticType, int, BigRationalArithmeticType> PowMethod { get { return new Func<BigRationalArithmeticType, int, BigRationalArithmeticType>((b, e) => Wrap(BigRational.Pow(b.InternalValue, e))); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType> AbsMethod { get { return Wrap(BigRational.Abs); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType> NegateMethod { get { return Wrap(BigRational.Negate); } }
		protected override Func<string, BigRationalArithmeticType> ParseMethod { get { return new Func<string, BigRationalArithmeticType>((str) => Wrap(BigRational.Parse(str))); } }
		protected override Func<BigRationalArithmeticType, int> SignMethod { get { return new Func<BigRationalArithmeticType, int>((bi) => bi.InternalValue.Sign); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, int> CompareMethod { get { return new Func<BigRationalArithmeticType, BigRationalArithmeticType, int>((l, r) => BigRational.Compare(l.InternalValue, r.InternalValue)); } }
		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, bool> EqualsMethod { get { return new Func<BigRationalArithmeticType, BigRationalArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }
		protected override DivRemDelegate DivRemMethod { get { return DivRemFunction; } }

		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> ModPowMethod { get { throw new NotImplementedException(); } }
		private BigRationalArithmeticType DivRemFunction(BigRationalArithmeticType dividend, BigRationalArithmeticType divisor, out BigRationalArithmeticType rem) { throw new NotImplementedException(); }

		protected override Func<BigRationalArithmeticType, BigRationalArithmeticType, BigRationalArithmeticType> ModMethod { get { throw new NotImplementedException(); } }

		public override IArithmetic<BigRationalArithmeticType, BigRational> Clone()
		{
			return new BigRationalArithmeticType(new BigRational(this.InternalValue.WholePart.Clone(), new Fraction(this.InternalValue.FractionalPart.Numerator.Clone(), this.InternalValue.FractionalPart.Denominator.Clone())));
		}
	}
}
