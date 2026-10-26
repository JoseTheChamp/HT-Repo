using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using System.Collections.Generic;
using System.Linq;

namespace DwarfMaze.Dwarfs.Strategies
{
	class PathFollowingStrategy : IMovementStrategy
	{
		readonly Queue<Position> _remainingPath;

		public PathFollowingStrategy(Maze maze)
		{
			IReadOnlyList<Position> path = MazeSolver.FindShortestPath(maze, maze.Start, maze.Finish);
			_remainingPath = new Queue<Position>(path ?? Enumerable.Empty<Position>());
		}

		public Position? GetNextPosition(Position current)
		{
			if (_remainingPath.Count == 0) return null;

			return _remainingPath.Dequeue();
		}
	}
}
