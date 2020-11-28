using PolynomialLibrary;
using NUnit.Framework;

namespace TestPolynomial
{
    [TestFixture(Category = "FieldArithmetic")]
    public class FieldArithmetic<T>
    {
        private TestContext m_testContext;
        public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

        [Test]
        [TestCase("3*X^4 + 29*X^3 + 5*X^2 + 21*X + 17", "X^3 + X^2 + X", "-24*X^2 - 5*X + 17")]
        public virtual void ModulusPolynomial(string dividend, string modulusPolynomial, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(dividend);
            Polynomial<T> modPoly = Polynomial<T>.Parse(modulusPolynomial);

            string actual = Polynomial<T>.Field<T>.Modulus(start, modPoly).ToString();

            TestContext.WriteLine($"{start} Mod({modPoly})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^4 + 29*X^3 + 5*X^2 + 21*X + 17", "2", "X^4 + X^3 + X^2 + X + 1")]
        public virtual void ModulusInteger(string dividend, string modulusInteger, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(dividend);
            T modInt = GenericArithmetic<T>.Parse(modulusInteger);

            string actual = Polynomial<T>.Field<T>.Modulus(start, modInt).ToString();

            TestContext.WriteLine($"{start} % {modInt}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^4 + 29*X^3 + 5*X^2 + 21*X + 17", "X^3 + X^2 + X", "2", "X + 1")]
        public virtual void ModulusPolynomialModulusInteger(string dividend, string modulusPolynomial, string modulusInteger, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(dividend);
            Polynomial<T> modPoly = Polynomial<T>.Parse(modulusPolynomial);
            T modInt = GenericArithmetic<T>.Parse(modulusInteger);

            string actual = Polynomial<T>.Field<T>.ModMod(start, modPoly, modInt).ToString();

            TestContext.WriteLine($"{start} Mod({modPoly},{modInt})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^2 + 2*X + 1", "20", "3", "X + 2")]
        public virtual void Multiply(string multiplicand, string multiplier, string modulus, string expected)
        {
            Polynomial<T> left = Polynomial<T>.Parse(multiplicand);
            T right = GenericArithmetic<T>.Parse(multiplier);
            T modInt = GenericArithmetic<T>.Parse(modulus);

            string actual = Polynomial<T>.Field<T>.Multiply(left, right, modInt).ToString();

            TestContext.WriteLine($"(({left}) * ({right})) % {modInt}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^4 + 2*X^3 + 4*X^2 + 2*X + 1", "X^2 + 1", "2", "X^2 + 1")]
        public virtual void Divide(string dividend, string divisor, string modulus, string expected)
        {
            Polynomial<T> left = Polynomial<T>.Parse(dividend);
            Polynomial<T> right = Polynomial<T>.Parse(divisor);
            T modInt = GenericArithmetic<T>.Parse(modulus);

            Polynomial<T> remainder;
            string actual = Polynomial<T>.Field<T>.Divide(left, right, modInt, out remainder).ToString();

            TestContext.WriteLine($"(({left}) / ({right})) % {modInt}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual} (rem: {remainder})");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^2 + 2*X + 1", "2", "X^3 + X^2 + X", "2", "X + 1")]
        public virtual void ExponentiateMod(string root, string exponent, string modulusPolynomial, string modulusInteger, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(root);
            T exp = GenericArithmetic<T>.Parse(exponent);
            Polynomial<T> modPoly = Polynomial<T>.Parse(modulusPolynomial);
            T modInt = GenericArithmetic<T>.Parse(modulusInteger);

            string actual = Polynomial<T>.Field<T>.ExponentiateMod(start, exp, modPoly, modInt).ToString();

            TestContext.WriteLine($"({start})^{exp}) Mod({modPoly}, {modInt})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^2 + 2*X + 1", "2", "2", "X^2 + 1")]
        public virtual void PowMod(string root, string exponent, string modulusInteger, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(root);
            int exp = int.Parse(exponent);
            T modInt = GenericArithmetic<T>.Parse(modulusInteger);

            string actual = Polynomial<T>.Field<T>.PowMod(start, exp, modInt).ToString();

            TestContext.WriteLine($"({start})^{exp}) % {modInt}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3*X^2 + 2*X + 1", "2", "X^3 + X^2 + X", "-2*X^2 + X + 1")]
        [TestCase("3*X^2 + 2*X + 1", "3", "X^3 + X^2 + X", "4*X^2 - 2*X + 1")]
        public virtual void ModPow(string root, string exponent, string modulusPolynomial, string expected)
        {
            Polynomial<T> start = Polynomial<T>.Parse(root);
            T exp = GenericArithmetic<T>.Parse(exponent);
            Polynomial<T> modPoly = Polynomial<T>.Parse(modulusPolynomial);

            string actual = Polynomial<T>.Field<T>.ModPow(start, exp, modPoly).ToString();

            TestContext.WriteLine($"({start})^{exp}) Mod({modPoly})");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }
    }
}
