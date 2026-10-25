using DwarfMaze.Dwarfs;
using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Mazes;
using System;
using System.IO;

namespace DwarfMaze
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			// docasny test - smazat
			string[] lines = File.ReadAllLines("Maze.dat");
			Maze maze = MazeLoader.Parse(lines);
			DwarfFactory factory = new DwarfFactory(maze, new Random(42));

			Console.WriteLine("cesta: " + MazeSolver.FindShortestPath(maze, maze.Start, maze.Finish).Count + " (134)");

			Test(maze, factory.Create(DwarfType.LeftHand), "542");
			Test(maze, factory.Create(DwarfType.RightHand), "1058");
			Test(maze, factory.Create(DwarfType.PathFollower), "134");
			Test(maze, factory.Create(DwarfType.Teleporter), "max 90");

			Maze ring = MazeLoader.Parse(new string[]
			{
				"#######",
				"#     #",
				"# ### #",
				"# # # #",
				"# ### #",
				"#  S  F",
				"#######",
			});
			DwarfFactory ringFactory = new DwarfFactory(ring, new Random(42));

			Console.WriteLine("kruh L: " + Walk(ringFactory.Create(DwarfType.LeftHand)).State + " (Lost)");
			Console.WriteLine("kruh P: " + Walk(ringFactory.Create(DwarfType.RightHand)).State + " (Finished)");

			Console.ReadKey();
		}

		static void Test(Maze maze, Dwarf dwarf, string expected)
		{
			bool atStart = dwarf.Position == maze.Start;
			Walk(dwarf);
			Console.WriteLine($"{dwarf.Name}: start {atStart}, {dwarf.State}, kroku {dwarf.Steps} ({expected})");
		}

		static Dwarf Walk(Dwarf dwarf)
		{
			for (int i = 0; i < 100000 && dwarf.State == DwarfState.Walking; i++)
				dwarf.Step();

			return dwarf;
		}
	}
}