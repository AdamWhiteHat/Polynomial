using System;
using ExtendedArithmetic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Numerics;

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

			Polynomial first = Polynomial.Parse("3*X^2 + 2*X + 1");
			Polynomial second = Polynomial.Parse("X^2 + 1");

			Polynomial result = Polynomial.Field.Modulus(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestModMod()
		{
			BigInteger prime = 9923;
			Polynomial g = Polynomial.Parse($"X^{prime} - X");
			Polynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");
			Polynomial expected = Polynomial.Parse("7726*X^2 + 1477*X + 7301");

			Stopwatch timer = Stopwatch.StartNew();
			Polynomial actual = Polynomial.Field.ModMod(g, f, prime);
			long timing = timer.ElapsedMilliseconds;

			Assert.AreEqual(expected, actual, $"{g} ≡ {expected} (mod {f}, {prime})");

			Assert.IsTrue(timing < 1000, "Stopwatch.ElapsedMilliseconds");
		}

		[TestMethod]
		public void TestModMod_LeadingCoefficientNotMonic()
		{
			BigInteger prime = 9923;
			Polynomial g = Polynomial.Parse($"X^{prime} - X");
			Polynomial f = Polynomial.Parse("11*X^3 + 15*X^2 + 29*X + 8");
			Polynomial expected = Polynomial.Parse("6128*X^2 + 8051*X + 1146");

			Stopwatch timer = Stopwatch.StartNew();
			Polynomial actual = Polynomial.Field.ModMod(g, f, prime);
			long timing = timer.ElapsedMilliseconds;

			Assert.AreEqual(expected, actual, $"{g} ≡ {expected} (mod {f}, {prime})");

			Assert.IsTrue(timing < 1000, "Stopwatch.ElapsedMilliseconds");
		}

		[TestMethod]
		public void TestGCD001()
		{
			BigInteger prime = 9929;
			Polynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");
			Polynomial h = Polynomial.Parse("7449*X^2 + 4697*X + 5984");
			Polynomial expected = Polynomial.Parse("1");

			Stopwatch timer = Stopwatch.StartNew();
			Polynomial actual = Polynomial.Field.GCD(h, f, prime);
			long timing = timer.ElapsedMilliseconds;

			Assert.AreEqual(expected, actual, $"gcd({f}, {h}) ≡ {expected} (mod {prime})");

			Assert.IsTrue(timing < 1000, "Stopwatch.ElapsedMilliseconds");
		}

		[TestMethod]
		public void TestGCD002()
		{
			BigInteger prime = 9923;
			Polynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");
			Polynomial h = Polynomial.Parse("7726*X^2 + 1477*X + 7301");
			Polynomial expected = Polynomial.Parse("X + 9076");

			Stopwatch timer = Stopwatch.StartNew();
			Polynomial actual = Polynomial.Field.GCD(h, f, prime);
			long timing = timer.ElapsedMilliseconds;

			Assert.AreEqual(expected, actual, $"gcd({f}, {h}) ≡ {expected} (mod {prime})");

			Assert.IsTrue(timing < 1000, "Stopwatch.ElapsedMilliseconds");
		}

		[TestMethod]
		public void TestModularInverse()
		{
			BigInteger prime = 9929;
			Polynomial h = Polynomial.Parse("7449*X^2 + 4697*X + 5984");
			Polynomial expected = Polynomial.Parse("2480*X^2 + 5232*X + 3945");
			Polynomial actual = Polynomial.Field.ModularInverse(h, prime);

			Assert.AreEqual(expected, actual, $"h' = {h}");
		}

		[TestMethod]
		public void TestIsIrreducibleMethods()
		{
			BigInteger prime = 9929;
			Polynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");
			bool expected = true;

			bool actual1 = Polynomial.Field.IsIrreducibleOverField(f, prime);
			bool actual2 = Polynomial.Field.IsIrreducibleOverP(f, prime);

			Assert.AreEqual(expected, actual1, $"Polynomial.Field.IsIrreducibleOverField({f}, {prime})");
			Assert.AreEqual(expected, actual2, $"Polynomial.Field.IsIrreducibleOverP({f}, {prime})");
		}

		[TestMethod]
		public void TestModPow()
		{
			BigInteger exponent = 2;
			Polynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");
			Polynomial a = Polynomial.Parse("11 + 7*X");

			Polynomial expected = Polynomial.Parse("49*X^2 + 154*X + 121");
			Polynomial actual = Polynomial.Field.ModPow(a, exponent, f);

			Assert.AreEqual(expected, actual, $"Polynomial.Field.ModPow({a}, {exponent}, {f})");
		}
	}
}
