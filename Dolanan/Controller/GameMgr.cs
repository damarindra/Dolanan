using Dolanan.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Controller
{
	public static class GameMgr
	{
		public static void Init(GameMin game)
		{
			Game = game;
		}

		public static void Load(SpriteBatch spriteBatch)
		{
			SpriteBatch = spriteBatch;
		}
		
		public static GameMin Game { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
	}
}