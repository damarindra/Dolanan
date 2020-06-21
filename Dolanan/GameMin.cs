using System;
using System.Runtime.InteropServices;
using Dolanan.Controller;
using Dolanan.Editor;
using Dolanan.Editor.ImGui;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Scene;
using Dolanan.Tools;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dolanan
{
	/// <summary>
	///     GameMin is a Game class. We try our best to make it as minimum as possible
	///     Contains basic setup for your game settings, such as window settings
	///     Not different with how MonoGame implement it. Using this will speed up your setup.
	/// </summary>
	public class GameMin : Game
	{
		private bool _debugFps;
		private bool _debugShowCollision;
		private Vector2 _scaleRenderTarget = new Vector2(1, 1);
#if DEBUG
		public ImGuiRenderer ImGuiRenderer { get; private set; }

		public delegate void DrawImGuiWindow();

		public DrawImGuiWindow OnImGuiDraw;
#endif

		protected RenderTarget2D RenderTarget;
		protected SpriteBatch SpriteBatch;
		public World World;

		public GameMin()
		{
			Graphics = new GraphicsDeviceManager(this);
			GameMgr.Init(this);
			GameSettings.InitializeGameSettings(Graphics, Window);
			Window.ClientSizeChanged += OnWindowResize;
			Content.RootDirectory = "Content";


#if DEBUG
			// Configuration Input, basic debugging stuff
			Input.AddInputAction("Alt", new InputAction(Keys.LeftAlt, Keys.RightAlt));
			Input.AddInputAction("Enter", new InputAction(Keys.Enter));
			Input.AddInputAction("Show Collision", new InputAction(Keys.F4));
			Input.AddInputAction("Show FPS", new InputAction(Keys.F3));
			Input.AddInputAction("cmd", new InputAction(Keys.OemTilde));
#endif

			IsMouseVisible = true;
		}

		public float ScaleRenderTarget => GameSettings.WindowKeep == WindowSizeKeep.Width
			? _scaleRenderTarget.X
			: _scaleRenderTarget.Y;

		public Rectangle RenderDestination { get; private set; }

		public GraphicsDeviceManager Graphics { get; }

		public bool IsGameFinishedInitialize { get; private set; }

		private void OnWindowResize(object? sender, EventArgs e)
		{
			WindowChanged();
		}

		private void WindowChanged()
		{
			if (World != null)
			{
				_scaleRenderTarget.X = Window.ClientBounds.Width / (float) World.Camera.ViewportRectSize.X;
				_scaleRenderTarget.Y = Window.ClientBounds.Height / (float) World.Camera.ViewportRectSize.Y;
				if (!Graphics.IsFullScreen && Graphics.PreferredBackBufferHeight != Window.ClientBounds.Height &&
				    Graphics.PreferredBackBufferWidth != Window.ClientBounds.Width)
				{
					Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
					Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
					Graphics.ApplyChanges();
				}
			}
		}

		protected override void Initialize()
		{
			// Add your initialization logic here
			World = new World();

#if DEBUG
			ImGuiRenderer = new ImGuiRenderer(this);
			ImGuiRenderer.RebuildFontAtlas();
			
			ImGuiRenderer.SetupMonoGameWindowInput();
#endif

			base.Initialize();

			IsGameFinishedInitialize = true;
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			GameMgr.Load(SpriteBatch);
			new ResFont().Load();

			RenderTarget =
				new RenderTarget2D(GraphicsDevice, World.Camera.ViewportRectSize.X, World.Camera.ViewportRectSize.Y);

			// use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			GameMgr.DrawState = DrawState.None;
#if DEBUG
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

			if (Input.IsInputActionJustPressed("Show Collision")) _debugShowCollision = !_debugShowCollision;
			if (Input.IsInputActionJustPressed("Show FPS")) _debugFps = !_debugFps;
#endif

			// TODO: Use preprocessor for windows only
			// Read here : https://gamedev.stackexchange.com/questions/55657/monogame-cross-platform-conditional-compilation-symbols
			if (IsActive && GameSettings.ClipCursor)
				ClipCursor();

#if DEBUG
			if (!EditorMode.IsActive)
			{
#endif
				World.Update(gameTime);
				Process(gameTime);
#if DEBUG
			}
#endif
			if (GameSettings.IsDirty)
				GameSettings.ApplyChanges();


			base.Update(gameTime);

#if DEBUG
			if (!EditorMode.IsActive)
			{
#endif
				World.LateUpdate(gameTime);
				UIInput.HandleInput();
#if DEBUG
			}
#endif

			Input.LastFrameKeyboardState = Keyboard.GetState();
			Input.LastFrameGamePadState = GamePad.GetState(0);
			Input.LastFrameMouseState = Mouse.GetState();
			UIInput.Reset();
		}

		/// <summary>
		///     Process is exactly the same as Update. Dolanan already using the MonoGame.Update for updating basic stuff
		///     If you need the same as MonoGame.Update, just use override this function! It is exactly the same as Update
		///     without losing basic Dolanan feature.
		///     Process is called right after World.Update
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void Process(GameTime gameTime)
		{
		}

		protected override void Draw(GameTime gameTime)
		{
			GameMgr.ResetState();
			GraphicsDevice.SetRenderTarget(RenderTarget);
			GraphicsDevice.Clear(GameSettings.BackgroundColor);
			GameMgr.DrawState = DrawState.Draw;

			GameMgr.BeginDrawWorld();
			World.Draw(gameTime);
			SpriteBatch.End();

			DrawProcess(gameTime);

			GameMgr.BeginDrawWorld();
			if (_debugShowCollision) World.DrawCollision();
			SpriteBatch.End();

			GameMgr.DrawState = DrawState.BackDraw;
			BackBufferRender();

			GameMgr.BeginDrawAuto();
			BackDraw(gameTime, RenderDestination);
			if (_debugFps && ResFont.Instance.TryGet("16px", out var font))
				FPSCounter.Draw(gameTime, SpriteBatch, font);

			SpriteBatch.End();

#if DEBUG
			ImGuiRenderer.BeforeLayout(gameTime);
			OnImGuiDraw?.Invoke();
// 			{
// 			if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
// 			if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
// 			ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));
// 				
// 			}
//
// 			
// 			if (show_another_window)
// 			{
// 				ImGui.SetNextWindowSize(new System.Numerics.Vector2(200, 100), ImGuiCond.FirstUseEver);
// 				ImGui.Begin("Another Window", ref show_another_window);
// 				ImGui.Text("Hello");
// 				ImGui.End();
// 			}
//
// 			// 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
			if (show_test_window)
			{
				ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
				ImGui.ShowDemoWindow(ref show_test_window);
			}

			ImGuiRenderer.AfterLayout();
#endif

			base.Draw(gameTime);
		}

		private bool show_test_window = true;
		private bool show_another_window = true;

		/// <summary>
		///     DrawProcess is exactly the same as Draw. Dolanan already using the MonoGame.Draw for drawing basic stuff
		///     If you need the same as MonoGame.Draw, just use override this function! It is exactly the same as Update
		///     without losing basic Dolanan feature.
		///     DrawProcess is called right after GameWorld rendered. (Before BackBufferRender)
		/// </summary>
		/// <param name="gameTime"></param>
		protected virtual void DrawProcess(GameTime gameTime)
		{
		}

		/// <summary>
		///     Render after BackBufferRender (Whole game world render). It useful for debugging, fixed rendering to screen, etc
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="worldRect">The back buffer render size</param>
		protected virtual void BackDraw(GameTime gameTime, Rectangle worldRect)
		{
			// SpriteBatch.Draw(ScreenDebugger.Pixel, new Rectangle(Camera.ScreenToCameraSpace(Mouse.GetState().Position),
			// 	new Point(5, 5)), Color.Yellow);
			World.BackDraw(gameTime, worldRect);
		}

		/// <summary>
		///     Rendering back into screen with scaling factor
		/// </summary>
		private void BackBufferRender()
		{
			GraphicsDevice.SetRenderTarget(null);
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			// destination is window screen space!
			var _rectangle = new Rectangle();
			_rectangle.Width = (int) (World.Camera.ViewportRectSize.X * ScaleRenderTarget);
			_rectangle.Height = (int) (World.Camera.ViewportRectSize.Y * ScaleRenderTarget);

			_rectangle.X = GameSettings.WindowKeep == WindowSizeKeep.Height
				? (Window.ClientBounds.Width - _rectangle.Width) / 2
				: 0;
			_rectangle.Y = GameSettings.WindowKeep == WindowSizeKeep.Width
				? (Window.ClientBounds.Height - _rectangle.Height) / 2
				: 0;

			RenderDestination = _rectangle;
			SpriteBatch.Draw(RenderTarget, RenderDestination,
				null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
			SpriteBatch.End();
		}

		[DllImport("user32.dll")]
		private static extern void ClipCursor(ref Rectangle rect);

		private void ClipCursor()
		{
			var rect = Window.ClientBounds;
			rect.Width += rect.X;
			rect.Height += rect.Y;

			ClipCursor(ref rect);
		}
	}
}