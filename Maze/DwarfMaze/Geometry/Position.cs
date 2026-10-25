using System;

namespace DwarfMaze.Geometry
{
	readonly struct Position : IEquatable<Position>
	{

		public int X { get; }
		public int Y { get; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Position Move(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					return new Position(X, Y - 1);
				case Direction.Right:
					return new Position(X + 1, Y);
				case Direction.Down:
					return new Position(X, Y + 1);
				case Direction.Left:
					return new Position(X - 1, Y);
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		public bool Equals(Position other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj)
		{
			return obj is Position position && Equals((Position)obj);
		}

		public override int GetHashCode()
		{
			int hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			return hashCode;
		}

		public override string ToString() => $"[{X}, {Y}]";

		public static bool operator ==(Position left, Position right) => left.Equals(right);

		public static bool operator !=(Position left, Position right) => !left.Equals(right);

	}
}
