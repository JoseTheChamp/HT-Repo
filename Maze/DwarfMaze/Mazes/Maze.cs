using DwarfMaze.Geometry;

namespace DwarfMaze.Mazes
{
	class Maze
	{
		readonly Cell[,] _cells;
		public int Width => _cells.GetLength(1);
		public int Height => _cells.GetLength(0);
		public Position Start { get; }
		public Position Finish { get; }

		public Maze(Cell[,] cells, Position start, Position finish)
		{
			_cells = (Cell[,])cells.Clone();
			Start = start;
			Finish = finish;
		}

		public Cell this[Position position] => _cells[position.Y, position.X];

		public bool IsInside(Position position) => position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
		public bool IsWalkable(Position position) => IsInside(position) && this[position] != Cell.Wall;
	}
}
