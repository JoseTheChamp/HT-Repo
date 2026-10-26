using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwarfMaze.Tests
{
	[TestClass]
	public class MazeLoaderTests
	{
		[TestMethod]
		public void Parse_TrailingSpacesAndEmptyLines_AreIgnored()
		{
			Maze maze = MazeLoader.Parse(new[] { "#####", "#S F#   ", "#####", "", "" });

			Assert.AreEqual(5, maze.Width);
			Assert.AreEqual(3, maze.Height);
		}

		[TestMethod]
		public void Parse_ShorterLine_IsFilledWithEmptyCells()
		{
			Maze maze = MazeLoader.Parse(new[] { "#####", "#S  F", "###" });

			Assert.AreEqual(Cell.Empty, maze[new Position(4, 2)]);
		}

		[TestMethod]
		public void Parse_EmptyFile_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new string[0]));
		}

		[TestMethod]
		public void Parse_NoStart_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#  F#", "#####" }));
		}

		[TestMethod]
		public void Parse_NoFinish_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#S  #", "#####" }));
		}

		[TestMethod]
		public void Parse_TwoStarts_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#S S#", "#  F#", "#####" }));
		}

		[TestMethod]
		public void Parse_TwoFinishes_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#SFF#", "#####" }));
		}

		[TestMethod]
		public void Parse_UnknownCharacter_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#S x#", "#  F#", "#####" }));
		}

		[TestMethod]
		public void Parse_FinishBehindWall_Throws()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#S#F#", "#####" }));
		}

		[TestMethod]
		public void Parse_WalledInFinish_Throws()
		{
			string[] lines =
			{
				"#S#####",
				"#     #",
				"# ### #",
				"# #F# #",
				"# ### #",
				"#     #",
				"#######",
			};

			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(lines));
		}

		[TestMethod]
		public void Parse_WalledInStart_Throws()
		{
			string[] lines =
			{
				"#######",
				"#     #",
				"# ### #",
				"# #S# #",
				"# ### #",
				"#    F#",
				"#######",
			};

			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(lines));
		}

		[TestMethod]
		public void Parse_FinishInClosedRoom_Throws()
		{
			string[] lines =
			{
				"#S######",
				"#   #  #",
				"#   # F#",
				"#####  #",
				"########",
			};

			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(lines));
		}

		[TestMethod]
		public void Parse_FinishOnlyDiagonally_Throws()
		{
			string[] lines =
			{
				"####",
				"#S##",
				"##F#",
				"####",
			};

			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(lines));
		}

		[TestMethod]
		public void Parse_LowercaseStart_IsUnknownCharacter()
		{
			Assert.ThrowsException<MazeFormatException>(() => MazeLoader.Parse(new[] { "#####", "#s F#", "#####" }));
		}
	}
}
