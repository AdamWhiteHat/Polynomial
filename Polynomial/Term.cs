using System;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExtendedArithmetic
{
	public class Term : ITerm
	{
		[DataMember]
		public int Exponent { get; set; }

		[DataMember]
		public BigInteger CoEfficient { get; set; }

		[IgnoreDataMember]
		private const string IndeterminateSymbol = "X";

		public Term()
		{
		}

		public Term(BigInteger coefficient, int exponent)
		{
			Exponent = exponent;
			CoEfficient = coefficient.Clone();
		}

		public static ITerm[] GetTerms(BigInteger[] terms)
		{
			List<ITerm> results = new List<ITerm>();

			int degree = 0;
			foreach (BigInteger term in terms)
			{
				results.Add(new Term(term.Clone(), degree));

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
			return new Term(this.CoEfficient.Clone(), this.Exponent);
		}

		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
