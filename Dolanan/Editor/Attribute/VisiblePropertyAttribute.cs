using System;
using System.Reflection;
using Dolanan.Editor.ImGui;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Property)]
	public class VisiblePropertyAttribute : System.Attribute, IPropertyDrawer
	{
		private Type _type = null;
		internal Actor ObjectActor = null;
		internal PropertyInfo PropertyInfo = null;
		
		#if DEBUG
		public virtual void OnDrawProperty()
		{
			if(ObjectActor == null || PropertyInfo == null)
				return;

			_type = PropertyInfo.PropertyType;
			string name = PropertyInfo.Name;
			if (_type == typeof(string))
			{
				string oldVal = (string) PropertyInfo.GetValue(ObjectActor);
				ImGuiMg.InputText(name, ref oldVal);
				PropertyInfo.SetValue(EditorMode.SelectedActor, oldVal);
			}else if (_type == typeof(int))
			{
				int oldVal = (int) PropertyInfo.GetValue(ObjectActor);
				ImGuiNET.ImGui.InputInt(name, ref oldVal);
				PropertyInfo.SetValue(EditorMode.SelectedActor, oldVal);
			}else if (_type == typeof(Vector2))
			{
				Vector2 oldVal = (Vector2) PropertyInfo.GetValue(ObjectActor);
				ImGuiMg.Vector2(name, ref oldVal);
				PropertyInfo.SetValue(EditorMode.SelectedActor, oldVal);
			}
		}
		
		#endif
	}
}