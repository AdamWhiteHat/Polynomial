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
			string expecting1 = "2*X^3 + X^2 + 11*X - 55";
			string expecting2 = "2*X^3 - 15*X^2 - 2*X + 8";
			string expecting3 = "X^4 - 1113*X^3 - 1071664697503898457630532*X^2 + 1864690457315613267626353*X + 648130055243730514233550";

			BigInteger N1 = BigInteger.Parse("3218147");
			BigInteger N2 = BigInteger.Parse("45113");
			BigInteger N3 = BigInteger.Parse("1522605027922533360535618378132637429718068114961380688657908494580122963258952897654000350692006139");

			BigInteger m1 = BigInteger.Parse("117");
			BigInteger m2 = BigInteger.Parse("31");
			BigInteger m3 = BigInteger.Parse("6246644847868435165459221");

			Polynomial f1 = new Polynomial(N1, m1);
			Polynomial f2 = new Polynomial(N2, m2);
			Polynomial f3 = new Polynomial(N3, m3);

			string before1 = f1.ToString();
			string before2 = f2.ToString();
			string before3 = f3.ToString();

			TestContext.WriteLine($"Before:");
			TestContext.WriteLine($"#1: {before1}");
			TestContext.WriteLine($"#2: {before2}");
			TestContext.WriteLine($"#3: {before3}");

			var fSm1 = Polynomial.MakeCoefficientsSmaller(f1, m1);
			var fSm2 = Polynomial.MakeCoefficientsSmaller(f2, m2);
			var fSm3 = Polynomial.MakeCoefficientsSmaller(f3, m3);

			string result1 = fSm1.ToString();
			string result2 = fSm2.ToString();
			string result3 = fSm3.ToString();

			TestContext.WriteLine($"After:");
			TestContext.WriteLine($"{result1}");
			TestContext.WriteLine($"{result2}");
			TestContext.WriteLine($"{result3}");

			Assert.AreEqual(expecting1, result1);
			Assert.AreEqual(expecting2, result2);
			Assert.AreEqual(expecting3, result3);
		}

		[TestMethod]
		public void TestReciprocalPolynomial()
		{
			string start = "5*X^4 + 4*X^3 + 3*X^2 + 2*X + 1";
			string expecting = "X^4 + 2*X^3 + 3*X^2 + 4*X + 5";

			Polynomial poly = Polynomial.Parse(start);
			
			Polynomial result = Polynomial.GetReciprocalPolynomial(poly);

			string actual = result.ToString();

			TestContext.WriteLine($"({start})^-1 = {actual}");
			Assert.AreEqual(expecting, actual);
		}
	}
}
