#define DEBUG_SCREEN

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGame.Engine;
using Humper.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;


[assembly: InternalsVisibleTo("ScreenDebugger")]

namespace CoreGame.Tools
{
	public static class ScreenDebugger
	{
		private static Texture2D Texture2D
		{
			get
			{
				if (_texture2D == null)
				{
					_texture2D = GameClient.Instance.Content.Load<Texture2D>("1px");
				}

				return _texture2D;
			}
		}
		private static Texture2D _texture2D;

		static List<LineDebug> _lineDebugs = new List<LineDebug>();
		static List<SquareDebug> _squareDebugs = new List<SquareDebug>();

		internal static void Draw(SpriteBatch spriteBatch)
		{
#if DEBUG_SCREEN
			spriteBatch.Begin(transformMatrix: GameClient.Instance.World.Camera.GetTopLeftMatrix());
			foreach (var lineDebug in _lineDebugs)
			{
				spriteBatch.Draw(Texture2D, lineDebug.Line.V1, null, lineDebug.Color,
					lineDebug.Line.V2.AngleToPoint(lineDebug.Line.V1), Vector2.Zero, new Vector2(lineDebug.Width, 1), SpriteEffects.None, 1f);
			}
			foreach (var squareDebug in _squareDebugs)
			{
				spriteBatch.Draw(Texture2D, squareDebug.Position, null, Color.White, 0
					, new Vector2(Texture2D.Width/(float)2, Texture2D.Height/(float)2), squareDebug.Size, SpriteEffects.None, 1f);
			}
			spriteBatch.End();
			_lineDebugs.Clear();
			_squareDebugs.Clear();
#endif
		}

		public static void DebugDraw(LineDebug lineDebug)
		{
#if DEBUG_SCREEN
			_lineDebugs.Add(lineDebug);
#endif
		}

		public static void DrawSquare(SquareDebug s)
		{
#if DEBUG_SCREEN
			_squareDebugs.Add(s);
#endif
		}
	}

	public struct SquareDebug
	{
		public Vector2 Position;
		public float Size;

		public SquareDebug(Vector2 position, float size)
		{
			Position = position;
			Size = size;
		}
	}

	public struct LineDebug
	{
		public Line Line;
		public int Width;
		public Color Color;

		public LineDebug(Line line, int width)
		{
			Line = line;
			Width = width;
			Color = Color.White;
		}
	}
	
	
	
	
	public static class Debug
	{
		private static Texture2D pixel;

		public static Rectangle ToRectangle(this RectangleF r)
		{
			return new Rectangle((int)r.X,(int)r.Y,(int)r.Width,(int)r.Height);
		}

		public static void Draw(this SpriteBatch spriteBatch, RectangleF rect, Color color)
		{
			spriteBatch.Draw(rect.ToRectangle(), color);
		}

		public static void Draw(this SpriteBatch spriteBatch, RectangleF rect, Color color, float fillOpacity)
		{
			spriteBatch.Draw(rect.ToRectangle(), color, fillOpacity);
		}

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
	}
}
// #undef DEBUG_SCREEN