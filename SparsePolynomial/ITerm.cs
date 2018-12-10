using System.Numerics;

namespace SparsePolynomialLibrary
{
	public interface ITerm : ICloneable<ITerm>
	{
		int Exponent { get; }
		BigInteger CoEfficient { get; set; }
	}

}
