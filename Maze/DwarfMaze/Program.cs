using DwarfMaze.Dwarfs;
using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Mazes;
using DwarfMaze.Rendering;
using System;
using System.IO;

namespace DwarfMaze
{
	internal static class Program
	{
		const string DefaultMazeFileName = "Maze.dat";

		static readonly DwarfType[] DwarfsOrder =
		{
			DwarfType.LeftHand,
			DwarfType.RightHand,
			DwarfType.Teleporter,
			DwarfType.PathFollower,
		};

		static int Main(string[] args)
		{
			string mazePath = args.Length > 0 ? args[0] : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultMazeFileName);

			Maze maze;
			try
			{
				maze = MazeLoader.Load(mazePath);
			}
			catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is MazeFormatException)
			{
				Console.WriteLine($"Bludiště '{mazePath}' nelze načíst: {e.Message}");
				WaitForKey();
				return 1;
			}

			ConsoleRenderer renderer = new ConsoleRenderer(maze, DwarfsOrder.Length);
			DwarfRun run = new DwarfRun(new DwarfFactory(maze, new Random()), DwarfsOrder, renderer);

			try
			{
				renderer.Prepare();
				run.Run();
			}
			finally
			{
				renderer.Restore();
			}

			WaitForKey();
			return 0;
		}

		static void WaitForKey()
		{
			if (Console.IsInputRedirected) return;

			while (Console.KeyAvailable)
			{
				Console.ReadKey(intercept: true);
			}

			Console.Write("Stiskněte libovolnou klávesu...");
			Console.ReadKey(intercept: true);
		}
	}
}
