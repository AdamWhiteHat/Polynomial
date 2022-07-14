using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(Fraction))]
	[TestFixture(Category = "PolynomialArithmetic - Fraction")]
	public class PolynomialArithmetic_Fraction : PolynomialArithmetic<Fraction>
	{
		/*
		[Test]
		[TestCase("32766", "31", "3", "1 + 2975/29791*X^3")]
		public override void MakeCoefficientsSmaller(string value, string polybase, string forceDegree, string expected)
		{
			base.MakeCoefficientsSmaller(value, polybase, forceDegree, expected);
		}
		*/
	}
}
