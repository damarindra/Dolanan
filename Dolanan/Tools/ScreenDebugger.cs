
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
		public static Texture2D Pixel
		{
			get
			{
				if(_pixel == null){
					_pixel = new Texture2D(GameMgr.SpriteBatch.GraphicsDevice, 1, 1);
					_pixel.SetData(new Color[] { Color.White });
				}

				return _pixel;
			}
		}
		private static Texture2D _pixel;
		
		public static void Draw(this SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			spriteBatch.Draw(Pixel, destinationRectangle: rect, color: color);
		}

		public static void Draw(this SpriteBatch spriteBatch, Rectangle rect, Color stroke, float fillOpacity)
		{
			var fill = new Color(stroke, fillOpacity);
			spriteBatch.DrawFill(rect, fill);
			spriteBatch.DrawStroke(rect, stroke);
		}

		public static void DrawFill(this SpriteBatch spriteBatch, Rectangle rect, Color fill)
		{
			spriteBatch.Draw(Pixel, destinationRectangle: rect, color: fill);
		}

		public static void DrawStroke(this SpriteBatch spriteBatch, Rectangle rect, Color stroke)
		{
			var left = new Rectangle(rect.Left, rect.Top, 1, rect.Height);
			var right = new Rectangle(rect.Right - 1, rect.Top, 1, rect.Height);
			var top = new Rectangle(rect.Left, rect.Top, rect.Width, 1);
			var bottom = new Rectangle(rect.Left, rect.Bottom - 1, rect.Width, 1);

			spriteBatch.Draw(Pixel, destinationRectangle: left, color: stroke);
			spriteBatch.Draw(Pixel, destinationRectangle: right, color: stroke);
			spriteBatch.Draw(Pixel, destinationRectangle: top, color: stroke);
			spriteBatch.Draw(Pixel, destinationRectangle: bottom, color: stroke);
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
			spriteBatch.Draw(Pixel, point, null, color, angle, origin, scale, SpriteEffects.None, 0);
		}
	}
}
// #undef DEBUG_SCREEN