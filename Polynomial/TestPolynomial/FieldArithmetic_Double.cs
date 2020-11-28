using System;
using PolynomialLibrary;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "FieldArithmetic - Double")]
	public class FieldArithmetic_Double : FieldArithmetic<Double>
	{
		[Test]
		[TestCase("5*X^3", "2", "X^3")]
		public override void ModulusInteger(string dividend, string modulusInteger, string expected)
		{
			base.ModulusInteger(dividend, modulusInteger, expected);
		}

		[Test]
		[TestCase("X^5", "2", "X^2 + X", "3", "2*X")]
		public override void ExponentiateMod(string root, string exponent, string modulusPolynomial, string modulusInteger, string expected)
		{
			base.ExponentiateMod(root, exponent, modulusPolynomial, modulusInteger, expected);
		}

		// [Test]
		public override void ModulusPolynomial(string dividend, string modulusPolynomial, string expected)
		{
			//base.ModulusPolynomial();
		}

		// [Test]
		public override void ModulusPolynomialModulusInteger(string dividend, string modulusPolynomial, string modulusInteger, string expected)
		{
			//base.ModulusPolynomialModulusInteger();
		}
	}
}
