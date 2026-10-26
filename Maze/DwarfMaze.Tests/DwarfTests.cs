using System;
using DwarfMaze.Dwarfs;
using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwarfMaze.Tests
{
	[TestClass]
	public class DwarfTests
	{
		[TestMethod]
		public void NewDwarf_StandsOnStart()
		{
			Maze maze = MazeLoader.Parse(TestMazes.Corridor);

			Dwarf dwarf = new DwarfFactory(maze, new Random(42)).Create(DwarfType.LeftHand);

			Assert.AreEqual(maze.Start, dwarf.Position);
			Assert.AreEqual(DwarfState.Walking, dwarf.State);
		}

		[TestMethod]
		public void Corridor_EveryDwarfNeeds12Steps()
		{
			Maze maze = MazeLoader.Parse(TestMazes.Corridor);
			DwarfFactory factory = new DwarfFactory(maze, new Random(42));

			foreach (DwarfType type in new[] { DwarfType.LeftHand, DwarfType.RightHand, DwarfType.Teleporter, DwarfType.PathFollower })
			{
				Dwarf dwarf = Walk(factory.Create(type));

				Assert.AreEqual(DwarfState.Finished, dwarf.State, type.ToString());
				Assert.AreEqual(12, dwarf.Steps, type.ToString());
			}
		}

		[TestMethod]
		public void PathFollower_TakesShorterWay()
		{
			Maze maze = MazeLoader.Parse(TestMazes.TwoWays);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.PathFollower));

			Assert.AreEqual(DwarfState.Finished, dwarf.State);
			Assert.AreEqual(4, dwarf.Steps);
		}

		[TestMethod]
		public void RightHand_TakesShortWay()
		{
			Maze maze = MazeLoader.Parse(TestMazes.TwoWays);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.RightHand));

			Assert.AreEqual(DwarfState.Finished, dwarf.State);
			Assert.AreEqual(4, dwarf.Steps);
		}

		[TestMethod]
		public void LeftHand_GoesAroundTheBlock()
		{
			Maze maze = MazeLoader.Parse(TestMazes.TwoWays);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.LeftHand));

			Assert.AreEqual(DwarfState.Finished, dwarf.State);
			Assert.AreEqual(12, dwarf.Steps);
		}

		[TestMethod]
		public void LeftHand_AroundPillar_GetsLost()
		{
			Maze maze = MazeLoader.Parse(TestMazes.Ring);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.LeftHand));

			Assert.AreEqual(DwarfState.Lost, dwarf.State);
		}

		[TestMethod]
		public void RightHand_AroundPillar_ReachesFinish()
		{
			Maze maze = MazeLoader.Parse(TestMazes.Ring);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.RightHand));

			Assert.AreEqual(DwarfState.Finished, dwarf.State);
			Assert.AreEqual(3, dwarf.Steps);
		}

		[TestMethod]
		public void Teleporter_InCorridor_WalksToFinishBeforeTeleporting()
		{
			Maze maze = MazeLoader.Parse(TestMazes.Corridor);

			Dwarf dwarf = Walk(new DwarfFactory(maze, new Random(42)).Create(DwarfType.Teleporter));

			Assert.AreEqual(DwarfState.Finished, dwarf.State);
			Assert.AreEqual(12, dwarf.Steps);
		}

		[TestMethod]
		public void StartNextToFinish_EveryDwarfNeedsOneStep()
		{
			Maze maze = MazeLoader.Parse(new[] { "####", "#SF#", "####" });
			DwarfFactory factory = new DwarfFactory(maze, new Random(42));

			foreach (DwarfType type in new[] { DwarfType.LeftHand, DwarfType.RightHand, DwarfType.Teleporter, DwarfType.PathFollower })
			{
				Dwarf dwarf = Walk(factory.Create(type));

				Assert.AreEqual(DwarfState.Finished, dwarf.State, type.ToString());
				Assert.AreEqual(1, dwarf.Steps, type.ToString());
			}
		}

		[TestMethod]
		public void WalledInFinish_WallFollowersAndNavigatorGetLost()
		{
			Cell[,] cells = { { Cell.Start, Cell.Empty, Cell.Wall, Cell.Finish } };
			Maze maze = new Maze(cells, new Position(0, 0), new Position(3, 0));
			DwarfFactory factory = new DwarfFactory(maze, new Random(42));

			foreach (DwarfType type in new[] { DwarfType.LeftHand, DwarfType.RightHand, DwarfType.PathFollower })
			{
				Assert.AreEqual(DwarfState.Lost, Walk(factory.Create(type)).State, type.ToString());
			}
		}

		static Dwarf Walk(Dwarf dwarf)
		{
			for (int i = 0; i < 100000 && dwarf.State == DwarfState.Walking; i++)
				dwarf.Step();

			return dwarf;
		}
	}
}
