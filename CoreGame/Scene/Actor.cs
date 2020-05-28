#nullable enable
using System.Collections.Generic;
using System.Numerics;
using CoreGame.Component;
using CoreGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CoreGame.Scene
{
	public class Actor : BaseObject
	{
		public Transform2D transform { get; set; } = new Transform2D();

		// Component stuff
		// Render
		private readonly List<BaseComponent> _components = new List<BaseComponent>();

		public Actor()
		{
			Matrix m = Matrix.Identity;
			LateInit(null, Vector2.Zero, 0, Vector2.One);
		}
		
		public Actor(Vector2 position)
		{
			LateInit(null, position, 0, Vector2.One);
		}
		
		public Actor(Actor parent)
		{
			LateInit(parent, Vector2.Zero, 0, Vector2.One);
		}

		/// <summary>
		/// Called after Game Initialization. Usually called LoadContent. Called only at LoadContent on Game cycle.
		/// 
		/// Good for loading content
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="position"></param>
		/// <param name="rotation"></param>
		/// <param name="scale"></param>
		protected virtual void LateInit(Actor? parent, Vector2 position, float rotation, Vector2 scale)
		{
			transform.Parent = parent?.transform;
			transform.Position = position;
			transform.Rotation = rotation;
			transform.Scale = scale;
		}
		
		//Extension stuff
		public T AddComponent<T>(string name) where T : BaseComponent, new()
		{
			T t = new T {Owner = this, Transform = {Parent = transform}};
			t.Name = name;
			_components.Add(t);
			return t;
		}

		public void RemoveComponent(BaseComponent component)
		{
			if (_components.Contains(component))
				_components.Remove(component);
		}
		
		// MonoGame Update
		public virtual void Update(GameTime gameTime)
		{
			transform.UpdateComponent(gameTime);
			foreach (var component in _components)
			{
				component.UpdateComponent(gameTime);
			}
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var component in _components)
			{
				component.DrawComponent(gameTime, spriteBatch);
			}
		}

		// Called after Update
		public virtual void LateUpdate(GameTime gameTime)
		{
			
		}
	}
}