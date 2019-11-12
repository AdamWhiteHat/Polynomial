using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialLibrary
{
	public class Term<T> : ITerm<T>
	{
		public int Exponent { get; private set; }
		public T CoEfficient { get; set; }

		private const string IndeterminateSymbol = "X";

		public Term(T coefficient, int exponent)
		{
			Exponent = exponent;
			CoEfficient = GenericArithmetic<T>.Clone(coefficient);
		}

		public static ITerm<T>[] GetTerms(T[] terms)
		{
			List<ITerm<T>> results = new List<ITerm<T>>();

			int degree = 0;
			foreach (T term in terms)
			{
				results.Add(new Term<T>(GenericArithmetic<T>.Clone(term), degree));

				degree += 1;
			}

			return results.ToArray();
		}

		public T Evaluate(T indeterminate)
		{
			return GenericArithmetic<T>.Multiply(CoEfficient, GenericArithmetic<T>.Power(indeterminate, Exponent));
		}

		public ITerm<T> Clone()
		{
			return new Term<T>(GenericArithmetic<T>.Clone(this.CoEfficient), this.Exponent);
		}

		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
