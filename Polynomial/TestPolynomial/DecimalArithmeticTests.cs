using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	using PolynomialLibrary;

	[TestClass]
	public class DecimalArithmeticTests
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		static DecimalArithmeticTests()
		{
			DecimalArithmeticType.Initialize();
		}

		[TestMethod]
		public void TestAddition()
		{
			string expecting = "24*X - 1";

			DecimalArithmeticType test = new DecimalArithmeticType(Decimal.Zero);


			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X + 2");
			var second = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X - 3");

			var result = Polynomial<DecimalArithmeticType, Decimal>.Add(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result.ToString()}");
			TestContext.WriteLine($"Expecting: {expecting.ToString()}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestSubtraction()
		{
			string expecting = "7*X^2 + 1*X";

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("7*X^2 + 3*X - 2");
			var second = Polynomial<DecimalArithmeticType, Decimal>.Parse("2*X - 2");

			var result = Polynomial<DecimalArithmeticType, Decimal>.Subtract(first, second);

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

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X + 2");
			var second = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X - 3");

			var result = Polynomial<DecimalArithmeticType, Decimal>.Multiply(first, second);

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

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("288*X^2 + 36*X - 2");
			var second = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X + 2");

			var result = Polynomial<DecimalArithmeticType, Decimal>.Divide(first, second);

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

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("12*X + 1");

			var result = Polynomial<DecimalArithmeticType, Decimal>.Square(first);

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

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			var second = Polynomial<DecimalArithmeticType, Decimal>.Parse("X^3 + 6*X^2 + 11*X + 6");

			var result = Polynomial<DecimalArithmeticType, Decimal>.GCD(first, second);

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

			var first = Polynomial<DecimalArithmeticType, Decimal>.Parse("288*X^2 + 36*X - 2");

			var result = Polynomial<DecimalArithmeticType, Decimal>.GetDerivativePolynomial(first);

			TestContext.WriteLine($"f' where f(x) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
