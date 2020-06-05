using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public struct Line
	{
		public Vector2 V1, V2;

		public Line(Vector2 v1, Vector2 v2)
		{
			V1 = v1;
			V2 = v2;
		}
	}
}