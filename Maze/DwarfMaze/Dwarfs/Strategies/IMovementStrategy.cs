using DwarfMaze.Geometry;

namespace DwarfMaze.Dwarfs
{
	interface IMovementStrategy
	{
		Position? GetNextPosition(Position current);
	}
}
