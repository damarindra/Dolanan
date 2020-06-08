using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	public class Image : UIComponent
	{
		public Image(Actor owner) : base(owner) { }

		public Texture2D Texture2D;

		public override void Start()
		{
			base.Start();
			Texture2D = ScreenDebugger.Pixel;
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			
			GameMgr.SpriteBatch.Draw(Texture2D, UIActor.RectTransform.Rectangle.ToRectangle(), Color.White);
		}
	}
}