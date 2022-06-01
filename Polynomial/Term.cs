using System;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExtendedArithmetic
{
	/// <summary>
	/// Represents a single term in a <see cref="ExtendedArithmetic.Polynomial"/>.
	/// </summary>
	/// <seealso cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Term}" />
	public class Term : ICloneable<Term>
	{
		/// <summary>
		/// The coefficient.
		/// </summary>
		[DataMember]
		public BigInteger CoEfficient { get; set; }

		/// <summary>
		/// The exponent.
		/// </summary>
		[DataMember]
		public int Exponent { get; set; }

		[IgnoreDataMember]
		private const string IndeterminateSymbol = "X";

		public Term()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Term"/> class with the specified coefficient and exponent.
		/// </summary>
		public Term(BigInteger coefficient, int exponent)
		{
			Exponent = exponent;
			CoEfficient = coefficient.Clone();
		}

		/// <summary>
		/// Converts an array of <see cref="System.Numerics.BigInteger" /> into an array of <see cref="ExtendedArithmetic.Term"/>.
		/// </summary>
		public static Term[] GetTerms(BigInteger[] terms)
		{
			List<Term> results = new List<Term>();

			int degree = 0;
			foreach (BigInteger term in terms)
			{
				results.Add(new Term(term.Clone(), degree));

				degree += 1;
			}

			return results.ToArray();
		}

		public static Term Parse(string input)
		{
			string inputString = input.Replace(" ", "").Replace("−", "-");
			string[] termParts = inputString.Split(new char[] { '*' });

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

			BigInteger coefficient = BigInteger.Parse(termParts[0]);

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
			int exponent = int.Parse(variableParts[1]);

			return new Term(coefficient, exponent);
		}

		/// <summary>
		/// Evaluates the value of this Term given the specified indeterminate.
		/// </summary>
		public BigInteger Evaluate(BigInteger indeterminate)
		{
			return BigInteger.Multiply(CoEfficient, BigInteger.Pow(indeterminate, Exponent));
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public Term Clone()
		{
			return new Term(this.CoEfficient.Clone(), this.Exponent);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return new Tuple<BigInteger, int>(CoEfficient, Exponent).GetHashCode();
		}

		/// <summary>
		/// Returns the string representation of this Term.
		/// </summary>
		public override string ToString()
		{
			return $"{CoEfficient}*{IndeterminateSymbol}^{Exponent}";
		}
	}
}
