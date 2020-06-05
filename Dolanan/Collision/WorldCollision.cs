using System;
using System.Collections.Generic;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Collision
{
	public class WorldCollision
	{
		public List<Body> Colliders = new List<Body>();

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
		public bool Check(Body a, Body b, Vector2 velo, out Vector2 outVel, out Vector2 outRemainder, out Hit hitInfo)
		{
			outVel = velo;
			outRemainder = Vector2.Zero;
			hitInfo = new Hit();

			// Check if the target location is overlapped
			a.Position += velo;
			if (!a.Overlaps(b))
			{
				a.Position -= velo;
				return false;
			}
			a.Position -= velo;

			hitInfo.Actor = b.Owner;
			hitInfo.Body = b;
			
			// fuck off, it just trigger
			if (a.IsTrigger || b.IsTrigger)
				return true;
			
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
			}
			// between box already touching
			else
			{
				Vector2 plane = new Vector2(a.Min.Y == b.Max.Y || a.Max.Y == b.Min.Y ? 1 : 0,
					a.Min.X == b.Max.X || a.Max.X == b.Min.X ? 1 : 0);
				outVel = plane * velo;
				outRemainder = velo - outVel;

				if (plane.X != 0 && velo.Y != 0)
					hitInfo.Normal.Y = MathF.Sign(-velo.Y);
				else if (plane.Y != 0 && velo.X != 0)
					hitInfo.Normal.X = MathF.Sign(-velo.X);
			}
			
			// TODO: Create stuck handler. Just push out from it! This just for safety. Need a lot of testing tbh. But I'm sure it is fine.
			
			return true;
		}
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="body"></param>
		/// <param name="velo"></param>
		/// <param name="resultVelo"></param>
		/// <param name="remainder"></param>
		/// <param name="hit"></param>
		/// <param name="bounce"></param>
		public void CheckCollision(Body body, Vector2 velo, out Vector2 resultVelo, out Vector2 remainder, out Hit hit, int bounce = 3)
		{
			resultVelo = Vector2.Zero;
			remainder = Vector2.Zero;
			hit = new Hit();

			bool isAlreadyFoundSolidBody = false;

			int bounceCounter = 0;

			HashSet<Body> newCollidedBody = new HashSet<Body>();
			
			foreach (Body b in Colliders)
			{
				if(b == body)
					continue;

				if (Check(body, b, velo, out var rVelo, out var rRemain, out var rHit))
				{
					// flag to prevent overlap body take over the result Solid Hit
					if (b.IsTrigger && isAlreadyFoundSolidBody)
					{
						// DO NOTHING
					}
					else
					{
						resultVelo = rVelo;
						remainder = rRemain;
						hit = rHit;
						hit.Actor = b.Owner;
						hit.Body = b;
						isAlreadyFoundSolidBody = true;
					}
					
					// Register collision for delegate Collison
					body.RegisterCollision(b);
					b.RegisterCollision(body);

					newCollidedBody.Add(b);

					velo = rVelo;

					if (b.IsTrigger)
					{
						bounceCounter++;
						if (bounceCounter >= bounce)
						{
							break;
						}
					}
				}
			}
			
			// no collision
			if (bounceCounter == 0)
				resultVelo = velo;

			// Exiting the collision
			HashSet<Body> bodyExit = new HashSet<Body>(body.CollidedBodies);
			bodyExit.ExceptWith(newCollidedBody);
			
			if (bodyExit.Count > 0)
			{
				foreach (Body b in bodyExit)
				{
					if (b.IsTrigger || body.IsTrigger)
					{
						b.OnTriggerExit?.Invoke(body);
						body.OnTriggerExit?.Invoke(b);
					}
					else
					{
						b.OnCollisionExit?.Invoke(body);
						body.OnCollisionExit?.Invoke(b);
					}
					b.CollidedBodies.Remove(body);
					body.CollidedBodies.Remove(b);
				}
			}
		}
	}
}