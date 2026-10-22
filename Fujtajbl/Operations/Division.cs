namespace Fujtajbl.Operations
{
	class Division : Operation
	{
		public override string Symbol => "/";

		public override string GetValidationError(int a, int b)
		{
			return b == 0 ? "Nulou delit nelze." : null;
		}

		public override long Calculate(int a, int b) => (long)a / b;

		public override string FormatResult(int a, int b)
		{
			var result = base.FormatResult(a, b);
			var remainder = (long)a % b;

			return remainder == 0 ? result : $"{result} (zbytek {remainder})";
		}
	}
}
