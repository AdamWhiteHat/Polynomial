using System;
using System.Linq;
using System.Numerics;
using PolynomialLibrary;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class FieldTests
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestMod()
		{
			string expecting = "2*X - 2";

			var first = Polynomial<BigIntegerArithmeticType, BigInteger>.Parse("3*x^2 + 2*x + 1");
			var second = Polynomial<BigIntegerArithmeticType, BigInteger>.Parse("x^2 + 1");

			var result = Polynomial<BigIntegerArithmeticType, BigInteger>.Field.Modulus(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
