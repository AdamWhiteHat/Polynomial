using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Int16
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestBaseMExpansionConstructor()
		{
			string expected = "X^3 + 3*X + 3";

			Int16 n = 13 * 11;

			IPolynomial<Int16> poly = new Polynomial<Int16>(n, 5);
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

			IPolynomial<Int16> first = Polynomial<Int16>.Parse("12*X + 2");
			IPolynomial<Int16> second = Polynomial<Int16>.Parse("12*X - 3");

			IPolynomial<Int16> sum = Polynomial<Int16>.Add(first, second);
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

			IPolynomial<Int16> first = Polynomial<Int16>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<Int16> second = Polynomial<Int16>.Parse("2*X - 2");

			IPolynomial<Int16> difference = Polynomial<Int16>.Subtract(first, second);
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

			IPolynomial<Int16> first = Polynomial<Int16>.Parse("12*X + 2");
			IPolynomial<Int16> second = Polynomial<Int16>.Parse("12*X - 3");

			IPolynomial<Int16> product = Polynomial<Int16>.Multiply(first, second);
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

			IPolynomial<Int16> first = Polynomial<Int16>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<Int16> second = Polynomial<Int16>.Parse("12*X + 2");

			IPolynomial<Int16> quotient = Polynomial<Int16>.Divide(first, second);
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

			IPolynomial<Int16> first = Polynomial<Int16>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<Int16> second = Polynomial<Int16>.Parse("X^2 + 1");

			IPolynomial<Int16> residue = Polynomial<Int16>.Field<Int16>.Modulus(first, second);
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
