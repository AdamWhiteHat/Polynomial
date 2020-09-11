using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[DataTestMethod]
		[DataRow("5*X^3")]
		public virtual void TestBaseMExpansionConstructor(string expected)
		{
			T n = GenericArithmetic<T>.Parse("24565");
			T m = GenericArithmetic<T>.Parse("17");

			IPolynomial<T> poly = new Polynomial<T>(n, m, 3);

			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("X^3 + 3*X^2 + 3*X - 1")]
		public virtual void TestMakeCoefficientsSmaller(string expected)
		{
			T n = GenericArithmetic<T>.Parse("32766");
			T polyBase = GenericArithmetic<T>.Parse("31");

			IPolynomial<T> poly = new Polynomial<T>(n, polyBase, 3);
			Polynomial<T>.MakeCoefficientsSmaller(poly, polyBase);

			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("144*X^2 - 12*X - 6")]
		public virtual void TestParse(string expected)
		{
			IPolynomial<T> poly = Polynomial<T>.Parse(expected);
			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("24*X - 1")]
		public virtual void TestAddition(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("12*X + 2");
			IPolynomial<T> second = Polynomial<T>.Parse("12*X - 3");

			IPolynomial<T> sum = Polynomial<T>.Add(first, second);
			string actual = sum.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("7*X^2 + X")]
		public virtual void TestSubtraction(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("7*X^2 + 3*X - 2");
			IPolynomial<T> second = Polynomial<T>.Parse("2*X - 2");

			IPolynomial<T> difference = Polynomial<T>.Subtract(first, second);
			string actual = difference.ToString();

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[DataTestMethod]
		[DataRow("144*X^2 - 12*X - 6")]
		public virtual void TestMultiply(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("12*X + 2");
			IPolynomial<T> second = Polynomial<T>.Parse("12*X - 3");

			IPolynomial<T> product = Polynomial<T>.Multiply(first, second);
			string actual = product.ToString();

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("24*X - 1")]
		public virtual void TestDivide(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("288*X^2 + 36*X - 2");
			IPolynomial<T> second = Polynomial<T>.Parse("12*X + 2");

			IPolynomial<T> quotient = Polynomial<T>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("2*X - 2")]
		public virtual void TestMod(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("3*X^2 + 2*X + 1");
			IPolynomial<T> second = Polynomial<T>.Parse("X^2 + 1");

			IPolynomial<T> residue = Polynomial<T>.Field<T>.Modulus(first, second);
			string actual = residue.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("144*X^2 + 24*X + 1")]
		public virtual void TestSquare(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("12*X + 1");

			IPolynomial<T> square = Polynomial<T>.Square(first);
			string actual = square.ToString();

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("576*X + 36")]
		public virtual void TestDerivative(string expected)
		{
			IPolynomial<T> first = Polynomial<T>.Parse("288*X^2 + 36*X - 2");

			IPolynomial<T> derivative = Polynomial<T>.GetDerivativePolynomial(first);
			string actual = derivative.ToString();

			TestContext.WriteLine($"f' where f(X) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		/*
		[DataTestMethod]
		[DataRow("X^2 + 3*X + 2")]		
		public virtual void TestGCD(string expected)
		{
			throw new NotImplementedException();

			IPolynomial<T> first = Polynomial<T>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			IPolynomial<T> second = Polynomial<T>.Parse("X^3 + 6*X^2 + 11*X + 6");

			IPolynomial<T> gcd = Polynomial<T>.GCD(first, second);
			string actual = gcd.ToString();

			TestContext.WriteLine($"GCD({first} , {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}
		*/
	}
}
