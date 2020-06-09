using Dolanan.Components;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Collision
{
	/// <summary>
	///     AABB is the base class for Box Collider, don't use it! Use Body!
	/// </summary>
	public class AABB : Component
	{
		public Vector2 Offset;

		public AABB(Actor owner) : base(owner)
		{
		}

		public Vector2 Position
		{
			get => Owner.Transform.GlobalPosition - Offset;
			set => Owner.Transform.GlobalPosition = value + Offset;
			// Center = _position + _size / 2f;
		}

		/// <summary>
		///     For dynamic aabb only, this will shrink the aabb when simulating. Change this only if you understand
		/// </summary>
		public Vector2 Size
		{
			get;
			set;
			// Center = _position + _size / 2f;
			// Extents = _size / 2f;
		} = Vector2.One * 32;

		// public Vector2 Center { get; private set; }
		// public Vector2 Extents { get; private set; }
		public Vector2 Min => Position;
		public Vector2 Max => Position + Size;

		public override void Start()
		{
			base.Start();
			Position = Owner.Transform.GlobalPosition;
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