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

		[TestMethod]
		public void TestPow()
		{
			DecimalArithmeticType exp = new DecimalArithmeticType(5);
			DecimalArithmeticType mod = new DecimalArithmeticType(45113);
			var f = Polynomial<DecimalArithmeticType, Decimal>.Parse("X^3 + 15*X^2 + 29*X + 8");

			var fPrime = Polynomial<DecimalArithmeticType, Decimal>.GetDerivativePolynomial(f);

			var f3 = Polynomial<DecimalArithmeticType, Decimal>.Pow(f, 3);

			var field = Polynomial<DecimalArithmeticType, Decimal>.Field.Modulus(fPrime, mod);


			var result = Polynomial<DecimalArithmeticType, Decimal>.Field.ModPow(f, exp, fPrime);

			result = Polynomial<DecimalArithmeticType, Decimal>.Field.ModMod(result, fPrime, mod);

			TestContext.WriteLine("");
			TestContext.WriteLine("");
			TestContext.WriteLine($"f = {f}");
			TestContext.WriteLine($"g =  {mod}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"f^5 mod g = {result}");
			TestContext.WriteLine("Expecting =  34448*X + 38064");
			TestContext.WriteLine("");
			TestContext.WriteLine("");


		}

	}
}
