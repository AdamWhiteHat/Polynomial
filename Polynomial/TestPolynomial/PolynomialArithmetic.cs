using PolynomialLibrary;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "PolynomialArithmetic")]
	public class PolynomialArithmetic<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[Test]
		[TestCase("5*X^3")]
		public virtual void BaseMExpansionConstructor(string expected)
		{
			T n = GenericArithmetic<T>.Parse("24565");
			T m = GenericArithmetic<T>.Parse("17");

			Polynomial<T> poly = new Polynomial<T>(n, m, 3);

			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("X^3 + 3*X^2 + 3*X - 1")]
		public virtual void MakeCoefficientsSmaller(string expected)
		{
			T n = GenericArithmetic<T>.Parse("32766");
			T polyBase = GenericArithmetic<T>.Parse("31");

			Polynomial<T> poly = new Polynomial<T>(n, polyBase, 3);
			Polynomial<T>.MakeCoefficientsSmaller(poly, polyBase);

			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("144*X^2 - 12*X - 6")]
		public virtual void Parse(string expected)
		{
			Polynomial<T> poly = Polynomial<T>.Parse(expected);
			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(new Polynomial<T>().ToString(), Polynomial<T>.Zero.ToString());
			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("12*X + 2", "12*X - 3", "24*X - 1")]
		public virtual void Addition(string augend, string addend, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(augend);
			Polynomial<T> second = Polynomial<T>.Parse(addend);

			Polynomial<T> sum = Polynomial<T>.Add(first, second);
			string actual = sum.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("7*X^2 + 3*X - 2", "2*X - 2", "7*X^2 + X")]
		public virtual void Subtraction(string minuend, string subtrahend, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(minuend);
			Polynomial<T> second = Polynomial<T>.Parse(subtrahend);

			Polynomial<T> difference = Polynomial<T>.Subtract(first, second);
			string actual = difference.ToString();

			TestContext.WriteLine($"({first}) - ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("12*X + 2", "12*X - 3", "144*X^2 - 12*X - 6")]
		public virtual void Multiply(string multiplicand, string multiplier, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(multiplicand);
			Polynomial<T> second = Polynomial<T>.Parse(multiplier);

			Polynomial<T> product = Polynomial<T>.Multiply(first, second);
			string actual = product.ToString();

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("288*X^2 + 36*X - 2", "12*X + 2", "24*X - 1")]
		public virtual void Divide(string dividend, string divisor, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(dividend);
			Polynomial<T> second = Polynomial<T>.Parse(divisor);

			Polynomial<T> quotient = Polynomial<T>.Divide(first, second);
			string actual = quotient.ToString();

			TestContext.WriteLine($"({first}) / ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("X^2 + 1", "2", "X^4 + 2*X^2 + 1")]
		[TestCase("X^8 + 4", "2", "X^16 + 8*X^8 + 16")]
		[TestCase("X^9 - X^8", "3", "X^27 - 3*X^26 + 3*X^25 - X^24")]
		public virtual void Pow(string root, string exponent, string expected)
		{
			Polynomial<T> poly = Polynomial<T>.Parse(root);
			T exp = GenericArithmetic<T>.Parse(exponent);

			Polynomial<T> result = Polynomial<T>.Pow(poly, exp);
			string actual = result.ToString();

			TestContext.WriteLine($"({poly})^{exp}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("3*X^2 + 2*X + 1", "X^2 + 1", "2*X - 2")]
		public virtual void Mod(string dividend, string modulus, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(dividend);
			Polynomial<T> second = Polynomial<T>.Parse(modulus);

			Polynomial<T> residue = Polynomial<T>.Field<T>.Modulus(first, second);
			string actual = residue.ToString();

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("12*X + 1", "144*X^2 + 24*X + 1")]
		public virtual void Square(string root, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(root);

			Polynomial<T> square = Polynomial<T>.Square(first);
			string actual = square.ToString();

			TestContext.WriteLine($"({first})^2");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("288*X^2 + 36*X - 2", "576*X + 36")]
		public virtual void Derivative(string polynomial, string expected)
		{
			Polynomial<T> first = Polynomial<T>.Parse(polynomial);

			Polynomial<T> derivative = Polynomial<T>.GetDerivativePolynomial(first);
			string actual = derivative.ToString();

			TestContext.WriteLine($"f' where f(X) = ({first})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("X^3 - 15*X^2 + 71*X - 105", "3", "5", "7")]
		public virtual void FromRoots(string expected, params object[] roots)
		{
			List<T> theRoots = new List<T>();
			foreach (object obj in roots)
			{
				T root = GenericArithmetic<T>.Parse(obj.ToString());
				theRoots.Add(root);
			}

			Polynomial<T> poly = Polynomial<T>.FromRoots(theRoots.ToArray());
			string actual = poly.ToString();

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("X^3 - 15*X^2 + 71*X - 105", "3", "0")]
		[TestCase("X^2 - 4*X + 13", "2", "9")]
		[TestCase("X^2 + 1", "0", "1")]
		public virtual void Evaluate(string polynomial, string indeterminateValue, string expected)
		{
			Polynomial<T> poly = Polynomial<T>.Parse(polynomial);

			T indeterminate = GenericArithmetic<T>.Parse(indeterminateValue);
			T result = poly.Evaluate(indeterminate);
			string actual = GenericArithmetic<T>.ToString(result);

			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("2*X^3 + X^2 + 10*X + 62", "117", "X^3 + 118*X^2 + 10*X + 62")]
		[TestCase("5*X^2 + 19*X + 43", "79", "X^2 + 335*X + 43")]
		public virtual void MakeMonic(string polynomial, string polynomialBase, string expected)
		{
			Polynomial<T> poly = Polynomial<T>.Parse(polynomial);

			T polyBase = GenericArithmetic<T>.Parse(polynomialBase);

			Polynomial<T> result = Polynomial<T>.MakeMonic(poly, polyBase);
			string actual = result.ToString();

			TestContext.WriteLine($"MakeMonic: {poly} Base: {polyBase}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		/*
		[Test]
		[TestCase("X^2 + 3*X + 2")]		
		public virtual void GCD(string expected)
		{
			throw new NotImplementedException();

			Polynomial<T> first = Polynomial<T>.Parse("X^4 + 8*X^3 + 21*X^2 + 22*X + 8");
			Polynomial<T> second = Polynomial<T>.Parse("X^3 + 6*X^2 + 11*X + 6");

			Polynomial<T> gcd = Polynomial<T>.GCD(first, second);
			string actual = gcd.ToString();

			TestContext.WriteLine($"GCD({first} , {second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}
		*/
	}
}
