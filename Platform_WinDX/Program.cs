using System;
using Dolanan;
using DolananSample;

namespace Platform_WinDX
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Starting Win DX");
			using (var game = new TopDown())
			{
				game.Run();
			}
		}
	}
}