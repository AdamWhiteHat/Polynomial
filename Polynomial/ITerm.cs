using System.Numerics;

namespace ExtendedArithmetic
{
	public interface ITerm : ICloneable<ITerm>
	{
		int Exponent { get; }
		BigInteger CoEfficient { get; set; }
	}

}
