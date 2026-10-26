using System;
using System.Runtime.InteropServices;

namespace DwarfMaze.Rendering
{
	// Zapne zpracování VT (escape) sekvencí. Windows Terminal ho má zapnuté vždy,
	// klasická konzole ve výchozím stavu ne – sekvenci by jen vypsala jako text.
	static class VirtualTerminal
	{
		const int StdOutputHandle = -11;
		const uint EnableVirtualTerminalProcessing = 0x0004;

		[DllImport("kernel32.dll")]
		static extern IntPtr GetStdHandle(int handle);

		[DllImport("kernel32.dll")]
		static extern bool GetConsoleMode(IntPtr handle, out uint mode);

		[DllImport("kernel32.dll")]
		static extern bool SetConsoleMode(IntPtr handle, uint mode);

		public static bool Enable()
		{
			IntPtr output = GetStdHandle(StdOutputHandle);
			return GetConsoleMode(output, out uint mode) && SetConsoleMode(output, mode | EnableVirtualTerminalProcessing);
		}
	}
}
