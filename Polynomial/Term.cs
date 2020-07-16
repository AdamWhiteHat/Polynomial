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
		public static ITerm<T> Zero = null;

		public int Exponent { get; private set; }
		public T CoEfficient { get; set; }

		private const string IndeterminateSymbol = "X";

		static Term()
		{
			Zero = new Term<T>(GenericArithmetic<T>.Zero, 0);
		}

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

		public static ITerm<T> Divide(ITerm<T> left, ITerm<T> right)
		{
			T coefficent = GenericArithmetic<T>.Divide(left.CoEfficient, right.CoEfficient);
			int exponent = left.Exponent - right.Exponent;
			if (exponent < 0 || GenericArithmetic<T>.Equals(coefficent, GenericArithmetic<T>.Zero))
			{
				return Term<T>.Zero;
			}
			return new Term<T>(coefficent, exponent);
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
