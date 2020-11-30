using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public static class HelperMethods
	{
		public static Complex Parse(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				throw new ArgumentException($"Argument {nameof(s)} cannot be null, empty or whitespace");
			}

			string input = new string(s.Where(c => !char.IsWhiteSpace(c)).ToArray());
			string[] parts = input.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length <= 0 || parts.Length > 2)
			{
				throw new FormatException($"Argument {nameof(s)} not of the correct format. Expecting format: \"(1.75, 3.5)\"");
			}

			double imaginary = 0;
			double real = double.Parse(parts[0]);

			if (parts.Length == 2)
			{
				imaginary = double.Parse(parts[1]);
			}

			return new Complex(real, imaginary);
		}
	}
}
