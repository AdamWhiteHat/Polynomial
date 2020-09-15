using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class Field<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public virtual void TestModulusPolynomial()
		{
			string expecting = "-24*X^2 - 5*X + 17";

			T from = GenericArithmetic<T>.Parse("3218147");
			IPolynomial<T> poly = new Polynomial<T>(from, GenericArithmetic<T>.Parse("30"));
			IPolynomial<T> modPoly = new Polynomial<T>(GenericArithmetic<T>.Parse("14"), GenericArithmetic<T>.Parse("2"));

			IPolynomial<T> actual = Polynomial<T>.Field<T>.Modulus(poly, modPoly);

			TestContext.WriteLine($"{poly} Mod({modPoly})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual.ToString());
		}

		[TestMethod]
		public virtual void TestModulusInteger()
		{
			string expecting = "X^4 + X^3 + X^2 + X + 1";

			T from = GenericArithmetic<T>.Parse("3218147");
			IPolynomial<T> poly = new Polynomial<T>(from, GenericArithmetic<T>.Parse("30"));
			T mod = GenericArithmetic<T>.Parse("2");

			IPolynomial<T> actual = Polynomial<T>.Field<T>.Modulus(poly, mod);

			TestContext.WriteLine($"{poly} Mod({mod})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual.ToString());
		}


		[TestMethod]
		public virtual void TestModulusPolynomialModulusInteger()
		{
			string expecting = "X + 1";

			T from = GenericArithmetic<T>.Parse("3218147");
			IPolynomial<T> poly = new Polynomial<T>(from, GenericArithmetic<T>.Parse("30"));
			IPolynomial<T> modPoly = new Polynomial<T>(GenericArithmetic<T>.Parse("14"), GenericArithmetic<T>.Parse("2"));
			T mod = GenericArithmetic<T>.Parse("2");

			IPolynomial<T> actual = Polynomial<T>.Field<T>.ModMod(poly, modPoly, mod);

			TestContext.WriteLine($"{poly} Mod({modPoly},{mod})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual.ToString());
		}

		[TestMethod]
		public virtual void TestMultiply()
		{
			string expecting = "X + 2";

			IPolynomial<T> first = Polynomial<T>.Parse("3*X^2 + 2*X + 1");
			T multiplier = GenericArithmetic<T>.Parse("20");
			T mod = GenericArithmetic<T>.Parse("3");

			IPolynomial<T> actual = Polynomial<T>.Field<T>.Multiply(first, multiplier, mod);

			TestContext.WriteLine($"({first}) * ({multiplier})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, actual.ToString());
		}

		[TestMethod]
		public virtual void TestDivide()
		{
			string expecting = "X^2 + 1";

			IPolynomial<T> first = Polynomial<T>.Parse("3*X^4 + 2*X^3 + 4*X^2 + 2*X + 1");
			IPolynomial<T> second = Polynomial<T>.Parse("X^2 + 1");
			IPolynomial<T> remainder;
			T mod = GenericArithmetic<T>.Parse("2");

			IPolynomial<T> quotient = Polynomial<T>.Field<T>.Divide(first, second, mod, out remainder);

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = Quotient: {quotient} Remainder: {remainder}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, quotient.ToString());
		}

		[TestMethod]
		public virtual void TestExponentiateMod()
		{
			string expecting = "X + 1";

			IPolynomial<T> first = Polynomial<T>.Parse("3*X^2 + 2*X + 1");
			T two = GenericArithmetic<T>.Parse("2");
			IPolynomial<T> modPoly = new Polynomial<T>(GenericArithmetic<T>.Parse("14"), two);
			T mod = two;

			IPolynomial<T> actual = Polynomial<T>.Field<T>.ExponentiateMod(first, two, modPoly, mod);

			TestContext.WriteLine($"({first})^{two}) Mod({modPoly}, {mod})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {actual}");
			TestContext.WriteLine($"Expecting: {expecting}");


			Assert.AreEqual(expecting, actual.ToString());
		}
	}
}
