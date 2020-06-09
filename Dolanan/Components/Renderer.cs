using System;
using System.Runtime.Serialization;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components
{
	public class Renderer : Component
	{
		private Vector2 _origin;

		private Pivot _pivot = Pivot.Center;
		private Point _srcSize;
		private Texture2D _texture;

		public SpriteEffects SpriteEffect = SpriteEffects.None;

		public Renderer(Actor owner) : base(owner)
		{
		}

		public Texture2D Texture2D
		{
			get => _texture;
			set
			{
				if (_texture == null)
				{
					SrcLocation = Point.Zero;
					SrcSize = new Point(value.Width, value.Height);
				}

				_texture = value;
			}
		}

		/// <summary>
		///     rectangle texture, cropping the texture with this rect
		/// </summary>
		public Rectangle SrcRectangle => new Rectangle(SrcLocation, _srcSize);

		public Point SrcLocation { get; set; }

		public Point SrcSize
		{
			get => _srcSize;
			set
			{
				_origin.X = Pivot.X * value.X;
				_origin.Y = Pivot.X * value.Y;
				_srcSize = value;
			}
		}

		public Pivot Pivot
		{
			get => _pivot;
			set
			{
				_origin.X = value.X * _srcSize.X;
				_origin.Y = value.Y * _srcSize.Y;
				_pivot = value;
			}
		}

		public Vector2 Origin => _origin;
		public Color ModulatedColor { get; set; } = Color.White;

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			if (Texture2D != null)
				GameMgr.SpriteBatch.Draw(Texture2D, Owner.Transform.GlobalPosition, SrcRectangle, ModulatedColor,
					Owner.Transform.GlobalRotation, _origin, Owner.Transform.GlobalScale, SpriteEffect, layerZDepth);
		}
	}


	[DataContract]
	public struct Pivot : IEquatable<Pivot>
	{
		static Pivot()
		{
			Center = new Pivot(.5f, .5f);
			TopLeft = new Pivot(0, 0);
			TopRight = new Pivot(1, 0);
			BottomLeft = new Pivot(0, 1);
			BottomRight = new Pivot(1, 1);
		}

		public float X;
		public float Y;

		public Pivot(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Pivot(float value)
		{
			X = value;
			Y = value;
		}

		public static Pivot Center { get; }
		public static Pivot TopLeft { get; }
		public static Pivot TopRight { get; }
		public static Pivot BottomLeft { get; }
		public static Pivot BottomRight { get; }

		public bool Equals(Vector2 other)
		{
			return X == (double) other.X && Y == (double) other.Y;
		}

		public override bool Equals(object obj)
		{
			return obj is Pivot other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (X.GetHashCode() * 397) ^ Y.GetHashCode();
		}

		public bool Equals(Pivot other)
		{
			return X == (double) other.X && Y == (double) other.Y;
		}

		public static Pivot operator -(Pivot value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static Pivot operator +(Pivot value1, Pivot value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		public static Pivot operator -(Pivot value1, Pivot value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		public static Pivot operator *(Pivot value1, Pivot value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		public static Pivot operator *(Pivot value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		public static Pivot operator *(float scaleFactor, Pivot value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		public static Pivot operator /(Pivot value1, Pivot value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		public static Pivot operator /(Pivot value1, float divider)
		{
			var num = 1f / divider;
			value1.X *= num;
			value1.Y *= num;
			return value1;
		}

		public static bool operator ==(Pivot value1, Vector2 value2)
		{
			return value1.X == (double) value2.X && value1.Y == (double) value2.Y;
		}

		public static bool operator !=(Pivot value1, Vector2 value2)
		{
			return value1.X != (double) value2.X || value1.Y != (double) value2.Y;
		}

		public static implicit operator Vector2(Pivot val)
		{
			return new Vector2(val.X, val.Y);
		}

		public static implicit operator Point(Pivot val)
		{
			return new Point((int) val.X, (int) val.Y);
		}
	}
}