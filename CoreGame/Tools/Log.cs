using System;
using System.Runtime.CompilerServices;

namespace CoreGame.Tools
{
	public static class Log
	{
		public static void Print(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
			Console.WriteLine("'" + msg + "' at line " + lineNumber + " (" + caller + ")" + " (" + path + ")");
		}
		public static void PrintWarning(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.WriteLine("'" + msg + "' at line " + lineNumber + " (" + caller + ")"+ " (" + path + ")");
			Console.ResetColor();
		}
		public static void PrintError(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.WriteLine("'" + msg + "' at line " + lineNumber + " (" + caller + ")"+ " (" + path + ")");
			Console.ResetColor();
		}
	}
}