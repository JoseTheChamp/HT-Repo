using System;
using Fujtajbl.Operations;

namespace Fujtajbl
{
	static class Program
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
				int a = ReadNumber("Zadejte cislo A");
				int b = ReadNumber("Zadejte cislo B");

				CalculateAndPrint(a, b);
			}
			while (WantsAnotherCalculation());
		}

		static void CalculateAndPrint(int a, int b)
		{
			while (true)
			{
				Operation operation = ReadOperation();

				string error = operation.GetValidationError(a, b);
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
			for (int i = 0; i < Operations.Length; i++)
				Console.WriteLine($"{i + 1}: a {Operations[i].Symbol} b");

			int choice = ConsoleInput.ReadInt(1, Operations.Length);
			return Operations[choice - 1];
		}

		static bool WantsAnotherCalculation()
		{
			Console.WriteLine("Chcete spustit novy vypocet? (a/n)");
			return ConsoleInput.ReadYesNo();
		}
	}
}
