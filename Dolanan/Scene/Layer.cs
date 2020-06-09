using System;
using System.Collections.Generic;
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
				actor.Draw(gameTime, AutoYSort ? actor.Transform.Position.Y * float.Epsilon : LayerZ);
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
		///     Please use AddActor<T> for creating new actor. This function only useful when you want to move actor to
		/// 	new layer
		/// </summary>
		/// <param name="actor">Actor</param>
		/// <param name="recursive">add all child actor</param>
		public void AddActor(Actor actor)
		{
			if (actor.Layer != this)
			{
				actor.Layer.Actors.Remove(actor);
			}
			if (Actors.Contains(actor))
			{
				Log.PrintWarning("Trying to add actor that already added : " + actor.Name);
				return;
			}
			Actors.Add(actor);
		}

		public T GetActor<T>()
		{
			return Actors.OfType<T>().First();
		}

		public T[] GetActors<T>()
		{
			return Actors.OfType<T>().ToArray();
		}

		public virtual void LoadLayer()
		{
			foreach (var actor in GameWorld.DynamicActor) Actors.Add(actor);
			// Register the collision into World
			foreach (var actor in Actors)
			foreach (var body in actor.GetComponents<Body>())
				GameWorld.Colliders.Add(body);

			IsLoaded = true;
		}

		public virtual void UnloadLayer()
		{
			IsLoaded = false;
			foreach (var actor in GameWorld.DynamicActor) Actors.Remove(actor);

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