using System.Reflection.Metadata.Ecma335;
using CoreGame.Engine;
using Microsoft.Xna.Framework;

namespace CoreGame.Collision
{
	
	//TODO : Stuck after colliding. AABB only is not stuck
	public class Body : AABB
	{
		public Transform2D Transform { get; private set; }
		public Vector2 Origin;

		public override Vector2 Position
		{
			get => Transform.GlobalPosition;
			set
			{
				Transform.GlobalPosition = value;
				
			}
		}

		public Body(Transform2D transform2D, Vector2 origin, Vector2 size) : base()
		{
			Transform = transform2D;
			Origin = origin;
		}

		public void Move(Vector2 velocity)
		{
			
		}
	}
}