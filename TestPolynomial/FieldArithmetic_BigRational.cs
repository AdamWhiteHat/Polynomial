using ExtendedNumerics;
using NUnit.Framework;

namespace TestPolynomial
{
	[TestOf(typeof(BigRational))]
	[TestFixture(Category = "FieldArithmetic - BigRational")]
	public class FieldArithmetic_BigRational : FieldArithmetic<BigRational>
	{
		public override void ModulusInteger(string dividend, string modulusInteger, string expected)
		{
			//base.ModulusInteger(dividend, modulusInteger, expected);
		}

		public override void ModulusPolynomialModulusInteger(string dividend, string modulusPolynomial, string modulusInteger, string expected)
		{
			//base.ModulusPolynomialModulusInteger(dividend, modulusPolynomial, modulusInteger, expected);
		}

		public override void ModulusPolynomial(string dividend, string modulusPolynomial, string expected)
		{
			//base.ModulusPolynomial(dividend, modulusPolynomial, expected);
		}

		public override void Multiply(string multiplicand, string multiplier, string modulus, string expected)
		{
			//base.Multiply(multiplicand, multiplier, modulus, expected);
		}

		public override void Divide(string dividend, string divisor, string modulus, string expected)
		{
			//base.Divide(dividend, divisor, modulus, expected);			
		}

		public override void PowMod(string root, string exponent, string modulusInteger, string expected)
		{
			//base.PowMod(root, exponent, modulusInteger, expected);
		}

		public override void ModPow(string root, string exponent, string modulusPolynomial, string expected)
		{
			//base.ModPow(root, exponent, modulusPolynomial, expected);
		}

		public override void ExponentiateMod(string root, string exponent, string modulusPolynomial, string modulusInteger, string expected)
		{
			//base.ExponentiateMod(root, exponent, modulusPolynomial, modulusInteger, expected);
		}
	}
}
