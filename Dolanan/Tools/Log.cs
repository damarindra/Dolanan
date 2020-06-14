using System;
using System.Runtime.CompilerServices;

namespace Dolanan.Tools
{
	public static class Log
	{
		public static void Print(object msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if DEBUG
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
#endif
		}

		public static void PrintWarning(object msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
			Console.ResetColor();
#endif
		}

		public static void PrintError(object msg, [CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null, [CallerFilePath] string path = null)
		{
#if DEBUG
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("'" + msg + "'" + " (" + path + ")" + " (" + caller + ")" + " line :" + lineNumber);
			Console.ResetColor();
#endif
		}
	}
}