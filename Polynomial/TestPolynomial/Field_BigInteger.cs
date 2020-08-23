using System;
using System.Linq;
using System.Numerics;
using PolynomialLibrary;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Field_BigInteger
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestMod()
		{
			string expecting = "2*X - 2";

			IPolynomial<BigInteger> first = Polynomial<BigInteger>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<BigInteger> second = Polynomial<BigInteger>.Parse("X^2 + 1");

			IPolynomial<BigInteger> result = Polynomial<BigInteger>.Field<BigInteger>.Modulus(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
