using System;
using System.Runtime.InteropServices;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Scene;
using Dolanan.Scene.Object;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Dolanan
{
	/// <summary>
	/// GameMin is a Game class. We try our best to make it as minimum as possible
	/// Contains basic setup for your game settings, such as window settings
	/// Not different with how MonoGame implement it. Using this will speed up your setup.
	/// </summary>
	public class GameMin : Game
	{
		public bool IsGameFinishedInitialize { get; private set; }

		public GraphicsDeviceManager Graphics
		{
			get => _graphics;
		}

		protected GraphicsDeviceManager _graphics;
		protected SpriteBatch SpriteBatch;
		protected RenderTarget2D RenderTarget;

		public World World;
		private Vector2 _scaleRenderTarget = new Vector2(1, 1);

		private bool _debugShowCollision = false;
		private bool _debugFPS = false;

		public GameMin()
		{
			_graphics = new GraphicsDeviceManager(this);
			GameSettings.InitializeGameSettings(_graphics, Window);
			Window.ClientSizeChanged += OnWindowResize;
			
			// Configuration Input
			Input.AddInputAction("Alt", new InputAction(Keys.LeftAlt, Keys.RightAlt));
			Input.AddInputAction("Enter", new InputAction(Keys.Enter));
			Input.AddInputAction("Show Collision", new InputAction(Keys.F4));
			Input.AddInputAction("Show FPS", new InputAction(Keys.F3));

			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		private void OnWindowResize(object? sender, EventArgs e)
		{
			WindowChanged();
		}

		void WindowChanged()
		{
			if (World != null)
			{
				_scaleRenderTarget.X = Window.ClientBounds.Width / (float) World.Camera.ViewportSize.X;
				_scaleRenderTarget.Y = Window.ClientBounds.Height / (float) World.Camera.ViewportSize.Y;
				if (!_graphics.IsFullScreen && _graphics.PreferredBackBufferHeight != Window.ClientBounds.Height &&
				    _graphics.PreferredBackBufferWidth != Window.ClientBounds.Width)
				{
					_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
					_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
					_graphics.ApplyChanges();
				}
			}
		}
		
		protected override void Initialize()
		{
			// Add your initialization logic here
			GameMgr.Init(this);
			
			World = new World();
			// _scaleRenderTarget.X = Window.ClientBounds.Width / (float) World.Camera.ViewportSize.X;
			// _scaleRenderTarget.Y = Window.ClientBounds.Height / (float) World.Camera.ViewportSize.Y;

			base.Initialize();
			
			World.Start();
			
			IsGameFinishedInitialize = true;
		}
		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			GameMgr.Load(SpriteBatch);
			new ResAnimatedSprite().Load();
			new ResFont().Load();

			RenderTarget =
				new RenderTarget2D(GraphicsDevice, World.Camera.ViewportSize.X, World.Camera.ViewportSize.Y);

			// use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			// // Don't worry, World.Start will only called every member (Layers, Actors, Components) once. Whenever we add
			// // new actor at runtime, it will not directly registered to the world. It will wait the end of frame, and
			// // register all of them. Next update frame will call Start (Layers, Actors, Components) only once!
			// World.Start();
			
			//Log.Print(gameTime.ElapsedGameTime.Milliseconds.ToString());
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}
			if (Input.IsInputActionPressed("Alt") && Input.IsInputActionJustPressed("Enter"))
			{
				if (GameSettings.WindowMode == WindowMode.Borderless)
					GameSettings.WindowMode = WindowMode.Window;
				else
					GameSettings.WindowMode = WindowMode.Borderless;
			}

			if (Input.IsInputActionJustPressed("Show Collision")) _debugShowCollision = !_debugShowCollision;
			if (Input.IsInputActionJustPressed("Show FPS")) _debugFPS = !_debugFPS;

			// TODO: Use preprocessor for windows only
			// Read here : https://gamedev.stackexchange.com/questions/55657/monogame-cross-platform-conditional-compilation-symbols
			if(IsActive && GameSettings.ClipCursor)
				ClipCursor();
			
			World.Update(gameTime);
			Process(gameTime);

			if (GameSettings.IsDirty)
				GameSettings.ApplyChanges();

			Input.LastFrameKeyboardState = Keyboard.GetState();
			Input.LastFrameGamePadState = GamePad.GetState(0);

			base.Update(gameTime);
			
			World.LateUpdate(gameTime);
		}

		/// <summary>
		/// Process is exactly the same as Update. Dolanan already using the MonoGame.Update for updating basic stuff
		/// If you need the same as MonoGame.Update, just use override this function! It is exactly the same as Update
		/// without losing basic Dolanan feature.
		/// Process is called right after World.Update
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Process(GameTime gameTime) { }

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(RenderTarget);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			SpriteBatch.Begin(transformMatrix: World.Camera.GetTopLeftMatrix(), samplerState: SamplerState.PointClamp);
			World.Draw(gameTime);
			SpriteBatch.End();
			
			SpriteBatch.Begin(transformMatrix: World.Camera.GetTopLeftMatrix(), samplerState: SamplerState.PointClamp);
			if (_debugShowCollision)
			{
				World.DrawCollision();
			}
			SpriteBatch.End();
			
			BackBufferRender();

			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			if(_debugFPS && ResFont.Instance.TryGet("bitty", out var font))
				FPSCounter.Draw(gameTime, SpriteBatch, font);
			//SpriteBatch.Draw(ScreenDebugger.Pixel, new Rectangle(0,0, 128, 128), Color.White);
			
			SpriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Rendering back into screen with scaling factor
		/// </summary>
		void BackBufferRender()
		{
			GraphicsDevice.SetRenderTarget(null);
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			//_spriteBatch.Draw(_renderTarget, World.Camera.BoundingBox2D.ToRectangle(), Color.White);
			// destination is window screen space!
			Rectangle destination = new Rectangle();
			destination.Width = (int) (World.Camera.ViewportSize.X * ((GameSettings.WindowKeep == WindowSizeKeep.Width)
				? _scaleRenderTarget.X
				: _scaleRenderTarget.Y));
			destination.Height = (int) (World.Camera.ViewportSize.Y * ((GameSettings.WindowKeep == WindowSizeKeep.Width)
				? _scaleRenderTarget.X
				: _scaleRenderTarget.Y));

			destination.X = (int) (((GameSettings.WindowKeep == WindowSizeKeep.Height)
				                       ? (Window.ClientBounds.Width - destination.Width) / 2
				                       : 0)); //- ((1 - _scaleRenderTarget) * World.Camera.ViewportSize.X) / 2);
			destination.Y = (int) (((GameSettings.WindowKeep == WindowSizeKeep.Width)
				                       ? (Window.ClientBounds.Height - destination.Height) / 2
				                       : 0));
			//- ((1 - _scaleRenderTarget) * World.Camera.ViewportSize.Y) / 2);

			//Console.WriteLine(destination + " ||| " + Window.ClientBounds.Width);
			SpriteBatch.Draw(RenderTarget, destination,
				null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
			SpriteBatch.End();
		}

		[DllImport("user32.dll")]
		static extern void ClipCursor(ref Rectangle rect);

		private void ClipCursor()
		{
			Rectangle rect = Window.ClientBounds;
			rect.Width += rect.X;
			rect.Height += rect.Y;
	    
			ClipCursor(ref rect);
		}
	}
}