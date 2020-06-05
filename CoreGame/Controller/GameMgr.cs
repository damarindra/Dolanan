using CoreGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Controller
{
	public static class GameMgr
	{
		public static void Init(GameClient game)
		{
			Game = game;
		}

		public static void Load(SpriteBatch spriteBatch)
		{
			SpriteBatch = spriteBatch;
		}
		
		public static GameClient Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
	}
}