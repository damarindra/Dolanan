using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	/// <summary>Describes a 2D-rectangle.</summary>
	[DataContract][Obsolete]
	public struct BoundingBox2D : IEquatable<BoundingBox2D>
	{
		private static BoundingBox2D _emptyBoundingBox2D;

		/// <summary>
		/// The x coordinate of the top-left corner of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float X;

		/// <summary>
		/// The y coordinate of the top-left corner of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Y;

		/// <summary>
		/// The width of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Width;

		/// <summary>
		/// The height of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Height;

		/// <summary>
		/// Returns a <see cref="T:Microsoft.Xna.Framework.Rectangle" /> with X=0, Y=0, Width=0, Height=0.
		/// </summary>
		public static BoundingBox2D Empty
		{
			get { return BoundingBox2D._emptyBoundingBox2D; }
		}

		/// <summary>
		/// Returns the x coordinate of the left edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Left
		{
			get { return this.X; }
		}

		/// <summary>
		/// Returns the x coordinate of the right edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Right
		{
			get { return this.X + this.Width; }
		}

		/// <summary>
		/// Returns the y coordinate of the top edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Top
		{
			get { return this.Y; }
		}

		/// <summary>
		/// Returns the y coordinate of the bottom edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Bottom
		{
			get { return this.Y + this.Height; }
		}

		/// <summary>
		/// Whether or not this <see cref="T:Microsoft.Xna.Framework.Rectangle" /> has a <see cref="F:Microsoft.Xna.Framework.Rectangle.Width" /> and
		/// <see cref="F:Microsoft.Xna.Framework.Rectangle.Height" /> of 0, and a <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of (0, 0).
		/// </summary>
		public bool IsEmpty
		{
			get { return this.Width == 0 && this.Height == 0 && this.X == 0 && this.Y == 0; }
		}

		/// <summary>
		/// The top-left coordinates of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public Vector2 Location
		{
			get { return new Vector2(this.X, this.Y); }
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		/// <summary>
		/// The width-height coordinates of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public Vector2 Size
		{
			get { return new Vector2(this.Width, this.Height); }
			set
			{
				this.Width = value.X;
				this.Height = value.Y;
			}
		}

		/// <summary>
		/// A <see cref="T:Microsoft.Xna.Framework.Vector2" /> located in the center of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <remarks>
		/// If <see cref="F:Microsoft.Xna.Framework.Rectangle.Width" /> or <see cref="F:Microsoft.Xna.Framework.Rectangle.Height" /> is an odd number,
		/// the center point will be rounded down.
		/// </remarks>
		public Vector2 Center
		{
			get { return new Vector2(this.X + this.Width / 2, this.Y + this.Height / 2); }
		}

		internal string DebugDisplayString
		{
			get
			{
				return this.X.ToString() + "  " + (object) this.Y + "  " + (object) this.Width + "  " +
				       (object) this.Height;
			}
		}

		public BoundingBox2D(Matrix matrix, Vector2 topLeftOffset, Vector2 topRightOffset, Vector2 bottomLeftOffset,
			Vector2 bottomRightOffest)
		{
			Vector2 topLeft = Vector2.Transform(topLeftOffset, matrix);
			Vector2 topRight = Vector2.Transform(topRightOffset, matrix);
			Vector2 bottomLeft = Vector2.Transform(bottomLeftOffset, matrix);
			Vector2 bottomRight = Vector2.Transform(bottomRightOffest, matrix);

			X = MathEx.Min(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X);
			Y = MathEx.Min(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y);
			Width = MathEx.Max(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X) - X;
			Height = MathEx.Max(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y) - Y;
		}

		/// <summary>
		/// Creates a new instance of <see cref="T:Microsoft.Xna.Framework.Rectangle" /> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="width">The width of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="height">The height of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public BoundingBox2D(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Creates a new instance of <see cref="T:Microsoft.Xna.Framework.Rectangle" /> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="size">The width and height of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public BoundingBox2D(Vector2 location, Vector2 size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}

		/// <summary>
		/// Compares whether two <see cref="T:Microsoft.Xna.Framework.Rectangle" /> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(BoundingBox2D a, BoundingBox2D b)
		{
			return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
		}

		/// <summary>
		/// Compares whether two <see cref="T:Microsoft.Xna.Framework.Rectangle" /> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
		public static bool operator !=(BoundingBox2D a, BoundingBox2D b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Gets whether or not the provided coordinates lie within the bounds of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="x">The x coordinate of the point to check for containment.</param>
		/// <param name="y">The y coordinate of the point to check for containment.</param>
		/// <returns><c>true</c> if the provided coordinates lie inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise.</returns>
		public bool Contains(float x, float y)
		{
			return (double) this.X <= (double) x && (double) x < (double) (this.X + this.Width) &&
			       (double) this.Y <= (double) y && (double) y < (double) (this.Y + this.Height);
		}

		/// <summary>
		/// Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies within the bounds of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns><c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise.</returns>
		public bool Contains(Vector2 value)
		{
			return (double) this.X <= (double) value.X && (double) value.X < (double) (this.X + this.Width) &&
			       (double) this.Y <= (double) value.Y && (double) value.Y < (double) (this.Y + this.Height);
		}

		/// <summary>
		/// Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies within the bounds of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref Vector2 value, out bool result)
		{
			result = (double) this.X <= (double) value.X && (double) value.X < (double) (this.X + this.Width) &&
			         (double) this.Y <= (double) value.Y && (double) value.Y < (double) (this.Y + this.Height);
		}

		/// <summary>
		/// Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" /> lies within the bounds of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns><c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" />'s bounds lie entirely inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise.</returns>
		public bool Contains(BoundingBox2D value)
		{
			return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y &&
			       value.Y + value.Height <= this.Y + this.Height;
		}

		/// <summary>
		/// Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" /> lies within the bounds of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" />'s bounds lie entirely inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref BoundingBox2D value, out bool result)
		{
			result = this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y &&
			         value.Y + value.Height <= this.Y + this.Height;
		}

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return obj is BoundingBox2D rectangle && this == rectangle;
		}

		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="other">The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(BoundingBox2D other)
		{
			return this == other;
		}

		/// <summary>
		/// Gets the hash code of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <returns>Hash code of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</returns>
		public override int GetHashCode()
		{
			return (((17 * 23 + this.X.GetHashCode()) * 23 + this.Y.GetHashCode()) * 23 + this.Width.GetHashCode()) *
				23 + this.Height.GetHashCode();
		}

		/// <summary>
		/// Adjusts the edges of this <see cref="T:Microsoft.Xna.Framework.Rectangle" /> by specified horizontal and vertical amounts.
		/// </summary>
		/// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
		/// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			this.X -= (float) horizontalAmount;
			this.Y -= (float) verticalAmount;
			this.Width += (float) horizontalAmount * 2;
			this.Height += (float) verticalAmount * 2;
		}

		/// <summary>
		/// Gets whether or not the other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <returns><c>true</c> if other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle; <c>false</c> otherwise.</returns>
		public bool Intersects(BoundingBox2D value)
		{
			return value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom &&
			       this.Top < value.Bottom;
		}

		/// <summary>
		/// Gets whether or not the other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <param name="result"><c>true</c> if other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
		public void Intersects(ref BoundingBox2D value, out bool result)
		{
			result = value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom &&
			         this.Top < value.Bottom;
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that contains overlapping region of two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns>Overlapping region of the two rectangles.</returns>
		public static BoundingBox2D Intersect(BoundingBox2D value1, BoundingBox2D value2)
		{
			BoundingBox2D result;
			BoundingBox2D.Intersect(ref value1, ref value2, out result);
			return result;
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that contains overlapping region of two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result">Overlapping region of the two rectangles as an output parameter.</param>
		public static void Intersect(ref BoundingBox2D value1, ref BoundingBox2D value2, out BoundingBox2D result)
		{
			if (value1.Intersects(value2))
			{
				float num1 = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				float x = Math.Max(value1.X, value2.X);
				float y = Math.Max(value1.Y, value2.Y);
				float num2 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new BoundingBox2D(x, y, num1 - x, num2 - y);
			}
			else
				result = new BoundingBox2D(0, 0, 0, 0);
		}

		/// <summary>
		/// Changes the <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="offsetX">The x coordinate to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="offsetY">The y coordinate to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public void Offset(float offsetX, float offsetY)
		{
			this.X += offsetX;
			this.Y += offsetY;
		}

		/// <summary>
		/// Changes the <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="amount">The x and y components to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public void Offset(Vector2 amount)
		{
			this.X += amount.X;
			this.Y += amount.Y;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String" /> representation of this <see cref="T:Microsoft.Xna.Framework.Rectangle" /> in the format:
		/// {X:[<see cref="F:Microsoft.Xna.Framework.Rectangle.X" />] Y:[<see cref="F:Microsoft.Xna.Framework.Rectangle.Y" />] Width:[<see cref="F:Microsoft.Xna.Framework.Rectangle.Width" />] Height:[<see cref="F:Microsoft.Xna.Framework.Rectangle.Height" />]}
		/// </summary>
		/// <returns><see cref="T:System.String" /> representation of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</returns>
		public override string ToString()
		{
			return "{X:" + this.X.ToString() + " Y:" + this.Y.ToString() + " Width:" + this.Width.ToString() +
			       " Height:" + this.Height.ToString() + "}";
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns>The union of the two rectangles.</returns>
		public static BoundingBox2D Union(BoundingBox2D value1, BoundingBox2D value2)
		{
			float x = Math.Min(value1.X, value2.X);
			float y = Math.Min(value1.Y, value2.Y);
			return new BoundingBox2D(x, y, Math.Max(value1.Right, value2.Right) - x,
				Math.Max(value1.Bottom, value2.Bottom) - y);
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result">The union of the two rectangles as an output parameter.</param>
		public static void Union(ref BoundingBox2D value1, ref BoundingBox2D value2, out BoundingBox2D result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}

		/// <summary>
		/// Deconstruction method for <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Deconstruct(out float x, out float y, out float width, out float height)
		{
			x = this.X;
			y = this.Y;
			width = this.Width;
			height = this.Height;
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle((int) X, (int) Y, (int) Width, (int) Height);
		}
	}
}