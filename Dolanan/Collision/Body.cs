﻿using System.Collections.Generic;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Collision
{
	public delegate void Collision(Body other);

	//TODO : Stuck after colliding. AABB only is not stuck
	public class Body : AABB
	{
		public BodyType BodyType = BodyType.Static;
		public bool IsTrigger = false;

		public Collision OnCollisionEnter;

		public Collision OnCollisionExit;

		public Collision OnCollisionStay;
		public Collision OnTriggerEnter;
		public Collision OnTriggerExit;
		public Collision OnTriggerStay;
		public string Tag = "";

		public Body(Actor owner) : base(owner)
		{
		}

		/// <summary>
		///     Registered collided bodies, including the overlap body. It will trigger the Exit delegate
		/// </summary>
		public List<Body> CollidedBodies { get; } = new List<Body>();

		public override void Start()
		{
			base.Start();
			if (Owner.Layer.IsLoaded)
				GameMgr.Game.World.Colliders.Add(this);

			// This handle the parent change. Ex : when Actor.Detach is called, the parent will be null, which mean
			// it needs to be removed from the world collision.
			Owner.OnParentChange += parent =>
			{
				// Remove body from WorldColliders if parent is null
				if (parent == null)
					GameMgr.Game.World.Colliders.Remove(this);
				// add if parent is not null
				else if (!GameMgr.Game.World.Colliders.Contains(this))
					GameMgr.Game.World.Colliders.Add(this);
			};
		}

		public Hit Move(Vector2 velocity)
		{
			if (BodyType == BodyType.Static)
				return default;
			GameMgr.Game.World.CheckCollision(this, velocity, out var resultVelo, out var resultRemainder, out var hit);

			if (BodyType == BodyType.Kinematic && hit.Actor != null)
				if (hit.Normal != Vector2.Zero)
					resultVelo += resultRemainder.Slide(hit.Normal);
			Position += resultVelo;

			return hit;
		}

		/// <summary>
		///     Registering other body for handling OnCollision delegate
		/// </summary>
		/// <param name="other"></param>
		public void RegisterCollision(Body other)
		{
			if (BodyType == BodyType.Static)
				return;

			if (other.IsTrigger)
			{
				// Overlapping
				if (!CollidedBodies.Contains(other))
					OnTriggerEnter?.Invoke(other);
				else
					OnTriggerStay?.Invoke(other);
			}
			else
			{
				// OnCollision
				if (!CollidedBodies.Contains(other))
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
	///     Body Type
	///     Static : can't receive collision delegate. It just a wall
	///     Kinematic : Moveable Body, can receive collision delegate
	///     Overlap : Moveable Body, but no collision occured. can received trigger delegate
	/// </summary>
	public enum BodyType
	{
		Static,
		Kinematic
	}
}