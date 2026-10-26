using DwarfMaze.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwarfMaze.Tests
{
	[TestClass]
	public class DirectionsTests
	{
		[DataTestMethod]
		[DataRow(Direction.Up, Direction.Left)]
		[DataRow(Direction.Right, Direction.Up)]
		[DataRow(Direction.Down, Direction.Right)]
		[DataRow(Direction.Left, Direction.Down)]
		public void TurnLeft(Direction direction, Direction expected)
		{
			Assert.AreEqual(expected, direction.TurnLeft());
		}

		[DataTestMethod]
		[DataRow(Direction.Up, Direction.Right)]
		[DataRow(Direction.Right, Direction.Down)]
		[DataRow(Direction.Down, Direction.Left)]
		[DataRow(Direction.Left, Direction.Up)]
		public void TurnRight(Direction direction, Direction expected)
		{
			Assert.AreEqual(expected, direction.TurnRight());
		}

		[DataTestMethod]
		[DataRow(Direction.Up, Direction.Down)]
		[DataRow(Direction.Right, Direction.Left)]
		[DataRow(Direction.Down, Direction.Up)]
		[DataRow(Direction.Left, Direction.Right)]
		public void TurnBack(Direction direction, Direction expected)
		{
			Assert.AreEqual(expected, direction.TurnBack());
		}

		[TestMethod]
		public void Move_Up_DecreasesY()
		{
			Position position = new Position(5, 5);

			Assert.AreEqual(new Position(5, 4), position.Move(Direction.Up));
			Assert.AreEqual(new Position(5, 5), position);
		}
	}
}
