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
		public T Exponent { get; private set; }
		public T CoEfficient { get; set; }

		private const string IndeterminateSymbol = "X";

		public Term(T coefficient, T exponent)
		{
			Exponent = exponent;
			CoEfficient = GenericArithmetic<T>.Clone(coefficient);
		}

		public static ITerm<T>[] GetTerms(T[] terms)
		{
			List<ITerm<T>> results = new List<ITerm<T>>();

			T degree = GenericArithmetic<T>.Zero;
			foreach (T term in terms)
			{
				results.Add(new Term<T>(GenericArithmetic<T>.Clone(term), degree));

				degree = GenericArithmetic<T>.Increment(degree);
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
