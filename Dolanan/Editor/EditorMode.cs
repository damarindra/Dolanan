using System;

namespace Dolanan.Editor
{
	public static class EditorMode
	{
		public static ToolMode ToolMode = ToolMode.None;
		/// <summary>
		/// 	Enable disable editor mode
		/// </summary>
		public static bool IsActive { get; set; }
	}

	[Flags]
	public enum ToolMode
	{
		None = 0,
		Select = 1,
		Location = 2,
		Rotation = 4,
		Scale = 8
	}
}