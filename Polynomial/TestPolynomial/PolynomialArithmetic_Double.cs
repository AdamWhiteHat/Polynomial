using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Double
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			string expected = "2.00931623307253*X^3";

			double n = 1811 * 1777;

			IPolynomial<double> sum = new Polynomial<double>(n, 117, 3);
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
			string expected = "7*X^2 + X";

			IPolynomial<double> first = Polynomial<double>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<double> second = Polynomial<double>.Parse("2*X - 2");

			IPolynomial<double> difference = Polynomial<double>.Subtract(first, second);
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
			string expected = "144*X^2 - 12*X - 6";

			IPolynomial<double> first = Polynomial<double>.Parse("12*X + 2");
			IPolynomial<double> second = Polynomial<double>.Parse("12*X - 3");

			IPolynomial<double> product = Polynomial<double>.Multiply(first, second);
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
			string expected = "24*X - 1";

			IPolynomial<double> first = Polynomial<double>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<double> second = Polynomial<double>.Parse("12*X + 2");

			IPolynomial<double> quotient = Polynomial<double>.Divide(first, second);
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
			string expected = "2*X - 2";

			IPolynomial<double> first = Polynomial<double>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<double> second = Polynomial<double>.Parse("X^2 + 1");

			IPolynomial<double> residue = Polynomial<double>.Field<double>.Modulus(first, second);
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
