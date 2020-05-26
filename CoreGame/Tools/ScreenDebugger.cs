#define DEBUG_SCREEN

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
}
// #undef DEBUG_SCREEN