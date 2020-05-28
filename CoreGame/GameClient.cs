using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreGame.Component;
using CoreGame.Engine;
using CoreGame.Scene;
using CoreGame.Scene.Object;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sigil;
using Sigil.NonGeneric;

namespace CoreGame
{
	/// <summary>
	/// This is base class to starting your game.
	/// Contains basic setup for your game settings, such as window settings
	/// 
	/// </summary>
	class GameClient : Game
	{
		public static GameClient Instance = null;

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

		public GameClient()
		{
			Instance = this;

			_graphics = new GraphicsDeviceManager(this);
			GameSettings.InitializeGameSettings(_graphics, Window);
			Window.ClientSizeChanged += OnWindowResize;

			Input.AddInputAction("Alt", new InputAction(Keys.LeftAlt, Keys.RightAlt));
			Input.AddInputAction("Enter", new InputAction(Keys.Enter));

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
			World = new World();
			// _scaleRenderTarget.X = Window.ClientBounds.Width / (float) World.Camera.ViewportSize.X;
			// _scaleRenderTarget.Y = Window.ClientBounds.Height / (float) World.Camera.ViewportSize.Y;

			base.Initialize();
		}
		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			RenderTarget =
				new RenderTarget2D(GraphicsDevice, World.Camera.ViewportSize.X, World.Camera.ViewportSize.Y);

			// use this.Content to load your game content here
			Console.WriteLine("Starting Game");
			
			
			p = new Player(Content.Load<Texture2D>("player"));
			p.Name = "Player";
			p.transform.Position = Vector2.Zero;
			//p.transform.Rotation = MathHelper.ToRadians(45);
			//p.transform.Position += p.transform.Right * _graphics.PreferredBackBufferWidth / 2;
			
			spr = new SpriteActor(Content.Load<Texture2D>("square_64x64"));
			// spr.transform.Position = new Vector2(World.Camera.ViewportSize.X / 2, 0);
			// spr.transform.Rotation = MathHelper.ToRadians(45);
			spr.Sprite.SrcSize = new Point(64, 64);
			spr.Name = "Spr";
			
			World.AddActor(p);
			World.Camera.FollowActor = p;
			World.AddActor(spr);
			
			// World.Camera.FollowActor = p;
		}

		protected override void Update(GameTime gameTime)
		{
			//Log.Print(gameTime.ElapsedGameTime.Milliseconds.ToString());
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			if (Input.IsInputActionPressed("Alt") && Input.IsInputActionJustPressed("Enter"))
			{
				if (GameSettings.WindowMode == WindowMode.Borderless)
					GameSettings.WindowMode = WindowMode.Window;
				else
					GameSettings.WindowMode = WindowMode.Borderless;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.R))
				spr.transform.Rotation += 0.01f;
			
			// TODO: Use preprocessor for windows only
			// Read here : https://gamedev.stackexchange.com/questions/55657/monogame-cross-platform-conditional-compilation-symbols
			if(IsActive)
				ClipCursor();
			
			World.Update(gameTime);

			if (GameSettings.IsDirty)
				GameSettings.ApplyChanges();

			Input.LastFrameKeyboardState = Keyboard.GetState();
			Input.LastFrameGamePadState = GamePad.GetState(0);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			ScreenDebugger.DebugDraw(new LineDebug(new Line(Vector2.Zero, Vector2.UnitX * -50) , 10));
			GraphicsDevice.SetRenderTarget(RenderTarget);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			SpriteBatch.Begin(transformMatrix: World.Camera.GetTopLeftMatrix(), samplerState: SamplerState.PointClamp);
			World.Draw(gameTime, SpriteBatch);
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