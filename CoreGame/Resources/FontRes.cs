using CoreGame.Controller;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Resources
{
	public class FontRes
	{
		public static SpriteFont Font;
		
		public static void Init()
		{
			Font = GameMgr.Game.Content.Load<SpriteFont>("Fonts/bitty");
		}
	}
}