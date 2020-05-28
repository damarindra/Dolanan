using CoreGame.Engine;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Component
{
	public class BaseComponent : BaseObject
	{
		public Transform2DComponent TransformComponent = new Transform2DComponent();
		public Actor Owner;

		public BaseComponent()
		{
		}

		public virtual void UpdateComponent(GameTime gameTime)
		{
			TransformComponent.UpdateComponent(gameTime);
		}
		public virtual void DrawComponent(GameTime gameTime, SpriteBatch spriteBatch) { }
	}

	public interface IComponent
	{
		public abstract void UpdateComponent(GameTime gameTime);
	}

	public interface IDrawableComponent : IComponent
	{
		public abstract void DrawComponent(GameTime gameTime, SpriteBatch spriteBatch);
	}
}