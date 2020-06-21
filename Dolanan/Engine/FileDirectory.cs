using System.IO;
using Dolanan.Editor;

namespace Dolanan.Engine
{
	public static class FileDirectory
	{
		/// <summary>
		/// 	Writing file, if run on debug, the file at project path will be automatically updated / created.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="content"></param>
		public static void WriteFile(string path, string content)
		{
			File.WriteAllText(path, content);
#if DEBUG
			File.WriteAllText(EditorSettings.ProjectPath + "/" + path, content);
#endif
		}
	}
}