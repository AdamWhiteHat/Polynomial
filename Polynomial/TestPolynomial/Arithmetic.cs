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
		public void TestDivide()
		{
			string expecting = "24*X - 1";

			IPolynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");
			IPolynomial second = Polynomial.Parse("12*X + 2");

			IPolynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
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
