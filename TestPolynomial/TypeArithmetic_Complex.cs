using System;
using System.Numerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(Complex))]
	[TestFixture(Category = "TypeArithmetic - Complex")]
	public class TypeArithmetic_Complex : TypeArithmetic<Complex>
	{
		[Test]
		[TestCase("5", "3", "(8, 0)")]
		public override void Addition(string augend, string addend, string expected)
		{
			base.Addition(augend, addend, expected);
		}

		[Test]
		[TestCase("5", "3", "(2, 0)")]
		public override void Subtraction(string minuend, string subtrahend, string expected)
		{
			base.Subtraction(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("5", "3", "(15, 0)")]
		public override void Multiplication(string multiplicand, string multiplier, string expected)
		{
			base.Multiplication(multiplicand, multiplier, expected);
		}

		[Test]
		[TestCase("207", "69", "(3, 0)")]
		public override void Division(string dividend, string divisor, string expected)
		{
			base.Division(dividend, divisor, expected);
		}

		[Test]
		[TestCase("25", "(5, 0)")]
		public override void Sqrt(string radicand, string expected)
		{
			base.Sqrt(radicand, expected);
		}

		[Test]
		[TestCase("16", 2, "(4, 0)")]
		public override void Logarithm(string argument, double logBase, string expected)
		{
			base.Logarithm(argument, logBase, expected);
		}

		[Test]
		[TestCase("2981", "(8.00001409367807, 0)")]
		public override void Ln(string argument, string expected)
		{
			base.Ln(argument, expected);
		}

		[Test]
		[TestCase("5", 2, "(25, 0)")]
		public override void Power(string argument, int exponent, string expected)
		{
			base.Power(argument, exponent, expected);
		}

		[Test]
		[TestCase("4", "(-4, 0)")]
		public override void Negate(string number, string expected)
		{
			base.Negate(number, expected);
		}

		// [Test] //Actually, removing this attribute will prevent the test from showing up.
		public override void Sign(string number, string expected)
		{
			// There is no such equivalent method or property for the Complex numeric type.
			// Just return, which will allow the test to succeed.
			//base.Sign();
		}

		[Test]
		[TestCase("-4", "(4, 0)")]
		public override void Abs(string number, string expected)
		{
			base.Abs(number, expected);
		}
	}
}
