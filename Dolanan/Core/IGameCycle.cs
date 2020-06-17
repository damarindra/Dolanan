using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public interface IGameCycle
	{
		/// <summary>
		///     Called only once, at the start of the constructor. Before anything else!
		/// </summary>
		public void Initialize();

		/// <summary>
		///     Called only once, at the end of the constructor.
		/// </summary>
		public void Start();

		/// <summary>
		///     Called every frame
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime);

		/// <summary>
		///     Called every frame, after Update
		/// </summary>
		/// <param name="gameTime"></param>
		public void LateUpdate(GameTime gameTime);

		/// <summary>
		///     Draw method, just like Update! layerZDepth already calculated in Layer.Draw. AutoYSort will automatically
		///     sort using Y position. If you want to ignore AutoYSort, just get Layer.LayerZ (already ordered without AutoYSort)
		/// </summary>
		/// <param name="gameTime">delta Time</param>
		/// <param name="layerZDepth">layerDepth, should be passed to SpriteBatch.Draw</param>
		public void Draw(GameTime gameTime, float layerZDepth);
	}
}