using System.Numerics;

namespace PolynomialLibrary
{
	public interface ITerm : ICloneable<ITerm>
	{
		int Exponent { get; }
		BigInteger CoEfficient { get; set; }
	}

}
