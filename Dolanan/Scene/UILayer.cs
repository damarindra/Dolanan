using Dolanan.Controller;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UILayer : Layer
	{
		private UISpace _uiSpace = UISpace.Screen;

		public UILayer(World gameWorld, int layerZ) : base(gameWorld, layerZ) {
		}

		public UISpace UISpace
		{
			get => _uiSpace;
			set
			{
				if (value == UISpace.Screen)
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
				if (_uiSpace == UISpace.Screen)
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

		public virtual void BackDraw(GameTime gameTime, Rectangle renderRect)
		{
			if (UISpace == UISpace.Screen)
				// if (ScreenCanvas.RectTransform.Rectangle != renderRect.ToRectangleF())
				// 	ScreenCanvas.RectTransform.Rectangle = renderRect.ToRectangleF();
				base.Draw(gameTime, LayerZ);
		}
	}

	public enum UISpace
	{
		Screen,
		World
	}
}