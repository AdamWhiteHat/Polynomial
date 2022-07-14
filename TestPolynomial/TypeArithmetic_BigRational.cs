using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigRational))]
	[TestFixture(Category = "TypeArithmetic - BigRational")]
	public class TypeArithmetic_BigRational : TypeArithmetic<BigRational>
	{
		public override void Logarithm(string argument, double logBase, string expected)
		{
			//base.Logarithm(argument, logBase, expected);
		}

		public override void Ln(string argument, string expected)
		{
			//base.Ln(argument, expected);
		}

		public override void Sqrt(string radicand, string expected)
		{
			//base.Sqrt(radicand, expected);
		}

		public override void Power(string argument, int exponent, string expected)
		{
			//base.Power(argument, exponent, expected);
		}
	}
}
