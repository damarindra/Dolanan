using System;
using System.Linq;
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
			float s = MathF.Sin(angleRad);
			float c = MathF.Cos(angleRad);
			
			// translate point back to origin (0,0)
			Vector2 tempV = v - pivot;
			
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

		// public static Matrix SetRotationScaleAndSkew(this ref Matrix matrix, float rotation, Vector2 scale, float skew)
		// {
		// 	matrix.M11 = 
		// }
	}
}