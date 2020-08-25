using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Int32
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestBaseMExpansionConstructor()
		{
			string expected = "2*X^3 + X^2 + 10*X + 62";

			Int32 n = 1811 * 1777;

			IPolynomial<Int32> poly = new Polynomial<Int32>(n, 117, 3);
			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestAddition()
		{
			string expected = "24*X - 1";

			IPolynomial<Int32> first = Polynomial<Int32>.Parse("12*X + 2");
			IPolynomial<Int32> second = Polynomial<Int32>.Parse("12*X - 3");

			IPolynomial<Int32> sum = Polynomial<Int32>.Add(first, second);
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

			IPolynomial<Int32> first = Polynomial<Int32>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<Int32> second = Polynomial<Int32>.Parse("2*X - 2");

			IPolynomial<Int32> difference = Polynomial<Int32>.Subtract(first, second);
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

			IPolynomial<Int32> first = Polynomial<Int32>.Parse("12*X + 2");
			IPolynomial<Int32> second = Polynomial<Int32>.Parse("12*X - 3");

			IPolynomial<Int32> product = Polynomial<Int32>.Multiply(first, second);
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

			IPolynomial<Int32> first = Polynomial<Int32>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<Int32> second = Polynomial<Int32>.Parse("12*X + 2");

			IPolynomial<Int32> quotient = Polynomial<Int32>.Divide(first, second);
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

			IPolynomial<Int32> first = Polynomial<Int32>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<Int32> second = Polynomial<Int32>.Parse("X^2 + 1");

			IPolynomial<Int32> residue = Polynomial<Int32>.Field<Int32>.Modulus(first, second);
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
