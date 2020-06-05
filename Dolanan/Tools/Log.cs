using System;
using System.Runtime.CompilerServices;

namespace Dolanan.Tools
{
	public static class Log
	{
		public static void Print(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if LOG
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
#endif
		}
		public static void PrintWarning(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if LOG
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
			Console.ResetColor();
#endif
		}
		public static void PrintError(string msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if LOG
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
			Console.ResetColor();
#endif
		}
	}
}