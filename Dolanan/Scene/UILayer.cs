using Dolanan.Engine;

namespace Dolanan.Scene
{
	public class UILayer : Layer
	{
		public UILayer(World gameWorld, LayerName layerName) : base(gameWorld, layerName) { }

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

		public T AddUIActor<T>(string name) where T : UIActor
		{
			return AddActor<T>(name);
		}
	}

	public enum UISpace
	{
		Screen, World
	}
}