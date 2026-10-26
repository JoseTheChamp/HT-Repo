using DwarfMaze.Dwarfs;
using DwarfMaze.Geometry;
using DwarfMaze.Mazes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DwarfMaze.Rendering
{
	// Bludiště se vykreslí jen jednou. Potom se překreslují pouze buňky, kde se něco změnilo a řádky trpaslíků.
	class ConsoleRenderer
	{
		const int StatusWidth = 80;
		const ConsoleColor WallColor = ConsoleColor.DarkGray;
		const ConsoleColor StartColor = ConsoleColor.Green;
		const ConsoleColor FinishColor = ConsoleColor.Red;

		readonly Maze _maze;
		readonly string[] _lastStatusTexts;
		int _drawnWidth;
		int _drawnHeight;
		bool _redrawNeeded;

		int StatusTop => _maze.Height + 1;
		int SummaryTop => StatusTop + _lastStatusTexts.Length + 1;
		int RequiredWidth => Math.Max(_maze.Width, StatusWidth);
		int RequiredHeight => SummaryTop + 2;

		public ConsoleRenderer(Maze maze, int dwarfCount)
		{
			_maze = maze;
			_lastStatusTexts = new string[dwarfCount];
		}

		public void Prepare()
		{
			Console.Title = "Trpaslíci v bludišti";
			Console.CursorVisible = false;

			ResizeWindow(RequiredWidth, RequiredHeight);
			WaitUntilWindowFits();
			Console.Clear();
		}

		public void Restore()
		{
			Console.ResetColor();
			Console.CursorVisible = true;
			MoveCursor(0, SummaryTop + 1);
		}

		public void DrawMaze()
		{
			_drawnWidth = Console.WindowWidth;
			_drawnHeight = Console.WindowHeight;

			Console.ForegroundColor = WallColor;
			char[] row = new char[_maze.Width];

			for (int y = 0; y < _maze.Height; y++)
			{
				for (int x = 0; x < _maze.Width; x++)
				{
					row[x] = ToSymbol(_maze[new Position(x, y)]);
				}

				if (!MoveCursor(0, y)) break;
				Console.Write(row);
			}

			Console.ResetColor();
			DrawMazeCell(_maze.Start);
			DrawMazeCell(_maze.Finish);
		}

		public bool WindowChanged() => _redrawNeeded || Console.WindowWidth != _drawnWidth || Console.WindowHeight != _drawnHeight;

		public void Redraw(IReadOnlyList<Dwarf> dwarfs)
		{
			WaitUntilWindowFits();
			_redrawNeeded = false;
			Console.Clear();
			DrawMaze();

			for (int index = 0; index < dwarfs.Count; index++)
			{
				DrawCell(dwarfs[index].Position, dwarfs);
				_lastStatusTexts[index] = null;
				DrawStatus(index, dwarfs[index]);
			}
		}

		public void DrawCell(Position position, IEnumerable<Dwarf> dwarfs)
		{
			Dwarf dwarf = dwarfs.LastOrDefault(d => d.State != DwarfState.Finished && d.Position == position);
			if (dwarf != null)
			{
				Write(position, dwarf.Symbol, dwarf.Color);
			}
			else
			{
				DrawMazeCell(position);
			}
		}

		public void DrawStatus(int index, Dwarf dwarf)
		{
			string text = $"{dwarf.Symbol}:  {dwarf.Name,-10}  pozice: {dwarf.Position,-9}  krok: {dwarf.Steps,4}  {Describe(dwarf.State)}";
			if (text == _lastStatusTexts[index] || !MoveCursor(0, StatusTop + index)) return;

			_lastStatusTexts[index] = text;
			Console.ForegroundColor = dwarf.Color;
			Console.Write(text.PadRight(StatusWidth));
			Console.ResetColor();
		}

		public void DrawSummary(IReadOnlyCollection<Dwarf> dwarfs)
		{
			int lostCount = dwarfs.Count(dwarf => dwarf.State == DwarfState.Lost);

			if (!MoveCursor(0, SummaryTop)) return;
			Console.Write(lostCount == 0
				? "Všichni trpaslíci jsou v cíli."
				: $"V cíli: {dwarfs.Count - lostCount}, zabloudilo: {lostCount}.");
		}

		void DrawMazeCell(Position position)
		{
			Cell cell = _maze[position];
			Write(position, ToSymbol(cell), ColorOf(cell));
		}

		void Write(Position position, char symbol, ConsoleColor color)
		{
			if (!MoveCursor(position.X, position.Y)) return;

			Console.ForegroundColor = color;
			Console.Write(symbol);
			Console.ResetColor();
		}

		bool MoveCursor(int left, int top)
		{
			try
			{
				Console.SetCursorPosition(left, top);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				_redrawNeeded = true;
				return false;
			}
		}

		static char ToSymbol(Cell cell)
		{
			switch (cell)
			{
				case Cell.Wall:
					return '#';
				case Cell.Start:
					return 'S';
				case Cell.Finish:
					return 'F';
				default:
					return ' ';
			}
		}

		static ConsoleColor ColorOf(Cell cell)
		{
			switch (cell)
			{
				case Cell.Start:
					return StartColor;
				case Cell.Finish:
					return FinishColor;
				default:
					return WallColor;
			}
		}

		static string Describe(DwarfState state)
		{
			switch (state)
			{
				case DwarfState.Walking:
					return "hledá cestu";
				case DwarfState.Finished:
					return "v cíli";
				default:
					return "zabloudil, do cíle nedojde";
			}
		}

		void WaitUntilWindowFits()
		{
			for (int i = 0; i < 20 && !WindowFits(); i++)
			{
				Thread.Sleep(50);
			}

			while (!WindowFits())
			{
				Console.Clear();
				Console.WriteLine("Bludiště se do okna nevejde.");
				Console.WriteLine($"Okno má {Console.WindowWidth} × {Console.WindowHeight} znaků, potřeba je alespoň {RequiredWidth} × {RequiredHeight}.");
				Console.WriteLine("Zvětšete nebo maximalizujte okno, případně zmenšete písmo");
				Console.WriteLine("(Ctrl a minus, nebo Ctrl a kolečko myši).");

				WaitForWindowResize();
			}

			Thread.Sleep(300);
		}

		bool WindowFits() => Console.WindowWidth >= RequiredWidth && Console.WindowHeight >= RequiredHeight;

		static void WaitForWindowResize()
		{
			int width = Console.WindowWidth;
			int height = Console.WindowHeight;

			while (Console.WindowWidth == width && Console.WindowHeight == height)
			{
				Thread.Sleep(200);
			}
		}

		// Velikost okna nastaví VT sekvence ESC[8;výška;šířka t
		// Console.SetWindowSize mi nezafungovala
		static void ResizeWindow(int width, int height)
		{
			if (VirtualTerminal.Enable()) Console.Write($"\u001b[8;{height};{width}t");
		}
	}
}
