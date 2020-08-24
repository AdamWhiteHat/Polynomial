using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class TypeArithmetic_Double
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			double first = 5d;
			double second = 3d;

			double expected = first + second;
			double actual = GenericArithmetic<double>.Add(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSubtraction()
		{
			double first = 5d;
			double second = 3d;

			double expected = first - second;
			double actual = GenericArithmetic<double>.Subtract(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestMultiplication()
		{
			double first = 5d;
			double second = 3d;

			double expected = first * second;
			double actual = GenericArithmetic<double>.Multiply(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void TestDivision()
		{
			double first = 69d;
			double second = 160d;

			double expected = first / second;
			double actual = GenericArithmetic<double>.Divide(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSqrt()
		{
			double input = 25d;

			double expected = Math.Sqrt(input);
			double actual = GenericArithmetic<double>.SquareRoot(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLog()
		{
			double input = 16d;
			double logBase = 2d;

			double expected = Math.Log(input, logBase);
			double actual = GenericArithmetic<double>.Log(input, logBase);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLn()
		{
			double input = 2981d;

			double expected = Math.Log(input);
			double actual = GenericArithmetic<double>.Log(input, Math.E);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestPower()
		{
			double input = 5d;

			double expected = Math.Pow(input, input);
			double actual = GenericArithmetic<double>.Power(input, input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestTruncate()
		{
			double input = 15d / 7d;

			double expected = Math.Truncate(input);
			double actual = GenericArithmetic<double>.Truncate(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestNegate()
		{
			double input = 4d;

			double expected = -(input);
			double actual = GenericArithmetic<double>.Negate(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSign()
		{
			double input = -4d;

			double expected = Math.Sign(input);
			double actual = GenericArithmetic<double>.Sign(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestAbs()
		{
			double input = -4d;

			double expected = Math.Abs(input);
			double actual = GenericArithmetic<double>.Abs(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}
	}
}
