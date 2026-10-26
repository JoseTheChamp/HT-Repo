using DwarfMaze.Geometry;

namespace DwarfMaze.Dwarfs.Strategies
{
	interface IMovementStrategy
	{
		Position? GetNextPosition(Position current);
	}
}
