using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolynomialLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPolynomial
{
	[TestClass]
	public class Field
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestMod()
		{
			string expecting = "2*X - 2";

			IPolynomial first = Polynomial.Parse("3*x^2 + 2*x + 1");
			IPolynomial second = Polynomial.Parse("x^2 + 1");

			IPolynomial result = Polynomial.Field.Modulus(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}
	}
}
