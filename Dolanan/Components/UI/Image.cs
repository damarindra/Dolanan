using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	public class Image : UIComponent
	{
		public Color ColorTint = Color.White;
		public Rectangle SrcTextureRectangle = new Rectangle(0, 0, 128, 128);
		public bool Stretch = true;

		public Texture2D Texture2D;

		public Image(Actor owner) : base(owner)
		{
		}

		public override void Start()
		{
			base.Start();
			Texture2D = ScreenDebugger.Pixel;
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

			if (Stretch)
				GameMgr.SpriteBatch.Draw(Texture2D, UIActor.RectTransform.Rectangle.ToRectangle(), ColorTint);
			else
				GameMgr.SpriteBatch.Draw(Texture2D, UIActor.RectTransform.Rectangle.ToRectangle(), SrcTextureRectangle,
					ColorTint);
		}
	}
}