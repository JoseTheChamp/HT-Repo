using System;
using System.Collections.Generic;
using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwarfMaze.Tests
{
	[TestClass]
	public class MazeSolverTests
	{
		[TestMethod]
		public void TwoWays_FindsTheShorterOne()
		{
			Maze maze = MazeLoader.Parse(TestMazes.TwoWays);

			IReadOnlyList<Position> path = MazeSolver.FindShortestPath(maze, maze.Start, maze.Finish);

			Assert.AreEqual(4, path.Count);
			Assert.AreEqual(maze.Finish, path[path.Count - 1]);
		}

		[TestMethod]
		public void NoWay_ReturnsNull()
		{
			Cell[,] cells = { { Cell.Start, Cell.Wall, Cell.Finish } };
			Maze maze = new Maze(cells, new Position(0, 0), new Position(2, 0));

			Assert.IsNull(MazeSolver.FindShortestPath(maze, maze.Start, maze.Finish));
		}
	}
}
