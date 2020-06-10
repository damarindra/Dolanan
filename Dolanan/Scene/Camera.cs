using System;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Scene
{
	public delegate void ViewportChange(Point viewport);

	/// <summary>
	///     Camera used for virtually rendering the world.
	///     It actually shifting the spritebatch matrix to the opposite of camera.
	/// </summary>
	public class Camera : Actor
	{
		private readonly bool _pixelPerfect = false;

		private Point _viewportSize;

		public ViewportChange OnViewportChanged;

		public Camera(string name, Layer layer) : base(name, layer)
		{
		}

		public Transform2D Transform { get; private set; }
		public Actor FollowActor { get; set; }
		public float SmoothSpeed { get; set; }
		public bool UseSmooth { get; set; }

		public Point ViewportSize
		{
			get => _viewportSize;
			set
			{
				_viewportSize = value;
				OnViewportChanged?.Invoke(_viewportSize);
			}
		}

		public Rectangle Limit { get; set; }

		public Vector2 Min => Transform.GlobalPosition - new Vector2(ViewportSize.X / 2f, ViewportSize.Y / 2f);
		public Vector2 Max => Transform.GlobalPosition + new Vector2(ViewportSize.X / 2f, ViewportSize.Y / 2f);

		/// <summary>
		///     Used for shifting the spriteBatch
		/// </summary>
		/// <returns></returns>
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
			Limit = new Rectangle(0, 0, GameSettings.ViewportSize.X * 2, GameSettings.ViewportSize.Y * 2);
		}

		public override void LateUpdate(GameTime gameTime)
		{
			if (FollowActor == null)
				return;

			Vector2 position;
			if (UseSmooth)
			{
				var s = (float) gameTime.ElapsedGameTime.TotalSeconds * SmoothSpeed;
				position = (FollowActor.Transform.GlobalPosition - Transform.Position) * s + Transform.Position;
			}
			else
			{
				position = FollowActor.Transform.Position;
			}

			//Verify limit
			if (Limit != Rectangle.Empty)
			{
				position.X = MathEx.Clamp(position.X, ViewportSize.X / 2,
					Limit.X + Limit.Width - ViewportSize.X / 2);
				position.Y = MathEx.Clamp(position.Y, ViewportSize.Y / 2,
					Limit.Y + Limit.Height - ViewportSize.Y / 2);
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

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			base.Draw(gameTime, layerZDepth);
			Point mousePos = Mouse.GetState().Position;
			
			GameMgr.SpriteBatch.Draw(ScreenDebugger.Pixel, new Rectangle(ScreenToWorld(mousePos), new Point(5, 5)), Color.Red);
		}

		public static Point ScreenToWorld(Point position)
		{
			Vector2 deltaScale = GameMgr.Game.World.Camera.ViewportSize.ToVector2() / GameMgr.Game.Window.ClientBounds.Size.ToVector2();

			// give an offset on mouse position, our render x and y location not always 0
			position -= GameMgr.Game.RenderDestination.Location;
			// scale size between window size and render size
			Vector2 scaleOffset = GameMgr.Game.Window.ClientBounds.Size.ToVector2() / GameMgr.Game.RenderDestination.Size.ToVector2();

			Matrix m = Matrix.CreateTranslation(new Vector3((position.ToVector2()), 0)) *
			           Matrix.CreateScale(new Vector3(scaleOffset, 1)) *
			           Matrix.Invert(GameMgr.Game.World.Camera.GetTopLeftMatrix())* 
			           Matrix.CreateScale(new Vector3(deltaScale, 1)) ;

			return m.Translation.ToVector2().ToPoint(); 
		}

		public static float Aspect =>
			GameMgr.Game.World.Camera.ViewportSize.X / (float)GameMgr.Game.World.Camera.ViewportSize.Y;
	}
}