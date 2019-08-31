using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
	public class Term<TAlgebra, TNumber> where TAlgebra : IArithmetic<TAlgebra, TNumber>
	{

		public int Exponent { get; protected set; }
		public TAlgebra CoEfficient { get; set; }

		protected static string IndeterminateSymbol { get { return "X"; } }

		public Func<TAlgebra, int, ITerm<TAlgebra, TNumber>> TermConstructorMethod;

		public static Func<TAlgebra, int, ITerm<TAlgebra, TNumber>> InstanceConstructor;

		private Term()
		{
			InstanceConstructor = TermConstructorMethod;
		}

		public Term(TAlgebra coefficient, int exponent)
			: this()
		{
			CoEfficient = coefficient;
			Exponent = exponent;
		}

		public TAlgebra Evaluate(TAlgebra indeterminate)
		{
			return CoEfficient.Multiply(indeterminate.Pow(Exponent));
		}

		public ITerm<TAlgebra, TNumber> Clone()
		{
			return InstanceConstructor.Invoke(CoEfficient, Exponent);
		}

		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
