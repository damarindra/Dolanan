using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Controller
{
	public static class GameMgr
	{
		public static GameMin Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
		public static DrawState DrawState = DrawState.Draw;

		public static void Init(GameMin game)
		{
			Game = game;
		}

		public static void Load(SpriteBatch spriteBatch)
		{
			SpriteBatch = spriteBatch;
		}
	}

	public enum DrawState
	{
		Draw, BackDraw
	}
}