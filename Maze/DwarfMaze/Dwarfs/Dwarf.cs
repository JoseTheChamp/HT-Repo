using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using System;

namespace DwarfMaze.Dwarfs
{
	class Dwarf
	{
		readonly Maze _maze;
		readonly IMovementStrategy _strategy;
		public string Name { get; }
		public char Symbol { get; }
		public ConsoleColor Color { get; }
		public int Steps { get; private set; }
		public Position Position { get; private set; }
		public DwarfState State { get; private set; } = DwarfState.Walking;

		public Dwarf(string name, char symbol, ConsoleColor color, Maze maze, IMovementStrategy strategy)
		{
			Name = name;
			Symbol = symbol;
			Color = color;
			_maze = maze;
			_strategy = strategy;
			Position = maze.Start;
		}

		public void Step()
		{
			if (State != DwarfState.Walking) return;

			Position? next = _strategy.GetNextPosition(Position);
			if (next == null)
			{
				State = DwarfState.Lost;
				return;
			}

			Position = next.Value;
			Steps++;

			if (Position == _maze.Finish) State = DwarfState.Finished;
		}
	}
}
