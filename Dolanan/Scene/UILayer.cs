using System;
using Dolanan.Controller;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UILayer : Layer
	{
		private UISpace _uiSpace = UISpace.Window;

		/// <summary>
		///     Scaling all UIActor in this layer
		/// </summary>
		public float Scaling = 2;

		public UILayer(World gameWorld, string name) : base(gameWorld, name)
		{
		}

		public UISpace UISpace
		{
			get => _uiSpace;
			set
			{
				if (value == UISpace.Viewport)
					ScreenCanvas.RectTransform.Rectangle = new RectangleF(0, 0, GameSettings.RenderSize.X,
						GameSettings.RenderSize.Y);
				_uiSpace = value;
			}
		}

		public UIActor ScreenCanvas { get; private set; }

		public override void Start()
		{
			base.Start();
			ScreenCanvas = CreateActor<UIActor>("Canvas");
			ScreenCanvas.RectTransform.Rectangle =
				new RectangleF(0, 0, GameSettings.RenderSize.X, GameSettings.RenderSize.Y);
			GameMgr.Game.World.Camera.OnViewportChanged += viewport =>
			{
				if (_uiSpace == UISpace.Viewport)
					ScreenCanvas.RectTransform.Rectangle = new RectangleF(0, 0, GameSettings.RenderSize.X,
						GameSettings.RenderSize.Y);
			};
		}

		public new T CreateActor<T>(string name) where T : UIActor
		{
			UIActor actor = base.CreateActor<T>(name);
			if (ScreenCanvas != null)
				actor.SetParent(ScreenCanvas);
			return (T) actor;
		}

		public override void Update(GameTime gameTime)
		{
			if (!IsLoaded)
				return;
			foreach (var actor in Actors)
			{
				actor.Update(gameTime);
				if (actor.GetType().IsSubclassOf(typeof(UIActor)))
				{
					var ac = (UIActor) actor;
					ac.RectTransform = ac.RectTransform;
				}
			}
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
			else if (UISpace == UISpace.Window && ScreenCanvas.RectTransform.Rectangle.Size !=
				GameMgr.Game.Window.ClientBounds.Size.ToVector2())
			{
				ScreenCanvas.RectTransform.SetRectSize(GameMgr.Game.Window.ClientBounds.Size.ToVector2());
			}
			else if (UISpace == UISpace.Viewport && ScreenCanvas.RectTransform.Rectangle != worldRect.ToRectangleF())
			{
				ScreenCanvas.RectTransform.Rectangle = worldRect.ToRectangleF();
			}

			// just placeholder, UI back draw draw at .97 is fine I think
			base.Draw(gameTime, .97f);
		}
	}

	public enum UISpace
	{
		[Obsolete]
		// follow camera viewport (best for UI screen if your game window is consistent)
		Viewport,

		// place in world
		World,

		// follow window size (best for UI screen)
		Window
	}
}