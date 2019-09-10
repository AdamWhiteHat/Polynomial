using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public interface ITerm<TAlgebra, TNumber> : ICloneable<ITerm<TAlgebra, TNumber>> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		int Exponent { get; }
		TAlgebra CoEfficient { get; set; }
	}
}
