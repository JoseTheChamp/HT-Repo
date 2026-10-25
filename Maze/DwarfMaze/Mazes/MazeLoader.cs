using DwarfMaze.Geometry;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DwarfMaze.Mazes
{
	static class MazeLoader
	{
		public static Maze Load(string path)
		{
			return Parse(File.ReadAllLines(path));
		}

		public static Maze Parse(IReadOnlyList<string> lines)
		{
			List<string> rows = lines.Select(line => line.TrimEnd()).ToList();
			while (rows.Count > 0 && rows[rows.Count - 1].Length == 0) { 
				rows.RemoveAt(rows.Count - 1);
			}

			if (rows.Count == 0) throw new MazeFormatException("Soubor je prázdný.");

			int width = rows.Max(row => row.Length);
			Cell[,] cells = new Cell[rows.Count, width];

			Position? start = null;
			Position? finish = null;

			for (int y = 0; y < rows.Count; y++) {
				for (int x = 0; x < width; x++) {
					Position position = new Position(x, y);
					Cell cell = ParseCell(x < rows[y].Length ? rows[y][x] : ' ', position);
					cells[y, x] = cell;

					if (cell == Cell.Start)
					{
						if (start is null)
						{
							start = position;
						}
						else
						{
							throw new MazeFormatException($"Bludiště musí mít pouze jeden start. (S)");
						}
					}
					else if( cell == Cell.Finish)
					{
						if (finish is null)
						{
							finish = position;
						}
						else
						{
							throw new MazeFormatException($"Bludiště musí mít pouze jeden cíl. (F)");
						}
					}
				}
			}

			if (start is null) throw new MazeFormatException("Bludiště nemá start (S).");
			if (finish is null) throw new MazeFormatException("Bludiště nemá cíl (F).");

			Maze maze = new Maze(cells, start.Value, finish.Value);

			if (MazeSolver.FindShortestPath(maze, maze.Start, maze.Finish) is null)
			{
				throw new MazeFormatException("Cíle (F) nelze ze startu (S) dosáhnout.");
			}

			return maze;
		}

		static Cell ParseCell(char symbol, Position position)
		{
			switch (symbol)
			{
				case '#':
					return Cell.Wall;
				case ' ':
					return Cell.Empty;
				case 'S':
					return Cell.Start;
				case 'F':
					return Cell.Finish;
				default:
					throw new MazeFormatException($"Neznámý znak '{symbol}' na řádku {position.Y + 1}, sloupci {position.X + 1}.");
			}
		}
	}
}
