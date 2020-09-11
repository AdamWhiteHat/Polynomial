using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Double : PolynomialArithmetic<Double>
	{
		[DataTestMethod]
		[DataRow("1.09986237454265*X^3 + 3.63797880709171E-12")]
		public override void TestMakeCoefficientsSmaller(string expected)
		{
			base.TestMakeCoefficientsSmaller(expected);
		}
	}
}
