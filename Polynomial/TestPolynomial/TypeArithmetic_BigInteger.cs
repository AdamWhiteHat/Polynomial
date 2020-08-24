using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class TypeArithmetic_BigInteger
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestAddition()
		{
			BigInteger first = 5;
			BigInteger second = 3;

			BigInteger expected = BigInteger.Add(first, second);
			BigInteger actual = GenericArithmetic<BigInteger>.Add(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSubtraction()
		{
			BigInteger first = 5;
			BigInteger second = 3;

			BigInteger expected = BigInteger.Subtract(first, second);
			BigInteger actual = GenericArithmetic<BigInteger>.Subtract(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestMultiplication()
		{
			BigInteger first = 5;
			BigInteger second = 3;

			BigInteger expected = BigInteger.Multiply(first, second);
			BigInteger actual = GenericArithmetic<BigInteger>.Multiply(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void TestDivision()
		{
			BigInteger first = 207;
			BigInteger second = 69;

			BigInteger expected = BigInteger.Divide(first, second);
			BigInteger actual = GenericArithmetic<BigInteger>.Divide(first, second);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSqrt()
		{
			BigInteger input = 25;

			BigInteger expected = (input).SquareRoot();
			BigInteger actual = GenericArithmetic<BigInteger>.SquareRoot(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLog()
		{
			BigInteger input = 16;
			double logBase = 2;

			double expected = BigInteger.Log(input, logBase);
			double actual = GenericArithmetic<BigInteger>.Log(input, logBase);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestLn()
		{
			BigInteger input = 2981;

			double expected = BigInteger.Log(input);
			double actual = GenericArithmetic<BigInteger>.Log(input, Math.E);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestPower()
		{
			BigInteger input = 5;
			int exp = (int)input;

			BigInteger expected = BigInteger.Pow(input, exp);
			BigInteger actual = GenericArithmetic<BigInteger>.Power(input, exp);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestNegate()
		{
			BigInteger input = 4;

			BigInteger expected = BigInteger.Negate(input);
			BigInteger actual = GenericArithmetic<BigInteger>.Negate(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestSign()
		{
			BigInteger input = -4;

			BigInteger expected = input.Sign;
			BigInteger actual = GenericArithmetic<BigInteger>.Sign(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestAbs()
		{
			BigInteger input = -4;

			BigInteger expected = BigInteger.Abs(input);
			BigInteger actual = GenericArithmetic<BigInteger>.Abs(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}
	}
}
