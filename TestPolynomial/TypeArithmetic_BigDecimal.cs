using ExtendedNumerics;
using NUnit.Framework;
using System.Numerics;

namespace TestPolynomial
{
	[TestOf(typeof(BigDecimal))]
	[TestFixture(Category = "TypeArithmetic - BigDecimal")]
	public class TypeArithmetic_BigDecimal : TypeArithmetic<BigDecimal>
	{
		public override void Logarithm(string argument, double logBase, string expected)
		{
			//base.Logarithm(argument, logBase, expected);
		}

		public override void Ln(string argument, string expected)
		{
			//base.Ln(argument, expected);
		}

		public override void Sqrt(string radicand, string expected)
		{
			//base.Sqrt(radicand, expected);
		}
	}
}
