using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
	public class Term : ITerm
	{
		public int Exponent { get; private set; }
		public BigInteger CoEfficient { get; set; }
		private static string IndeterminateSymbol = "X";

		public Term(BigInteger coefficient, int exponent)
		{
			Exponent = exponent;
			CoEfficient = coefficient;
		}

		public static ITerm[] GetTerms(BigInteger[] terms)
		{
			List<ITerm> results = new List<ITerm>();

			int degree = 0;
			foreach (BigInteger term in terms)
			{
				results.Add(new Term(term, degree));

				degree += 1;
			}

			return results.ToArray();
		}

		public BigInteger Evaluate(BigInteger indeterminate)
		{
			return BigInteger.Multiply(CoEfficient, BigInteger.Pow(indeterminate, Exponent));
		}

		public ITerm Clone()
		{
			return new Term(this.CoEfficient, this.Exponent);
		}

		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
