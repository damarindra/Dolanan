using System;
using System.Runtime.InteropServices;
using CoreGame.Controller;
using CoreGame.Engine;
using CoreGame.Resources;
using CoreGame.Scene;
using CoreGame.Scene.Object;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CoreGame
{
	/// <summary>
	/// This is base class to starting your game.
	/// Contains basic setup for your game settings, such as window settings
	/// 
	/// </summary>
	public class GameClient : Game
	{
		public static GameClient Instance = null;

		public bool IsGameFinishedInitialize { get; private set; }

		public GraphicsDeviceManager Graphics
		{
			get => _graphics;
		}

		protected GraphicsDeviceManager _graphics;
		protected SpriteBatch SpriteBatch;
		protected RenderTarget2D RenderTarget;

		public World World;
		private Player p;
		private SpriteActor spr;
		private Vector2 _scaleRenderTarget = new Vector2(1, 1);

		private bool _debugShowCollision = false;

		public GameClient()
		{
			Instance = this;

			_graphics = new GraphicsDeviceManager(this);
			GameSettings.InitializeGameSettings(_graphics, Window);
			Window.ClientSizeChanged += OnWindowResize;
			
			
			Input.AddInputAction("Alt", new InputAction(Keys.LeftAlt, Keys.RightAlt));
			Input.AddInputAction("Enter", new InputAction(Keys.Enter));
			Input.AddInputAction("Show Collision", new InputAction(Keys.F4));

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
			World = new World(GameSettings.WorldCollisionSize.X, GameSettings.WorldCollisionSize.Y);
			// _scaleRenderTarget.X = Window.ClientBounds.Width / (float) World.Camera.ViewportSize.X;
			// _scaleRenderTarget.Y = Window.ClientBounds.Height / (float) World.Camera.ViewportSize.Y;
			GameMgr.Init(this);
			
			World.Initialize();

			base.Initialize();

			World.PostInit();
			IsGameFinishedInitialize = true;
		}
		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			GameMgr.Load(SpriteBatch);
			FontRes.Init();
			new ResAnimatedSprite().Load();

			RenderTarget =
				new RenderTarget2D(GraphicsDevice, World.Camera.ViewportSize.X, World.Camera.ViewportSize.Y);

			// use this.Content to load your game content here
			Console.WriteLine("Starting Game");
			
			p = World.CreateActor<Player>("Player");
			
			World.Camera.FollowActor = p;
			
			spr = World.CreateActor<SpriteActor>("Sprite");
			spr.Sprite.Texture2D = Content.Load<Texture2D>("Graphics/square_64x64");
			spr.Sprite.SrcSize = new Point(64, 64);

			//
			// var tileTex = Content.Load<Texture2D>("tiles_16x16");
			// for(int x = -64; x < 64; x++){
			// 	for (int y = -64; y < 64; y++)
			// 	{
			// 		var tile = new SpriteActor();
			// 		tile.Sprite.Texture2D = tileTex;
			// 		tile.Sprite.SrcSize = new Point(16, 16);
			// 		tile.Transform.GlobalPosition = new Vector2(x * 16, y * 16);
			// 		World.AddActor(tile);
			// 	}
			// }
		}

		protected override void Update(GameTime gameTime)
		{
			// Why start on Update ? isn't it will call every frame. yes it is.
			// Actor.Start() only called once after the object created in runtime. So, this is fine,
			// since only new Actor that called start.
			World.Start();

			
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

			if (Keyboard.GetState().IsKeyDown(Keys.R))
				spr.Transform.Rotation += 0.01f;
			
			if (Input.IsInputActionJustPressed("Show Collision")) _debugShowCollision = !_debugShowCollision;

			// TODO: Use preprocessor for windows only
			// Read here : https://gamedev.stackexchange.com/questions/55657/monogame-cross-platform-conditional-compilation-symbols
			// if(IsActive)
			// 	ClipCursor();
			
			World.Update(gameTime);

			if (GameSettings.IsDirty)
				GameSettings.ApplyChanges();

			Input.LastFrameKeyboardState = Keyboard.GetState();
			Input.LastFrameGamePadState = GamePad.GetState(0);

			base.Update(gameTime);
			
			World.LateUpdate(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			ScreenDebugger.DebugDraw(new LineDebug(new Line(Vector2.Zero, Vector2.UnitX * -50) , 10));
			GraphicsDevice.SetRenderTarget(RenderTarget);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			SpriteBatch.Begin(transformMatrix: World.Camera.GetTopLeftMatrix(), samplerState: SamplerState.PointClamp);
			World.Draw(gameTime);
			SpriteBatch.End();

			
			SpriteBatch.Begin(transformMatrix: World.Camera.GetTopLeftMatrix(), samplerState: SamplerState.PointClamp);
			if (_debugShowCollision)
			{
				World.DrawCollision();
				//World.DrawDebug((int)b.X, (int) b.Y, (int) b.Width, (int) b.Height, DrawCell, DrawBox, DrawString);
			}
			SpriteBatch.End();


			ScreenDebugger.Draw(SpriteBatch);

			BackBufferRender();

			base.Draw(gameTime);
		}

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

		public T LoadContent<T>(string assetName)
		{
			return Content.Load<T>(assetName);
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