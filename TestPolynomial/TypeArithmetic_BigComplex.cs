using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using System;

namespace TestPolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "TypeArithmetic - BigComplex")]
	public class TypeArithmetic_BigComplex : TypeArithmetic<BigComplex>
	{
		public override void Logarithm(string argument, double logBase, string expected)
		{
		}

		public override void Ln(string argument, string expected)
		{
			BigComplex input = GenericArithmetic<BigComplex>.Parse(argument);

			string actual = GenericArithmetic<BigComplex>.Log(input, Math.E).ToString();

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"Actual:   {actual}");
			TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

			Assert.AreEqual(expected, actual);
		}
	}
}
