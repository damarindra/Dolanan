﻿using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Dolanan.Engine;
using Dolanan.Tools;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components
{
	public class Renderer : Component
	{
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
		/// rectangle texture, cropping the texture with this rect
		/// </summary>
		public Rectangle SrcRectangle
		{
			get => new Rectangle(SrcLocation, _srcSize);
		}

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
		
		public RendererPivot Pivot
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

		public SpriteEffects SpriteEffect = SpriteEffects.None;

		private RendererPivot _pivot = RendererPivot.Center;
		private Vector2 _origin;
		private Point _srcSize;
		private Texture2D _texture;

		public Renderer(Actor owner) : base(owner) { }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			if(Texture2D != null)
				GameMgr.SpriteBatch.Draw(Texture2D, Owner.Transform.GlobalPosition, SrcRectangle, ModulatedColor,
					Owner.Transform.GlobalRotation, _origin, Owner.Transform.GlobalScale, SpriteEffect, layerZDepth);
		}
	}


	[DataContract]
	public struct RendererPivot : IEquatable<RendererPivot>
	{
		static RendererPivot()
		{
			RendererPivot.Center = new RendererPivot(.5f, .5f);
			RendererPivot.TopLeft = new RendererPivot(0, 0);
			RendererPivot.TopRight = new RendererPivot(1, 0);
			RendererPivot.BottomLeft = new RendererPivot(0, 1);
			RendererPivot.BottomRight = new RendererPivot(1, 1);
		}

		public float X;
		public float Y;

		public RendererPivot(float x, float y)
		{
			X = x;
			Y = y;
		}

		public RendererPivot(float value)
		{
			X = value;
			Y = value;
		}

		public static RendererPivot Center { get; private set; }
		public static RendererPivot TopLeft { get; private set; }
		public static RendererPivot TopRight { get; private set; }
		public static RendererPivot BottomLeft { get; private set; }
		public static RendererPivot BottomRight { get; private set; }

		public bool Equals(Vector2 other)
		{
			return (double) this.X == (double) other.X && (double) this.Y == (double) other.Y;
		}

		public override bool Equals(object obj)
		{
			return obj is RendererPivot other && Equals(other);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() * 397 ^ this.Y.GetHashCode();
		}

		public bool Equals(RendererPivot other)
		{
			return (double) this.X == (double) other.X && (double) this.Y == (double) other.Y;
		}

		public static RendererPivot operator -(RendererPivot value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static RendererPivot operator +(RendererPivot value1, RendererPivot value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		public static RendererPivot operator -(RendererPivot value1, RendererPivot value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		public static RendererPivot operator *(RendererPivot value1, RendererPivot value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		public static RendererPivot operator *(RendererPivot value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		public static RendererPivot operator *(float scaleFactor, RendererPivot value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		public static RendererPivot operator /(RendererPivot value1, RendererPivot value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		public static RendererPivot operator /(RendererPivot value1, float divider)
		{
			float num = 1f / divider;
			value1.X *= num;
			value1.Y *= num;
			return value1;
		}

		public static bool operator ==(RendererPivot value1, Vector2 value2)
		{
			return (double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y;
		}

		public static bool operator !=(RendererPivot value1, Vector2 value2)
		{
			return (double) value1.X != (double) value2.X || (double) value1.Y != (double) value2.Y;
		}

		public static implicit operator Vector2(RendererPivot val)
		{
			return new Vector2(val.X, val.Y);
		}

		public static implicit operator Point(RendererPivot val)
		{
			return new Point((int) val.X, (int) val.Y);
		}
	}
}