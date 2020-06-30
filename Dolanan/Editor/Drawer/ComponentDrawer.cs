using System;
using System.Collections.Generic;
using System.Reflection;
using Dolanan.Components;
using Dolanan.Editor.Attribute;

namespace Dolanan.Editor.Drawer
{
	[TargetType(typeof(Component))]
	public class ComponentDrawer
	{
		private Type componentType = null;
		private FieldInfo[] fields;
		
		public virtual void OnDrawComponent(Component comp)
		{
			if (comp.GetType() != componentType)
			{
				fields = null;
				fields = comp.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
				componentType = comp.GetType();
			}
			
			if (ImGuiNET.ImGui.CollapsingHeader(comp.ToString()))
			{
				foreach (var fieldInfo in fields)
				{
					if (DrawerDictionary.TryGetPropertyDrawer(fieldInfo.FieldType, out var drawer))
						drawer.OnDrawProperty(fieldInfo, comp);
				}
			}
		}
	}
}