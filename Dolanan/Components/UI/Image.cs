using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	public class Image : UIComponent
	{
		public bool Stretch = true;
		protected Texture2D Texture;
		public Rectangle TextureRectangle = Rectangle.Empty;
		public Color TintColor = Color.White;

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

			var origin = Transform.Pivot * TextureRectangle.Size.ToVector2();
			if (Stretch)
			{
				var destinationRect = Owner.RectTransform.GlobalRectangle;
				destinationRect.Location += Transform.Pivot * destinationRect.Size;
				GameMgr.Draw(Owner, Texture2D, destinationRect.ToRectangle(), TextureRectangle,
					TintColor, Transform.GlobalRotation, origin, SpriteEffects.None, layerZDepth);
			}
			else
			{
				GameMgr.Draw(Owner, Texture2D,
					Transform.GlobalLocationByPivot + Vector2.One * 3,
					TextureRectangle,
					TintColor,
					Transform.GlobalRotation,
					origin,
					Owner.Transform.GlobalScaleRendering,
					SpriteEffects.None,
					layerZDepth);
			}
		}
	}
}