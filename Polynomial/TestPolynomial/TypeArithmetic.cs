using System;
using System.Numerics;
using PolynomialLibrary;
using NUnit.Framework;

namespace TestPolynomial
{
    [TestFixture(Category = "TypeArithmetic")]
    public class TypeArithmetic<T>
    {
        private TestContext m_testContext;
        public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

        [Test]
        [TestCase("5", "3", "8")]
        public virtual void Addition(string augend, string addend, string expected)
        {
            T left = GenericArithmetic<T>.Parse(augend);
            T right = GenericArithmetic<T>.Parse(addend);

            string actual = GenericArithmetic<T>.Add(left, right).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("5", "3", "2")]
        public virtual void Subtraction(string minuend, string subtrahend, string expected)
        {
            T left = GenericArithmetic<T>.Parse(minuend);
            T right = GenericArithmetic<T>.Parse(subtrahend);

            string actual = GenericArithmetic<T>.Subtract(left, right).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("5", "3", "15")]
        public virtual void Multiplication(string multiplicand, string multiplier, string expected)
        {
            T left = GenericArithmetic<T>.Parse(multiplicand);
            T right = GenericArithmetic<T>.Parse(multiplier);

            string actual = GenericArithmetic<T>.Multiply(left, right).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("207", "69", "3")]
        public virtual void Division(string dividend, string divisor, string expected)
        {
            T left = GenericArithmetic<T>.Parse(dividend);
            T right = GenericArithmetic<T>.Parse(divisor);

            string actual = GenericArithmetic<T>.Divide(left, right).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("25", "5")]
        public virtual void Sqrt(string radicand, string expected)
        {
            T input = GenericArithmetic<T>.Parse(radicand);

            string actual = GenericArithmetic<T>.SquareRoot(input).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("16", 2, "4")]
        public virtual void Logarithm(string argument, double logBase, string expected)
        {
            T arg = GenericArithmetic<T>.Parse(argument);

            string actual = GenericArithmetic<T>.Log(arg, logBase).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("2981", "8")]
        public virtual void Ln(string argument, string expected)
        {
            T input = GenericArithmetic<T>.Parse(argument);

            string actual = GenericArithmetic<T>.Log(input, Math.E).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("5", 2, "25")]
        public virtual void Power(string argument, int exponent, string expected)
        {
            T input = GenericArithmetic<T>.Parse(argument);

            string actual = GenericArithmetic<T>.Power(input, exponent).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("4", "-4")]
        public virtual void Negate(string number, string expected)
        {
            T input = GenericArithmetic<T>.Parse(number);

            string actual = GenericArithmetic<T>.Negate(input).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("-4", "-1")]
        public virtual void Sign(string number, string expected)
        {
            T input = GenericArithmetic<T>.Parse(number);

            string actual = GenericArithmetic<T>.Sign(input).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("-4", "4")]
        public virtual void Abs(string number, string expected)
        {
            T input = GenericArithmetic<T>.Parse(number);

            string actual = GenericArithmetic<T>.Abs(input).ToString();

            TestContext.WriteLine($"Expected: {expected}");
            TestContext.WriteLine($"Actual:   {actual}");
            TestContext.WriteLine($"Passed:   {expected.Equals(actual)}");

            Assert.AreEqual(expected, actual);
        }
    }
}
