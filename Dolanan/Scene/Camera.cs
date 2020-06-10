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

		private Point _viewportRectSize;

		public ViewportChange OnViewportChanged;

		public Camera(string name, Layer layer) : base(name, layer)
		{
		}

		public Transform2D Transform { get; private set; }
		public Actor FollowActor { get; set; }
		public float SmoothSpeed { get; set; }
		public bool UseSmooth { get; set; }

		/// <summary>
		/// Camera size to render the world
		/// </summary>
		public Point ViewportRectSize
		{
			get => _viewportRectSize;
			set
			{
				_viewportRectSize = value;
				OnViewportChanged?.Invoke(_viewportRectSize);
			}
		}

		public Rectangle Limit { get; set; }

		public Vector2 Min => Transform.GlobalPosition - new Vector2(ViewportRectSize.X / 2f, ViewportRectSize.Y / 2f);
		public Vector2 Max => Transform.GlobalPosition + new Vector2(ViewportRectSize.X / 2f, ViewportRectSize.Y / 2f);

		/// <summary>
		///     Used for shifting the spriteBatch
		/// </summary>
		/// <returns></returns>
		public Matrix GetTopLeftMatrix()
		{
			return Matrix.CreateTranslation(-(Transform.Position.X - ViewportRectSize.X / 2),
				-(Transform.Position.Y - ViewportRectSize.Y / 2), 0);
		}


		public override void Start()
		{
			ViewportRectSize = GameSettings.ViewportSize;
			Transform = AddComponent<Transform2D>();
			Transform.Position = new Vector2(ViewportRectSize.X / 2, ViewportRectSize.Y / 2);
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
				position.X = MathEx.Clamp(position.X, ViewportRectSize.X / 2,
					Limit.X + Limit.Width - ViewportRectSize.X / 2);
				position.Y = MathEx.Clamp(position.Y, ViewportRectSize.Y / 2,
					Limit.Y + Limit.Height - ViewportRectSize.Y / 2);
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
			
			GameMgr.SpriteBatch.Draw(ScreenDebugger.Pixel, new Rectangle(ScreenToWorld(mousePos).ToPoint(), new Point(5, 5)), Color.Red);
		}

		/// <summary>
		/// Translate Screen position to World (example : mouse position to world position)
		/// </summary>
		/// <param name="position">Position on pixel point</param>
		/// <returns></returns>
		public static Vector2 ScreenToWorld(Point position)
		{
			Vector2 deltaScale = GameMgr.Game.World.Camera.ViewportRectSize.ToVector2() / 
			                     GameMgr.Game.Window.ClientBounds.Size.ToVector2();

			// give an offset on mouse position, our render x and y location not always 0
			position -= GameMgr.Game.RenderDestination.Location;
			// scale size between window size and render size
			Vector2 scaleOffset = GameMgr.Game.Window.ClientBounds.Size.ToVector2() / 
			                      GameMgr.Game.RenderDestination.Size.ToVector2();

			Matrix m = Matrix.CreateTranslation(new Vector3((position.ToVector2()), 0)) *
			           Matrix.CreateScale(new Vector3(scaleOffset, 1)) *
			           Matrix.Invert(GameMgr.Game.World.Camera.GetTopLeftMatrix())* 
			           Matrix.CreateScale(new Vector3(deltaScale, 1)) ;

			return m.Translation.ToVector2();
		}

		public static Vector2 ScreenToViewport(Point position)
		{
			return Vector2.Clamp(position.ToVector2() / GameMgr.Game.Window.ClientBounds.Size.ToVector2(), Vector2.Zero, Vector2.One);
		}

		public static Point WorldToScreen(Vector2 worldPosition)
		{
			Matrix m = Matrix.CreateTranslation(new Vector3(worldPosition, 0)) * GameMgr.Game.World.Camera.GetTopLeftMatrix();
			return (m.Translation.ToVector2() * GameMgr.Game.ScaleRenderTarget +
			       GameMgr.Game.RenderDestination.Location.ToVector2()).ToPoint();
		}

		public static float Aspect =>
			GameMgr.Game.World.Camera.ViewportRectSize.X / (float)GameMgr.Game.World.Camera.ViewportRectSize.Y;
	}
}