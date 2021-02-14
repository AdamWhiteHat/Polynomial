using System;
using System.Linq;
using System.Numerics;
using ExtendedArithmetic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Arithmetic
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestConstructorAndEvaluation()
		{
			string expecting = "3218147";
			BigInteger value = BigInteger.Parse(expecting);

			BigInteger base1 = 117;
			BigInteger base2 = 2;
			Polynomial poly1 = new Polynomial(value, base1);
			Polynomial poly2 = new Polynomial(value, base2);
			BigInteger eval1 = poly1.Evaluate(base1);
			BigInteger eval2 = poly2.Evaluate(base2);

			string result1 = eval1.ToString();
			string result2 = eval2.ToString();

			TestContext.WriteLine($"Evaluate({poly1}) where x = {base1}");
			TestContext.WriteLine($"Evaluate({poly2}) where x = {base2}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"First result   = {result1}");
			TestContext.WriteLine($"Expecting: {expecting}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Second result  = {result2}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result1);
			Assert.AreEqual(expecting, result2);
		}

		[TestMethod]
		public void TestAddition()
		{
			string expecting = "24*X - 1";

			Polynomial first = Polynomial.Parse("12*X + 2");
			Polynomial second = Polynomial.Parse("12*X - 3");

			Polynomial result = Polynomial.Add(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestSubtraction()
		{
			string expecting = "7*X^2 + X";

			Polynomial first = Polynomial.Parse("7*X^2 + 3*X - 2");
			Polynomial second = Polynomial.Parse("2*X - 2");

			Polynomial result = Polynomial.Subtract(first, second);

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestMultiply()
		{
			string expecting = "144*X^2 - 12*X - 6";

			Polynomial first = Polynomial.Parse("12*X + 2");
			Polynomial second = Polynomial.Parse("12*X - 3");

			Polynomial result = Polynomial.Multiply(first, second);

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide1()
		{
			string expecting = "24*X - 1";

			Polynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");
			Polynomial second = Polynomial.Parse("12*X + 2");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide2()
		{
			string expecting = "2*X - 2";

			Polynomial first = Polynomial.Parse("6*X^2 - 6");
			Polynomial second = Polynomial.Parse("3*X + 3");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide3()
		{
			string expecting = "6";

			Polynomial first = Polynomial.Parse("6*X^2 - 6");
			Polynomial second = Polynomial.Parse("X^2 - 1");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide4()
		{
			string expecting = "6*X - 1";

			Polynomial first = Polynomial.Parse("36*X^2 - 1");
			Polynomial second = Polynomial.Parse("6*X + 1");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide5()
		{
			string expecting = "6*X + 1";

			Polynomial first = Polynomial.Parse("36*X^2 - 1");
			Polynomial second = Polynomial.Parse("6*X - 1");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestDivide6()
		{
			string expecting = "144*X^2 + 18*X - 1";

			Polynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");
			Polynomial second = Polynomial.Parse("2");

			Polynomial result = Polynomial.Divide(first, second);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestFactor1()
		{
			Polynomial toFactor = Polynomial.Parse("X^4 - 19*X^3 + 29*X^2 + 571*X + 858");

			Polynomial first = Polynomial.Parse("X - 11");
			Polynomial second = Polynomial.Parse("X - 13");
			Polynomial third = Polynomial.Parse("X + 2");
			Polynomial fourth = Polynomial.Parse("X + 3");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})({second})({third})({fourth})");

			Assert.IsTrue(results.Contains(first), first.ToString());
			Assert.IsTrue(results.Contains(second), second.ToString());
			Assert.IsTrue(results.Contains(third), third.ToString());
			Assert.IsTrue(results.Contains(fourth), fourth.ToString());
		}

		[TestMethod]
		public void TestFactor2()
		{
			Polynomial toFactor = Polynomial.Parse("2*X^4 + X^3 - 19*X^2 - 9*X + 9");

			Polynomial first = Polynomial.Parse("2*X - 1");
			Polynomial second = Polynomial.Parse("X - 3");
			Polynomial third = Polynomial.Parse("X + 1");
			Polynomial fourth = Polynomial.Parse("X + 3");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})({second})({third})({fourth})");

			Assert.IsTrue(results.Contains(first), first.ToString());
			Assert.IsTrue(results.Contains(second), second.ToString());
			Assert.IsTrue(results.Contains(third), third.ToString());
			Assert.IsTrue(results.Contains(fourth), fourth.ToString());
		}

		[TestMethod]
		public void TestFactor3()
		{
			Polynomial toFactor = Polynomial.Parse("2*X^3 + 7*X^2 + 5*X + 1");

			Polynomial first = Polynomial.Parse("2*X + 1");
			Polynomial second = Polynomial.Parse("X^2 + 3*X + 1");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})({second})");

			Assert.IsTrue(results.Contains(first), first.ToString());
			Assert.IsTrue(results.Contains(second), second.ToString());
		}

		[TestMethod]
		public void TestFactor4()
		{
			Polynomial toFactor = Polynomial.Parse("84*X^3 + 36*X^2 + 12*X + 24");

			Polynomial first = Polynomial.Parse("12");
			Polynomial second = Polynomial.Parse("7*X^3 + 3*X^2 + X + 2");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})({second})");

			Assert.IsTrue(results.Contains(first), first.ToString());
			Assert.IsTrue(results.Contains(second), second.ToString());
		}

		[TestMethod]
		public void TestFactor5()
		{
			Polynomial toFactor = Polynomial.Parse("X^2 - 5*X - 6");

			Polynomial first = Polynomial.Parse("X + 1");
			Polynomial second = Polynomial.Parse("X - 6");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})({second})");

			Assert.IsTrue(results.Contains(first), first.ToString());
			Assert.IsTrue(results.Contains(second), second.ToString());
		}

		[TestMethod]
		public void TestFactorIrreducible()
		{
			Polynomial toFactor = Polynomial.Parse("7*X^3 + 3*X^2 + X + 2");
			Polynomial first = Polynomial.Parse("7*X^3 + 3*X^2 + X + 2");

			List<Polynomial> results = Polynomial.Factor(toFactor);

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = ({string.Join(")(", results.Select(p => p.ToString()))})");
			TestContext.WriteLine($"Expecting: ({first})");

			Assert.IsTrue(results.Contains(first), first.ToString());
		}

		[TestMethod]
		public void TestDivisionMutation()
		{
			BigInteger N = 1811 * 1777;
			BigInteger m = 117;

			Polynomial two = new Polynomial(new Term[] { new Term(2, 0) });
			Polynomial three = new Polynomial(new Term[] { new Term(3, 0) });
			Polynomial seventeen = new Polynomial(new Term[] { new Term(17, 0) });

			Polynomial f = new Polynomial(N, m);

			Polynomial r2 = Polynomial.Divide(f, two);
			Polynomial r3 = Polynomial.Divide(f, three);
			Polynomial r17 = Polynomial.Divide(f, seventeen);

			string e2 = "X^3 + 5*X + 31";
			string e3 = "3*X + 20";
			string e17 = "3";

			TestContext.WriteLine($"Expecting: {e2} ; Result: {r2}");
			TestContext.WriteLine($"Expecting: {e3} ; Result: {r3}");
			TestContext.WriteLine($"Expecting: {e17} ; Result: {r17}");

			Assert.AreEqual(e2, r2.ToString());
			Assert.AreEqual(e3, r3.ToString());
			Assert.AreEqual(e17, r17.ToString());
		}

		[TestMethod]
		public void TestSquare()
		{
			string expecting = "144*X^2 + 24*X + 1";

			Polynomial first = Polynomial.Parse("12*X + 1");

			Polynomial result = Polynomial.Square(first);

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestGCD1()
		{
			string firstPolyString = "84*X^3 + 36*X^2 + 12*X + 24";
			string secondPolyString = "7*X^3 + 3*X^2 + X + 2";

			string expecting = secondPolyString;

			Polynomial first = Polynomial.Parse(firstPolyString);
			Polynomial second = Polynomial.Parse(secondPolyString);

			Polynomial result = Polynomial.GCD(first, second);
			string actual = result.ToString();

			TestContext.WriteLine($"GCD({first} , {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual);
		}

		[TestMethod]
		public void TestGCD2()
		{
			// GCD(X^2 + 7*X + 6, X^2 - 5*X - 6) = X + 1
			string polyString1 = "X^2 + 7*X + 6";
			string polyString2 = "X^2 - 5*X - 6";
			string expecting = "X + 1";

			Polynomial first = Polynomial.Parse(polyString1);
			Polynomial second = Polynomial.Parse(polyString2);

			Polynomial result = Polynomial.GCD(first, second);
			string actual = result.ToString();

			TestContext.WriteLine($"GCD({first}, {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual);
		}

		[TestMethod]
		public void TestDerivative()
		{
			string expecting = "576*X + 36";

			Polynomial first = Polynomial.Parse("288*X^2 + 36*X - 2");

			Polynomial result = Polynomial.GetDerivativePolynomial(first);

			TestContext.WriteLine($"f' where f(X) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}


		[TestMethod]
		public void TestFunctionalComposition()
		{
			BigInteger N = 3218147;
			BigInteger pBase = 117;
			Polynomial polyn = new Polynomial(N, pBase);
			Polynomial.MakeCoefficientsSmaller(polyn, pBase);

			Polynomial indeterminate = Polynomial.Parse("6*X + 1");
			Polynomial constant6 = Polynomial.Parse("6");
			Polynomial constant117 = Polynomial.Parse("117");
			Polynomial minusOne = Polynomial.Subtract(Polynomial.Zero, Polynomial.One);
			Polynomial inversePolyn = Polynomial.Multiply(polyn, minusOne);

			Polynomial composition1 = polyn.FunctionalComposition(indeterminate);
			Polynomial composition2 = constant6.FunctionalComposition(constant117);
			Polynomial composition3 = polyn.FunctionalComposition(Polynomial.Zero);
			Polynomial composition4 = polyn.FunctionalComposition(minusOne);
			Polynomial composition5 = polyn.FunctionalComposition(inversePolyn);

			string expecting1 = "432*X^3 + 252*X^2 + 108*X + 75";
			string expecting2 = "6";
			string expecting3 = "62";
			string expecting4 = "51";
			string expecting5 = "-16*X^9 - 24*X^8 - 252*X^7 - 1726*X^6 - 2744*X^5 - 15811*X^4 - 55320*X^3 - 60050*X^2 - 229500*X - 473370";

			string actual1 = composition1.ToString();
			string actual2 = composition2.ToString();
			string actual3 = composition3.ToString();
			string actual4 = composition4.ToString();
			string actual5 = composition5.ToString();

			Assert.AreEqual(expecting1, actual1);
			Assert.AreEqual(expecting2, actual2);
			Assert.AreEqual(expecting3, actual3);
			Assert.AreEqual(expecting4, actual4);
			Assert.AreEqual(expecting5, actual5);
		}
	}
}
