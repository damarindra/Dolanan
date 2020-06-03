using System;
using System.Linq;
using CoreGame.Engine;
using CoreGame.Tools;
using Humper;
using Humper.Base;
using Humper.Responses;
using Microsoft.Xna.Framework;
using World = CoreGame.Scene.World;

namespace CoreGame.Collision
{
	public class Body : IBox
	{
		public Transform2D Transform { get; private set; }
		public float X => Bounds.X;
		public float Y => Bounds.Y;
		public float Width => Bounds.Width;
		public float Height => Bounds.Height;
		public RectangleF Bounds => _bounds;
		public object Data { get; set; }

		private RectangleF _bounds;
		private Vector2 _origin;
		private World _world;

		public Body(World world, Transform2D transform, Vector2 origin, float width, float height)
		{
			_origin = origin;
			_world = world;
			Transform = transform;
			_bounds = new RectangleF(transform.GlobalPosition.X - origin.X, transform.GlobalPosition.Y - origin.Y,
				width, height);
		}

		public void Teleport(Vector2 location)
		{
			if (!_world.IsValidLocation(location))
			{
				Log.PrintError("Location is Not a Valid place : " + location + ". Out of Bounds");
				return;
			}
			Transform.Position = location;
			_bounds.Location = location - _origin;
		}
		
		public IMovement Move(float x, float y, Func<ICollision, ICollisionResponse> filter)
		{
			// x -= _origin.X;
			// y -= _origin.Y;
			var movement = this.Simulate(x, y, filter);
			this._bounds.X = movement.Destination.X;
			this._bounds.Y = movement.Destination.Y;
			_world.Update(this, movement.Origin);
			Transform.Position = new Vector2(_bounds.X + _origin.X, _bounds.Y+ _origin.Y);
			return movement;
		}

		public IMovement Move(float x, float y, Func<ICollision, CollisionResponses> filter)
		{
			// x -= _origin.X;
			// y -= _origin.Y;
			var movement = this.Simulate(x, y, filter);
			this._bounds.X = movement.Destination.X;
			this._bounds.Y = movement.Destination.Y;
			_world.Update(this, movement.Origin);
			Transform.Position = new Vector2(_bounds.X + _origin.X, _bounds.Y + _origin.Y);
			return movement;
		}

		public IMovement Simulate(float x, float y, Func<ICollision, ICollisionResponse> filter)
		{
			// x -= _origin.X;
			// y -= _origin.Y;
			return _world.Simulate(this, x - _origin.X, y - _origin.Y, filter);
		}

		public IMovement Simulate(float x, float y, Func<ICollision, CollisionResponses> filter)
		{
			// x -= _origin.X;
			// y -= _origin.Y;
			return _world.Simulate(this, x - _origin.X, y - _origin.Y, collision =>
			{
				if (collision.Hit == null)
					return null;
				return CollisionResponse.Create(collision, filter(collision));
			});
			
			// return Move(x, y, (col) =>
			// {
			// 	if (col.Hit == null)
			// 		return null;
			//
			// 	return CollisionResponse.Create(col, filter(col));
			// });
		}

		#region Tags

		private CollisionTagFlag tags;

		public IBox AddTags(params Enum[] newTags)
		{
			foreach (var tag in newTags)
			{
				this.AddTag(tag);
			}

			return this;
		}

		public IBox RemoveTags(params Enum[] newTags)
		{
			foreach (var tag in newTags)
			{
				this.RemoveTag(tag);
			}

			return this;
		}

		private void AddTag(Enum tag)
		{
			if (tags == CollisionTagFlag.None)
			{
				tags = (CollisionTagFlag)tag;
			}
			else
			{
				var t = this.tags.GetType();
				var ut = Enum.GetUnderlyingType(t);

				if (ut != typeof(ulong))
					this.tags = (CollisionTagFlag)Enum.ToObject(t, Convert.ToInt64(this.tags) | Convert.ToInt64(tag));
				else
					this.tags = (CollisionTagFlag)Enum.ToObject(t, Convert.ToUInt64(this.tags) | Convert.ToUInt64(tag));
			}
		}

		private void RemoveTag(Enum tag)
		{
			if (tags != CollisionTagFlag.None)
			{
				var t = this.tags.GetType();
				var ut = Enum.GetUnderlyingType(t);

				if (ut != typeof(ulong))
					this.tags = (CollisionTagFlag)Enum.ToObject(t, Convert.ToInt64(this.tags) & ~Convert.ToInt64(tag));
				else
					this.tags = (CollisionTagFlag)Enum.ToObject(t, Convert.ToUInt64(this.tags) & ~Convert.ToUInt64(tag));
			}
		}

		public bool HasTag(params Enum[] values)
		{
			return (tags != CollisionTagFlag.None) && values.Any((value) => this.tags.HasFlag(value));
		}

		public bool HasTags(params Enum[] values)
		{
			return (tags != CollisionTagFlag.None) && values.All((value) => this.tags.HasFlag(value));
		}

		#endregion
	}

	[Flags]
	public enum CollisionTagFlag
	{
		None = 0,
		Default = 1,
		Wall = 2,
		Dynamic = 4,
		Tag3 = 8,
		Tag4 = 16,
		Cross = 32
	}
}