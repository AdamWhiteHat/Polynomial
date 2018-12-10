using System;
using System.Numerics;

namespace SparsePolynomialLibrary
{
	public interface IPolynomial : ICloneable<IPolynomial>, IComparable, IComparable<IPolynomial>
	{
		int Degree { get; }
		ITerm[] Terms { get; }

		BigInteger this[int degree]
		{
			get;
			set;
		}

		//void RemoveZeros();
		BigInteger Evaluate(BigInteger indeterminateValue);
	}

}
