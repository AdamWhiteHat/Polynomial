using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(Fraction))]
	[TestFixture(Category = "TypeArithmetic - Fraction")]
	public class TypeArithmetic_Fraction : TypeArithmetic<Fraction>
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

		[Test]
		public void TestClone()
		{
			var f2 = new Fraction(43, 2);
			var f1 = new Fraction(2, 2);
			var f0 = new Fraction(30, 2);

			var ft2 = new Term<Fraction>(f2, 2);
			var ft1 = new Term<Fraction>(f1, 1);
			var ft0 = new Term<Fraction>(f0, 0);

			string expected2 = "43/2*X^2";
			string expected1 = "X";
			string expected0 = "30/2";

			string actual2 = ft2.ToString();
			string actual1 = ft1.ToString();
			string actual0 = ft0.ToString();

			Assert.AreEqual(expected2, actual2);
			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected0, actual0);
		}
	}
}
