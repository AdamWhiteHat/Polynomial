using System;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Arithmetic
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			string expecting = "24*X - 1";

			IPolynomial first = Polynomial.Parse("12*X + 2");
			IPolynomial second = Polynomial.Parse("12*X - 3");

			IPolynomial result = Polynomial.Add(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result.ToString()}");
			TestContext.WriteLine($"Expecting: {expecting.ToString()}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestSubtraction()
		{
			string expecting = "7*X^2 + X";

			IPolynomial first = Polynomial.Parse("7*X^2 + 3*X - 2");
			IPolynomial second = Polynomial.Parse("2*X - 2");

			IPolynomial result = Polynomial.Subtract(first, second);

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestMultiply()
		{
			string expecting = "144*X^2 - 12*X - 6";

			IPolynomial first = Polynomial.Parse("12*X + 2");
			IPolynomial second = Polynomial.Parse("12*X - 3");

			IPolynomial result = Polynomial.Multiply(first, second);

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide001()
		{
			string expecting = "24*X - 1";

			IPolynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");
			IPolynomial second = Polynomial.Parse("12*X + 2");

			IPolynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result:    {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide002()
		{
			string expected = "X^3 + 15*X^2 + 29*X + 8";

			IPolynomial dividend = Polynomial.Parse("X ^ 6 + 30 * X ^ 5 + 283 * X ^ 4 + 886 * X ^ 3 + 1081 * X ^ 2 + 464 * X + 64");
			IPolynomial divisor = Polynomial.Parse(expected);

			IPolynomial quotient = Polynomial.Divide(dividend, divisor);

			TestContext.WriteLine($"({dividend}) / ({divisor})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result:    {quotient}");
			TestContext.WriteLine($"Expecting: {expected}");

			Assert.AreEqual(expected, quotient.ToString());
		}

		[TestMethod]
		public void TestDivideRemainder()
		{
			string expected = "301*X + 57";

			IPolynomial dividend = Polynomial.Parse("144*X^4 + 744*X^3 + 985*X^2 + 450*X + 63");
			IPolynomial divisor = Polynomial.Parse("144*X^2 + 24*X + 1");
			IPolynomial remainder = Polynomial.Zero;

			IPolynomial quotient = Polynomial.Divide(dividend, divisor, out remainder);

			TestContext.WriteLine($"({dividend}) / ({divisor})");
			TestContext.WriteLine($"= {quotient}");
			TestContext.WriteLine($"rem: {remainder}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result:    {remainder}");
			TestContext.WriteLine($"Expecting: {expected}");

			Assert.AreEqual(expected, remainder.ToString());
		}

		[TestMethod]
		public void TestSquare()
		{
			string expecting = "144*X^2 + 24*X + 1";

			IPolynomial first = Polynomial.Parse("12*X + 1");

			IPolynomial result = Polynomial.Square(first);

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestGCD()
		{
			string expecting = "X^2 + 3*X + 2";

			IPolynomial first = Polynomial.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			IPolynomial second = Polynomial.Parse("X^3 + 6*X^2 + 11*X + 6");

			//IPolynomial result = Polynomial.Multiply(mult, Polynomial.Parse("X + 1"));
			IPolynomial result = Polynomial.GCD(first, second);

			TestContext.WriteLine($"GCD({first} , {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDerivative()
		{
			string expecting = "576*X + 36";

			IPolynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");

			IPolynomial result = Polynomial.GetDerivativePolynomial(first);

			TestContext.WriteLine($"f' where f(x) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
