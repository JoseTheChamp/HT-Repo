using DwarfMaze.Dwarfs;
using DwarfMaze.Dwarfs.Strategies;
using DwarfMaze.Geometry;
using DwarfMaze.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DwarfMaze
{
	class DwarfRun
	{
		const int StepDelayMilliseconds = 100;
		const int NewDwarfIntervalMilliseconds = 5000;

		readonly DwarfFactory _factory;
		readonly IReadOnlyList<DwarfType> _insertionOrder;
		readonly ConsoleRenderer _renderer;
		readonly List<Dwarf> _dwarfs = new List<Dwarf>();

		public DwarfRun(DwarfFactory factory, IReadOnlyList<DwarfType> insertionOrder, ConsoleRenderer renderer)
		{
			_factory = factory;
			_insertionOrder = insertionOrder;
			_renderer = renderer;
		}

		public void Run()
		{
			_renderer.DrawMaze();
			Stopwatch clock = Stopwatch.StartNew();

			while (!IsOver())
			{
				if (_renderer.WindowChanged()) _renderer.Redraw(_dwarfs);

				MoveDwarfs();
				InsertDwarfs(clock.ElapsedMilliseconds);
				Thread.Sleep(StepDelayMilliseconds);
			}

			if (_renderer.WindowChanged()) _renderer.Redraw(_dwarfs);
			_renderer.DrawSummary(_dwarfs);
		}

		bool IsOver()
		{
			return _dwarfs.Count == _insertionOrder.Count && _dwarfs.All(dwarf => dwarf.State != DwarfState.Walking);
		}

		void MoveDwarfs()
		{
			for (int index = 0; index < _dwarfs.Count; index++)
			{
				Dwarf dwarf = _dwarfs[index];
				Position previousPosition = dwarf.Position;

				dwarf.Step();

				if (dwarf.Position != previousPosition)
				{
					_renderer.DrawCell(previousPosition, _dwarfs);
					_renderer.DrawCell(dwarf.Position, _dwarfs);
				}

				_renderer.DrawStatus(index, dwarf);
			}
		}

		void InsertDwarfs(long elapsedMilliseconds)
		{
			while (_dwarfs.Count < _insertionOrder.Count && elapsedMilliseconds >= _dwarfs.Count * NewDwarfIntervalMilliseconds)
			{
				Dwarf dwarf = _factory.Create(_insertionOrder[_dwarfs.Count]);
				_dwarfs.Add(dwarf);

				_renderer.DrawCell(dwarf.Position, _dwarfs);
				_renderer.DrawStatus(_dwarfs.Count - 1, dwarf);
			}
		}
	}
}
