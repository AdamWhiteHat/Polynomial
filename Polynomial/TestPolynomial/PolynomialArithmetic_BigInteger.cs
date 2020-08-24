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
		public void TestAddition()
		{
			string expecting = "24*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X - 3");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Add(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result.ToString()}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestSubtraction()
		{
			string expecting = "7*X^2 + X";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("2*X - 2");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Subtract(first, second);

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

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X - 3");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Multiply(first, second);

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide1()
		{
			string expecting = "24*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("12*X + 2");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide2()
		{
			string expecting = "2*X - 2";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("6*X^2 - 6");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("3*X + 3");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide3()
		{
			string expecting = "6";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("6*X^2 - 6");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("X^2 - 1");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide4()
		{
			string expecting = "6*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("36*X^2 - 1");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("6*X + 1");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide5()
		{
			string expecting = "6*X + 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("36*X^2 - 1");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("6*X - 1");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide6()
		{
			string expecting = "144*X^2 + 18*X - 1";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("2");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Divide(first, second);

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

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("12*X + 1");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Square(first);

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestGCD()
		{
			throw new NotImplementedException();

			string expecting = "X^2 + 3*X + 2";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("X^3 + 6*X^2 + 11*X + 6");

			//IPolynomial<BigInteger> result = Polynomial<BigInteger>.Multiply(mult, Polynomial<BigInteger>.Parse("X + 1"));
			IPolynomial<BigInteger> result = Polynomial<BigInteger>.GCD(first, second);

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

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("288*X^2 + 36*X - 2");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.GetDerivativePolynomial(first);

			TestContext.WriteLine($"f' where f(X) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
