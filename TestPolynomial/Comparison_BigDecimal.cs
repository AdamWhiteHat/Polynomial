using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigDecimal))]
	[TestFixture(Category = "Comparison - BigDecimal")]
	public class Comparison_BigDecimal : Comparison<BigDecimal>
	{
	}
}
