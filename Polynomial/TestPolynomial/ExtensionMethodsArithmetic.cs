using System;
using System.Linq;
using System.Numerics;
using ExtendedArithmetic;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestPolynomial
{
    [TestFixture(Category = "Extension Methods Arithmetic")]
    public class ExtensionMethodsArithmetic
    {
        private TestContext m_testContext;
        public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

        private static IEnumerable<T> ParseParametersArray<T>(IEnumerable<object> parameters)
        {
            return parameters.Select(obj => GenericArithmetic<T>.Parse(obj.ToString()));
        }

        #region BigIntegerExtensionMethods

        [Test]
        [TestCase("4295032831", "65537", "65536")]
        public virtual void Mod(string dividend, string modulus, string expected)
        {
            BigInteger n = BigInteger.Parse(dividend);
            BigInteger p = BigInteger.Parse(modulus);

            BigInteger result = n.Mod(p);
            string actual = result.ToString();

            TestContext.WriteLine($"{dividend} % {modulus} = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("-1")]
        public virtual void Clone(string expected)
        {
            BigInteger n = BigInteger.Parse(expected);

            BigInteger result = n.Clone();
            string actual = result.ToString();

            TestContext.WriteLine($"{n}.Clone() == {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("1044722", "010011110000111111110000")]
        public virtual void ConvertToBase2(string number, string expected)
        {
            BigInteger num = BigInteger.Parse(number);

            string actual = string.Join("", num.ConvertToBase2().Select(b => b ? "1" : "0"));

            TestContext.WriteLine($"ConvertBase2({num}) = {actual}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("7", "49")]
        public virtual void Square(string number, string expected)
        {
            BigInteger num = BigInteger.Parse(number);

            BigInteger result = num.Square();
            string actual = result.ToString();

            TestContext.WriteLine($"{num}^2 = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("144", "12")]
        public virtual void SquareRoot(string number, string expected)
        {
            BigInteger num = BigInteger.Parse(number);

            BigInteger result = num.SquareRoot();
            string actual = result.ToString();

            TestContext.WriteLine($"Sqrt({num}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("2197", "3", "13")]
        public virtual void NthRoot(string number, string root, string expected)
        {
            BigInteger num = BigInteger.Parse(number);
            int rt = int.Parse(root);

            BigInteger result = num.NthRoot(rt);
            string actual = result.ToString();

            TestContext.WriteLine($"{rt}thRoot({num}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("3157729", "True")]
        public virtual void IsSquare(string number, string expected)
        {
            BigInteger num = BigInteger.Parse(number);

            string actual = num.IsSquare().ToString();

            TestContext.WriteLine($"IsSquare({num}) = {actual}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region IEnumerableBigIntegerExtensionMethods

        [Test]
        [TestCase(new object[] { "324", "36", "60", "84", "144" })]
        public virtual void BigIntegerSum(params object[] numbers)
        {
            string expected = numbers[0].ToString();

            IEnumerable<BigInteger> parameters = ParseParametersArray<BigInteger>(numbers.Skip(1));

            BigInteger result = parameters.Sum();
            string actual = result.ToString();

            TestContext.WriteLine($"Sum({string.Join(", ", parameters)}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(new object[] { "26127360", "36", "60", "84", "144" })]
        public virtual void BigIntegerProduct(params object[] numbers)
        {
            string expected = numbers[0].ToString();

            IEnumerable<BigInteger> parameters = ParseParametersArray<BigInteger>(numbers.Skip(1));

            BigInteger result = parameters.Product();
            string actual = result.ToString();

            TestContext.WriteLine($"Product({string.Join(", ", parameters)}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(new object[] { "12", "36", "60", "84", "144" })]
        public virtual void BigIntegerGCD(params object[] numbers)
        {
            string expected = numbers[0].ToString();
            IEnumerable<BigInteger> parameters = ParseParametersArray<BigInteger>(numbers.Skip(1));

            BigInteger result = parameters.GCD();
            string actual = result.ToString();

            TestContext.WriteLine($"GCD({string.Join(", ", parameters)}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ExtendedArithmetic.HelperMethods

        [Test]
        [TestCase("(1.75, -3.5)")]
        public virtual void ComplexParse(string expected)
        {
            Complex result = HelperMethods.Parse(expected);
            string actual = result.ToString();

            TestContext.WriteLine($"Complex.Parse({expected}) = {result}");
            TestContext.WriteLine("");
            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected == actual}");

            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
