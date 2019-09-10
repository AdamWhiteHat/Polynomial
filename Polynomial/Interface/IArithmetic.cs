using System;
using System.Linq;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public interface IArithmetic<TAlgebra, TNumber> : ICloneable<IArithmetic<TAlgebra, TNumber>> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		TAlgebra MinusOne { get; }
		TAlgebra Zero { get; }
		TAlgebra One { get; }
		TAlgebra Two { get; }


		TNumber InternalValue { get; }
		TAlgebra Value { get; }

		TAlgebra Add(TAlgebra addend);
		TAlgebra Subtract(TAlgebra subtrahend);
		TAlgebra Multiply(TAlgebra multiplier);
		TAlgebra Divide(TAlgebra divisor);
		TAlgebra DivRem(TAlgebra divisor, out TAlgebra remainder);
		TAlgebra Mod(TAlgebra mod);
		TAlgebra ModPow(TAlgebra exp, TAlgebra mod);
		TAlgebra Pow(int exp);
		TAlgebra Abs();
		TAlgebra Negate();

		TAlgebra Parse(string input);

		int Sign();
		int Compare(TAlgebra other);
		bool Equals(TAlgebra other);
	}
}
