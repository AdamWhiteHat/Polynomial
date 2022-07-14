using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "PolynomialArithmetic - Float")]
	public class PolynomialArithmetic_Float : PolynomialArithmetic<float>
	{
		[Test]
		[TestCase("32766", "31", "3", "1.099862*X^3 + 0.001953125")]
		public override void MakeCoefficientsSmaller(string value, string polybase, string forceDegree, string expected)
		{
			base.MakeCoefficientsSmaller(value, polybase, forceDegree, expected);
		}
	}
}
