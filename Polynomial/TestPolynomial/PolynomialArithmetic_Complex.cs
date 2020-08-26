using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Complex
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestBaseMExpansionConstructor()
		{
			string expected = "(2.00931623307253, 0)*X^3";

			Complex n = 1811 * 1777;

			IPolynomial<Complex> poly = new Polynomial<Complex>(n, 117, 3);
			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}


		[TestMethod]
		public void TestParse()
		{
			string expected = "(144, 0)*X^2 + (-12.5, -0.5)*X + (-3, -6.1)";

			IPolynomial<Complex> poly = Polynomial<Complex>.Parse(expected);
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
			string expected = "(-11.25, 1)*X^2 + (-1, -0.5)";

			IPolynomial<Complex> first = Polynomial<Complex>.Parse("(-12.0, 0)*X^2 - (2, 0)");
			IPolynomial<Complex> second = Polynomial<Complex>.Parse("(0.75, 1)*X^2 + (1.0, -0.5)");

			IPolynomial<Complex> sum = Polynomial<Complex>.Add(first, second);
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
			string expected = "(7, 0)*X^2 + X";

			IPolynomial<Complex> first = Polynomial<Complex>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<Complex> second = Polynomial<Complex>.Parse("2*X - 2");

			IPolynomial<Complex> difference = Polynomial<Complex>.Subtract(first, second);
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
			string expected = "(9, 12)*X^4 + (10.5, -8)*X^2 + (-2, 1)";

			IPolynomial<Complex> first = Polynomial<Complex>.Parse("(12.0, 0)*X^2 - (2, 0)");
			IPolynomial<Complex> second = Polynomial<Complex>.Parse("(0.75, 1)*X^2 + (1.0, -0.5)");

			IPolynomial<Complex> product = Polynomial<Complex>.Multiply(first, second);
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
			string expected = "(24, 0)*X + (-1, 0)";

			IPolynomial<Complex> first = Polynomial<Complex>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<Complex> second = Polynomial<Complex>.Parse("12*X + 2");

			IPolynomial<Complex> quotient = Polynomial<Complex>.Divide(first, second);
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
			string expected = "(2, 0)*X + (-2, 0)";

			IPolynomial<Complex> first = Polynomial<Complex>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<Complex> second = Polynomial<Complex>.Parse("X^2 + 1");

			IPolynomial<Complex> residue = Polynomial<Complex>.Field<Complex>.Modulus(first, second);
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
