using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "PolynomialArithmetic - BigComplex")]
	public class PolynomialArithmetic_BigComplex : PolynomialArithmetic<BigComplex>
	{
	}
}
