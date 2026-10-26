using System;
using System.Text.RegularExpressions;

namespace Fujtajbl
{
	static class ConsoleInput
	{
		static readonly Regex WholeNumberPattern = new Regex("^[+-]?[0-9]+$");
		static readonly Regex DecimalNumberPattern = new Regex("^[+-]?[0-9]*[.,][0-9]+$");

		public static int ReadInt(int min, int max)
		{
			while (true)
			{
				string input = ReadTrimmedLine();

				if (int.TryParse(input, out int number) && number >= min && number <= max)
					return number;

				Console.WriteLine(DescribeInvalidNumber(input, min, max));
			}
		}

		public static bool ReadYesNo()
		{
			while (true)
			{
				switch (ReadTrimmedLine().ToLowerInvariant())
				{
					case "a":
					case "ano":
						return true;
					case "n":
					case "ne":
						return false;
				}

				Console.WriteLine("Odpovezte prosim a (ano) nebo n (ne):");
			}
		}

		static string DescribeInvalidNumber(string input, int min, int max)
		{
			if (input.Length == 0) return "Nezadali jste zadnou hodnotu. Zadejte cele cislo:";
			if (WholeNumberPattern.IsMatch(input)) return $"Cislo {input} je mimo povoleny rozsah {min} az {max}. Zadejte cislo v tomto rozsahu:";
			if (DecimalNumberPattern.IsMatch(input)) return "Desetinna cisla nejsou podporovana. Zadejte cele cislo:";
			return $"\"{input}\" neni cele cislo. Zadejte cele cislo:";
		}

		static string ReadTrimmedLine()
		{
			return Console.ReadLine().Trim();
		}
	}
}
