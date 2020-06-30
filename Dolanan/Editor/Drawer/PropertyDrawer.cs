using System;
using System.Reflection;
using Dolanan.Editor.Attribute;
using Dolanan.Editor.ImGui;
using Microsoft.Xna.Framework;

namespace Dolanan.Editor.Drawer
{
	public class PropertyDrawer : IPropertyDrawer
	{
		public virtual void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if (obj == null)
				return;
		}
	}
	
	[TargetType(typeof(int))]
	public class IntDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			int oldVal = (int)fieldInfo.GetValue(obj);
			int newVal = oldVal;
			ImGuiNET.ImGui.DragInt(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);
			
		}
	}
	[TargetType(typeof(float))]
	public class FloatDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			float oldVal = (float)fieldInfo.GetValue(obj);
			float newVal = oldVal;
			ImGuiNET.ImGui.DragFloat(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);

		}
	}
	[TargetType(typeof(string))]
	public class StringDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			string oldVal = (string)fieldInfo.GetValue(obj);
			string newVal = oldVal;
			ImGuiMg.InputText(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);

		}
	}
	[TargetType(typeof(bool))]
	public class BoolDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			bool oldVal = (bool)fieldInfo.GetValue(obj);
			bool newVal = oldVal;
			ImGuiNET.ImGui.Checkbox(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);

		}
	}
	[TargetType(typeof(Vector2))]
	public class Vector2Drawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			Vector2 oldVal = (Vector2)fieldInfo.GetValue(obj);
			Vector2 newVal = oldVal;
			ImGuiMg.DragVector2(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);

		}
	}
	[TargetType(typeof(Point))]
	public class PointDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			Point oldVal = (Point)fieldInfo.GetValue(obj);
			Point newVal = oldVal;
			ImGuiMg.DragPoint(fieldInfo.Name, ref newVal);
			if(newVal != oldVal)
				fieldInfo.SetValue(obj, newVal);
		}
	}
	[TargetType(typeof(Rectangle))]
	public class RectangleDrawer : PropertyDrawer
	{
		public override void OnDrawProperty(FieldInfo fieldInfo, Object obj)
		{
			if(obj == null)
				return;
			
			Rectangle oldVal = (Rectangle)fieldInfo.GetValue(obj);
			Point loc = oldVal.Location;
			Point size = oldVal.Size;
			ImGuiNET.ImGui.Text(fieldInfo.Name);
			ImGuiMg.DragPoint("Location", ref loc);
			ImGuiMg.DragPoint("Size", ref size);
			if(loc != oldVal.Location || size != oldVal.Size)
				fieldInfo.SetValue(obj, new Rectangle(loc, size));
		}
	}
}