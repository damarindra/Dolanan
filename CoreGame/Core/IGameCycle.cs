using Microsoft.Xna.Framework;

namespace CoreGame.Engine
{
	public interface IGameCycle
	{
		/// <summary>
		/// Called only once, when Game is initialize. New Actor / Component that created in runtime pass this function
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// Called when start entering to world
		/// </summary>
		public abstract void Start();
		public abstract void Update(GameTime gameTime);
		public abstract void LateUpdate(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, float layerZDepth);
	}
}