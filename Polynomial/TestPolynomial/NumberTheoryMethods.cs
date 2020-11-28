using System;
using System.Linq;
using System.Numerics;
using PolynomialLibrary;
using NUnit.Framework;

namespace TestPolynomial
{
    [TestFixture(Category = "Number Theory Methods")]
    public class NumberTheoryMethods<T>
    {
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        private TestContext testContextInstance;

        [Test(Description = "EulersCriterionMethod")]
        [TestCase("17", "3", "-1")]
        [TestCase("2", "17", "1")]
        public virtual void EulersCriterion(string a, string p, string expected)
        {
            BigInteger arg = BigInteger.Parse(a);
            BigInteger mod = BigInteger.Parse(p);

            BigInteger result = Polynomial<T>.Algorithms.EulersCriterion(arg, mod);
            string actual = result.ToString();

            TestContext.WriteLine($"{arg}^(({p}-1)/2)  ≡ {result} (mod {p})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "LegendreSymbol")]
        [TestCase("9", "11", "1")]
        [TestCase("11", "5", "1")]
        [TestCase("17", "23", "-1")]
        [TestCase("29", "29", "0")]
        public virtual void LegendreSymbol(string a, string p, string expected)
        {
            BigInteger arg = BigInteger.Parse(a);
            BigInteger mod = BigInteger.Parse(p);

            BigInteger result = Polynomial<T>.Algorithms.LegendreSymbol(arg, mod);
            string actual = result.ToString();

            TestContext.WriteLine($"({arg} | {mod}) == {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        //[Test(Description = "TestLegendreSymbolSearch(")]
        [TestCase("1", "-1", "71", "7")]
        [TestCase("11", "1", "41", "16")]
        public virtual void LegendreSymbolSearch(string start, string goal, string modulus, string expected)
        {
            BigInteger strt = BigInteger.Parse(start);
            BigInteger gol = BigInteger.Parse(goal);
            BigInteger mod = BigInteger.Parse(modulus);

            BigInteger result = Polynomial<T>.Algorithms.LegendreSymbolSearch(strt, mod, gol);
            string actual = result.ToString();

            TestContext.WriteLine($"Search for LegendreSymbol == {goal} with mod {mod}: {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "TestTonelliShanks")]
        [TestCase("29", "53", "20")]
        [TestCase("56", "101", "37")]
        [TestCase("1030", "10009", "1632")]
        public virtual void TonelliShanks(string n, string p, string expected)
        {
            BigInteger target = BigInteger.Parse(n);
            BigInteger prime = BigInteger.Parse(p);

            BigInteger result = Polynomial<T>.Algorithms.TonelliShanks(target, prime);
            string actual = result.ToString();

            TestContext.WriteLine($"{target} ≡ {result}^2 (mod {prime})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        /*
		[Test(Description ="TestChineseRemainderTheorem")]
		[TestCase("105")]	
		public virtual  void ChineseRemainderTheorem(string expected, BigInteger[] nCollection, BigInteger[] aCollection)
		{
			BigInteger result = Polynomial<T>.Algorithms.ChineseRemainderTheorem(nCollection, aCollection);
			string actual = result.ToString();

			TestContext.WriteLine($"ChineseRemainderTheorem => N ≡ a[i] (mod {result})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}
		*/

        [Test(Description = "TestModularMultiplicativeInverse")]
        [TestCase("3", "11", "4")]
        [TestCase("10", "17", "12")]
        public virtual void ModularMultiplicativeInverse(string a, string m, string expected)
        {
            BigInteger argument = BigInteger.Parse(a);
            BigInteger mod = BigInteger.Parse(m);

            BigInteger result = Polynomial<T>.Algorithms.ModularMultiplicativeInverse(argument, mod);
            string actual = result.ToString();

            TestContext.WriteLine($"{argument}*{result} ≡ 1 (mod {mod})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "TestEulersTotientPhi001")]
        [TestCase("41", "40")]
        [TestCase("42", "12")]
        [TestCase("360", "96")]
        public virtual void EulersTotientPhi(string n, string expected)
        {
            int argument = int.Parse(n);

            BigInteger result = Polynomial<T>.Algorithms.EulersTotientPhi(argument);
            string actual = result.ToString();

            TestContext.WriteLine($"Phi({argument}) == {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

    }

}

