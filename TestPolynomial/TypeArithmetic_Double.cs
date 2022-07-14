using System;
using NUnit.Framework;

namespace TestPolynomial
{
    [TestFixture(Category = "TypeArithmetic - Double")]
    public class TypeArithmetic_Double : TypeArithmetic<Double>
    {
        [Test]
        [TestCase("2981", "8.00001409367807")]
        public override void Ln(string argument, string expected)
        {
            base.Ln(argument, expected);
        }
    }
}
