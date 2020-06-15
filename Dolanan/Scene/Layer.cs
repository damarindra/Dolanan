using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dolanan.Collision;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	/// <summary>
	///     Layer is container for Actors. Layer also act as a Scene / Level. World can have more than one Layer
	///     Use case:
	/// </summary>
	public class Layer : IGameCycle
	{
		protected List<Actor> Actors = new List<Actor>();

		/// <summary>
		///     Auto Y Sort only work when Actor Position is positive.
		/// </summary>
		public bool AutoYSort = false;

		/// <summary>
		///     Initialize layer
		/// </summary>
		/// <param name="gameWorld"></param>
		/// <param name="layerName"></param>
		/// <param name="useCollision"></param>
		public Layer(World gameWorld, int layerZ)
		{
			Initialize();
			GameWorld = gameWorld;
			LayerZ = layerZ;
			LoadLayer();
			Start();
		}

		public bool IsLoaded { get; private set; }

		public World GameWorld { get; }

		// TODO : LayerZ used for layerDepth spriteBatch.Draw, the range only between 0 - 1, so fix this!
		public int LayerZ { get; }

		public virtual void Initialize()
		{
		}

		public virtual void Start()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!IsLoaded)
				return;
			foreach (var actor in Actors) actor.Update(gameTime);
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
			if (!IsLoaded)
				return;
			foreach (var actor in Actors) actor.LateUpdate(gameTime);
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth)
		{
			if (!IsLoaded)
				return;
			foreach (var actor in Actors)
				actor.Draw(gameTime,
					AutoYSort ? actor.Transform.Location.Y * float.Epsilon : LayerZ / (float) GameWorld.LayerCount);
		}

		/// <summary>
		///     Add Actor to the current layer.
		/// </summary>
		/// <param name="name"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T AddActor<T>(string name) where T : Actor
		{
			var actor = (T) Activator.CreateInstance(typeof(T), name, this);
			return actor;
		}

		/// <summary>
		///     Internal use only, please use <see cref="AddActor{T}" /> for creating new actor.
		///     This function only useful when you want to move actor to new layer. All childs will move to this layer too
		/// </summary>
		/// <param name="actor">Actor</param>
		internal void AddActor([NotNull] Actor actor)
		{
			if (actor.Layer != null && actor.Layer != this) actor.Layer.Actors.Remove(actor);
			if (Actors.Contains(actor))
			{
				Log.PrintWarning("Trying to add actor that already added : " + actor.Name);
				return;
			}

			Actors.Add(actor);
			actor.OnLayerChange?.Invoke(this);
			foreach (var child in actor.Transform.Childs) AddActor(child.Owner);
		}

		public T GetActor<T>()
		{
			return Actors.OfType<T>().First();
		}

		public T[] GetActors<T>()
		{
			return Actors.OfType<T>().ToArray();
		}

		/// <summary>
		///     Removing actor from layer. Only allowed root actor. Use Actor.Detach, it is safer
		/// </summary>
		/// <param name="actor"></param>
		public void RemoveActor(Actor actor)
		{
			if (actor.Transform.Parent != null)
			{
				Log.PrintError("Trying to remove non-root Actor from layer : " + LayerZ +
				               ". This is not allowed. Use Actor.RemoveChild.");
				return;
			}

			Actors.Remove(actor);
			actor.Layer = null;

			RemoveActorRecursive(actor);
		}

		internal void RemoveActorRecursive(Actor actor)
		{
			Actors.Remove(actor);
			actor.Layer = null;
			foreach (var child in actor.Transform.Childs)
			{
				Actors.Remove(child.Owner);
				child.Owner.Layer = null;
				RemoveActorRecursive(child.Owner);
			}
		}

		public virtual void LoadLayer()
		{
			// Register the collision into World
			foreach (var actor in Actors)
			foreach (var body in actor.GetComponents<Body>())
				GameWorld.Colliders.Add(body);

			IsLoaded = true;
		}

		public virtual void UnloadLayer()
		{
			IsLoaded = false;

			// Remove the collision from World
			foreach (var actor in Actors)
			foreach (var body in actor.GetComponents<Body>())
				GameWorld.Colliders.Remove(body);
		}
	}

	//List enum edit here
	public enum LayerName
	{
		Background = 0,
		Default = 1,
		Foreground = 2
	}
}