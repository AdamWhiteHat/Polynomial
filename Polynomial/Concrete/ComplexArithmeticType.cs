using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public class ComplexArithmeticType : ArithmeticType<ComplexArithmeticType, Complex>, IArithmetic<ComplexArithmeticType, Complex>
	{
		static ComplexArithmeticType()
		{
			Initialize();
		}

		private ComplexArithmeticType()
			: base()
		{
		}

		public ComplexArithmeticType(Complex value)
			: base(value)
		{
		}

		public static void Initialize()
		{
			if (ArithmeticType<ComplexArithmeticType, Complex>.Instance == null)
			{
				ArithmeticType<ComplexArithmeticType, Complex>.Instance = new ComplexArithmeticType();
			}
		}


		private static ComplexArithmeticType Wrap(Complex value)
		{
			return new ComplexArithmeticType(value).Value;
		}

		private static Func<ComplexArithmeticType, ComplexArithmeticType> Wrap(Func<Complex, Complex> function)
		{
			return new Func<ComplexArithmeticType, ComplexArithmeticType>((n) => Wrap(function.Invoke(n.InternalValue)));
		}

		private static Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> Wrap(Func<Complex, Complex, Complex> function)
		{
			return new Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType>((l, r) => Wrap(function.Invoke(l.InternalValue, r.InternalValue)));
		}

		public override ComplexArithmeticType MinusOne { get { return Wrap(new Complex(-(1.0d), 0.0d)); } }
		public override ComplexArithmeticType Zero { get { return Wrap(Complex.Zero); } }
		public override ComplexArithmeticType One { get { return Wrap(Complex.One); } }
		public override ComplexArithmeticType Two { get { return Wrap(Complex.Add(Complex.One, Complex.One)); } }

		protected override Func<Complex, ComplexArithmeticType> ConstructionMethod { get { return new Func<Complex, ComplexArithmeticType>((n) => Wrap(n)); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> AdditionMethod { get { return Wrap(Complex.Add); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> SubtractionMethod { get { return Wrap(Complex.Subtract); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> MultiplicationMethod { get { return Wrap(Complex.Multiply); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> DivisionMethod { get { return Wrap(Complex.Divide); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> ModPowMethod { get { throw new NotImplementedException(); /* return new Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType>((val, exp, mod) => Wrap(Complex.ModPow(val.InternalValue, exp.InternalValue, mod.InternalValue))); */ } }
		protected override Func<ComplexArithmeticType, int, ComplexArithmeticType> PowMethod { get { return new Func<ComplexArithmeticType, int, ComplexArithmeticType>((b, e) => Wrap(Complex.Pow(b.InternalValue, e))); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType> AbsMethod { get { return new Func<ComplexArithmeticType, ComplexArithmeticType>((ct) => Wrap(new Complex(Complex.Abs(ct.InternalValue), 0.0d))); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType> NegateMethod { get { return Wrap(Complex.Negate); } }
		protected override Func<string, ComplexArithmeticType> ParseMethod { get { return new Func<string, ComplexArithmeticType>((str) => Wrap(new Complex(double.Parse(str), 0.0d))); } }
		protected override Func<ComplexArithmeticType, int> SignMethod { get { return new Func<ComplexArithmeticType, int>((bi) => Math.Sign(((Complex)(bi.InternalValue)).Real)); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, bool> EqualsMethod { get { return new Func<ComplexArithmeticType, ComplexArithmeticType, bool>((l, r) => l.InternalValue.Equals(r.InternalValue)); } }
		protected override Func<ComplexArithmeticType, ComplexArithmeticType, int> CompareMethod
		{
			get
			{
				return new Func<ComplexArithmeticType, ComplexArithmeticType, int>(
				(l, r) =>
				{
					Complex left = (Complex)(l.InternalValue);
					Complex right = (Complex)(r.InternalValue);
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

		public override string ToString()
		{
			Complex value = this.InternalValue;

			if (value.Imaginary == 0)
			{
				return value.Real.ToString();
			}
			else if (value.Imaginary < 0)
			{
				return $"({value.Real} - {Math.Abs(value.Imaginary)}i)";
			}
			else
			{
				return $"({value.Real} + {value.Imaginary}i)";
			}
		}


		// delegate ComplexArithmeticType DivRemDelegate(ComplexArithmeticType dividend, ComplexArithmeticType divisor, out ComplexArithmeticType rem);
		protected override DivRemDelegate DivRemMethod { get { throw new NotImplementedException(); } }

		/*
		private ComplexArithmeticType DivRemFunction(ComplexArithmeticType dividend, ComplexArithmeticType divisor, out ComplexArithmeticType rem)
		{
			Complex outRemainder = -1;
			Complex result = Complex.Divide(dividend.InternalValue, divisor.InternalValue, out outRemainder);
			rem = Wrap(outRemainder);
			return Wrap(result);			
		}
		*/

		protected override Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType> ModMethod
		{
			get
			{
				throw new NotImplementedException();
				/*
				return new Func<ComplexArithmeticType, ComplexArithmeticType, ComplexArithmeticType>(
					(input, modulus) =>
					{
						Complex mod = modulus.InternalValue;
						if (mod.Equals(Complex.Zero)) { throw new DivideByZeroException($"Parameter '{nameof(mod)}' must not be zero."); }
						Complex src = input.InternalValue;
						Complex r = (src >= mod) ? src % mod : src;
						return Wrap((r < 0) ? r + mod : r);
					});
				*/
			}
		}

	}
}
