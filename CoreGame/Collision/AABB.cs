using System;
using Microsoft.Xna.Framework;

namespace CoreGame.Collision
{
	public class AABB
	{
		public virtual Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				// Center = _position + _size / 2f;
			}
		}
		public virtual Vector2 Size
		{
			get => _size;
			set
			{
				_size = value;
				// Center = _position + _size / 2f;
				// Extents = _size / 2f;
			}
		}

		// public Vector2 Center { get; private set; }
		// public Vector2 Extents { get; private set; }
		public Vector2 Min => Position;
		public Vector2 Max => Position + Size;
		/// <summary>
		/// For dynamic aabb only, this will shrink the aabb when simulating. Change this only if you understand
		/// </summary>
		// public float CollisionCheck = 0.0001f;
		// public Vector2 MinEx => Position - new Vector2(CollisionCheck);
		// public Vector2 MaxEx => Position + Size + new Vector2(CollisionCheck);

		// public Vector2 TopLeft => Min;
		// public Vector2 TopRight => Position + Vector2.UnitX * Size.X;
		// public Vector2 BottomLeft => Position + Vector2.UnitY * Size.Y;
		// public Vector2 BottomRight => Max;

		private Vector2 _position, _size;
		
		public AABB(){}
		public AABB(Vector2 position, Vector2 size)
		{
			Position = position;
			Size = size;
		}

		public bool Overlaps(AABB other)
		{
			return Min.X < other.Max.X &&
			       Max.X > other.Min.X &&
			       Min.Y < other.Max.Y &&
			       Max.Y > other.Min.Y;
			
			// Obsolete
			// Vector2 T = other.Center - Center;
			//
			// return MathF.Abs(T.X) <= (Extents.X + other.Extents.X) &&
			//        MathF.Abs(T.Y) <= (Extents.Y + other.Extents.Y);
		}
	}
}