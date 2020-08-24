using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Decimal
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			string expected = "2.0093162330725337519113543659*X^3";

			decimal n = 1811 * 1777;

			IPolynomial<decimal> sum = new Polynomial<decimal>(n, 117, 3);
			string actual = sum.ToString();

			TestContext.WriteLine($"{sum}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestSubtraction()
		{
			string expected = "7.0*X^2 + X";

			IPolynomial<decimal> first = Polynomial<decimal>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<decimal> second = Polynomial<decimal>.Parse("2*X - 2");

			IPolynomial<decimal> difference = Polynomial<decimal>.Subtract(first, second);
			string actual = difference.ToString();

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestMultiply()
		{
			string expected = "144.00*X^2 - 12.00*X - 6.00";

			IPolynomial<decimal> first = Polynomial<decimal>.Parse("12*X + 2");
			IPolynomial<decimal> second = Polynomial<decimal>.Parse("12*X - 3");

			IPolynomial<decimal> product = Polynomial<decimal>.Multiply(first, second);
			string actual = product.ToString();

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");


			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide()
		{
			string expected = "24.0*X - 1.0";

			IPolynomial<decimal> first = Polynomial<decimal>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<decimal> second = Polynomial<decimal>.Parse("12*X + 2");

			IPolynomial<decimal> quotient = Polynomial<decimal>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestMod()
		{
			string expected = "2.0*X - 2.00";

			IPolynomial<decimal> first = Polynomial<decimal>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<decimal> second = Polynomial<decimal>.Parse("X^2 + 1");

			IPolynomial<decimal> residue = Polynomial<decimal>.Field<decimal>.Modulus(first, second);
			string actual = residue.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}
	}
}
