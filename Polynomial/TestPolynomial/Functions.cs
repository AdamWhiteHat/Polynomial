using System;
using System.Numerics;
using ExtendedArithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Functions
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestMakeCoefficientsSmaller()
		{
			string expecting1 = "6246644847868435165458107*X^3 + 5174980150364536707828689*X^2 + 1864690457315613267626353*X + 648130055243730514233550";
			string expecting2 = "X^4 - 1113*X^3 - 1071664697503898457630532*X^2 + 1864690457315613267626353*X + 648130055243730514233550";

			BigInteger N = BigInteger.Parse("1522605027922533360535618378132637429718068114961380688657908494580122963258952897654000350692006139");
			BigInteger m = BigInteger.Parse("6246644847868435165459221");

			IPolynomial f = new Polynomial(N, m);

			string result1 = f.ToString();

			TestContext.WriteLine($"Before:");
			TestContext.WriteLine($"{result1}");

			Polynomial.MakeCoefficientsSmaller(f, m);

			string result2 = f.ToString();

			TestContext.WriteLine($"After:");
			TestContext.WriteLine($"{result2}");

			Assert.AreEqual(expecting1, result1);
			Assert.AreEqual(expecting2, result2);
		}
	}
}
