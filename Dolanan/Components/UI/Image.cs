using System;
using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	public class Image : UIComponent
	{
		protected Texture2D Texture;
		public Color TintColor = Color.White;
		public Rectangle TextureRectangle = Rectangle.Empty;

		public Image(Actor owner) : base(owner)
		{
		}

		public Texture2D Texture2D
		{
			get => Texture;
			set
			{
				Texture = value;
				if (TextureRectangle == Rectangle.Empty)
					TextureRectangle = Texture.Bounds;
			}
		}

		public override void Start()
		{
			base.Start();
			Texture2D = ScreenDebugger.Pixel;
		}

		public void FitRectangleToImage()
		{
			Owner.RectTransform.SetRectSize(TextureRectangle.Size.ToVector2());
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

			GameMgr.SpriteBatch.Draw(Texture2D, Owner.RectTransform.GlobalRectangle.ToRectangle(), TextureRectangle,
				TintColor);
		}
	}
}