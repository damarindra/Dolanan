using System;
using System.Collections.Generic;
using Dolanan.Editor.Drawer;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TargetTypeAttribute : System.Attribute
	{
		public Type type { get; private set; }
		
		public TargetTypeAttribute(Type type)
		{
			this.type = type;
		}
	}

	internal static class DrawerDictionary
	{
		private static readonly Dictionary<Type, PropertyDrawer> PropertyDrawers = new Dictionary<Type, PropertyDrawer>();
		private static readonly Dictionary<Type, ComponentDrawer> ComponentDrawers = new Dictionary<Type, ComponentDrawer>();

		internal static void Add(Type type, Type drawer)
		{
			if(drawer.IsSubclassOf(typeof(PropertyDrawer)) || drawer == typeof(PropertyDrawer))
				PropertyDrawers.TryAdd(type, (PropertyDrawer)Activator.CreateInstance(drawer));
			else if (drawer.IsSubclassOf(typeof(ComponentDrawer)) || drawer == typeof(ComponentDrawer))
				ComponentDrawers.TryAdd(type, (ComponentDrawer) Activator.CreateInstance(drawer));
		}

		internal static bool TryGetPropertyDrawer(Type type, out PropertyDrawer drawer)
		{
			return PropertyDrawers.TryGetValue(type, out drawer);
		}
		internal static bool TryGetComponentDrawer(Type type, out ComponentDrawer drawer)
		{
			return ComponentDrawers.TryGetValue(type, out drawer);
		}
	}
}