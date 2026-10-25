using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using System.Collections.Generic;
using System.Linq;

namespace DwarfMaze.Dwarfs.Strategies
{
	class WallFollowingStrategy : IMovementStrategy
	{
		readonly Maze _maze;
		readonly Hand _hand;
		readonly HashSet<(Position, Direction)> _visitedStates = new HashSet<(Position, Direction)>();
		Direction _facing;

		public WallFollowingStrategy(Maze maze, Hand hand)
		{
			_maze = maze;
			_hand = hand;
			_facing = Directions.All.FirstOrDefault(direction => maze.IsWalkable(maze.Start.Move(direction)));
		}

		public Position? GetNextPosition(Position current)
		{
			Direction[] candidates = new[] { TowardHand(_facing), _facing, AwayFromHand(_facing), _facing.TurnBack() };

			foreach (Direction direction in candidates)
			{
				Position next = current.Move(direction);
				if (!_maze.IsWalkable(next)) continue;

				_facing = direction;

				// Detekce ztracení - trpaslík chodí v cyklu
				if (!_visitedStates.Add((next, direction))) return null;
				return next;
			}

			return null;
		}

		Direction TowardHand(Direction direction) => _hand == Hand.Left ? direction.TurnLeft() : direction.TurnRight();

		Direction AwayFromHand(Direction direction) => _hand == Hand.Left ? direction.TurnRight() : direction.TurnLeft();
	}
}
