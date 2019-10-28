using System;
using System.Numerics;

namespace PolynomialLibrary
{
	public interface IPolynomial<T> : ICloneable<IPolynomial<T>>, IComparable, IComparable<IPolynomial<T>>
	{
		T Degree { get; }
		ITerm<T>[] Terms { get; }

		T this[T degree]
		{
			get;
			set;
		}

		//void RemoveZeros();
		T Evaluate(T indeterminateValue);
	}

}
