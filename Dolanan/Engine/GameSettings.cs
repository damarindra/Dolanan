﻿using System;
using System.IO;
using Dolanan.Controller;
using Dolanan.Core.Utility;
using Dolanan.Editor.ImGui;
using Dolanan.Tools;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Engine
{
	public static class GameSettings
	{
		public static string ConfigFilePath = "Settings/GameSettings.cfg";
		
		public static bool IsDirty;

		public static Color BackgroundColor
		{
			get => _backgroundColor;
			set => _backgroundColor = value;
		}

		private static Color _backgroundColor = Color.DimGray;
		private static Point _windowSize = new Point(960, 540);
		private static Point _renderSize = new Point(960, 540);

		private static bool _allowWindowResize = true;
		private static WindowMode _windowMode = WindowMode.Window;
		private static WindowSizeKeep _windowKeep = WindowSizeKeep.Width;
		private static GraphicsDeviceManager _graphics;
		private static GameWindow _window;

		// TODO make window size as a scaler from RenderSize. float WindowScaler = 2. then the result window will be RenderSize * WindowScaler
		/// <summary>
		///     Window Size
		///     Tips : Pixel Art Guide
		///     720p 1440p = 640, 480 (divide / multiply by even number)
		///     1080p 2160p = 960, 540 (divide / multiply by even number)
		/// </summary>
		public static Point WindowSize
		{
			get => _windowSize;
			set
			{
				_windowSize = value;
				IsDirty = true;
			}
		}

		/// <summary>
		///     Viewport size is the camera that will render the world
		///     Tips : Pixel Art Guide
		///     720p 1440p = 640, 480 (divide / multiply by even number)
		///     1080p 2160p = 960, 540 (divide / multiply by even number)
		/// </summary>
		public static Point RenderSize
		{
			get => _renderSize;
			set
			{
				_renderSize = value;
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

		public static bool IsSetupDone = false;
		
		public static void InitializeGameSettings(GraphicsDeviceManager graphics, GameWindow window)
		{
			
			_graphics = graphics;
			_window = window;
			LoadFromCfg();

#if DEBUG
			if(!IsSetupDone) GameMgr.Game.OnImGuiDraw += DrawImGuiWindow;
#endif
			IsSetupDone = true;
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

		public static void LoadFromCfg()
		{
			var configs = DolananParser.ParseCfg(File.ReadAllText(ConfigFilePath));
			foreach (var cfg in configs)
			{
				var keyVal = cfg.Split('=');
				switch (keyVal[0])
				{
					case "BackgroundColor":
						BackgroundColor = MathEx.HextToColor(keyVal[1]);
						break;
					case "WindowSize":
						if (DolananParser.TryParseToPoint(keyVal[1], out var p)) WindowSize = p;
						break;
					case "RenderSize":
						if (DolananParser.TryParseToPoint(keyVal[1], out p)) RenderSize = p;
						break;
					case "ClipCursor":
						if (Boolean.TryParse(keyVal[1], out var b)) ClipCursor = b;
						break;
					case "AllowWindowResize":
						if (Boolean.TryParse(keyVal[1], out b)) AllowWindowResize = b;
						break;
					case "WindowMode":
						if (Enum.TryParse(typeof(WindowMode), keyVal[1], true, out var e)) WindowMode = (WindowMode) e;
						break;
					case "WindowKeep":
						if (Enum.TryParse(typeof(WindowSizeKeep), keyVal[1], true, out e)) WindowKeep = (WindowSizeKeep) e;
						break;
				}
			}

			Configure();
		}

#if DEBUG
		public static bool ShowWindow
		{
			get => _showWindow;
			set => _showWindow = value;
		}
		private static bool _showWindow = true;
		public static void DrawImGuiWindow()
		{
			if (ShowWindow)
			{
				ImGui.Begin("Game Settings", ref _showWindow);
				//ImGuiMg.ColorEdit("Background Color", ref _backgroundColor);
				ImGui.End();
			}
		}
#endif
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