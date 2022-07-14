using System;
using System.Numerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigInteger))]
	[TestFixture(Category = "FieldArithmetic - BigInteger")]
	public class FieldArithmetic_BigInteger : FieldArithmetic<BigInteger>
	{
	}
}
