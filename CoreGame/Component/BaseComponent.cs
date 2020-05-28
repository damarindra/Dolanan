using CoreGame.Engine;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Component
{
	public class BaseComponent : BaseObject
	{
		public Transform2D Transform = new Transform2D();
		public Actor Owner;

		public BaseComponent()
		{
		}

		public virtual void UpdateComponent(GameTime gameTime)
		{
			Transform.UpdateComponent(gameTime);
		}
		public virtual void DrawComponent(GameTime gameTime, SpriteBatch spriteBatch) { }
	}
}