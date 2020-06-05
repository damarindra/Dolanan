using CoreGame.Scene;
using Microsoft.Xna.Framework;

namespace CoreGame.Collision
{
	public struct Hit
	{
		public Actor Actor;
		public Vector2 Normal;
		public Body Body;
	}
}