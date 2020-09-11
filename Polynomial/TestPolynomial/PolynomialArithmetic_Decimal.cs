using System;
using System.Numerics;
using PolynomialLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class PolynomialArithmetic_Decimal : PolynomialArithmetic<Decimal>
	{
		[DataTestMethod]
		[DataRow("5.0*X^3")]
		public override void TestBaseMExpansionConstructor(string expected)
		{
			base.TestBaseMExpansionConstructor(expected);
		}

		[DataTestMethod]
		[DataRow("1.0998623745426471081870363533*X^3")]
		public override void TestMakeCoefficientsSmaller(string expected)
		{
			base.TestMakeCoefficientsSmaller(expected);
		}

		[DataTestMethod]
		[DataRow("144.0*X^2 - 12.0*X + 6.51")]
		public override void TestParse(string expected)
		{
			base.TestParse(expected);
		}

		[DataTestMethod]
		[DataRow("24.0*X - 1.0")]
		public override void TestAddition(string expected)
		{
			base.TestAddition(expected);
		}

		[DataTestMethod]
		[DataRow("7.0*X^2 + X")]
		public override void TestSubtraction(string expected)
		{
			base.TestSubtraction(expected);
		}

		[DataTestMethod]
		[DataRow("144.00*X^2 - 12.00*X - 6.00")]
		public override void TestMultiply(string expected)
		{
			base.TestMultiply(expected);
		}

		[DataTestMethod]
		[DataRow("24.0*X - 1.0")]
		public override void TestDivide(string expected)
		{
			base.TestDivide(expected);
		}

		[DataTestMethod]
		[DataRow("2.0*X - 2.00")]
		public override void TestMod(string expected)
		{
			base.TestMod(expected);
		}

		[DataTestMethod]
		[DataRow("144.00*X^2 + 24.00*X + 1.00")]
		public override void TestSquare(string expected)
		{
			base.TestSquare(expected);
		}

		[DataTestMethod]
		[DataRow("576.0*X + 36.0")]
		public override void TestDerivative(string expected)
		{
			base.TestDerivative(expected);
		}
	}
}
