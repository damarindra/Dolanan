using System;
using Dolanan;

namespace Platform_OpenGL
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Console.WriteLine("Starting OpenGL");
			using (var game = new GameMin())
			{
				game.Run();
			}
		}
	}
}