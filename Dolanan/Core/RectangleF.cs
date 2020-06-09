using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	/// <summary>Describes a 2D-rectangle.</summary>
	[DataContract]
	public struct RectangleF : IEquatable<RectangleF>
	{
		/// <summary>
		///     The x coordinate of the top-left corner of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float X;

		/// <summary>
		///     The y coordinate of the top-left corner of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Y;

		/// <summary>
		///     The width of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Width;

		/// <summary>
		///     The height of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		[DataMember] public float Height;

		/// <summary>
		///     Returns a <see cref="T:Microsoft.Xna.Framework.Rectangle" /> with X=0, Y=0, Width=0, Height=0.
		/// </summary>
		public static RectangleF Empty { get; }

		/// <summary>
		///     Returns the x coordinate of the left edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Left => X;

		/// <summary>
		///     Returns the x coordinate of the right edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Right => X + Width;

		/// <summary>
		///     Returns the y coordinate of the top edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Top => Y;

		/// <summary>
		///     Returns the y coordinate of the bottom edge of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public float Bottom => Y + Height;

		/// <summary>
		///     Whether or not this <see cref="T:Microsoft.Xna.Framework.Rectangle" /> has a
		///     <see cref="F:Microsoft.Xna.Framework.Rectangle.Width" /> and
		///     <see cref="F:Microsoft.Xna.Framework.Rectangle.Height" /> of 0, and a
		///     <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of (0, 0).
		/// </summary>
		public bool IsEmpty => Width == 0 && Height == 0 && X == 0 && Y == 0;

		/// <summary>
		///     The top-left coordinates of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public Vector2 Location
		{
			get => new Vector2(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		/// <summary>
		///     The width-height coordinates of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		public Vector2 Size
		{
			get => new Vector2(Width, Height);
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}

		/// <summary>
		///     A <see cref="T:Microsoft.Xna.Framework.Vector2" /> located in the center of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <remarks>
		///     If <see cref="F:Microsoft.Xna.Framework.Rectangle.Width" /> or
		///     <see cref="F:Microsoft.Xna.Framework.Rectangle.Height" /> is an odd number,
		///     the center point will be rounded down.
		/// </remarks>
		public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);

		internal string DebugDisplayString =>
			X + "  " + Y + "  " + Width + "  " +
			Height;

		public RectangleF(Matrix matrix, Vector2 topLeftOffset, Vector2 topRightOffset, Vector2 bottomLeftOffset,
			Vector2 bottomRightOffest)
		{
			var topLeft = Vector2.Transform(topLeftOffset, matrix);
			var topRight = Vector2.Transform(topRightOffset, matrix);
			var bottomLeft = Vector2.Transform(bottomLeftOffset, matrix);
			var bottomRight = Vector2.Transform(bottomRightOffest, matrix);

			X = MathEx.Min(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X);
			Y = MathEx.Min(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y);
			Width = MathEx.Max(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X) - X;
			Height = MathEx.Max(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y) - Y;
		}

		/// <summary>
		///     Creates a new instance of <see cref="T:Microsoft.Xna.Framework.Rectangle" /> struct, with the specified
		///     position, width, and height.
		/// </summary>
		/// <param name="x">
		///     The x coordinate of the top-left corner of the created
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </param>
		/// <param name="y">
		///     The y coordinate of the top-left corner of the created
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </param>
		/// <param name="width">The width of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="height">The height of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public RectangleF(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		/// <summary>
		///     Creates a new instance of <see cref="T:Microsoft.Xna.Framework.Rectangle" /> struct, with the specified
		///     location and size.
		/// </summary>
		/// <param name="location">
		///     The x and y coordinates of the top-left corner of the created
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </param>
		/// <param name="size">The width and height of the created <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public RectangleF(Vector2 location, Vector2 size)
		{
			X = location.X;
			Y = location.Y;
			Width = size.X;
			Height = size.Y;
		}

		/// <summary>
		///     Compares whether two <see cref="T:Microsoft.Xna.Framework.Rectangle" /> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(RectangleF a, RectangleF b)
		{
			return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
		}

		/// <summary>
		///     Compares whether two <see cref="T:Microsoft.Xna.Framework.Rectangle" /> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="T:Microsoft.Xna.Framework.Rectangle" /> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
		public static bool operator !=(RectangleF a, RectangleF b)
		{
			return !(a == b);
		}

		/// <summary>
		///     Gets whether or not the provided coordinates lie within the bounds of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="x">The x coordinate of the point to check for containment.</param>
		/// <param name="y">The y coordinate of the point to check for containment.</param>
		/// <returns>
		///     <c>true</c> if the provided coordinates lie inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />;
		///     <c>false</c> otherwise.
		/// </returns>
		public bool Contains(float x, float y)
		{
			return X <= (double) x && x < (double) (X + Width) &&
			       Y <= (double) y && y < (double) (Y + Height);
		}

		/// <summary>
		///     Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies within the bounds of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns>
		///     <c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies inside this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise.
		/// </returns>
		public bool Contains(Vector2 value)
		{
			return X <= (double) value.X && value.X < (double) (X + Width) &&
			       Y <= (double) value.Y && value.Y < (double) (Y + Height);
		}

		/// <summary>
		///     Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies within the bounds of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result">
		///     <c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Vector2" /> lies inside this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise. As an output parameter.
		/// </param>
		public void Contains(ref Vector2 value, out bool result)
		{
			result = X <= (double) value.X && value.X < (double) (X + Width) &&
			         Y <= (double) value.Y && value.Y < (double) (Y + Height);
		}

		/// <summary>
		///     Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" /> lies within the bounds of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">
		///     The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to check for inclusion in this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </param>
		/// <returns>
		///     <c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" />'s bounds lie entirely inside
		///     this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise.
		/// </returns>
		public bool Contains(RectangleF value)
		{
			return X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y &&
			       value.Y + value.Height <= Y + Height;
		}

		/// <summary>
		///     Gets whether or not the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" /> lies within the bounds of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="value">
		///     The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to check for inclusion in this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </param>
		/// <param name="result">
		///     <c>true</c> if the provided <see cref="T:Microsoft.Xna.Framework.Rectangle" />'s bounds lie
		///     entirely inside this <see cref="T:Microsoft.Xna.Framework.Rectangle" />; <c>false</c> otherwise. As an output
		///     parameter.
		/// </param>
		public void Contains(ref RectangleF value, out bool result)
		{
			result = X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y &&
			         value.Y + value.Height <= Y + Height;
		}

		/// <summary>
		///     Compares whether current instance is equal to specified <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return obj is RectangleF rectangle && this == rectangle;
		}

		/// <summary>
		///     Compares whether current instance is equal to specified <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="other">The <see cref="T:Microsoft.Xna.Framework.Rectangle" /> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(RectangleF other)
		{
			return this == other;
		}

		/// <summary>
		///     Gets the hash code of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <returns>Hash code of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</returns>
		public override int GetHashCode()
		{
			return (((17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode()) * 23 + Width.GetHashCode()) *
				23 + Height.GetHashCode();
		}

		/// <summary>
		///     Adjusts the edges of this <see cref="T:Microsoft.Xna.Framework.Rectangle" /> by specified horizontal and vertical
		///     amounts.
		/// </summary>
		/// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
		/// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}

		/// <summary>
		///     Gets whether or not the other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <returns>
		///     <c>true</c> if other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle;
		///     <c>false</c> otherwise.
		/// </returns>
		public bool Intersects(RectangleF value)
		{
			return value.Left < Right && Left < value.Right && value.Top < Bottom &&
			       Top < value.Bottom;
		}

		/// <summary>
		///     Gets whether or not the other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <param name="result">
		///     <c>true</c> if other <see cref="T:Microsoft.Xna.Framework.Rectangle" /> intersects with this
		///     rectangle; <c>false</c> otherwise. As an output parameter.
		/// </param>
		public void Intersects(ref RectangleF value, out bool result)
		{
			result = value.Left < Right && Left < value.Right && value.Top < Bottom &&
			         Top < value.Bottom;
		}

		/// <summary>
		///     Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that contains overlapping region of two other
		///     rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns>Overlapping region of the two rectangles.</returns>
		public static RectangleF Intersect(RectangleF value1, RectangleF value2)
		{
			RectangleF result;
			Intersect(ref value1, ref value2, out result);
			return result;
		}

		/// <summary>
		///     Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that contains overlapping region of two other
		///     rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result">Overlapping region of the two rectangles as an output parameter.</param>
		public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			if (value1.Intersects(value2))
			{
				var num1 = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				var x = Math.Max(value1.X, value2.X);
				var y = Math.Max(value1.Y, value2.Y);
				var num2 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new RectangleF(x, y, num1 - x, num2 - y);
			}
			else
			{
				result = new RectangleF(0, 0, 0, 0);
			}
		}

		/// <summary>
		///     Changes the <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="offsetX">The x coordinate to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="offsetY">The y coordinate to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		/// <summary>
		///     Changes the <see cref="P:Microsoft.Xna.Framework.Rectangle.Location" /> of this
		///     <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="amount">The x and y components to add to this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		public void Offset(Vector2 amount)
		{
			X += amount.X;
			Y += amount.Y;
		}

		/// <summary>
		///     Returns a <see cref="T:System.String" /> representation of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />
		///     in the format:
		///     {X:[<see cref="F:Microsoft.Xna.Framework.Rectangle.X" />] Y:[<see cref="F:Microsoft.Xna.Framework.Rectangle.Y" />]
		///     Width:[<see cref="F:Microsoft.Xna.Framework.Rectangle.Width" />] Height:[
		///     <see cref="F:Microsoft.Xna.Framework.Rectangle.Height" />]}
		/// </summary>
		/// <returns><see cref="T:System.String" /> representation of this <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</returns>
		public override string ToString()
		{
			return "{X:" + X + " Y:" + Y + " Width:" + Width +
			       " Height:" + Height + "}";
		}

		/// <summary>
		///     Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <returns>The union of the two rectangles.</returns>
		public static RectangleF Union(RectangleF value1, RectangleF value2)
		{
			var x = Math.Min(value1.X, value2.X);
			var y = Math.Min(value1.Y, value2.Y);
			return new RectangleF(x, y, Math.Max(value1.Right, value2.Right) - x,
				Math.Max(value1.Bottom, value2.Bottom) - y);
		}

		/// <summary>
		///     Creates a new <see cref="T:Microsoft.Xna.Framework.Rectangle" /> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="value2">The second <see cref="T:Microsoft.Xna.Framework.Rectangle" />.</param>
		/// <param name="result">The union of the two rectangles as an output parameter.</param>
		public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			result = default;
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}

		/// <summary>
		///     Deconstruction method for <see cref="T:Microsoft.Xna.Framework.Rectangle" />.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Deconstruct(out float x, out float y, out float width, out float height)
		{
			x = X;
			y = Y;
			width = Width;
			height = Height;
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle((int) X, (int) Y, (int) Width, (int) Height);
		}
	}
}