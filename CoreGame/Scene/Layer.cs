using System;
using System.Collections.Generic;
using CoreGame.Controller;
using CoreGame.Engine;
using CoreGame.Resources;
using CoreGame.Tools;
using Humper;
using Humper.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CoreGame.Scene
{
	/// <summary>
	/// Layer is container for Actors. It also work for physics world.
	/// IMPORTANT
	/// 1. Physics can only work non-negative position! (Humper limitation)
	/// 2. Layer is NOT PHYSIC LAYER!! LayerName used for rendering priority!
	/// </summary>
	public class Layer : IGameCycle
	{
		/// <summary>
		/// Initialize layer
		/// </summary>
		/// <param name="gameWorld"></param>
		/// <param name="layerName"></param>
		/// <param name="useCollision"></param>
		public Layer(World gameWorld, LayerName layerName)
		{
			GameWorld = gameWorld;
			LayerName = layerName;
		}

		public World GameWorld { get; private set; }
		public LayerName LayerName { get; private set; }

		/// <summary>
		/// Auto Y Sort only work when Actor Position is positive.
		/// </summary>
		public bool AutoYSort = false;
		
		protected List<Actor> Actors = new List<Actor>();
		
		public T AddActor<T>(string name) where T : Actor
		{
			T actor = (T) Activator.CreateInstance(typeof(T), name, this);
			
			Actors.Add(actor);
			return actor;
		}
		// TODO AddActor Recursively
		/// <summary>
		/// Adding new actor to world
		/// </summary>
		/// <param name="actor">Actor</param>
		/// <param name="recursive">add all child actor</param>
		public void AddActor(Actor actor, bool recursive = true)
		{
			Actors.Add(actor);
			if (recursive)
			{
				foreach (var child in actor.GetChilds)
				{
					AddActor(child, recursive);
				}
			}
		}

		public virtual void Initialize()
		{
			foreach (Actor actor in Actors)
			{
				actor.Initialize();
			}
		}
		public virtual void Start()
		{
			foreach (Actor actor in Actors)
			{
				actor.Start();
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (Actor actor in Actors)
			{
				actor.Update(gameTime);
			}
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
			foreach (Actor actor in Actors)
			{
				actor.LateUpdate(gameTime);
			}
		}

		public virtual void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			foreach (Actor actor in Actors)
			{
				actor.Draw(gameTime, AutoYSort ?  actor.Transform.Position.Y * float.Epsilon : (float) LayerName);
			}
		}
		
	}

	//List enum edit here
	public enum LayerName
	{
		Background = 0,
		Default = 1,
		Foreground = 2,
		UI = 16
	}
}