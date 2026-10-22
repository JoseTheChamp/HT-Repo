using System;
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
			return ConsoleInput.ReadInt(int.MinValue, int.MaxValue);
		}

		static Operation ReadOperation()
		{
			Console.WriteLine();
			Console.WriteLine("Vyberte operaci");
			for (var i = 0; i < Operations.Length; i++)
				Console.WriteLine($"{i + 1}: a {Operations[i].Symbol} b");

			var choice = ConsoleInput.ReadInt(1, Operations.Length);
			return Operations[choice - 1];
		}

		static bool WantsAnotherCalculation()
		{
			Console.WriteLine("Chcete spustit novy vypocet? (a/n)");
			return ConsoleInput.ReadYesNo();
		}
	}
}
