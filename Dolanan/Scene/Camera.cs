using System;
using System.Drawing;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Dolanan.Scene
{
	/// <summary>
	/// Camera used for virtually rendering the world.
	/// It actually shifting the spritebatch matrix to the opposite of camera.
	/// </summary>
	public class Camera : Actor
	{
		public Transform2D Transform { get; private set; }
		public Actor FollowActor { get; set; }
		public float SmoothSpeed { get; set; }
		public bool UseSmooth { get; set; }
		public Point ViewportSize { get; set; }
		public Rectangle Limit { get; set; }

		private bool _pixelPerfect = false;
		
		public Camera(string name, Layer layer) : base(name, layer){}
		

		//TODO SpriteBatch with this matrix offsetting a little bit
		public Matrix GetTopLeftMatrix()
		{
			return Matrix.CreateTranslation(-(Transform.Position.X - ViewportSize.X / 2),
				-(Transform.Position.Y - ViewportSize.Y / 2), 0);
		}

		public override void Start()
		{
			ViewportSize = GameSettings.ViewportSize;
			Transform = AddComponent<Transform2D>();
			Transform.Position = new Vector2(ViewportSize.X / 2, ViewportSize.Y / 2);
			Limit = new Rectangle(0,0, GameSettings.WorldCollisionSize.X, GameSettings.WorldCollisionSize.Y);
		}

		public override void LateUpdate(GameTime gameTime)
		{
			if(FollowActor == null)
				return;
			
			var position = Transform.Position;
			if (UseSmooth)
			{
				float s = (float) gameTime.ElapsedGameTime.TotalSeconds * SmoothSpeed;
				position = ((FollowActor.Transform.GlobalPosition - Transform.Position) * s) + Transform.Position;
			}
			else
				position = FollowActor.Transform.Position;
			
			//Verify limit
			if (Limit != Rectangle.Empty)
			{
				position.X = MathEx.Clamp(position.X, ViewportSize.X / 2, 
					Limit.X + Limit.Width - (ViewportSize.X / 2));
				position.Y = MathEx.Clamp(position.Y, ViewportSize.Y / 2,
					Limit.Y + Limit.Height- (ViewportSize.Y / 2));
			}

			if (position != Transform.Position)
			{
				if (_pixelPerfect)
					Transform.Position = new Vector2(MathF.Round(position.X),
						MathF.Round(position.Y));
				else
					Transform.Position = position;
			}
		}
	}
}