using System;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Field_Double : Field<Double>
	{
		[TestMethod]
		public override void TestModulusInteger()
		{
			string expecting = "X^3";

			double n = 24565d;
			double x = 17d;
			double mod = 2;

			IPolynomial<double> poly = new Polynomial<double>(n, x, 3); // 5*X^3

			var result = Polynomial<double>.Field<double>.Modulus(poly, mod);

			string actual = result.ToString();

			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual);
		}

		[TestMethod]
		public override void TestExponentiateMod()
		{
			string expecting = "2*X";

			double n = 129140163d;
			double x = 3d;
			double exponent = 2;
			double mod = 3;

			IPolynomial<double> poly = new Polynomial<double>(n, x); // X^5
			IPolynomial<double> modPoly = Polynomial<double>.Add(new Polynomial<double>(4, 2), new Polynomial<double>(2, 2));

			var result = Polynomial<double>.Field<double>.ExponentiateMod(poly, exponent, modPoly, mod);

			string actual = result.ToString();

			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual);
		}

		public override void TestModulusPolynomial()
		{
			//base.TestModulusPolynomial();
		}

		public override void TestModulusPolynomialModulusInteger()
		{
			//base.TestModulusPolynomialModulusInteger();
		}
	}
}
