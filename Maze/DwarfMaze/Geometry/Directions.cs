using System.Collections.Generic;

namespace DwarfMaze.Geometry
{
	static class Directions
	{
		const int Count = 4;

		public static readonly IReadOnlyList<Direction> All = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

		public static Direction TurnRight(this Direction direction) => (Direction)(((int)direction + 1) % Count);
		public static Direction TurnLeft(this Direction direction) => (Direction)(((int)direction + Count - 1) % Count);
		public static Direction TurnBack(this Direction direction) => (Direction)(((int)direction + 2) % Count);

	}
}
