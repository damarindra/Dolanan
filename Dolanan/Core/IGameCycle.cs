using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public interface IGameCycle
	{
		/// <summary>
		/// Called only once, at the start of the constructor. Before anything else!
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// Called only once, at the end of the constructor.
		/// </summary>
		public abstract void Start();
		/// <summary>
		/// Called every frame
		/// </summary>
		/// <param name="gameTime"></param>
		public abstract void Update(GameTime gameTime);
		/// <summary>
		/// Called every frame, after Update
		/// </summary>
		/// <param name="gameTime"></param>
		public abstract void LateUpdate(GameTime gameTime);
		/// <summary>
		/// Draw method, just like Update!
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="layerZDepth"></param>
		public abstract void Draw(GameTime gameTime, float layerZDepth);
	}
}