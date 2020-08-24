using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Int64
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			string expected = "2*X^3 + X^2 + 10*X + 62";

			Int64 n = 1811 * 1777;

			IPolynomial<Int64> sum = new Polynomial<Int64>(n, 117, 3);
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

			IPolynomial<Int64> first = Polynomial<Int64>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<Int64> second = Polynomial<Int64>.Parse("2*X - 2");

			IPolynomial<Int64> difference = Polynomial<Int64>.Subtract(first, second);
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

			IPolynomial<Int64> first = Polynomial<Int64>.Parse("12*X + 2");
			IPolynomial<Int64> second = Polynomial<Int64>.Parse("12*X - 3");

			IPolynomial<Int64> product = Polynomial<Int64>.Multiply(first, second);
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

			IPolynomial<Int64> first = Polynomial<Int64>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<Int64> second = Polynomial<Int64>.Parse("12*X + 2");

			IPolynomial<Int64> quotient = Polynomial<Int64>.Divide(first, second);
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

			IPolynomial<Int64> first = Polynomial<Int64>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<Int64> second = Polynomial<Int64>.Parse("X^2 + 1");

			IPolynomial<Int64> residue = Polynomial<Int64>.Field<Int64>.Modulus(first, second);
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
