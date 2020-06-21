﻿using System;
using System.Linq;
using Dolanan.Core;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public static class MathEx
	{
		public static int PosMod(int i1, int i2)
		{
			var i = i1 % i2;
			if (i < 0 && i2 > 0 || i > 0 && i2 < 0)
				i += i2;

			return i;
		}

		public static float KindaSmallNumber => 0.00001f;

		public static float FPosMod(float f1, float f2)
		{
			var i = f1 % f2;
			if (i < 0 && f2 > 0 || i > 0 && f2 < 0)
				i += f2;

			return i;
		}

		public static float Clamp(float val, float min, float max)
		{
			return MathF.Min(MathF.Max(val, min), max);
		}

		public static float Min(params float[] floats)
		{
			return floats.Min();
		}

		public static float Max(params float[] floats)
		{
			return floats.Max();
		}

		/// <summary>
		///     Turn vector direction to Angle radian
		/// </summary>
		/// <param name="v"></param>
		/// <returns>Angle radian</returns>
		public static float ToAngle(Vector2 v)
		{
			return MathF.Atan2(v.Y, v.X);
		}

		/// <summary>
		///     Similar to LookAt
		/// </summary>
		/// <param name="v"></param>
		/// <param name="point"></param>
		/// <returns>angle radian</returns>
		public static float AngleToPoint(this Vector2 v, Vector2 point)
		{
			return MathF.Atan2(v.Y - point.Y, v.X - point.X);
		}

		public static RectangleF ToRectangleF(this Rectangle r)
		{
			return new RectangleF(r.X, r.Y, r.Width, r.Height);
		}

		/// <summary>
		///     Interpolation color
		/// </summary>
		/// <param name="c1">start</param>
		/// <param name="c2">to</param>
		/// <param name="t">0 - 1</param>
		/// <param name="functions"></param>
		/// <returns></returns>
		public static Color Interpolate(Color c1, Color c2, float t,
			Easing.Functions functions = Easing.Functions.Linear)
		{
			if (t >= 1)
				return c2;
			if (t <= 0)
				return c1;

			var i = Easing.Interpolate(t, functions);
			return new Color(c1.R + i * (c2.R - c1.R),
				c1.G + i * (c2.G - c1.G),
				c1.B + i * (c2.B - c1.B),
				c1.A + i * (c2.A - c1.A));
		}

		/// <summary>
		///     Interpolation color
		/// </summary>
		/// <param name="f1">start</param>
		/// <param name="f2">to</param>
		/// <param name="t">0 - 1</param>
		/// <param name="functions"></param>
		/// <returns></returns>
		public static float Interpolate(float f1, float f2, float t,
			Easing.Functions functions = Easing.Functions.Linear)
		{
			if (t >= 1)
				return f2;
			if (t <= 0)
				return f1;

			var i = Easing.Interpolate(t, functions);
			return f1 + i * (f2 - f1);
		}

		#region Vector2

		public static Vector2 ToDirection(float radian)
		{
			return new Vector2(MathF.Cos(radian), MathF.Sin(radian));
		}

		public static Vector2 Slide(this Vector2 v, Vector2 normal)
		{
			return v - normal * Vector2.Dot(v, normal);
		}

		public static Vector2 Reflect(this Vector2 v, Vector2 normal)
		{
			return 2.0f * normal * Vector2.Dot(v, normal) - v;
		}

		public static Vector2 ToVector2(this Vector3 v)
		{
			return new Vector2(v.X, v.Y);
		}

		public static Vector2 RotateAround(this Vector2 v, Vector2 pivot, float angleRad)
		{
			var s = MathF.Sin(angleRad);
			var c = MathF.Cos(angleRad);

			// translate point back to origin (0,0)
			var tempV = v - pivot;

			// rotate point
			tempV = new Vector2(
				tempV.X * c - tempV.Y * s,
				tempV.X * s + tempV.Y * c);

			return tempV + pivot;
		}

		#endregion

		#region Point

		public static Point Min(Point v1, Point v2)
		{
			Point vector2 = default;
			vector2.X = v1.X < v2.X ? v1.X : v2.X;
			vector2.Y = v1.Y < v2.Y ? v1.Y : v2.Y;
			return vector2;
		}

		public static Point Max(Point v1, Point v2)
		{
			Point vector2 = default;
			vector2.X = v1.X > v2.X ? v1.X : v2.X;
			vector2.Y = v1.Y > v2.Y ? v1.Y : v2.Y;
			return vector2;
		}

		#endregion

		#region Color
		
		/// <summary>
		///     Converts a hex string into a Microsoft.Xna.Color
		/// </summary>
		/// <param name="hex">The 6 digit or 8 digit hex string without the # at the beginning</param>
		/// <returns></returns>
		public static Color HextToColor(string hex)
		{
			hex = hex.Replace("#", "");
			if (hex.Length >= 6)
			{
				float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255.0f;
				float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255.0f;
				float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255.0f;

				if (hex.Length == 8)
				{
					float a = (HexToByte(hex[6]) * 16 + HexToByte(hex[7])) / 255.0f;
					return new Color(r, g, b, a);
				}

				return new Color(r, g, b);
			}

			return Color.White;
		}

		public static string ColorToHex(Color color)
		{
			string r = color.R.ToString("X");
			string g = color.G.ToString("X");
			string b = color.B.ToString("X");
			string a = color.A.ToString("X");

			if (r.Length == 1)
				r = "0" + r;
			if (g.Length == 1)
				g = "0" + g;
			if (b.Length == 1)
				b = "0" + b;
			if (a.Length == 1)
				a = "0" + a;

			return "#" + r + g + b + a;
		}

		/// <summary>
		///     Lookup table for base16 digits
		/// </summary>
		private const string Hex = "0123456789ABCDEF";

		/// <summary>
		///     Converts the given hex digit to a byte
		/// </summary>
		/// <param name="c">The hex digit as a char</param>
		/// <returns></returns>
		public static byte HexToByte(char c)
		{
			return (byte)Hex.IndexOf(char.ToUpper(c));
		}
		
		#endregion

		// public static Matrix SetRotationScaleAndSkew(this ref Matrix matrix, float rotation, Vector2 scale, float skew)
		// {
		// 	matrix.M11 = 
		// }
	}
}