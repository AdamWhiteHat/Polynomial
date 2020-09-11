using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class TypeArithmetic<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			T first = GenericArithmetic<T>.Parse("5");
			T second = GenericArithmetic<T>.Parse("3");

			T expected = GenericArithmetic<T>.Parse("8");
			T actual = GenericArithmetic<T>.Add(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSubtraction()
		{
			T first = GenericArithmetic<T>.Parse("5");
			T second = GenericArithmetic<T>.Parse("3");

			T expected = GenericArithmetic<T>.Parse("2");
			T actual = GenericArithmetic<T>.Subtract(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestMultiplication()
		{
			T first = GenericArithmetic<T>.Parse("5");
			T second = GenericArithmetic<T>.Parse("3");

			T expected = GenericArithmetic<T>.Parse("15");
			T actual = GenericArithmetic<T>.Multiply(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestDivision()
		{
			T first = GenericArithmetic<T>.Parse("207");
			T second = GenericArithmetic<T>.Parse("69");

			T expected = GenericArithmetic<T>.Parse("3");
			T actual = GenericArithmetic<T>.Divide(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSqrt()
		{
			T input = GenericArithmetic<T>.Parse("25");

			T expected = GenericArithmetic<T>.Parse("5");
			T actual = GenericArithmetic<T>.SquareRoot(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLog()
		{
			T input = GenericArithmetic<T>.Parse("16");
			double logBase = 2;

			double expected = 4d;
			double actual = GenericArithmetic<T>.Log(input, logBase);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLn()
		{
			T input = GenericArithmetic<T>.Parse("2981");

			double expected = Math.Log(2981d);
			double actual = GenericArithmetic<T>.Log(input, Math.E);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestPower()
		{
			T input = GenericArithmetic<T>.Parse("5");
			int exp = 2;

			T expected = GenericArithmetic<T>.Parse("25");
			T actual = GenericArithmetic<T>.Power(input, exp);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestNegate()
		{
			T input = GenericArithmetic<T>.Parse("4");

			T expected = GenericArithmetic<T>.Parse("-4");
			T actual = GenericArithmetic<T>.Negate(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSign()
		{
			T input = GenericArithmetic<T>.Parse("-4");

			T expected = GenericArithmetic<T>.Parse("-1");
			T actual = GenericArithmetic<T>.Sign(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestAbs()
		{
			T input = GenericArithmetic<T>.Parse("-4");

			T expected = GenericArithmetic<T>.Parse("4");
			T actual = GenericArithmetic<T>.Abs(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}
	}
}
