using CoreGame.Collision;
using CoreGame.Controller;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;

namespace CoreGame.Tools.GameHelper
{
	public static class CollisionHelper
	{
		public static Actor CreateColliderActor(Vector2 position, Vector2 size)
		{
			Actor result = GameMgr.Game.World.CreateActor<Actor>("fdsa");
			result.Transform.Position = position;
			Body b = result.AddComponent<Body>();
			b.Size = size;
			return result;
		}
	}
}