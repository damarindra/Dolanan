using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Dolanan.Engine;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Collision
{
	public delegate void Collision(Body other);
	
	//TODO : Stuck after colliding. AABB only is not stuck
	public class Body : AABB
	{
		public BodyType BodyType = BodyType.Static;
		public string Tag = "";
		public bool IsTrigger = false;

		public Collision OnCollisionEnter;
		public Collision OnTriggerEnter;

		public Collision OnCollisionStay;
		public Collision OnTriggerStay;

		public Collision OnCollisionExit;
		public Collision OnTriggerExit;
		
		/// <summary>
		/// Registered collided bodies, including the overlap body. It will trigger the Exit delegate
		/// </summary>
		public HashSet<Body> CollidedBodies { get; private set; } = new HashSet<Body>();

		public Body(Actor owner) : base(owner) { }

		public override void Start()
		{
			base.Start();
			if(Owner.Layer.IsLoaded)
				GameMgr.Game.World.Colliders.Add(this);
		}

		public Hit Move(Vector2 velocity)
		{
			if (BodyType == BodyType.Static)
				return default;
			Vector2 resultVelo;
			Vector2 resultRemainder;
			Hit hit;
			GameMgr.Game.World.CheckCollision(this, velocity, out resultVelo, out resultRemainder, out hit);
			
			if (BodyType == BodyType.Kinematic && hit.Actor != null)
			{
				if (hit.Normal != Vector2.Zero)
					resultVelo += resultRemainder.Slide(hit.Normal);
			}
			Position += resultVelo;

			return hit;
		}

		/// <summary>
		/// Registering other body for handling OnCollision delegate
		/// </summary>
		/// <param name="other"></param>
		public void RegisterCollision(Body other)
		{
			if(BodyType == BodyType.Static)
				return;
			
			if (other.IsTrigger)
			{
				// Overlapping
				if(!CollidedBodies.Contains(other))
					OnTriggerEnter?.Invoke(other);
				else
					OnTriggerStay?.Invoke(other);
			}
			else 
			{
				// OnCollision
				if(!CollidedBodies.Contains(other))
					OnCollisionEnter?.Invoke(other);
				else
					OnCollisionStay?.Invoke(other);
			}
			
			CollidedBodies.Add(other);
		}

		public void FlushCollidedBodies()
		{
			CollidedBodies.Clear();
		}
	}

	/// <summary>
	/// Body Type
	/// Static : can't receive collision delegate. It just a wall
	/// Kinematic : Moveable Body, can receive collision delegate
	/// Overlap : Moveable Body, but no collision occured. can received trigger delegate
	/// </summary>
	public enum BodyType
	{
		Static, Kinematic
	}
}