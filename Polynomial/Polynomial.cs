using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public partial class Polynomial<T> : ICloneable<Polynomial<T>>, IComparable, IComparable<Polynomial<T>>
	{
		public static Polynomial<T> Zero = null;
		public static Polynomial<T> One = null;
		public static Polynomial<T> Two = null;

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public Term<T>[] Terms { get { return _terms.ToArray(); } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<Term<T>> _terms;

		public int Degree { get; private set; }

		public T this[int degree]
		{
			get
			{
				Term<T> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(Term<T>))
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
				Term<T> term = Terms.FirstOrDefault(t => t.Exponent == degree);

				if (term == default(Term<T>))
				{
					if (GenericArithmetic<T>.NotEqual(value, GenericArithmetic<T>.Zero))
					{
						Term<T> newTerm = new Term<T>(value, degree);
						List<Term<T>> terms = _terms;
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

		public Polynomial() { _terms = new List<Term<T>>() { new Term<T>(GenericArithmetic<T>.Zero, 0) }; Degree = 0; }

		public Polynomial(Term<T>[] terms)
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

		private void SetTerms(IEnumerable<Term<T>> terms)
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

		private static List<Term<T>> GetPolynomialTerms(T value, T polynomialBase, int degree)
		{
			int deg = degree;
			T toAdd = value;
			List<Term<T>> result = new List<Term<T>>();
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

		public static Polynomial<T> FromRoots(params T[] roots)
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

		public static Polynomial<T> Parse(string input)
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

		public static T Evaluate(Term<T>[] terms, T indeterminateValue)
		{
			T result = GenericArithmetic<T>.Zero;
			foreach (Term<T> term in terms)
			{
				T termValue = term.Evaluate(indeterminateValue);
				result = GenericArithmetic<T>.Add(result, termValue);
			}
			return result;
		}

		#endregion

		#region Change Forms

		public static Polynomial<T> GetDerivativePolynomial(Polynomial<T> poly)
		{
			int deg = 0;
			List<Term<T>> terms = new List<Term<T>>();
			foreach (Term<T> term in poly.Terms)
			{
				deg = term.Exponent - 1;
				if (deg < 0)
				{
					continue;
				}
				terms.Add(new Term<T>(GenericArithmetic<T>.Multiply(term.CoEfficient, GenericArithmetic<T>.Convert(term.Exponent)), deg));
			}

			Polynomial<T> result = new Polynomial<T>(terms.ToArray());
			return result;
		}

		public static Polynomial<T> MakeMonic(Polynomial<T> polynomial, T polynomialBase)
		{
			int deg = polynomial.Degree;
			Polynomial<T> result = new Polynomial<T>(polynomial.Terms.ToArray());
			if (GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Abs(result[deg]), GenericArithmetic<T>.One))
			{
				T toAdd = GenericArithmetic<T>.Multiply(GenericArithmetic<T>.Decrement(result[deg]), polynomialBase);
				result[deg] = GenericArithmetic<T>.One;
				result[deg - 1] = GenericArithmetic<T>.Add(result[deg - 1], toAdd);
			}
			return result;
		}

		public static Polynomial<T> MakeCoefficientsSmaller(Polynomial<T> polynomial, T polynomialBase, T maxCoefficientSize = default(T))
		{
			T max = maxCoefficientSize;

			if (GenericArithmetic<T>.Equal(max, default(T)))
			{
				max = GenericArithmetic<T>.Divide(polynomialBase, GenericArithmetic<T>.Two);
			}

			Polynomial<T> result = polynomial.Clone();

			int pos = 0;
			int deg = result.Degree;
			while (pos < deg)
			{
				int posInc = pos + 1;

				if (GenericArithmetic<T>.GreaterThan(result[pos], max))
				{

					result[pos + 1] = GenericArithmetic<T>.Add(result[pos + 1], GenericArithmetic<T>.One);
					result[pos] = GenericArithmetic<T>.Negate(GenericArithmetic<T>.Subtract(polynomialBase, result[pos]));
				}

				pos++;
			}

			return result;
		}

		#endregion

		#region Arithmetic

		public static Polynomial<T> GCD(Polynomial<T> left, Polynomial<T> right)
		{
			Polynomial<T> a = left.Clone();
			Polynomial<T> b = right.Clone();

			if (b.Degree > a.Degree)
			{
				Polynomial<T> swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || GenericArithmetic<T>.Equal(b[0], GenericArithmetic<T>.Zero)))
			{
				Polynomial<T> temp = a;
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

		public static Polynomial<T> Divide(Polynomial<T> left, Polynomial<T> right)
		{
			Polynomial<T> remainder;
			Polynomial<T> quotient = Polynomial<T>.Divide(left, right, out remainder);
			return quotient;
		}

		public static Polynomial<T> Divide(Polynomial<T> left, Polynomial<T> right, out Polynomial<T> remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (right.Degree > left.Degree || right.CompareTo(left) == 1)
			{
				remainder = Polynomial<T>.Zero.Clone();
				return left;
			}

			Polynomial<T> lhs = left.Clone();
			Polynomial<T> rhs = right.Clone();
			Polynomial<T> total = Zero.Clone();

			int degree = Math.Max(left.Degree, right.Degree) - 1;

			bool done = false;
			while (!done)
			{
				Term<T> leftLC = lhs.Terms.Last();
				Term<T> rightLC = rhs.Terms.Last();

				Term<T> quotientTerm = Term<T>.Divide(leftLC, rightLC);

				Polynomial<T> multiplicand = new Polynomial<T>(new Term<T>[] { quotientTerm });

				total = Add(total, multiplicand);

				Polynomial<T> subtrahend = Multiply(multiplicand, rhs);
				Polynomial<T> difference = Subtract(lhs, subtrahend);

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

		public static Polynomial<T> Multiply(Polynomial<T> left, Polynomial<T> right)
		{
			if (left == null) { throw new ArgumentNullException(nameof(left)); }
			if (right == null) { throw new ArgumentNullException(nameof(right)); }

			int length = (left.Degree + right.Degree + 1);

			T[] terms = Enumerable.Repeat(GenericArithmetic<T>.Zero, length).ToArray();

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

		public static Polynomial<T> Product(params Polynomial<T>[] polys)
		{
			return Product(polys.ToList());
		}

		public static Polynomial<T> Product(IEnumerable<Polynomial<T>> polys)
		{
			Polynomial<T> result = null;

			foreach (Polynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = Polynomial<T>.Multiply(result, p);
				}
			}

			return result;
		}

		public static Polynomial<T> Square(Polynomial<T> poly)
		{
			return Polynomial<T>.Multiply(poly, poly);
		}

		public static Polynomial<T> Pow(Polynomial<T> poly, T exponent)
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

			Polynomial<T> total = Polynomial<T>.Square(poly);

			T counter = GenericArithmetic<T>.Subtract(exponent, GenericArithmetic<T>.Two);
			while (GenericArithmetic<T>.NotEqual(counter, GenericArithmetic<T>.Zero))
			{
				total = Polynomial<T>.Multiply(total, poly);
				counter = GenericArithmetic<T>.Decrement(counter);
			}

			return total;
		}

		public static Polynomial<T> Subtract(Polynomial<T> left, Polynomial<T> right)
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

			Polynomial<T> result = new Polynomial<T>(Term<T>.GetTerms(terms.ToArray()));

			return result;
		}

		public static Polynomial<T> Sum(params Polynomial<T>[] polys)
		{
			return Sum(polys.ToList());
		}

		public static Polynomial<T> Sum(IEnumerable<Polynomial<T>> polys)
		{
			Polynomial<T> result = null;

			foreach (Polynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = Polynomial<T>.Add(result, p);
				}
			}

			return result;
		}

		public static Polynomial<T> Add(Polynomial<T> left, Polynomial<T> right)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			int length = Math.Max(left.Degree, right.Degree) + 1;
			T[] terms = new T[length];
			for (int i = 0; i < terms.Length; i++)
			{
				terms[i] = GenericArithmetic<T>.Add(left[i], right[i]);
			}

			Polynomial<T> result = new Polynomial<T>(Term<T>.GetTerms(terms.ToArray()));
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

			Polynomial<T> other = obj as Polynomial<T>;

			if (other == null)
			{
				throw new ArgumentException();
			}

			return this.CompareTo(other);
		}

		public int CompareTo(Polynomial<T> other)
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

		public Polynomial<T> Clone()
		{
			var terms = _terms.Select(pt => pt.Clone()).ToArray();
			return new Polynomial<T>(terms);
		}

		public override string ToString()
		{
			List<string> stringTerms = new List<string>();

			int degree = Terms.Length;
			while (--degree >= 0)
			{
				Term<T> term = Terms[degree];

				if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.Zero))
				{
					if (term.Exponent == 0)
					{
						if (stringTerms.Count == 0)
						{
							stringTerms.Add(GenericArithmetic<T>.ToString(GenericArithmetic<T>.Zero));
						}
					}
					continue;
				}

				stringTerms.Add(term.ToString());
			}
			return string.Join(" + ", stringTerms).Replace("+ -", "- ");
		}

		#endregion

	}
}
