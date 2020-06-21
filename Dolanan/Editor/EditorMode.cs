using System;
using System.Collections.Generic;
using Dolanan.Controller;
using Dolanan.Scene;

namespace Dolanan.Editor
{
#if DEBUG
	
	public static class EditorMode
	{
		public static ToolMode ToolMode = ToolMode.None;
		/// <summary>
		/// 	Enable disable editor mode
		/// </summary>
		public static bool IsActive { get; set; }

		public static Actor SelectedActor
		{
			get => _selectedActor;
			set
			{
				if(_selectedActor != null)
					if(Inspector.TryGetValue(_selectedActor.GetType(), out var ev))
						if (GameMgr.Game.OnImGuiDraw != null)
							GameMgr.Game.OnImGuiDraw -= ev;
				
				_selectedActor = value;
				
				if(Inspector.TryGetValue(_selectedActor.GetType(), out var eve))
					if (GameMgr.Game.OnImGuiDraw != null)
						GameMgr.Game.OnImGuiDraw += eve;
			}
		}

		private static Actor _selectedActor = null;

		internal static Dictionary<Type, DrawImGuiWindow> Inspector = new Dictionary<Type, DrawImGuiWindow>();
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
#endif
}