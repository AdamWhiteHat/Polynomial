using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(Complex))]
	[TestFixture(Category = "PolynomialArithmetic - Complex")]
	public class PolynomialArithmetic_Complex : PolynomialArithmetic<Complex>
	{
		[Test]
		[TestCase("24565", "17", "(5, 0)*X^3")]
		public override void BaseMExpansionConstructor(string value, string polyBase, string expected)
		{
			base.BaseMExpansionConstructor(value, polyBase, expected);
		}

		[Test]
		[TestCase("32766", "31", "3", "(1.09986237454265, 0)*X^3 + (3.63797880709171E-12, 0)")]
		public override void MakeCoefficientsSmaller(string value, string polybase, string forceDegree, string expected)
		{
			base.MakeCoefficientsSmaller(value, polybase, forceDegree, expected);
		}

		[Test]
		[TestCase("(144, 0)*X^2 + (-12.5, -0.5)*X + (-3, -6.1)")]
		public override void Parse(string expected)
		{
			base.Parse(expected);
		}

		[Test]
		[TestCase("12*X + 2", "12*X - 3", "(24, 0)*X - (1, 0)")]
		public override void Addition(string augend, string addend, string expected)
		{
			base.Addition(augend, addend, expected);
		}

		[Test]
		public void Addition2()
		{
			string expected = "(-11.25, 1)*X^2 + (-1, -0.5)";

			Polynomial<Complex> first = Polynomial<Complex>.Parse("(-12.0, 0)*X^2 - (2, 0)");
			Polynomial<Complex> second = Polynomial<Complex>.Parse("(0.75, 1)*X^2 + (1.0, -0.5)");

			Polynomial<Complex> sum = Polynomial<Complex>.Add(first, second);
			string actual = sum.ToString();

			TestContext.WriteLine($"{sum}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("7*X^2 + 3*X - 2", "2*X - 2", "(7, 0)*X^2 + X")]
		public override void Subtraction(string minuend, string subtrahend, string expected)
		{
			base.Subtraction(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("12*X + 2", "12*X - 3", "(144, 0)*X^2 + (-12, 0)*X + (-6, 0)")]
		public override void Multiply(string multiplicand, string multiplier, string expected)
		{
			base.Multiply(multiplicand, multiplier, expected);
		}

		[Test]
		public void Multiply2()
		{
			string expected = "(9, 12)*X^4 + (10.5, -8)*X^2 + (-2, 1)";

			Polynomial<Complex> first = Polynomial<Complex>.Parse("(12.0, 0)*X^2 - (2, 0)");
			Polynomial<Complex> second = Polynomial<Complex>.Parse("(0.75, 1)*X^2 + (1.0, -0.5)");

			Polynomial<Complex> product = Polynomial<Complex>.Multiply(first, second);
			string actual = product.ToString();

			TestContext.WriteLine($"({first}) * ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected == actual}");


			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase("288*X^2 + 36*X - 2", "12*X + 2", "(24, 0)*X - (1, 0)")]
		public override void Divide(string dividend, string divisor, string expected)
		{
			base.Divide(dividend, divisor, expected);
		}

		[Test]
		[TestCase("X^2 + 1", "2", "X^4 + (2, 0)*X^2 + (1, 0)")]
		[TestCase("X^8 + 4", "2", "X^16 + (8, 0)*X^8 + (16, 0)")]
		[TestCase("X^9 - X^8", "3", "X^27 + (-3, 0)*X^26 + (3, 0)*X^25 - X^24")]
		public override void Pow(string polynomial, string exponent, string expected)
		{
			base.Pow(polynomial, exponent, expected);
		}

		[Test]
		[TestCase("3*X^2 + 2*X + 1", "X^2 + 1", "(2, 0)*X + (-2, 0)")]
		public override void Mod(string dividend, string modulus, string expected)
		{
			base.Mod(dividend, modulus, expected);
		}


		[Test]
		[TestCase("12*X + 1", "(144, 0)*X^2 + (24, 0)*X + (1, 0)")]
		public override void Square(string root, string expected)
		{
			base.Square(root, expected);
		}

		[Test]
		[TestCase("288*X^2 + 36*X - 2", "(576, 0)*X + (36, 0)")]
		public override void Derivative(string polynomial, string expected)
		{
			base.Derivative(polynomial, expected);
		}

		[Test]
		[TestCase("X^3 + (-15, 0)*X^2 + (71, 0)*X + (-105, 0)", "3", "5", "7")]
		public override void FromRoots(string expected, params object[] roots)
		{
			base.FromRoots(expected, roots);
		}

		[Test]
		[TestCase("X^3 - 15*X^2 + 71*X - 105", "3", "(0, 0)")]
		[TestCase("X^2 - 4*X + 13", "2", "(9, 0)")]
		[TestCase("X^2 + 1", "0", "(1, 0)")]
		public override void Evaluate(string polynomial, string indeterminateValue, string expected)
		{
			base.Evaluate(polynomial, indeterminateValue, expected);
		}

		[Test]
		[TestCase("2*X^3 + X^2 + 10*X + 62", "117", "X^3 + (118, 0)*X^2 + (10, 0)*X + (62, 0)")]
		[TestCase("5*X^2 + 19*X + 43", "79", "X^2 + (335, 0)*X + (43, 0)")]
		public override void MakeMonic(string polynomial, string polynomialBase, string expected)
		{
			base.MakeMonic(polynomial, polynomialBase, expected);
		}
	}
}
