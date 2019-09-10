using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
	public class Term<TAlgebra, TNumber> : ITerm<TAlgebra, TNumber> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{
		public int Exponent { get; protected set; }
		public TAlgebra CoEfficient { get; set; }

		protected static string IndeterminateSymbol { get { return "X"; } }

		public Term(TAlgebra coefficient, int exponent)
		{
			CoEfficient = coefficient.Clone().Value;
			Exponent = exponent;
		}

		public TAlgebra Evaluate(TAlgebra indeterminate)
		{
			return CoEfficient.Multiply(indeterminate.Pow(Exponent));
		}

		public ITerm<TAlgebra, TNumber> Clone()
		{
			return new Term<TAlgebra, TNumber>(this.CoEfficient.Clone().Value, this.Exponent);
		}

		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
