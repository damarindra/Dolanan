using System;
using System.Reflection;
using Dolanan.Editor.ImGui;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Editor.Attribute
{
	[AttributeUsage(AttributeTargets.Field)]
	public class VisiblePropertyAttribute : System.Attribute
	{
	}
}