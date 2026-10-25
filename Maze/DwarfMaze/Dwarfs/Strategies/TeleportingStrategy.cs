using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using System;
using System.Linq;

namespace DwarfMaze.Dwarfs.Strategies
{
	class TeleportingStrategy : IMovementStrategy
	{
		const int MinStepsBeforeTeleport = 30;
		const int MaxStepsBeforeTeleport = 90;

		readonly Maze _maze;
		readonly Random _random;
		int _stepsUntilTeleport;
		Position? _previous;

		public TeleportingStrategy(Maze maze, Random random)
		{
			_maze = maze;
			_random = random;
			_stepsUntilTeleport = random.Next(MinStepsBeforeTeleport, MaxStepsBeforeTeleport + 1);
		}
		public Position? GetNextPosition(Position current)
		{
			_stepsUntilTeleport--;
			Position next = _stepsUntilTeleport <= 0 ? _maze.Finish : RandomStep(current);
			_previous = current;
			return next;
		}

		Position RandomStep(Position current)
		{
			var options = Directions.All
				.Select(direction => current.Move(direction))
				.Where(position => _maze.IsWalkable(position) && position != _previous)
				.ToList();

			if (options.Count == 0) return _previous ?? current;

			return options[_random.Next(options.Count)];
		}

	}
}
