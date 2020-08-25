using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_BigInteger
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestBaseMExpansionConstructor()
		{
			string expected = "2*X^3 + X^2 + 10*X + 62";

			BigInteger n = 1811 * 1777;

			IPolynomial<BigInteger> poly = new Polynomial<BigInteger>(n, 117, 3);
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

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X - 3");

			IPolynomial<BigInteger> sum = Polynomial<BigInteger>.Add(first, second);
			string actual = sum.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
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

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("2*X - 2");

			IPolynomial<BigInteger> difference = Polynomial<BigInteger>.Subtract(first, second);
			string actual = difference.ToString();

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestMultiply()
		{
			string expected = "144*X^2 - 12*X - 6";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X - 3");

			IPolynomial<BigInteger> product = Polynomial<BigInteger>.Multiply(first, second);
			string actual = product.ToString();

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide1()
		{
			string expected = "24*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X + 2");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide2()
		{
			string expected = "2*X - 2";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("6*X^2 - 6");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("3*X + 3");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide3()
		{
			string expected = "6";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("6*X^2 - 6");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("X^2 - 1");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide4()
		{
			string expected = "6*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("36*X^2 - 1");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("6*X + 1");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide5()
		{
			string expected = "6*X + 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("36*X^2 - 1");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("6*X - 1");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDivide6()
		{
			string expected = "144*X^2 + 18*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("2");

			IPolynomial<BigInteger> quotient = Polynomial<BigInteger>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestSquare()
		{
			string expected = "144*X^2 + 24*X + 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 1");

			IPolynomial<BigInteger> square = Polynomial<BigInteger>.Square(first);
			string actual = square.ToString();

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestGCD()
		{
			throw new NotImplementedException();

			string expected = "X^2 + 3*X + 2";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("X^3 + 6*X^2 + 11*X + 6");

			IPolynomial<BigInteger> gcd = Polynomial<BigInteger>.GCD(first, second);
			string actual = gcd.ToString();

			TestContext.WriteLine($"GCD({first} , {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void TestDerivative()
		{
			string expected = "576*X + 36";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");

			IPolynomial<BigInteger> derivative = Polynomial<BigInteger>.GetDerivativePolynomial(first);
			string actual = derivative.ToString();

			TestContext.WriteLine($"f' where f(X) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}
	}
}
