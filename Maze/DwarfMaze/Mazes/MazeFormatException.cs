using System;

namespace DwarfMaze.Mazes
{
	class MazeFormatException : Exception
	{
		public MazeFormatException()
		{
		}

		public MazeFormatException(string message) : base(message)
		{
		}

		public MazeFormatException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
