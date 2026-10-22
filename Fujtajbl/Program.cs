using System;
using System.Text.RegularExpressions;
using Fujtajbl.Operations;

namespace Fujtajbl
{
	class Program
	{
		static readonly Operation[] Operations =
		{
			new Addition(),
			new Subtraction(),
			new Multiplication(),
			new Division(),
		};

		static readonly Regex WholeNumberPattern = new Regex("^[+-]?[0-9]+$");
		static readonly Regex DecimalNumberPattern = new Regex("^[+-]?[0-9]*[.,][0-9]+$");

		static void Main(string[] args)
		{
			do
			{
				var a = ReadNumber("Zadejte cislo A");
				var b = ReadNumber("Zadejte cislo B");

				CalculateAndPrint(a, b);
			}
			while (WantsAnotherCalculation());
		}

		static void CalculateAndPrint(int a, int b)
		{
			while (true)
			{
				var operation = ReadOperation();

				var error = operation.GetValidationError(a, b);
				if (error == null)
				{
					Console.WriteLine($"{a} {operation.Symbol} {b} = {operation.FormatResult(a, b)}");
					return;
				}

				Console.WriteLine($"{error} Vyberte jinou operaci.");
			}
		}

		static int ReadNumber(string prompt)
		{
			Console.WriteLine(prompt);
			return ReadInt(int.MinValue, int.MaxValue);
		}

		static Operation ReadOperation()
		{
			Console.WriteLine();
			Console.WriteLine("Vyberte operaci");
			for (var i = 0; i < Operations.Length; i++)
				Console.WriteLine($"{i + 1}: a {Operations[i].Symbol} b");

			var choice = ReadInt(1, Operations.Length);
			return Operations[choice - 1];
		}

		static int ReadInt(int min, int max)
		{
			while (true)
			{
				var input = ReadInput();

				if (int.TryParse(input, out var number) && number >= min && number <= max)
					return number;

				Console.WriteLine(DescribeInvalidNumber(input, min, max));
			}
		}

		static string DescribeInvalidNumber(string input, int min, int max)
		{
			if (input.Length == 0)
				return "Nezadali jste zadnou hodnotu. Zadejte cele cislo:";

			if (WholeNumberPattern.IsMatch(input))
				return $"Cislo {input} je mimo povoleny rozsah {min} az {max}. Zadejte cislo v tomto rozsahu:";

			if (DecimalNumberPattern.IsMatch(input))
				return "Desetinna cisla nejsou podporovana. Zadejte cele cislo:";

			return $"\"{input}\" neni cele cislo. Zadejte cele cislo:";
		}

		static string ReadInput()
		{
			return Console.ReadLine().Trim();
		}

		static bool WantsAnotherCalculation()
		{
			Console.WriteLine("Chcete spustit novy vypocet? (a/n)");

			while (true)
			{
				switch (ReadInput().ToLowerInvariant())
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
	}
}
