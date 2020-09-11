﻿using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public partial class Polynomial<T> : IPolynomial<T>
	{
		public static IPolynomial<T> Zero = null;
		public static IPolynomial<T> One = null;
		public static IPolynomial<T> Two = null;

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public ITerm<T>[] Terms { get { return _terms.ToArray(); } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<ITerm<T>> _terms;

		public int Degree { get; private set; }

		public T this[int degree]
		{
			get
			{
				ITerm<T> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm<T>))
				{
					return GenericArithmetic<T>.Zero;
				}
				else
				{
					return term.CoEfficient;
				}
			}
			set
			{
				ITerm<T> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(ITerm<T>))
				{
					if (GenericArithmetic<T>.NotEqual(value, GenericArithmetic<T>.Zero))
					{
						ITerm<T> newTerm = new Term<T>(value, degree);
						List<ITerm<T>> terms = _terms;
						terms.Add(newTerm);
						SetTerms(terms);
					}
				}
				else
				{
					term.CoEfficient = value;
				}

				RemoveZeros();
			}
		}

		#region Constructors

		static Polynomial()
		{
			Zero = new Polynomial<T>(Term<T>.GetTerms(new T[] { GenericArithmetic<T>.Zero }));
			One = new Polynomial<T>(Term<T>.GetTerms(new T[] { GenericArithmetic<T>.One }));
			Two = new Polynomial<T>(Term<T>.GetTerms(new T[] { GenericArithmetic<T>.Two }));
		}

		public Polynomial() { _terms = new List<ITerm<T>>() { new Term<T>(GenericArithmetic<T>.Zero, 0) }; Degree = 0; }

		public Polynomial(ITerm<T>[] terms)
		{
			SetTerms(terms);
		}

		public Polynomial(T n, T polynomialBase)
			: this
			(
				n,
				polynomialBase,
				GenericArithmetic<int>.Convert<T>(
					GenericArithmetic<T>.Truncate(
						GenericArithmetic<T>.Add(
							GenericArithmetic<T>.Convert(
								GenericArithmetic<T>.Log(n, GenericArithmetic<double>.Convert<T>(polynomialBase))
							),
							GenericArithmetic<T>.One
						)
					)
				)
			)
		{
		}

		public Polynomial(T n, T polynomialBase, int forceDegree)
		{
			Degree = forceDegree;
			SetTerms(GetPolynomialTerms(n, polynomialBase, Degree));
		}

		private void SetTerms(IEnumerable<ITerm<T>> terms)
		{
			_terms = terms.OrderBy(t => t.Exponent).ToList();
			RemoveZeros();
		}

		private void RemoveZeros()
		{
			_terms.RemoveAll(t => GenericArithmetic<T>.Equal(t.CoEfficient, GenericArithmetic<T>.Zero));
			if (!_terms.Any())
			{
				_terms = Term<T>.GetTerms(new T[] { GenericArithmetic<T>.Zero }).ToList();
			}
			SetDegree();
		}

		private void SetDegree()
		{
			if (_terms.Any())
			{
				Degree = _terms.Max(term => term.Exponent);
			}
			else
			{
				Degree = 0;
			}
		}

		private static List<ITerm<T>> GetPolynomialTerms(T value, T polynomialBase, int degree)
		{
			int deg = degree;
			T toAdd = value;
			List<ITerm<T>> result = new List<ITerm<T>>();
			while (deg >= 0 && GenericArithmetic<T>.GreaterThan(toAdd, GenericArithmetic<T>.Zero))
			{
				T placeValue = GenericArithmetic<T>.Power(polynomialBase, deg);

				if (GenericArithmetic<T>.Equal(placeValue, GenericArithmetic<T>.One))
				{
					result.Add(new Term<T>(toAdd, deg));
					toAdd = GenericArithmetic<T>.Zero;
				}
				else if (GenericArithmetic<T>.Equal(placeValue, toAdd))
				{
					result.Add(new Term<T>(GenericArithmetic<T>.One, deg));
					toAdd = GenericArithmetic<T>.Subtract(toAdd, placeValue);
				}
				else if (GenericArithmetic<T>.LessThan(placeValue, GenericArithmetic<T>.Abs(toAdd)))
				{
					T quotient = GenericArithmetic<T>.Divide(toAdd, placeValue);

					if (GenericArithmetic<T>.GreaterThan(quotient, placeValue))
					{
						quotient = placeValue;
					}

					result.Add(new Term<T>(quotient, deg));
					T toSubtract = GenericArithmetic<T>.Multiply(quotient, placeValue);

					toAdd = GenericArithmetic<T>.Subtract(toAdd, toSubtract);
				}

				deg -= 1;
			}
			return result.ToList();
		}

		public static IPolynomial<T> FromRoots(params T[] roots)
		{
			return Polynomial<T>.Product(
				roots.Select(
					zero => new Polynomial<T>(
						new Term<T>[]
						{
						new Term<T>( GenericArithmetic<T>.One,1),
						new Term<T>( GenericArithmetic<T>.Negate(zero), 0)
						}
					)
				)
			);
		}

		public static IPolynomial<T> Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "");

			// The below replaces the following logic: inputString = inputString.Replace("-", "+-");
			// in a way that doesn't flip negative coefficients. 
			// Begin logic
			int[] indices = inputString.FindAllIndexOf('-');

			int[] indicesToInsertAPlusCharacter =
					indices
						.Where(i => i <= 0 ? false : !_charactersPreceedingTermOperation.Contains(inputString[i - 1]))
						.OrderByDescending(i => i)
						.ToArray();

			string modifiedString = inputString;

			foreach (int index in indicesToInsertAPlusCharacter)
			{
				modifiedString = modifiedString.Insert(index, "+");
			}
			// End logic


			string[] stringTerms = modifiedString.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
			if (!stringTerms.Any())
			{
				throw new FormatException();
			}

			if (typeof(T) == typeof(Complex))
			{
				_coefficientPredicates.Add((c) => c == '(');
				_coefficientPredicates.Add((c) => c == ')');
				_coefficientPredicates.Add((c) => c == ',');
				_coefficientPredicates.Add((c) => c == '.');
			}
			if (typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
			{
				_coefficientPredicates.Add((c) => c == '.');
			}

			List<Term<T>> polyTerms = new List<Term<T>>();
			foreach (string stringTerm in stringTerms)
			{
				string[] termParts = stringTerm.Split(new char[] { '*' });

				if (termParts.Count() != 2)
				{
					if (termParts.Count() != 1)
					{
						throw new FormatException();
					}

					string temp = termParts[0];
					if (temp.All(c => _coefficientPredicates.Any(p => p(c))))
					{
						termParts = new string[] { temp, "X^0" };
					}
					else if (temp.All(c => _variablePredicates.Any(p => p(c))))
					{
						if (temp.Contains("-"))
						{
							temp = temp.Replace("-", "");
							termParts = new string[] { "-1", temp };
						}
						else { termParts = new string[] { "1", temp }; }
					}
					else
					{
						throw new FormatException();
					}
				}

				bool negateCoefficient = false;
				if (termParts[0].First() == '-')
				{
					termParts[0] = termParts[0].Substring(1, termParts[0].Length - 1);
					negateCoefficient = true;
				}

				T coefficient = GenericArithmetic<T>.Parse(termParts[0]);

				if (negateCoefficient)
				{
					coefficient = GenericArithmetic<T>.Negate(coefficient);
				}

				string[] variableParts = termParts[1].Split(new char[] { '^' });
				if (variableParts.Count() != 2)
				{
					if (variableParts.Count() != 1)
					{
						throw new FormatException();
					}

					string tmp = variableParts[0];
					if (tmp.All(c => _indeterminatePredicates.Any(p => p(c))))
					{
						variableParts = new string[] { tmp, "1" };
					}
				}
				int exponent = int.Parse(variableParts[1]);
				polyTerms.Add(new Term<T>(coefficient, exponent));
			}

			if (!polyTerms.Any())
			{
				throw new FormatException();
			}

			return new Polynomial<T>(polyTerms.ToArray());
		}

		private static char[] _charactersPreceedingTermOperation = new char[] { '(', ',', '*', '^', '+' };
		private static List<Predicate<char>> _coefficientPredicates = new List<Predicate<char>> { char.IsDigit, (c) => c == '-' };
		private static List<Predicate<char>> _variablePredicates = new List<Predicate<char>> { char.IsLetter, char.IsDigit, (c) => c == '^', (c) => c == '-' };
		private static List<Predicate<char>> _indeterminatePredicates = new List<Predicate<char>> { char.IsLetter };

		#endregion

		#region Evaluate

		public T Evaluate(T indeterminateValue)
		{
			return Evaluate(Terms, indeterminateValue);
		}

		public static T Evaluate(ITerm<T>[] terms, T indeterminateValue)
		{
			T result = GenericArithmetic<T>.Zero;
			foreach (ITerm<T> term in terms)
			{
				T placeValue = GenericArithmetic<T>.Power(indeterminateValue, term.Exponent);
				T addValue = GenericArithmetic<T>.Multiply(term.CoEfficient, placeValue);
				result = GenericArithmetic<T>.Add(result, addValue);
			}
			return result;
		}

		#endregion

		#region Change Forms

		public static IPolynomial<T> GetDerivativePolynomial(IPolynomial<T> poly)
		{
			int deg = 0;
			List<ITerm<T>> terms = new List<ITerm<T>>();
			foreach (ITerm<T> term in poly.Terms)
			{
				deg = term.Exponent - 1;
				if (deg < 0)
				{
					continue;
				}
				terms.Add(new Term<T>(GenericArithmetic<T>.Multiply(term.CoEfficient, GenericArithmetic<T>.Convert(term.Exponent)), deg));
			}

			IPolynomial<T> result = new Polynomial<T>(terms.ToArray());
			return result;
		}

		public static IPolynomial<T> MakeMonic(IPolynomial<T> polynomial, T polynomialBase)
		{
			int deg = polynomial.Degree;
			IPolynomial<T> result = new Polynomial<T>(polynomial.Terms.ToArray());
			if (GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Abs(result[deg]), GenericArithmetic<T>.One))
			{
				T toAdd = GenericArithmetic<T>.Multiply(GenericArithmetic<T>.Decrement(result[deg]), polynomialBase);
				result[deg] = GenericArithmetic<T>.One;
				result[deg - 1] = GenericArithmetic<T>.Add(result[deg - 1], toAdd);
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(IPolynomial<T> polynomial, T polynomialBase, T maxCoefficientSize = default(T))
		{
			T maxSize = maxCoefficientSize;

			if (GenericArithmetic<T>.Equal(maxSize, default(T)))
			{

				maxSize = GenericArithmetic<T>.Divide(polynomialBase, GenericArithmetic<T>.Two);
			}

			int pos = 0;
			int deg = polynomial.Degree;

			while (pos < deg)
			{
				int posInc = pos + 1;

				if (GenericArithmetic<T>.GreaterThan(polynomial[pos], maxSize) &&
					GenericArithmetic<T>.GreaterThan(polynomial[pos], polynomial[posInc]))
				{
					T diff = GenericArithmetic<T>.Subtract(polynomial[pos], maxSize);

					T toAdd = GenericArithmetic<T>.Increment(GenericArithmetic<T>.Divide(diff, polynomialBase));
					T toRemove = GenericArithmetic<T>.Multiply(toAdd, polynomialBase);

					polynomial[pos] = GenericArithmetic<T>.Subtract(polynomial[pos], toRemove);
					polynomial[posInc] = GenericArithmetic<T>.Add(polynomial[posInc], toAdd);
				}

				pos += 1;
			}
		}

		#endregion

		#region Arithmetic

		public static IPolynomial<T> GCD(IPolynomial<T> left, IPolynomial<T> right)
		{
			IPolynomial<T> a = left.Clone();
			IPolynomial<T> b = right.Clone();

			if (b.Degree > a.Degree)
			{
				IPolynomial<T> swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || GenericArithmetic<T>.Equal(b[0], GenericArithmetic<T>.Zero)))
			{
				IPolynomial<T> temp = a;
				a = b;
				b = Field<T>.Modulus(temp, b);
			}

			if (a.Degree == 0)
			{
				return Polynomial<T>.One;
			}
			else
			{
				return a;
			}
		}

		public static IPolynomial<T> Divide(IPolynomial<T> left, IPolynomial<T> right)
		{
			IPolynomial<T> remainder = new Polynomial<T>();
			IPolynomial<T> quotient = Polynomial<T>.Divide(left, right, out remainder);
			return quotient;
		}

		public static IPolynomial<T> Divide(IPolynomial<T> left, IPolynomial<T> right, out IPolynomial<T> remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree || right.CompareTo(left) == 1)
			{
				remainder = Polynomial<T>.Zero;
				return left;
			}

			IPolynomial<T> lhs = left.Clone();
			IPolynomial<T> rhs = right.Clone();
			IPolynomial<T> total = Zero;

			int degree = Math.Max(left.Degree, right.Degree) - 1;

			bool done = false;
			while (!done)
			{
				ITerm<T> leftLC = lhs.Terms.Last();
				ITerm<T> rightLC = rhs.Terms.Last();

				ITerm<T> quotientTerm = Term<T>.Divide(leftLC, rightLC);

				IPolynomial<T> multiplicand = new Polynomial<T>(new ITerm<T>[] { quotientTerm });

				total = Add(total, multiplicand);

				IPolynomial<T> subtrahend = Multiply(multiplicand, rhs);
				IPolynomial<T> difference = Subtract(lhs, subtrahend);

				lhs = difference.Clone();

				if (quotientTerm == Term<T>.Zero)
				{
					break;
				}

				if (lhs.Degree < 1)
				{
					T l = lhs[0];

					if (GenericArithmetic<T>.Equal(l, GenericArithmetic<T>.Zero))
					{
						done = true;
					}
					else if (rhs.Degree < 1)
					{
						T r = rhs[0];
						if (GenericArithmetic<T>.NotEqual(GenericArithmetic<T>.GCD(l, r), GenericArithmetic<T>.One))
						{
							continue;
						}
					}

					done = true;
				}

				degree--;
			}

			remainder = lhs.Clone();
			return total.Clone();
		}

		public static IPolynomial<T> Multiply(IPolynomial<T> left, IPolynomial<T> right)
		{
			if (left == null) { throw new ArgumentNullException(nameof(left)); }
			if (right == null) { throw new ArgumentNullException(nameof(right)); }

			int length = (left.Degree + right.Degree + 1);

			T[] terms = new T[length];

			for (int i = 0; i <= left.Degree; i++)
			{
				for (int j = 0; j <= right.Degree; j++)
				{
					int index = (i + j);
					terms[index] = GenericArithmetic<T>.Add(terms[index], GenericArithmetic<T>.Multiply(left[i], right[j]));
				}
			}
			return new Polynomial<T>(Term<T>.GetTerms(terms));
		}

		public static IPolynomial<T> Product(params IPolynomial<T>[] polys)
		{
			return Product(polys.ToList());
		}

		public static IPolynomial<T> Product(IEnumerable<IPolynomial<T>> polys)
		{
			IPolynomial<T> result = null;

			foreach (IPolynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p;
				}
				else
				{
					result = Polynomial<T>.Multiply(result, p);
				}
			}

			return result;
		}

		public static IPolynomial<T> Square(IPolynomial<T> poly)
		{
			return Polynomial<T>.Multiply(poly, poly);
		}

		public static IPolynomial<T> Pow(IPolynomial<T> poly, T exponent)
		{
			if (GenericArithmetic<T>.LessThan(exponent, GenericArithmetic<T>.Zero))
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported. Build this functionality if it is needed.");
			}
			else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.Zero))
			{
				return new Polynomial<T>(new Term<T>[] { new Term<T>(GenericArithmetic<T>.One, 0) });
			}
			else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.One))
			{
				return poly.Clone();
			}
			else if (GenericArithmetic<T>.Equal(exponent, GenericArithmetic<T>.Two))
			{
				return Square(poly);
			}

			IPolynomial<T> total = Polynomial<T>.Square(poly);

			T counter = GenericArithmetic<T>.Subtract(exponent, GenericArithmetic<T>.Two);
			while (GenericArithmetic<T>.NotEqual(counter, GenericArithmetic<T>.Zero))
			{
				total = Polynomial<T>.Multiply(total, poly);
				counter = GenericArithmetic<T>.Decrement(counter);
			}

			return total;
		}

		public static IPolynomial<T> Subtract(IPolynomial<T> left, IPolynomial<T> right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			int length = (Math.Max(left.Degree, right.Degree) + 1);
			T[] terms = new T[length];
			for (int i = 0; i < terms.Length; i++)
			{
				T l = left[i];
				T r = right[i];

				terms[i] = GenericArithmetic<T>.Subtract(l, r);
			}

			IPolynomial<T> result = new Polynomial<T>(Term<T>.GetTerms(terms.ToArray()));

			return result;
		}

		public static IPolynomial<T> Sum(params IPolynomial<T>[] polys)
		{
			return Sum(polys.ToList());
		}

		public static IPolynomial<T> Sum(IEnumerable<IPolynomial<T>> polys)
		{
			IPolynomial<T> result = null;

			foreach (IPolynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p;
				}
				else
				{
					result = Polynomial<T>.Add(result, p);
				}
			}

			return result;
		}

		public static IPolynomial<T> Add(IPolynomial<T> left, IPolynomial<T> right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			int length = Math.Max(left.Degree, right.Degree) + 1;
			T[] terms = new T[length];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = GenericArithmetic<T>.Add(left[i], right[i]);
			}

			IPolynomial<T> result = new Polynomial<T>(Term<T>.GetTerms(terms.ToArray()));
			return result;
		}

		#endregion

		#region Overrides and Interface implementations		

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new NullReferenceException();
			}

			IPolynomial<T> other = obj as IPolynomial<T>;

			if (other == null)
			{
				throw new ArgumentException();
			}

			return this.CompareTo(other);
		}

		public int CompareTo(IPolynomial<T> other)
		{
			if (other == null)
			{
				throw new ArgumentException();
			}

			if (other.Degree != this.Degree)
			{
				return (other.Degree > this.Degree) ? -1 : 1;
			}
			else
			{
				int counter = this.Degree;
				while (counter >= 0)
				{
					T thisCoefficient = this[counter];
					T otherCoefficient = other[counter];

					if (GenericArithmetic<T>.LessThan(thisCoefficient, otherCoefficient)) { return -1; }
					else if (GenericArithmetic<T>.GreaterThan(thisCoefficient, otherCoefficient)) { return 1; }

					counter--;
				}

				return 0;
			}
		}

		public IPolynomial<T> Clone()
		{
			var terms = _terms.Select(pt => pt.Clone()).ToArray();
			return new Polynomial<T>(terms);
		}

		public override string ToString()
		{
			List<string> stringTerms = new List<string>();
			int degree = GenericArithmetic<int>.Convert(Terms.Length);
			//while (GenericArithmetic<T>.GreaterThanOrEqual((degree = GenericArithmetic<T>.Decrement(degree)), GenericArithmetic<T>.Zero))
			while (--degree >= 0)
			{
				string termString = "";
				ITerm<T> term = Terms[degree];

				if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.Zero))
				{
					if (term.Exponent == 0)
					{
						if (stringTerms.Count == 0) { stringTerms.Add("0"); }
					}
					continue;
				}
				else if (GenericArithmetic<T>.GreaterThan(term.CoEfficient, GenericArithmetic<T>.One)
					|| GenericArithmetic<T>.LessThan(term.CoEfficient, GenericArithmetic<T>.MinusOne))
				{
					termString = $"{term.CoEfficient}";
				}

				if (term.Exponent == 0)
				{
					stringTerms.Add($"{term.CoEfficient}");
				}
				else if (term.Exponent == 1)
				{
					if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.One)) { stringTerms.Add("X"); }
					else if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.MinusOne)) { stringTerms.Add("-X"); }
					else { stringTerms.Add($"{term.CoEfficient}*X"); }
				}
				else
				{
					if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.One)) { stringTerms.Add($"X^{term.Exponent}"); }
					else if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.MinusOne)) { stringTerms.Add($"-X^{term.Exponent}"); }
					else { stringTerms.Add($"{term.CoEfficient}*X^{term.Exponent}"); }
				}
			}
			return string.Join(" + ", stringTerms).Replace("+ -", "- ");
		}

		#endregion

	}
}
