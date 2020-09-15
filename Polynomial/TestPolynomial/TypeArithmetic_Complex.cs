using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class TypeArithmetic_Complex : TypeArithmetic<Complex>
	{
		// [TestMethod] //Actually, removing this attribute will prevent the test from showing up.
		public override void TestSign()
		{
			// There is no such equivalent method or property for the Complex numeric type.
			// Just return, which will allow the test to succeed.
			//base.TestSign();
		}
	}
}
