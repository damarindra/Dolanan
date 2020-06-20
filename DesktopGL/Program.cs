using System;

namespace DolananSample
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			using (var game = new TopDown())
			{
				game.Run();
			}
		}
	}
}