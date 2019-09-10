using System;
using System.Linq;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public abstract class ArithmeticType<TAlgebra, TNumber> : IArithmetic<TAlgebra, TNumber> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		protected ArithmeticType()
		{
			Instance = (IArithmetic<TAlgebra, TNumber>)this;
			this.Value = (TAlgebra)(IArithmetic<TAlgebra, TNumber>)this;
		}

		public ArithmeticType(TNumber value)
			: this()
		{
			this.InternalValue = value;
		}

		public static IArithmetic<TAlgebra, TNumber> Instance { get; protected set; }

		public TNumber InternalValue { get; protected set; }
		public TAlgebra Value { get; protected set; }

		public abstract TAlgebra MinusOne { get; }
		public abstract TAlgebra Zero { get; }
		public abstract TAlgebra One { get; }
		public abstract TAlgebra Two { get; }

		protected abstract Func<TNumber, TAlgebra> ConstructionMethod { get; }

		protected abstract Func<TAlgebra, TAlgebra, TAlgebra> AdditionMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, TAlgebra> SubtractionMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, TAlgebra> MultiplicationMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, TAlgebra> DivisionMethod { get; }
		protected abstract DivRemDelegate DivRemMethod { get; }

		protected abstract Func<TAlgebra, TAlgebra, TAlgebra> ModMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, TAlgebra, TAlgebra> ModPowMethod { get; }
		protected abstract Func<TAlgebra, int, TAlgebra> PowMethod { get; }

		protected abstract Func<TAlgebra, TAlgebra> AbsMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra> NegateMethod { get; }

		protected abstract Func<string, TAlgebra> ParseMethod { get; }


		protected abstract Func<TAlgebra, int> SignMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, int> CompareMethod { get; }
		protected abstract Func<TAlgebra, TAlgebra, bool> EqualsMethod { get; }


		protected delegate TAlgebra DivRemDelegate(TAlgebra dividend, TAlgebra divisor, out TAlgebra rem);


		public static TAlgebra operator +(ArithmeticType<TAlgebra, TNumber> left, ArithmeticType<TAlgebra, TNumber> right)
		{
			return left.Add(right.Value);
		}

		public static TAlgebra operator -(ArithmeticType<TAlgebra, TNumber> left, ArithmeticType<TAlgebra, TNumber> right)
		{
			return left.Subtract(right.Value);
		}

		public static TAlgebra operator *(ArithmeticType<TAlgebra, TNumber> left, ArithmeticType<TAlgebra, TNumber> right)
		{
			return left.Multiply(right.Value);
		}

		public static TAlgebra operator /(ArithmeticType<TAlgebra, TNumber> left, ArithmeticType<TAlgebra, TNumber> right)
		{
			return left.Divide(right.Value);
		}


		public TAlgebra Add(TAlgebra addend)
		{
			return AdditionMethod.Invoke(Value, addend);
		}

		public TAlgebra Subtract(TAlgebra subtrahend)
		{
			return SubtractionMethod.Invoke(Value, subtrahend);
		}

		public TAlgebra Multiply(TAlgebra multiplier)
		{
			return MultiplicationMethod.Invoke(Value, multiplier);
		}

		public TAlgebra Divide(TAlgebra divisor)
		{
			return DivisionMethod.Invoke(Value, divisor);
		}

		public TAlgebra DivRem(TAlgebra divisor, out TAlgebra remainder)
		{
			return DivRemMethod.Invoke(Value, divisor, out remainder);
		}

		public TAlgebra Mod(TAlgebra mod)
		{
			return ModMethod.Invoke(Value, mod);
		}

		public TAlgebra ModPow(TAlgebra exp, TAlgebra mod)
		{
			return ModPowMethod.Invoke(Value, exp, mod);
		}

		public TAlgebra Abs()
		{
			return AbsMethod.Invoke(Value);
		}

		public TAlgebra Negate()
		{
			return NegateMethod.Invoke(Value);
		}

		public TAlgebra Pow(int exp)
		{
			return PowMethod.Invoke(Value, exp);
		}

		public TAlgebra Parse(string value)
		{
			return ParseMethod(value);
		}

		public int Sign()
		{
			return SignMethod.Invoke(Value);
		}

		public int Compare(TAlgebra other)
		{
			return CompareMethod.Invoke(Value, other);
		}

		public bool Equals(TAlgebra other)
		{
			return EqualsMethod.Invoke(Value, other);
		}

		public abstract IArithmetic<TAlgebra, TNumber> Clone();

		public override string ToString()
		{
			return InternalValue.ToString();
		}
	}
}
