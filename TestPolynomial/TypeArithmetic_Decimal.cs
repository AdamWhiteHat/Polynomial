using System;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestFixture(Category = "TypeArithmetic - Decimal")]
	public class TypeArithmetic_Decimal : TypeArithmetic<Decimal>
	{
		[Test]
		[TestCase("2981", "8.00001409367807")]
		public override void Ln(string argument, string expected)
		{
			base.Ln(argument, expected);
		}
	}
}
