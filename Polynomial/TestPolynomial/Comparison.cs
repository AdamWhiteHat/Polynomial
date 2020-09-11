using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Comparison<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		private static T Three = GenericArithmetic<T>.Parse("3");
		private static T Four = GenericArithmetic<T>.Parse("4");
		private static T Six = GenericArithmetic<T>.Parse("6");
		private static T Seven = GenericArithmetic<T>.Parse("7");
		private static T Eight = GenericArithmetic<T>.Parse("8");

		[TestMethod]
		public void TestEqual()
		{
			bool expected1 = true;
			bool expected2 = false;

			bool actual1 = GenericArithmetic<T>.Equal(Seven, Seven);
			bool actual2 = GenericArithmetic<T>.Equal(Three, Four);

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
		}

		[TestMethod]
		public void TestNotEqual()
		{
			bool expected1 = true;
			bool expected2 = false;

			bool actual1 = GenericArithmetic<T>.NotEqual(Six, Seven);
			bool actual2 = GenericArithmetic<T>.NotEqual(Seven, Seven);

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
		}

		[TestMethod]
		public void TestGreaterThan()
		{
			bool expected1 = true;
			bool expected2 = false;

			bool actual1 = GenericArithmetic<T>.GreaterThan(Seven, Six);
			bool actual2 = GenericArithmetic<T>.GreaterThan(Seven, Eight);

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
		}

		[TestMethod]
		public void TestLessThan()
		{
			bool expected1 = true;
			bool expected2 = false;

			bool actual1 = GenericArithmetic<T>.LessThan(Seven, Eight);
			bool actual2 = GenericArithmetic<T>.LessThan(Seven, Six);

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
		}
	}
}
