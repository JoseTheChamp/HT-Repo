using DwarfMaze.Mazes;
using System;

namespace DwarfMaze.Dwarfs.Strategies
{
	class DwarfFactory
	{
		readonly Maze _maze;
		readonly Random _random;

		public DwarfFactory(Maze maze, Random random)
		{
			_maze = maze;
			_random = random;
		}

		public Dwarf Create(DwarfType kind)
		{
			switch (kind)
			{
				case DwarfType.LeftHand:
					return new Dwarf("Levak", 'L', ConsoleColor.Cyan, _maze, new WallFollowingStrategy(_maze, Hand.Left));
				case DwarfType.RightHand:
					return new Dwarf("Pravak", 'P', ConsoleColor.Yellow, _maze, new WallFollowingStrategy(_maze, Hand.Right));
				case DwarfType.Teleporter:
					return new Dwarf("Teleporter", 'T', ConsoleColor.Magenta, _maze, new TeleportingStrategy(_maze, _random));
				case DwarfType.PathFollower:
					return new Dwarf("Navigator", 'N', ConsoleColor.White, _maze, new PathFollowingStrategy(_maze));
				default:
					throw new ArgumentOutOfRangeException(nameof(kind), kind, "Neznámý druh trpaslíka.");
			}
		}
	}
}
