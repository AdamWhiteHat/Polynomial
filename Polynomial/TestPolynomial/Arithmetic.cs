using System;
using System.Numerics;
using ExtendedArithmetic;
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
		public void TestDivide1()
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
		public void TestDivide2()
		{
			string expecting = "2*X - 2";

			IPolynomial first = Polynomial.Parse("6*X^2 - 6");
			IPolynomial second = Polynomial.Parse("3*X + 3");

			IPolynomial result = Polynomial.Divide(first, second);

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

			IPolynomial first = Polynomial.Parse("6*X^2 - 6");
			IPolynomial second = Polynomial.Parse("X^2 - 1");

			IPolynomial result = Polynomial.Divide(first, second);

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

			IPolynomial first = Polynomial.Parse("36*X^2 - 1");
			IPolynomial second = Polynomial.Parse("6*X + 1");

			IPolynomial result = Polynomial.Divide(first, second);

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

			IPolynomial first = Polynomial.Parse("36*X^2 - 1");
			IPolynomial second = Polynomial.Parse("6*X - 1");

			IPolynomial result = Polynomial.Divide(first, second);

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

			IPolynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");
			IPolynomial second = Polynomial.Parse("2");

			IPolynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivisionMutation()
		{
			BigInteger N = 1811 * 1777;
			BigInteger m = 117;

			IPolynomial two = new Polynomial(new ITerm[] { new Term(2, 0) });
			IPolynomial three = new Polynomial(new ITerm[] { new Term(3, 0) });
			IPolynomial seventeen = new Polynomial(new ITerm[] { new Term(17, 0) });

			IPolynomial f = new Polynomial(N, m);

			IPolynomial r2 = Polynomial.Divide(f, two);
			IPolynomial r3 = Polynomial.Divide(f, three);
			IPolynomial r17 = Polynomial.Divide(f, seventeen);

			string e2 = "X^3 + 5*X + 31";
			string e3 = "3*X + 20";
			string e17 = "3";

			TestContext.WriteLine($"Expecting: {e2} ; Result: {r2}");
			TestContext.WriteLine($"Expecting: {e3} ; Result: {r3}");
			TestContext.WriteLine($"Expecting: {e17} ; Result: {r17}");

			Assert.AreEqual(e2, r2.ToString());
			Assert.AreEqual(e3, r3.ToString());
			Assert.AreEqual(e17, r17.ToString());
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
			string expecting = "7*X^3 + 3*X^2 + X + 2";

			IPolynomial first = Polynomial.Parse("84*X^3 + 36*X^2 + 12*X + 24");
			IPolynomial second = Polynomial.Parse(expecting);

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
