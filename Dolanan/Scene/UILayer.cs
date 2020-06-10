using Dolanan.Controller;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UILayer : Layer
	{
		private UISpace _uiSpace = UISpace.Window;

		public UILayer(World gameWorld, int layerZ) : base(gameWorld, layerZ)
		{
		}

		public UISpace UISpace
		{
			get => _uiSpace;
			set
			{
				if (value == UISpace.Viewport)
					ScreenCanvas.RectTransform.Rectangle = new RectangleF(0, 0, GameSettings.ViewportSize.X,
						GameSettings.ViewportSize.Y);
				_uiSpace = value;
			}
		}

		public UIActor ScreenCanvas { get; private set; }

		public override void Start()
		{
			base.Start();
			ScreenCanvas = AddActor<UIActor>("Canvas");
			ScreenCanvas.RectTransform.Rectangle =
				new RectangleF(0, 0, GameSettings.ViewportSize.X, GameSettings.ViewportSize.Y);
			GameMgr.Game.World.Camera.OnViewportChanged += viewport =>
			{
				if (_uiSpace == UISpace.Viewport)
					ScreenCanvas.RectTransform.Rectangle = new RectangleF(0, 0, GameSettings.ViewportSize.X,
						GameSettings.ViewportSize.Y);
			};
		}

		public new T AddActor<T>(string name) where T : UIActor
		{
			return base.AddActor<T>(name);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void LateUpdate(GameTime gameTime)
		{
			base.LateUpdate(gameTime);
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			if (UISpace == UISpace.World)
				base.Draw(gameTime, layerZDepth);
		}

		/// <summary>
		///     Back Draw, occured after BackBufferRendering. Useful for drawing UI, debugging, etc. Always show in front.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="worldRect">BackBuffer Rectangle</param>
		public virtual void BackDraw(GameTime gameTime, Rectangle worldRect)
		{
			if (UISpace == UISpace.World)
			{
				// DO NOTHING
			}
			else if (UISpace == UISpace.Viewport && ScreenCanvas.RectTransform.Rectangle != worldRect.ToRectangleF())
			{
				ScreenCanvas.RectTransform.Rectangle = worldRect.ToRectangleF();
			}
			else if (UISpace == UISpace.Window && ScreenCanvas.RectTransform.Rectangle.Size !=
				GameMgr.Game.Window.ClientBounds.Size.ToVector2())
			{
				ScreenCanvas.RectTransform.SetRectSize(GameMgr.Game.Window.ClientBounds.Size.ToVector2());
			}

			base.Draw(gameTime, LayerZ);
		}
	}

	public enum UISpace
	{
		// follow camera viewport (best for UI screen if your game window is consistent)
		Viewport,

		// place in world
		World,

		// follow window size (best for UI screen)
		Window
	}
}