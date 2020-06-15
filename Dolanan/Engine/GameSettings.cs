using System;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Engine
{
	public static class GameSettings
	{
		public static bool IsDirty;

		public static Color BackgroundColor = Color.DimGray;


		/// <summary>
		///     Window Size
		///     Tips : Pixel Art Guide for windowSize
		///     720p 1440p = 640, 480 (divide / multiply by even number)
		///     1080p 2160p = 960, 540 (divide / multiply by even number)
		/// </summary>
		private static Point _windowSize = new Point(960, 540);

		/// <summary>
		///     Viewport size is the camera that will render the world
		/// </summary>
		private static Point _viewportSize = new Point(960, 540);


		private static bool _allowWindowResize = true;
		private static WindowMode _windowMode = WindowMode.Window;
		private static WindowSizeKeep _windowKeep = WindowSizeKeep.Width;
		private static GraphicsDeviceManager _graphics;
		private static GameWindow _window;

		public static Point WindowSize
		{
			get => _windowSize;
			set
			{
				_windowSize = value;
				IsDirty = true;
			}
		}

		public static Point ViewportSize
		{
			get => _viewportSize;
			set
			{
				_viewportSize = value;
				IsDirty = true;
			}
		}

		public static bool AllowWindowResize
		{
			get => _allowWindowResize;
			set
			{
				_allowWindowResize = value;
				IsDirty = true;
			}
		}

		public static WindowMode WindowMode
		{
			get => _windowMode;
			set
			{
				_windowMode = value;
				IsDirty = true;
			}
		}


		public static WindowSizeKeep WindowKeep
		{
			get => _windowKeep;
			set
			{
				_windowKeep = value;
				IsDirty = true;
			}
		}

		/// <summary>
		///     Clipping cursor, no need to apply
		/// </summary>
		public static bool ClipCursor { get; set; } = false;

		public static void InitializeGameSettings(GraphicsDeviceManager graphics, GameWindow window)
		{
			_graphics = graphics;
			_window = window;
			Configure();
		}

		public static void ApplyChanges()
		{
			IsDirty = false;
			try
			{
				Configure();
				_graphics.ApplyChanges();
			}
			catch (Exception e)
			{
				Log.Print(e.ToString());
				Log.Print("GameSettings.Init() never called, consider call it first");
				throw;
			}
		}

		private static void Configure()
		{
			_graphics.HardwareModeSwitch = WindowMode == WindowMode.FullScreen;

			//Switch to WIndow
			if (WindowMode == WindowMode.Window)
			{
				_graphics.IsFullScreen = false;
				_graphics.PreferredBackBufferHeight = WindowSize.Y;
				_graphics.PreferredBackBufferWidth = WindowSize.X;
			}
			else
			{
				// Set to fullscreen
				_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				_graphics.IsFullScreen = true;
			}

			if (_window.AllowUserResizing != AllowWindowResize)
				_window.AllowUserResizing = AllowWindowResize;
		}
	}

	public enum WindowSizeKeep
	{
		Width,

		Height
		// TODO : Expand will use the most possible between width or height ( we don't really need this actually)
		,
		Expand
	}

	public enum WindowMode
	{
		FullScreen,
		Borderless,
		Window
	}
}