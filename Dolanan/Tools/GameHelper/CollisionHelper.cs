using Dolanan.Collision;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Tools.GameHelper
{
	/// <summary>
	/// It is just a tool, helper. Basically I'm using it for testing only
	/// </summary>
	public static class CollisionHelper
	{
		public static Actor CreateColliderActor(Vector2 position, Vector2 size, string tag = "wall", BodyType bodyType = BodyType.Static)
		{
			Actor result = GameMgr.Game.World.CreateActor<Actor>("fdsa");
			result.Transform.Position = position;
			Body b = result.AddComponent<Body>();
			b.Size = size;
			b.Tag = tag;
			b.BodyType = bodyType;
			return result;
		}
	}
}