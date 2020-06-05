using System;
using System.Collections.Generic;
using CoreGame.Tools;
using Microsoft.Xna.Framework;

namespace CoreGame.Collision
{
	public class WorldCollision
	{
		public List<AABB> Colliders = new List<AABB>();

		/// <summary>
		/// Check collision between A and B, A is the moving body that uses velo. This only check, not moving the AABB a
		/// </summary>
		/// <param name="a">A moving body</param>
		/// <param name="b">B checker body</param>
		/// <param name="velo">Velocity of A</param>
		/// <param name="outVel">result</param>
		/// <param name="outRemainder">remainder velocity</param>
		/// <param name="hitInfo">Hit info</param>
		/// <returns></returns>
		public bool Check(AABB a, AABB b, Vector2 velo, out Vector2 outVel, out Vector2 outRemainder, out Hit hitInfo)
		{
			outVel = velo;
			outRemainder = Vector2.Zero;
			hitInfo = new Hit();

			a.Position += velo;
			if (!a.Overlaps(b))
			{
				a.Position -= velo;
				return false;
			}

			a.Position -= velo;
			// 0 means invalid, no check required. displacement is closest space between box
			Vector2 displacement = Vector2.Zero;
			
			// This basically check if there is any space between box.
			if (!(a.Min.X <= b.Max.X && a.Max.X >= b.Min.X))
				displacement.X = MathF.Min(MathF.Abs(a.Min.X - b.Max.X), MathF.Abs(a.Max.X - b.Min.X));
			if (!(a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y))
				displacement.Y = MathF.Min(MathF.Abs(a.Min.Y - b.Max.Y), MathF.Abs(a.Max.Y - b.Min.Y));

			if (displacement != Vector2.Zero)
			{
				Vector2 delta = new Vector2(velo.X != 0 ? displacement.X / velo.X : 0, velo.Y != 0 ? displacement.Y / velo.Y : 0);
				float timeDelta = MathF.Max(MathF.Abs(delta.X), MathF.Abs(delta.Y));
				outVel = timeDelta * velo;
				outRemainder = (1 - timeDelta) * velo;

				if (velo.X < 0)
					hitInfo.Normal.X = Math.Abs(a.Min.X + outVel.X - b.Max.X) < Single.Epsilon ? 1 : 0;
				else if (velo.X > 0)
					hitInfo.Normal.X = Math.Abs(a.Max.X + outVel.X - b.Min.X) < Single.Epsilon ? -1 : 0;
				
				if (velo.Y < 0 && Math.Abs(hitInfo.Normal.X) < Single.Epsilon)
					hitInfo.Normal.Y = Math.Abs(a.Min.Y + outVel.Y - b.Max.Y) < Single.Epsilon ? 1 : 0;
				else if (velo.Y > 0 && Math.Abs(hitInfo.Normal.X) < Single.Epsilon)
					hitInfo.Normal.Y = Math.Abs(a.Max.Y + outVel.Y - b.Min.Y) < Single.Epsilon ? -1 : 0;
				
				// Log.Print("Displace");
				// Log.Print(hitInfo.Normal.ToString());
				// Log.Print(delta.ToString());
			}
			// between box already touching
			else
			{
				Vector2 plane = new Vector2(a.Min.Y == b.Max.Y || a.Max.Y == b.Min.Y ? 1 : 0,
					a.Min.X == b.Max.X || a.Max.X == b.Min.X ? 1 : 0);
				outVel = plane * velo;
				outRemainder = velo - outVel;

				var aMin = a.Min;
				var aMax = a.Max;
				var bMin = b.Min;
				var bMax = b.Max;

				if (plane.X != 0 && velo.Y != 0)
					hitInfo.Normal.Y = MathF.Sign(-velo.Y);
				else if (plane.Y != 0 && velo.X != 0)
					hitInfo.Normal.X = MathF.Sign(-velo.X);

				Log.Print("Touching");
				Log.Print(plane.ToString());
				Log.Print(outVel.ToString());
				Log.Print(outRemainder.ToString());
			}
			
			return true;
		}
	}
}