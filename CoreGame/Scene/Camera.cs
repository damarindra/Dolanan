using System;
using System.Drawing;
using CoreGame.Engine;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace CoreGame.Scene
{
	public class Camera : Actor
	{
		public Actor FollowActor { get; set; }
		public float SmoothSpeed { get; set; }
		public bool UseSmooth { get; set; }

		public Point ViewportSize;

		private bool _pixelPerfect = false;

		public BoundingBox2D BoundingBox2D
		{
			get
			{
				return new BoundingBox2D(transform.Matrix, 
					new Vector2(-ViewportSize.X / 2, -ViewportSize.Y / 2), 
					new Vector2(ViewportSize.X / 2, -ViewportSize.Y / 2),
					new Vector2(-ViewportSize.X / 2, ViewportSize.Y / 2),
					new Vector2(ViewportSize.X / 2, ViewportSize.Y / 2));
			}
		}

		public Camera()
		{
			// TODO set viewport size default using window size
			//ViewportSize = PG_003.Instance.
		}

		public Camera(Vector2 globalPosition, Point size)
		{
			transform.GlobalPosition = globalPosition;
			ViewportSize = size;
		}

		public Camera(bool useSmooth, float smoothSpeed, bool pixelPerfect)
		{
			UseSmooth = useSmooth;
			SmoothSpeed = smoothSpeed;
			_pixelPerfect = pixelPerfect;
		}

		//TODO SpriteBatch with this matrix offsetting a little bit
		public Matrix GetTopLeftMatrix()
		{
			return Matrix.CreateTranslation(-(transform.Position.X - ViewportSize.X / 2),
				-(transform.Position.Y - ViewportSize.Y / 2), 0);
		}
		
		public override void LateUpdate(GameTime gameTime)
		{
			if(FollowActor == null)
				return;
			
			if (UseSmooth)
			{
				float s = (float) gameTime.ElapsedGameTime.TotalSeconds * SmoothSpeed;
				transform.GlobalPosition = ((FollowActor.transform.GlobalPosition - transform.Position) * s) + transform.GlobalPosition;
			}
			else
				transform.GlobalPosition = FollowActor.transform.GlobalPosition;

			if (_pixelPerfect)
				transform.GlobalPosition = new Vector2(MathF.Round(transform.GlobalPosition.X), MathF.Round(transform.GlobalPosition.Y));

			BoundingBox2D b = BoundingBox2D;
			ScreenDebugger.DrawSquare(new SquareDebug(b.Location, 10));
			ScreenDebugger.DrawSquare(new SquareDebug(b.Location + new Vector2(b.Width, 0), 10));
			ScreenDebugger.DrawSquare(new SquareDebug(b.Location+ new Vector2(b.Width, b.Height), 10));
			ScreenDebugger.DrawSquare(new SquareDebug(b.Location + new Vector2(0, b.Height), 10));
		}
	}
}