using CoreGame.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Controller
{
	public static class GameMgr
	{
		public static void Init(GameClient game, SpriteBatch spriteBatch)
		{
			Game = game;
			SpriteBatch = spriteBatch;
		}
		
		public static GameClient Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
	}
}