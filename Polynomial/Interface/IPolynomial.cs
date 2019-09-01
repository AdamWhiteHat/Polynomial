using System;
using System.Numerics;

namespace PolynomialLibrary
{
	public interface IPolynomial<TAlgebra, TNumber> : ICloneable<IPolynomial<TAlgebra, TNumber>>, IComparable<IPolynomial<TAlgebra, TNumber>>
		where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		int Degree { get; }
		ITerm<TAlgebra, TNumber>[] Terms { get; }

		TAlgebra this[int degree]
		{
			get;
			set;
		}

		TAlgebra Evaluate(TAlgebra indeterminateValue);

		//IPolynomial<TAlgebra, TNumber> Parse(string value);
	}

}
