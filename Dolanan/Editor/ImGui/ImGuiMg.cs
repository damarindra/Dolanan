﻿using System;
 using System.Linq;
 using Microsoft.Xna.Framework;
 using System.Text;
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
			int[] p = new[] {point.X, point.Y};

			ImGui.InputInt2(label, ref p[0]);

			if(point.X != p[0] || point.Y != p[1])
				point = new Point(p[0], p[1]);
		}
		public static void Vector2(string label, ref Vector2 vector2)
		{
			var v = vector2.ToNumVec2();
			ImGui.InputFloat2(label, ref v);
			var newVal = v.ToXnaVec2();

			if(!vector2.Equals(newVal))
				vector2 = newVal;
		}
		public static void DragPoint(string label, ref Point point)
		{
			int[] p = new[] {point.X, point.Y};

			ImGui.DragInt2(label, ref p[0]);

			if(point.X != p[0] || point.Y != p[1])
				point = new Point(p[0], p[1]);
		}
		public static void DragVector2(string label, ref Vector2 vector2)
		{
			var v = vector2.ToNumVec2();
			ImGui.DragFloat2(label, ref v);
			var newVal = v.ToXnaVec2();

			if(!vector2.Equals(newVal))
				vector2 = newVal;
		}

		public static void InputText(string label, ref string str, byte bufSize = 100)
		{
			byte[] bytes =  Encoding.UTF8.GetBytes(str);
			Array.Resize<Byte>(ref bytes, bufSize);
			ImGui.InputText(label, bytes, bufSize);
			string newStr = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			for (int i = 0; i < newStr.Length; i++)
			{
				if (newStr[i] == '\u0000')
				{
					newStr = newStr.Remove(i);
					break;
				}
			}
			// while (newStr[^1] == '\u0000')
			// 	newStr = newStr.Remove(newStr.Length - 1);
			if (!string.Equals(newStr, str, StringComparison.Ordinal))
				str = newStr;
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