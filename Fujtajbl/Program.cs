using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujtajbl
{
	class Program
	{
		static void Main(string[] args)
		{
Start:
			Console.WriteLine("Zadejte cislo A");
			var a = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Zadejte cislo B");
			var b = Int32.Parse(Console.ReadLine());

Select:
			Console.Write(Environment.NewLine);
			Console.Write("Vyberte operaci" + Environment.NewLine);
			Console.Write("1: a + b" + Environment.NewLine);
			Console.Write("2: a - b" + Environment.NewLine);
			Console.Write("3: a * b" + Environment.NewLine);
			Console.Write("4: a / b" + Environment.NewLine);

			var operace = Int32.Parse(Console.ReadLine());
			int found = 0;
			string vysledek = null;

			if (operace == 1)
			{
				vysledek = "Vysledek je = " + (a + b);
				found = 1;
			}
			else
			{
				if (operace == 2)
				{
					vysledek = "Vysledek je = " + (a - b);
					found = 2;
				}
				else
				{
					if (operace == 3)
					{
						vysledek = "Vysledek je = " + (a * b);
						found = 3;
					}
					else
					{
						if (operace == 4)
						{
							vysledek = "Vysledek je = " + (a / b);
							found = 4;
						}
					}
				}
			}

			if (found == 0)
			{
				Console.WriteLine("Neznama operace!");
				goto Select;
			}
			else
			{
				if (found == 1) Console.WriteLine("Operace a + b: " + vysledek);
				else
					if (found == 2) Console.WriteLine("Operace a - b: " + vysledek);
				else
					if (found == 3) Console.WriteLine("Operace a * b: " + vysledek);
				else
					if (found == 4) Console.WriteLine("Operace a / b: " + vysledek);
			}

			Console.WriteLine("Chcete spustit novy vypocet?");
			if (Console.ReadLine() == "a") goto Start;
		}
	}
}