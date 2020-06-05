using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Collision
{
	public struct Hit
	{
		public Actor Actor;
		public Vector2 Normal;
		public Body Body;
	}
}