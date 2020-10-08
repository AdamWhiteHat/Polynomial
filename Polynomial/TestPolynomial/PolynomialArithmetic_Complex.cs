using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Complex : PolynomialArithmetic<Complex>
	{
		[DataTestMethod]
		[DataRow("(5, 0)*X^3")]
		public override void TestBaseMExpansionConstructor(string expected)
		{
			base.TestBaseMExpansionConstructor(expected);
		}

		[DataTestMethod]
		[DataRow("(1.09986237454265, 0)*X^3 + (3.63797880709171E-12, 0)")]
		public override void TestMakeCoefficientsSmaller(string expected)
		{
			base.TestMakeCoefficientsSmaller(expected);
		}

		[DataTestMethod]
		[DataRow("(144, 0)*X^2 + (-12.5, -0.5)*X + (-3, -6.1)")]
		public override void TestParse(string expected)
		{
			base.TestParse(expected);
		}

		[DataTestMethod]
		[DataRow("(24, 0)*X + (-1, 0)")]
		public override void TestAddition(string expected)
		{
			base.TestAddition(expected);
		}

		[TestMethod]
		public void TestAddition2()
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
			TestContext.WriteLine($"Passed  = {expected == actual}");

			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("(7, 0)*X^2 + X")]
		public override void TestSubtraction(string expected)
		{
			base.TestSubtraction(expected);
		}

		[DataTestMethod]
		[DataRow("(144, 0)*X^2 + (-12, 0)*X + (-6, 0)")]
		public override void TestMultiply(string expected)
		{
			base.TestMultiply(expected);
		}

		[TestMethod]
		public void TestMultiply2()
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
			TestContext.WriteLine($"Passed  = {expected == actual}");


			Assert.AreEqual(expected, actual.ToString());
		}

		[DataTestMethod]
		[DataRow("(24, 0)*X + (-1, 0)")]
		public override void TestDivide(string expected)
		{
			base.TestDivide(expected);
		}

		[DataTestMethod]
		[DataRow("(2, 0)*X + (-2, 0)")]
		public override void TestMod(string expected)
		{
			base.TestMod(expected);
		}


		[DataTestMethod]
		[DataRow("(144, 0)*X^2 + (24, 0)*X + (1, 0)")]
		public override void TestSquare(string expected)
		{
			base.TestSquare(expected);
		}

		[DataTestMethod]
		[DataRow("(576, 0)*X + (36, 0)")]
		public override void TestDerivative(string expected)
		{
			base.TestDerivative(expected);
		}
	}
}
