using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Editor.Attribute;
using Dolanan.Scene;
using ImGuiNET;

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
				if (_selectedActor != null)
					if (Inspector.TryGetValue(_selectedActor.GetType(), out var ev))
						if (GameMgr.Game.OnImGuiDraw != null)
							GameMgr.Game.OnImGuiDraw -= ev;

				_selectedActor = value;

				if (Inspector.TryGetValue(_selectedActor.GetType(), out var eve))
					if (GameMgr.Game.OnImGuiDraw != null)
						GameMgr.Game.OnImGuiDraw += eve;
			}
		}

		private static Actor _selectedActor = null;

		internal static Dictionary<Type, DrawImGuiWindow> Inspector = new Dictionary<Type, DrawImGuiWindow>();

		internal static void SetupInspectorWindow()
		{
			// TODO Complete the ShowInEditor, 
			var editableTypes = (from t in Assembly.GetExecutingAssembly().GetTypes()
				where t.GetCustomAttributes<ShowInEditorAttribute>().Any()
				select t).ToList();


			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				var typeDrawer = type.GetCustomAttribute<TargetTypeAttribute>();
				if (typeDrawer != null)
				{
					DrawerDictionary.Add(typeDrawer.type, type);
				}
			}

			foreach (var type in editableTypes)
			{
				var properties =
					(from p in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						where p.GetCustomAttributes<VisiblePropertyAttribute>().Any()
						select p).ToList();
				properties.Reverse();

				DrawImGuiWindow drawImGuiWindow = (() =>
				{
					if (EditorMode.SelectedActor == null)
						return;
					ImGuiNET.ImGui.SetNextWindowSize(new System.Numerics.Vector2(320, 200), ImGuiCond.Appearing);
					ImGuiNET.ImGui.Begin(_selectedActor.Name);
					
					foreach (var propertyInfo in properties)
					{
						if(DrawerDictionary.TryGetPropertyDrawer(propertyInfo.FieldType, out var drawer))
						{
							drawer.OnDrawProperty(propertyInfo, EditorMode.SelectedActor);
						}
					}

					foreach (var comp in _selectedActor.Components)
					{
						if (DrawerDictionary.TryGetComponentDrawer(comp.GetType(), out var drawer))
							drawer.OnDrawComponent(comp);
						else if(DrawerDictionary.TryGetComponentDrawer(typeof(Component), out drawer))
							//draw default component drawer
							drawer.OnDrawComponent(comp);
					}
					ImGuiNET.ImGui.End();
				});

				EditorMode.Inspector.Add(type, drawImGuiWindow);
			}
		}
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