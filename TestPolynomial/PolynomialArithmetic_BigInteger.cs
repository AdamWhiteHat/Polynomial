using System.Numerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigInteger))]
	[TestFixture(Category = "PolynomialArithmetic - BigInteger")]
	public class PolynomialArithmetic_BigInteger : PolynomialArithmetic<BigInteger>
	{
	}
}
