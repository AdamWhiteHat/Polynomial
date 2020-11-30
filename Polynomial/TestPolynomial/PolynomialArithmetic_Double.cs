using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "PolynomialArithmetic - Double")]
	public class PolynomialArithmetic_Double : PolynomialArithmetic<Double>
	{
		[Test]
		[TestCase("1.09986237454265*X^3 + 3.63797880709171E-12")]
		public override void MakeCoefficientsSmaller(string expected)
		{
			base.MakeCoefficientsSmaller(expected);
		}
	}
}
