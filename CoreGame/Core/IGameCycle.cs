using Microsoft.Xna.Framework;

namespace CoreGame.Engine
{
	public interface IGameCycle
	{
		public abstract void Initialize();
		public abstract void Start();
		public abstract void Update(GameTime gameTime);
		public abstract void LateUpdate(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, float layerZDepth = 0);
	}
}