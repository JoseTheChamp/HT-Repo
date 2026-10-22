namespace Fujtajbl.Operations
{
	abstract class Operation
	{
		public abstract string Symbol { get; }

		// Vrací long, protože výsledek dvou intů (např. 100000 * 100000) se do intu nemusí vejít.
		public abstract long Calculate(int a, int b);

		// Vrací důvod, proč operaci s danými čísly nelze provést, nebo null, pokud je vše v pořádku.
		public virtual string GetValidationError(int a, int b)
		{
			return null;
		}

		// Vrací výsledek jako text pro uživatele; konkrétní operace k němu může přidat upřesnění.
		public virtual string FormatResult(int a, int b)
		{
			return Calculate(a, b).ToString();
		}
	}
}
