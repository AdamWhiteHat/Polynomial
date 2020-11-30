using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public class Term<T> : ICloneable<Term<T>>
	{
		public static Term<T> Zero = null;

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

		public static Term<T>[] GetTerms(T[] terms)
		{
			List<Term<T>> results = new List<Term<T>>();

			int degree = 0;
			foreach (T term in terms)
			{
				results.Add(new Term<T>(GenericArithmetic<T>.Clone(term), degree));

				degree += 1;
			}

			return results.ToArray();
		}

		public static Term<T> Divide(Term<T> left, Term<T> right)
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
			T placeValue = GenericArithmetic<T>.Power(indeterminate, Exponent);
			T result = GenericArithmetic<T>.Multiply(CoEfficient, placeValue);
			return result;
		}

		public Term<T> Clone()
		{
			return new Term<T>(GenericArithmetic<T>.Clone(this.CoEfficient), this.Exponent);
		}

		public override string ToString()
		{
			// Note: The only time the coefficient should be zero is when this term is the only term and the polynomial is a zero polynomial.
			// Otherwise, we don't store terms who's coefficient is zero (because they contribute nothing to the polynomial).
			if (GenericArithmetic<T>.Equal(CoEfficient, GenericArithmetic<T>.Zero))
			{
				return GenericArithmetic<T>.ToString(GenericArithmetic<T>.Zero);
			}

			string coefficientString = string.Empty;
			string exponentString = string.Empty;
			bool displayMuliplicationSymbol = false;

			// Note: We do nothing in the case that the Exponent is equal to zero; it is superfluous to display the indeterminate raised to the power of zero, e.g. "X^0"
			if (Exponent == 1)
			{
				exponentString = "X";
			}
			else if (Exponent != 0)
			{
				exponentString = $"X^{Exponent}";
			}

			bool displayExponentString = !string.IsNullOrWhiteSpace(exponentString);
			if (GenericArithmetic<T>.Equal(CoEfficient, GenericArithmetic<T>.One))
			{
				// No need to display the indeterminate being multiplied by one, only display if the indeterminate is hidden (because it's exponent is zero).
				if (!displayExponentString)
				{
					coefficientString = GenericArithmetic<T>.ToString(GenericArithmetic<T>.One);
				}
			}
			else if (GenericArithmetic<T>.Equal(CoEfficient, GenericArithmetic<T>.MinusOne))
			{
				coefficientString = "-";
				if (!displayExponentString)
				{
					coefficientString += GenericArithmetic<T>.ToString(GenericArithmetic<T>.One);
				}
			}
			else
			{
				coefficientString = GenericArithmetic<T>.ToString(CoEfficient);
				displayMuliplicationSymbol = displayExponentString;
			}

			return $"{coefficientString}{(displayMuliplicationSymbol ? "*" : "")}{exponentString}";
		}
	}
}
