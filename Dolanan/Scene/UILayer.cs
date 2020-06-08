using System;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UILayer : Layer
	{
		public UILayer(World gameWorld, int layerZ) : base(gameWorld, layerZ) { }

		public UISpace UISpace
		{
			get => _uiSpace;
			set
			{
				if(value == UISpace.Screen)
					ScreenCanvas.RectTransform.Rectangle = new RectangleF(0,0,GameSettings.ViewportSize.X, GameSettings.ViewportSize.Y);
				_uiSpace = value;
			}
		}
		public UIActor ScreenCanvas { get; private set; }

		private UISpace _uiSpace = UISpace.Screen;
		
		public override void Start()
		{
			base.Start();
			ScreenCanvas = AddActor<UIActor>("Canvas");
			ScreenCanvas.RectTransform.Rectangle = new RectangleF(0,0,GameSettings.ViewportSize.X, GameSettings.ViewportSize.Y);
		}

		public new T AddActor<T>(string name) where T : UIActor
		{
			return base.AddActor<T>(name);
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			// Only enable drawing on Draw whne UISpace is in World
			if (_uiSpace == UISpace.World)
			{
				base.Draw(gameTime, layerZDepth);
				Console.WriteLine("Draw called");

			}
		}

		/// <summary>
		/// Draw right after BackBufferDraw 
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="rectRender"></param>
		public virtual void BackDraw(GameTime gameTime, Rectangle rectRender)
		{
			if(_uiSpace == UISpace.World)
				return;
			foreach (Actor actor in Actors)
			{
				UIActor ac = (UIActor) actor;
				if (ac != null)
					ac.BackDraw(gameTime, rectRender);
			}
		}
	}

	public enum UISpace
	{
		Screen, World
	}
}