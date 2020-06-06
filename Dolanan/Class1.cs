using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Portable
{
	public static class Class1
	{
		public static Texture2D Bug;
		
		public static bool TryAccessMonoLib(Game game)
		{
			Bug = game.Content.Load<Texture2D>("bug");
			return game != null;
		}
	}
}