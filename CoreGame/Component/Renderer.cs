using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CoreGame.Engine;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Component
{
	public class Renderer : BaseComponent
	{
		public Texture2D Texture2D { get; set; }

		// BoundingBox2D will control the render. It will check if this boundingbox inside the camera boundingBox
		public BoundingBox2D BoundingBox
		{
			get
			{
				return new BoundingBox2D(TransformComponent.Matrix, 
					-_origin, 
					new Vector2(_srcSize.X - _origin.X, -_origin.Y),
					new Vector2(-_origin.X, _srcSize.Y - _origin.Y),
					new Vector2(_srcSize.X, _srcSize.Y) - _origin);
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

		private RendererPivot _pivot = RendererPivot.Center;
		private Vector2 _origin;

		public Color ModulatedColor { get; set; } = Color.White;

		public SpriteEffects SpriteEffect = SpriteEffects.None;
		private Point _srcSize;
		public float LayerDepth { get; set; }

		public Renderer() : base()
		{
		}

		public override void UpdateComponent(GameTime gameTime)
		{
			base.UpdateComponent(gameTime);

			BoundingBox2D box = BoundingBox;
			ScreenDebugger.DebugDraw(new LineDebug(new Line(box.Location, box.Location + TransformComponent.Down * 10),
				5));
			ScreenDebugger.DebugDraw(new LineDebug(new Line(box.Location + new Vector2(box.Width, 0),
					box.Location+ new Vector2(box.Width, 0) + TransformComponent.Down * 10),
				5));
			ScreenDebugger.DebugDraw(new LineDebug(new Line(box.Location+ new Vector2(0, box.Height),
					box.Location + new Vector2(0, box.Height) + TransformComponent.Down * 10),
				5));
			ScreenDebugger.DebugDraw(new LineDebug(new Line(box.Location + box.Size, box.Location+ box.Size + TransformComponent.Down * 10),
				5));
		}

		public override void DrawComponent(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Please check Game.Draw (https://github.com/MonoGame/MonoGame/issues/3624) 
			// NumDraws
			// NumClears
			// NumTargets
			// NumTextures
			if(GameClient.Instance.World.Camera.BoundingBox2D.Intersects(BoundingBox))
				spriteBatch.Draw(Texture2D, TransformComponent.GlobalPosition, SrcRectangle, ModulatedColor,
					TransformComponent.GlobalRotation, _origin, TransformComponent.Scale, SpriteEffect, LayerDepth);
			else
				Log.PrintWarning("Skipping render : " + Owner.Name);
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