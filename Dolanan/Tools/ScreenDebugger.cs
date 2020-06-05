
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dolanan.Controller;
using Dolanan.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Dolanan.Tools
{
	public static class ScreenDebugger
	{
		private static Texture2D pixel;

		public static void Draw(this SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			if (pixel == null)
			{
				pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				pixel.SetData(new Color[] { Color.White });
			}

			spriteBatch.Draw(pixel, destinationRectangle: rect, color: color);
		}

		public static void Draw(this SpriteBatch spriteBatch, Rectangle rect, Color stroke, float fillOpacity)
		{
			if (pixel == null)
			{
				pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				pixel.SetData(new Color[] { Color.White });
			}

			var fill = new Color(stroke, fillOpacity);
			spriteBatch.DrawFill(rect, fill);
			spriteBatch.DrawStroke(rect, stroke);
		}

		public static void DrawFill(this SpriteBatch spriteBatch, Rectangle rect, Color fill)
		{
			if (pixel == null)
			{
				pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				pixel.SetData(new Color[] { Color.White });
			}

			spriteBatch.Draw(pixel, destinationRectangle: rect, color: fill);
		}

		public static void DrawStroke(this SpriteBatch spriteBatch, Rectangle rect, Color stroke)
		{
			if (pixel == null)
			{
				pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				pixel.SetData(new Color[] { Color.White });
			}

			var left = new Rectangle(rect.Left, rect.Top, 1, rect.Height);
			var right = new Rectangle(rect.Right - 1, rect.Top, 1, rect.Height);
			var top = new Rectangle(rect.Left, rect.Top, rect.Width, 1);
			var bottom = new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, 1);

			spriteBatch.Draw(pixel, destinationRectangle: left, color: stroke);
			spriteBatch.Draw(pixel, destinationRectangle: right, color: stroke);
			spriteBatch.Draw(pixel, destinationRectangle: top, color: stroke);
			spriteBatch.Draw(pixel, destinationRectangle: bottom, color: stroke);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
		{
			var distance = Vector2.Distance(point1, point2);
			var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			DrawLine(spriteBatch, point1, distance, angle, color, thickness);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
		{
			var origin = new Vector2(0f, 0.5f);
			var scale = new Vector2(length, thickness);
			spriteBatch.Draw(pixel, point, null, color, angle, origin, scale, SpriteEffects.None, 0);
		}
	}
}
// #undef DEBUG_SCREEN