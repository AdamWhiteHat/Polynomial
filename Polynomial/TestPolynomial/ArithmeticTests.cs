using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	using PolynomialLibrary;

	[TestClass]
	public class Arithmetic
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			string expecting = "24*X - 1";


			var first = IPolynomial<BigIntegerArithmeticType<>, BigInteger>.Parse("12*X + 2");
			var second = Polynomial<TAlgebra, TNumber>.Parse("12*X - 3");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.Add(first, second);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<TAlgebra, TNumber> second = Polynomial<TAlgebra, TNumber>.Parse("2*X - 2");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.Subtract(first, second);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("12*X + 2");
			IPolynomial<TAlgebra, TNumber> second = Polynomial<TAlgebra, TNumber>.Parse("12*X - 3");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.Multiply(first, second);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<TAlgebra, TNumber> second = Polynomial<TAlgebra, TNumber>.Parse("12*X + 2");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.Divide(first, second);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("12*X + 1");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.Square(first);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			IPolynomial<TAlgebra, TNumber> second = Polynomial<TAlgebra, TNumber>.Parse("X^3 + 6*X^2 + 11*X + 6");

			//IPolynomial result = Polynomial.Multiply(mult, Polynomial.Parse("X + 1"));
			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.GCD(first, second);

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

			IPolynomial<TAlgebra, TNumber> first = Polynomial<TAlgebra, TNumber>.Parse("288*X^2 + 36*X - 2");

			IPolynomial<TAlgebra, TNumber> result = Polynomial<TAlgebra, TNumber>.GetDerivativePolynomial(first);

			TestContext.WriteLine($"f' where f(x) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
