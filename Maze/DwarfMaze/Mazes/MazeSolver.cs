using DwarfMaze.Geometry;
using System.Collections.Generic;

namespace DwarfMaze.Mazes
{
	static class MazeSolver
	{
		public static IReadOnlyList<Position> FindShortestPath(Maze maze, Position from, Position to)
		{
			Position?[,] camefrom = new Position?[maze.Height, maze.Width];
			Queue<Position> queue = new Queue<Position>();

			camefrom[from.Y, from.X] = from;
			queue.Enqueue(from);

			while (queue.Count > 0)
			{
				Position current = queue.Dequeue();
				if (current == to) return BuildPath(camefrom, from, to);

				foreach (Direction direction in Directions.All)
				{
					Position next = current.Move(direction);
					if (!maze.IsWalkable(next) || camefrom[next.Y, next.X] != null) continue;
					camefrom[next.Y, next.X] = current;
					queue.Enqueue(next);
				}
			}

			return null;
		}

		static IReadOnlyList<Position> BuildPath(Position?[,] cameFrom, Position from, Position to)
		{
			List<Position> path = new List<Position>();
			Position position = to;

			while (position != from)
			{
				path.Add(position);
				position = cameFrom[position.Y, position.X].Value;
			}

			path.Reverse();
			return path;
		}
	}
}
