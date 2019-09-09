using System;
using System.Linq;
using System.Numerics;
using PolynomialLibrary;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
	[TestClass]
	public class FieldTest
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[TestMethod]
		public void TestMod()
		{
			string expecting = "2*X - 2";

			IPolynomial first = Polynomial.Parse("3*x^2 + 2*x + 1");
			IPolynomial second = Polynomial.Parse("x^2 + 1");

			IPolynomial result = Polynomial.Field.ReducePolynomial(first, second);

			TestContext.WriteLine($"({first}) + ({second})");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Result   = {result}");
			TestContext.WriteLine($"Expecting: {expecting}");

			Assert.AreEqual(expecting, result.ToString());
		}

		[TestMethod]
		public void TestGF2()
		{
			BigInteger x = 7;

			bool[] bits = x.ConvertToBase2();

			IPolynomial poly = new Polynomial(Term.FromBits(bits));

			IPolynomial reduced = Polynomial.Field.ReduceInteger(poly, 2);

			TestContext.WriteLine($"{string.Join("", bits.Select(b => (b ? "1" : "0")))}");
			TestContext.WriteLine($"{poly}");
			TestContext.WriteLine($"{reduced}");
			TestContext.WriteLine("");

		}

		[TestMethod]
		public void TestPow()
		{
			BigInteger exp = 2;
			BigInteger mod = 45113;
			IPolynomial f = Polynomial.Parse("X^3 + 15*X^2 + 29*X + 8");

			IPolynomial fPrime = Polynomial.GetDerivativePolynomial(f);

			IPolynomial f3 = Polynomial.Pow(f, 3);

			Polynomial.Field field = new Polynomial.Field(fPrime, mod);


			IPolynomial result = field.Pow(f, exp);

			TestContext.WriteLine("");
			TestContext.WriteLine($"{f3}");
			TestContext.WriteLine($"{Polynomial.Field.ReduceInteger(f3, mod)}");
			TestContext.WriteLine($"{Polynomial.Field.ReducePolynomial(f3, fPrime)}");
			TestContext.WriteLine($"{Polynomial.Field.ReduceFully(f3, fPrime, mod)}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"f = {f}");
			TestContext.WriteLine($"g =  {mod}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"f^5 mod g =");
			TestContext.WriteLine($"{result}");
			TestContext.WriteLine("");

		}

		[TestMethod]
		public void TestPolynomialRing()
		{
			string expected = "22455983949710645412*X^2 + 54100105785512562427*X + 22939402657683071224";


			ITerm[] fTerms = new Term[]
			{
				new Term(8, 0),
				new Term(29, 1),
				new Term(15, 2),
				new Term(1, 3),
			};

			IPolynomial a1 = new Polynomial(new Term[] {
					new Term( 1, 1),
					new Term( -1,  0)   });
			IPolynomial a2 = new Polynomial(new Term[] {
					new Term( 1, 1),
					new Term( 3,   0)   });
			IPolynomial a3 = new Polynomial(new Term[] {
					new Term( 1, 1),
					new Term( 13,  0)   });
			IPolynomial a4 = new Polynomial(new Term[] {
					new Term( 1, 1),
					new Term( 104, 0)   });
			IPolynomial a5 = new Polynomial(new Term[] {
					new Term( 2, 1),
					new Term( 3,   0)   });
			IPolynomial a6 = new Polynomial(new Term[] {
					new Term( 2, 1),
					new Term( 25,  0)   });
			IPolynomial a7 = new Polynomial(new Term[] {
					new Term( 3, 1),
					new Term( -8,  0)   });
			IPolynomial a8 = new Polynomial(new Term[] {
					new Term( 5, 1),
					new Term( 48,  0)   });
			IPolynomial a9 = new Polynomial(new Term[] {
					new Term( 5, 1),
					new Term( 54,  0)   });
			IPolynomial a10 = new Polynomial(new Term[] {
					new Term( 6, 1),
					new Term( -43, 0)   });
			IPolynomial a11 = new Polynomial(new Term[] {
					new Term( 7, 1),
					new Term( -8,  0)   });
			IPolynomial a12 = new Polynomial(new Term[] {
					new Term( 7, 1),
					new Term( 11,  0)   });
			IPolynomial a13 = new Polynomial(new Term[] {
					new Term( 11, 1),
					new Term( 856, 0)   });

			List<IPolynomial> ideals = new List<IPolynomial>()
			{
				a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13
			};

			BigInteger N = 45113;
			BigInteger m = 31;

			IPolynomial f = new Polynomial(fTerms);
			int degree = f.Degree;

			TestContext.WriteLine("");
			TestContext.WriteLine($"\nN = {f.Evaluate(m)}\n\nf  = {f} (degree {f.Degree})\n\n");
			TestContext.WriteLine("");

			IPolynomial fd = Polynomial.GetDerivativePolynomial(f);
			IPolynomial d3 = Polynomial.Product(ideals);
			IPolynomial derivativeSquared = Polynomial.Square(fd);
			IPolynomial d2 = Polynomial.Multiply(d3, derivativeSquared);

			IPolynomial dd = Polynomial.Field.ReducePolynomial(d2, f);

			BigInteger valD = fd.Evaluate(m);
			BigInteger valD2 = derivativeSquared.Evaluate(m);

			BigInteger prod = d2.Evaluate(m);
			BigInteger prodSqrt = prod.SquareRoot();


			TestContext.WriteLine($"f' = {fd}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"δ  =  {d3}");
			TestContext.WriteLine($"δ  =  f'(x)^2 * {d2}");
			TestContext.WriteLine("");
			TestContext.WriteLine($"δ  ≡  {dd} (mod f)");
			TestContext.WriteLine("");
			TestContext.WriteLine($"value (reduced)  = {dd.Evaluate(m).Mod(N)}");
			TestContext.WriteLine("");

			Assert.AreEqual(expected, dd.ToString());
		}
	}
}
