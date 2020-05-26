using System.Collections.Generic;
using CoreGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CoreGame.Scene.Object;

namespace CoreGame.Scene
{
	//TODO create system that load and unload world. generally changing scene. World is scene
	public class World
	{
		public Camera Camera;
		public List<Actor> Actors = new List<Actor>();

		public World()
		{
			Camera = new Camera(new Vector2(0,0), GameSettings.ViewportSize);
			AddActor(Camera);
		}

		public void AddActor(Actor actor)
		{
			Actors.Add(actor);
		}

		public void Update(GameTime gameTime)
		{
			foreach (var actor in Actors)
			{
				actor.Update(gameTime);
			}
			
			foreach (var actor in Actors)
			{
				actor.LateUpdate(gameTime);
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var actor in Actors)
			{
				if(actor == Camera)
					continue;
				
				actor.Draw(gameTime, spriteBatch);
			}
		}
	}
}