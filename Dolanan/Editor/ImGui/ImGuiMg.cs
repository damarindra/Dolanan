﻿﻿using Microsoft.Xna.Framework;
using Num = System.Numerics;

namespace Dolanan.Editor.ImGui
{
#if DEBUG
	
	using ImGui = ImGuiNET.ImGui;
	public static class ImGuiMg
	{
		public static void ColorEdit(string label, ref Color color)
		{
			Num.Vector3 col = color.ToNumImGuiVec3();
			ImGui.ColorEdit3(label, ref col);
			if (col != color.ToNumImGuiVec3())
				color = col.ToXnaColor();
		}
	
		public static void Point(string label, ref Point point)
		{
			var v = point.ToVector2().ToNumVec2();
			ImGui.InputFloat2(label, ref v, "%.0f");
			var vP = v.ToXnaVec2().ToPoint();

			if(!point.Equals(vP))
				point = vP;
		}
		public static void Vector2(string label, ref Vector2 vector2)
		{
			var v = vector2.ToNumVec2();
			ImGui.InputFloat2(label, ref v);
			var newVal = v.ToXnaVec2();

			if(!vector2.Equals(newVal))
				vector2 = newVal;
		}
	
		public static Num.Vector3 ToNumVec3(this Vector3 v)
		{
			return new Num.Vector3(v.X, v.Y, v.Z);
		}
		public static Vector3 ToXnaVec3(this Num.Vector3 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}
		public static Num.Vector2 ToNumVec2(this Vector2 v)
		{
			return new Num.Vector2(v.X, v.Y);
		}
		public static Vector2 ToXnaVec2(this Num.Vector2 v)
		{
			return new Vector2(v.X, v.Y);
		}
		public static Num.Vector3 ToNumImGuiVec3(this Color v)
		{
			return new Num.Vector3(v.R / 255f, v.G / 255f, v.B / 255f);
		}
		public static Num.Vector3 ToNumVec3(this Color v)
		{
			return new Num.Vector3(v.R, v.G, v.B);
		}
		public static Color ToXnaColor(this Num.Vector3 v)
		{
			return new Color(v.X, v.Y, v.Z);
		}
	}
#endif
}