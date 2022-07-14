using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "PolynomialArithmetic - Decimal")]
	public class PolynomialArithmetic_Decimal : PolynomialArithmetic<Decimal>
	{
		[Test]
		[TestCase("24565", "17", "5*X^3")]
		public override void BaseMExpansionConstructor(string value, string polyBase, string expected)
		{
			base.BaseMExpansionConstructor(value, polyBase, expected);
		}

		[Test]
		[TestCase("32766", "31", "3", "1.0998623745426471081870363533*X^3")]
		public override void MakeCoefficientsSmaller(string value, string polybase, string forceDegree, string expected)
		{
			base.MakeCoefficientsSmaller(value, polybase, forceDegree, expected);
		}

		[Test]
		[TestCase("144*X^2 - 12*X + 6.51")]
		public override void Parse(string expected)
		{
			base.Parse(expected);
		}
	}
}
