namespace Fujtajbl.Operations
{
	class Multiplication : Operation
	{
		public override string Symbol => "*";

		public override long Calculate(int a, int b) => (long)a * b;
	}
}
