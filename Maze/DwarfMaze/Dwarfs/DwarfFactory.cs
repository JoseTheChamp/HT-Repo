using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Mazes;
using System;

namespace DwarfMaze.Dwarfs
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
					return new Dwarf("Levák", 'L', ConsoleColor.Cyan, _maze, new WallFollowingStrategy(_maze, Hand.Left));
				case DwarfType.RightHand:
					return new Dwarf("Pravák", 'P', ConsoleColor.Yellow, _maze, new WallFollowingStrategy(_maze, Hand.Right));
				case DwarfType.Teleporter:
					return new Dwarf("Teleportér", 'T', ConsoleColor.Magenta, _maze, new TeleportingStrategy(_maze, _random));
				case DwarfType.PathFollower:
					return new Dwarf("Navigátor", 'N', ConsoleColor.White, _maze, new PathFollowingStrategy(_maze));
				default:
					throw new ArgumentOutOfRangeException(nameof(kind), kind, "Neznámý druh trpaslíka.");
			}
		}
	}
}
