using System;
using System.Numerics;
using PolynomialLibrary;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "PolynomialArithmetic - Decimal")]
	public class PolynomialArithmetic_Decimal : PolynomialArithmetic<Decimal>
	{
		[Test]
		[TestCase("5*X^3")]
		public override void BaseMExpansionConstructor(string expected)
		{
			base.BaseMExpansionConstructor(expected);
		}

		[Test]
		[TestCase("1.0998623745426471081870363533*X^3")]
		public override void MakeCoefficientsSmaller(string expected)
		{
			base.MakeCoefficientsSmaller(expected);
		}

		[Test]
		[TestCase("144*X^2 - 12*X + 6.51")]
		public override void Parse(string expected)
		{
			base.Parse(expected);
		}
	}
}
