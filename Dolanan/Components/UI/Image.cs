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
		public Rectangle SrcTextureRectangle = Rectangle.Empty;
		public bool Stretch = true;

		public Texture2D Texture2D
		{
			get => _texture2D;
			set
			{
				_texture2D = value;
				if (SrcTextureRectangle == Rectangle.Empty)
					SrcTextureRectangle = _texture2D.Bounds;
			}
		}

		private Texture2D _texture2D;

		public Image(Actor owner) : base(owner)
		{
		}

		public override void Start()
		{
			base.Start();
			Texture2D = ScreenDebugger.Pixel;
		}

		public void FitRectangleToImage()
		{
			UIActor.RectTransform.SetRectSize(SrcTextureRectangle.Size.ToVector2());
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

			if (Stretch)
				GameMgr.SpriteBatch.Draw(Texture2D, UIActor.RectTransform.Rectangle.ToRectangle(), SrcTextureRectangle, ColorTint);
			else
				GameMgr.SpriteBatch.Draw(Texture2D, UIActor.RectTransform.Rectangle.ToRectangle(), SrcTextureRectangle,
					ColorTint);
		}
	}
}