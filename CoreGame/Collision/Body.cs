using System.Reflection.Metadata.Ecma335;
using CoreGame.Controller;
using CoreGame.Engine;
using Microsoft.Xna.Framework;

namespace CoreGame.Collision
{
	
	//TODO : Stuck after colliding. AABB only is not stuck
	public class Body : AABB
	{
		public Vector2 Origin;
		public BodyType BodyType = BodyType.Static;

		public Body(){}
		
		public Body(Transform2D transform2D, Vector2 origin, Vector2 size) : base()
		{
			Position = transform2D.GlobalPosition;
			Origin = origin;
			Size = size;
		}

		public void Move(Vector2 velocity)
		{
			if (BodyType == BodyType.Static)
				return;
			Vector2 resultVelo;
			Vector2 resutlRemainder;
			Hit hit;
			GameMgr.Game.World.CheckCollision(this, velocity, out resultVelo, out resutlRemainder, out hit);
			if (BodyType == BodyType.Kinematic)
			{
				if (hit.Normal != Vector2.Zero)
					resultVelo += resutlRemainder.Slide(hit.Normal);
				Position += resultVelo;
			}
		}
	}

	public enum BodyType
	{
		Static, Kinematic, Overlap
	}
}