using System.Numerics;

namespace PolynomialLibrary
{
	public interface ITerm<T> : ICloneable<ITerm<T>>
	{
		T Exponent { get; }
		T CoEfficient { get; set; }
	}

}
