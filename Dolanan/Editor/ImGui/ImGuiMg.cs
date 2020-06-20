﻿using Microsoft.Xna.Framework;
using Num = System.Numerics;

namespace Dolanan.Editor.ImGui
{
	using ImGui = ImGuiNET.ImGui;
	public static class ImGuiMg
	{
		public static void ColorEdit(string label, ref Color color)
		{
			Num.Vector3 col = color.XnaColorToNumVec3();
			ImGui.ColorEdit3(label, ref col);
			if (col != color.XnaColorToNumVec3())
				color = col.NumVec3ToXnaColor();
		}

		public static Num.Vector3 XnaVec3ToNumVec3(this Vector3 v)
		{
			return new Num.Vector3(v.X, v.Y, v.Z);
		}
		public static Vector3 NumVec3TpXnaVec3(this Num.Vector3 v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}
		public static Num.Vector3 XnaColorToNumVec3(this Color v)
		{
			return new Num.Vector3(v.R, v.G, v.B);
		}
		public static Color NumVec3ToXnaColor(this Num.Vector3 v)
		{
			return new Color(v.X, v.Y, v.Z);
		}
	}
}