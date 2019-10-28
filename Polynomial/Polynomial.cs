using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public partial class Polynomial<T> : IPolynomial<T>
	{
		public static IPolynomial<T> Zero = null;
		public static IPolynomial<T> One = null;
		public static IPolynomial<T> Two = null;

		public ITerm<T>[] Terms { get { return _terms.ToArray(); } }
		private List<ITerm<T>> _terms;
		public T Degree { get; private set; }

		public T this[T degree]
		{
			get
			{
				ITerm<T> term = Terms.FirstOrDefault(t => GenericArithmetic<T>.Equal(t.Exponent, degree));

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
				ITerm<T> term = Terms.FirstOrDefault(t => GenericArithmetic<T>.Equal(t.Exponent, degree));

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

		public Polynomial() { _terms = new List<ITerm<T>>() { new Term<T>(GenericArithmetic<T>.Zero, GenericArithmetic<T>.Zero) }; Degree = GenericArithmetic<T>.Zero; }

		public Polynomial(ITerm<T>[] terms)
		{
			SetTerms(terms);
		}

		public Polynomial(T n, T polynomialBase)
			: this(n, polynomialBase, GenericArithmetic<T>.Truncate(GenericArithmetic<T>.Add(GenericArithmetic<T>.Convert(GenericArithmetic<T>.Log(n, GenericArithmetic<double>.Convert<T>(polynomialBase))), GenericArithmetic<T>.One)))
		{
		}

		public Polynomial(T n, T polynomialBase, T forceDegree)
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
				Degree = GenericArithmetic<T>.Zero;
			}
		}

		private static List<ITerm<T>> GetPolynomialTerms(T value, T polynomialBase, T degree)
		{
			T d = degree; // (T)Math.Truncate(T.Log(value, (double)polynomialBase)+ 1);
			T toAdd = value;
			List<ITerm<T>> result = new List<ITerm<T>>();
			while (GenericArithmetic<T>.GreaterThanOrEqual(d, GenericArithmetic<T>.Zero) && GenericArithmetic<T>.GreaterThan(toAdd, GenericArithmetic<T>.Zero))
			{
				T placeValue = GenericArithmetic<T>.Power(polynomialBase, d);

				if (GenericArithmetic<T>.Equal(placeValue, GenericArithmetic<T>.One))
				{
					result.Add(new Term<T>(toAdd, d));
					toAdd = GenericArithmetic<T>.Zero;
				}
				else if (GenericArithmetic<T>.Equal(placeValue, toAdd))
				{
					result.Add(new Term<T>(GenericArithmetic<T>.One, d));
					toAdd = GenericArithmetic<T>.Subtract(toAdd, placeValue);
				}
				else if (GenericArithmetic<T>.LessThan(placeValue, GenericArithmetic<T>.Abs(toAdd)))
				{
					T quotient = GenericArithmetic<T>.Divide(toAdd, placeValue);

					if (GenericArithmetic<T>.GreaterThan(quotient, placeValue))
					{
						quotient = placeValue;
					}

					result.Add(new Term<T>(quotient, d));
					T toSubtract = GenericArithmetic<T>.Multiply(quotient, placeValue);

					toAdd = GenericArithmetic<T>.Subtract(toAdd, toSubtract);
				}

				d = GenericArithmetic<T>.Subtract(d, GenericArithmetic<T>.One);
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
						new Term<T>( GenericArithmetic<T>.One,GenericArithmetic<T>.One),
						new Term<T>( GenericArithmetic<T>.Negate(zero), GenericArithmetic<T>.Zero)
						}
					)
				)
			);
		}

		public static IPolynomial<T> Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "").Replace("-", "+-");
			string[] stringTerms = inputString.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

			if (!stringTerms.Any()) { throw new FormatException(); }

			List<Term<T>> polyTerms = new List<Term<T>>();
			foreach (string stringTerm in stringTerms)
			{
				string[] termParts = stringTerm.Split(new char[] { '*' });

				if (termParts.Count() != 2)
				{
					if (termParts.Count() != 1) { throw new FormatException(); }

					string temp = termParts[0];
					if (temp.All(c => char.IsDigit(c) || c == '-'))
					{
						termParts = new string[] { temp, "X^0" };
					}
					else if (temp.All(c => char.IsLetter(c) || c == '^' || c == '-' || char.IsDigit(c)))
					{
						if (temp.Contains("-"))
						{
							temp = temp.Replace("-", "");
							termParts = new string[] { "-1", temp };
						}
						else { termParts = new string[] { "1", temp }; }
					}
					else { throw new FormatException(); }
				}

				T coefficient = GenericArithmetic<T>.Parse(termParts[0]);

				string[] variableParts = termParts[1].Split(new char[] { '^' });
				if (variableParts.Count() != 2)
				{
					if (variableParts.Count() != 1) { throw new FormatException(); }

					string tmp = variableParts[0];
					if (tmp.All(c => char.IsLetter(c)))
					{
						variableParts = new string[] { tmp, "1" };
					}
				}
				T exponent = GenericArithmetic<T>.Parse(variableParts[1]);
				polyTerms.Add(new Term<T>(coefficient, exponent));
			}

			if (!polyTerms.Any()) { throw new FormatException(); }
			return new Polynomial<T>(polyTerms.ToArray());
		}

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
			T d = GenericArithmetic<T>.Zero;
			List<ITerm<T>> terms = new List<ITerm<T>>();
			foreach (ITerm<T> term in poly.Terms)
			{
				d = GenericArithmetic<T>.Subtract(term.Exponent, GenericArithmetic<T>.One);
				if (GenericArithmetic<T>.LessThan(d, GenericArithmetic<T>.Zero))
				{
					continue;
				}
				terms.Add(new Term<T>(GenericArithmetic<T>.Multiply(term.CoEfficient, term.Exponent), d));
			}

			IPolynomial<T> result = new Polynomial<T>(terms.ToArray());
			return result;
		}

		public static IPolynomial<T> MakeMonic(IPolynomial<T> polynomial, T polynomialBase)
		{
			T deg = polynomial.Degree;
			IPolynomial<T> result = new Polynomial<T>(polynomial.Terms.ToArray());
			if (GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Abs(result[deg]), GenericArithmetic<T>.One))
			{
				T toAdd = GenericArithmetic<T>.Multiply(GenericArithmetic<T>.Decrement(result[deg]), polynomialBase);
				result[deg] = GenericArithmetic<T>.One;
				result[GenericArithmetic<T>.Decrement(deg)] = GenericArithmetic<T>.Add(result[GenericArithmetic<T>.Decrement(deg)], toAdd);
			}
			return result;
		}

		public static void MakeCoefficientsSmaller(IPolynomial<T> polynomial, T polynomialBase, T maxCoefficientSize = default(T))
		{
			T maxSize = maxCoefficientSize;

			if (GenericArithmetic<T>.Equal(maxSize, default(T)))
			{
				maxSize = polynomialBase;
			}

			T pos = GenericArithmetic<T>.Zero;
			T deg = polynomial.Degree;

			while (GenericArithmetic<T>.LessThan(pos, deg))
			{
				if (GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Increment(pos), deg))
				{
					return;
				}

				T posInc = GenericArithmetic<T>.Increment(pos);

				if (GenericArithmetic<T>.GreaterThan(polynomial[pos], maxSize) &&
					GenericArithmetic<T>.GreaterThan(polynomial[pos], polynomial[posInc]))
				{
					T diff = GenericArithmetic<T>.Subtract(polynomial[pos], maxSize);

					T toAdd = GenericArithmetic<T>.Increment(GenericArithmetic<T>.Divide(diff, polynomialBase));
					T toRemove = GenericArithmetic<T>.Multiply(toAdd, polynomialBase);

					polynomial[pos] = GenericArithmetic<T>.Subtract(polynomial[pos], toRemove);
					polynomial[posInc] = GenericArithmetic<T>.Add(polynomial[posInc], toAdd);
				}

				pos = GenericArithmetic<T>.Increment(pos);
			}
		}

		#endregion

		#region Arithmetic

		public static IPolynomial<T> GCD(IPolynomial<T> left, IPolynomial<T> right)
		{
			IPolynomial<T> a = left.Clone();
			IPolynomial<T> b = right.Clone();

			if (GenericArithmetic<T>.GreaterThan(b.Degree, a.Degree))
			{
				IPolynomial<T> swap = b;
				b = a;
				a = swap;
			}

			while (!(b.Terms.Length == 0 || GenericArithmetic<T>.Equal(b[GenericArithmetic<T>.Zero], GenericArithmetic<T>.Zero)))
			{
				IPolynomial<T> temp = a;
				a = b;
				b = Field<T>.Modulus(temp, b);
			}

			if (GenericArithmetic<T>.Equal(a.Degree, GenericArithmetic<T>.Zero))
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

		private static ITerm<T> Divide(ITerm<T> left, ITerm<T> right)
		{
			T coefficent = GenericArithmetic<T>.Divide(left.CoEfficient, right.CoEfficient);
			T exponent = GenericArithmetic<T>.Subtract(left.Exponent, right.Exponent);
			return new Term<T>(coefficent, exponent);
		}

		public static IPolynomial<T> Divide(IPolynomial<T> left, IPolynomial<T> right, out IPolynomial<T> remainder)
		{
			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));
			if (GenericArithmetic<T>.GreaterThan(right.Degree, left.Degree) || right.CompareTo(left) == 1)
			{
				remainder = Polynomial<T>.Zero;
				return left;
			}

			IPolynomial<T> lhs = left.Clone();
			IPolynomial<T> rhs = right.Clone();
			IPolynomial<T> total = Zero;

			bool done = false;
			while (!done)
			{
				ITerm<T> leftLC = lhs.Terms.Last();
				ITerm<T> rightLC = rhs.Terms.Last();

				ITerm<T> quotientTerm = Divide(leftLC, rightLC);
				IPolynomial<T> multiplicand = new Polynomial<T>(new ITerm<T>[] { quotientTerm });

				total = Add(total, multiplicand);

				IPolynomial<T> subtrahend = Multiply(multiplicand, rhs);
				IPolynomial<T> difference = Subtract(lhs, subtrahend);

				lhs = difference.Clone();

				if (GenericArithmetic<T>.LessThan(lhs.Degree, GenericArithmetic<T>.One))
				{
					T l = lhs[GenericArithmetic<T>.Zero];

					if (GenericArithmetic<T>.Equal(l, GenericArithmetic<T>.Zero))
					{
						done = true;
					}
					else if (GenericArithmetic<T>.LessThan(rhs.Degree, GenericArithmetic<T>.One))
					{
						T r = rhs[GenericArithmetic<T>.Zero];
						if (GenericArithmetic<T>.NotEqual(GenericArithmetic<T>.GCD(l, r), GenericArithmetic<T>.One))
						{
							continue;
						}
					}

					done = true;
				}
			}

			remainder = lhs.Clone();
			return total.Clone();
		}

		public static IPolynomial<T> Multiply(IPolynomial<T> left, IPolynomial<T> right)
		{
			if (left == null) { throw new ArgumentNullException(nameof(left)); }
			if (right == null) { throw new ArgumentNullException(nameof(right)); }

			int length = GenericArithmetic<int>.Convert(GenericArithmetic<T>.Increment(GenericArithmetic<T>.Add(left.Degree, right.Degree)));

			T[] terms = new T[length];

			for (T i = GenericArithmetic<T>.Zero; GenericArithmetic<T>.LessThanOrEqual(i, left.Degree); i = GenericArithmetic<T>.Increment(i))
			{
				for (T j = GenericArithmetic<T>.Zero; GenericArithmetic<T>.LessThanOrEqual(j, right.Degree); j = GenericArithmetic<T>.Increment(j))
				{
					int index = GenericArithmetic<int>.Convert(GenericArithmetic<T>.Add(i, j));
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
				return new Polynomial<T>(new Term<T>[] { new Term<T>(GenericArithmetic<T>.One, GenericArithmetic<T>.Zero) });
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

			int length = GenericArithmetic<int>.Convert(GenericArithmetic<T>.Increment(GenericArithmetic<T>.Max(left.Degree, right.Degree)));
			T[] terms = new T[length];
			for (T i = GenericArithmetic<T>.Zero; GenericArithmetic<T>.LessThan(i, GenericArithmetic<T>.Convert(terms.Length)); i = GenericArithmetic<T>.Increment(i))
			{
				T l = left[i];
				T r = right[i];

				terms[GenericArithmetic<int>.Convert(i)] = GenericArithmetic<T>.Subtract(l, r);
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

			int length = GenericArithmetic<int>.Convert(GenericArithmetic<T>.Increment(GenericArithmetic<T>.Max(left.Degree, right.Degree)));
			T[] terms = new T[length];
			for (T i = GenericArithmetic<T>.Zero; GenericArithmetic<T>.LessThan(i, GenericArithmetic<T>.Convert(terms.Length)); i = GenericArithmetic<T>.Increment(i))
			{
				terms[GenericArithmetic<int>.Convert(i)] = GenericArithmetic<T>.Add(left[i], right[i]);
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

			if (GenericArithmetic<T>.NotEqual(other.Degree, this.Degree))
			{
				if (GenericArithmetic<T>.GreaterThan(other.Degree, this.Degree))
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				T counter = this.Degree;

				while (GenericArithmetic<T>.GreaterThanOrEqual(counter, GenericArithmetic<T>.Zero))
				{
					T thisCoefficient = this[counter];
					T otherCoefficient = other[counter];

					if (GenericArithmetic<T>.LessThan(thisCoefficient, otherCoefficient))
					{
						return -1;
					}
					else if (GenericArithmetic<T>.GreaterThan(thisCoefficient, otherCoefficient))
					{
						return 1;
					}

					counter = GenericArithmetic<T>.Decrement(counter);
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
			T degree = GenericArithmetic<T>.Convert(Terms.Length);
			while (GenericArithmetic<T>.GreaterThanOrEqual((degree = GenericArithmetic<T>.Decrement(degree)), GenericArithmetic<T>.Zero))
			{
				string termString = "";
				ITerm<T> term = Terms[GenericArithmetic<int>.Convert(degree)];

				if (GenericArithmetic<T>.Equal(term.CoEfficient, GenericArithmetic<T>.Zero))
				{
					if (GenericArithmetic<T>.Equal(term.Exponent, GenericArithmetic<T>.Zero))
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

				if (GenericArithmetic<T>.Equal(term.Exponent, GenericArithmetic<T>.Zero))
				{
					stringTerms.Add($"{term.CoEfficient}");
				}
				else if (GenericArithmetic<T>.Equal(term.Exponent, GenericArithmetic<T>.One))
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
