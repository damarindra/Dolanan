using System;
using Dolanan;

namespace Platform_WinDX
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Starting Win DX");
			using (var game = new GameMin())
			{
				game.Run();
			}
		}
	}
}