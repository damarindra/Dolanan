using CoreGame.Engine;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Component
{
	public class BaseComponent
	{
		public Transform2D TransformComponent = new Transform2D();
		public Actor Owner;
		public string Name;

		public BaseComponent()
		{
		}

		public virtual void UpdateComponent(GameTime gameTime)
		{
			TransformComponent.UpdateTransform();
		}
		public virtual void DrawComponent(GameTime gameTime, SpriteBatch spriteBatch) { }
	}
}