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
		public Rectangle TextureRectangle = Rectangle.Empty;
		public Color TintColor = Color.White;
		public bool Stretch = true;

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
			base.Draw(gameTime: gameTime, layerZDepth: layerZDepth);

			var origin = Transform.Pivot * TextureRectangle.Size.ToVector2();
			if (Stretch)
			{
				var destinationRect = Owner.RectTransform.GlobalRectangle;
				destinationRect.Location += Transform.Pivot * destinationRect.Size;
				GameMgr.SpriteBatch.Draw(texture: Texture2D, destinationRectangle: destinationRect.ToRectangle(), sourceRectangle: TextureRectangle,
					color: TintColor, rotation: Transform.GlobalRotation, origin: origin, effects: SpriteEffects.None, layerDepth: 0);
			}
			else
			{
				GameMgr.SpriteBatch.Draw(texture: Texture2D,
					position: Transform.GlobalLocationByPivot + Vector2.One * 3, 
					sourceRectangle: TextureRectangle,
					color: TintColor,
					rotation: Transform.GlobalRotation,
					origin: origin, 
					scale: 4,
					effects: SpriteEffects.None,
					layerDepth: Single.Epsilon);
				
				GameMgr.SpriteBatch.Draw(texture: Texture2D,
					position: Transform.GlobalLocationByPivot, 
					sourceRectangle: TextureRectangle,
					color: TintColor,
					rotation: Transform.GlobalRotation,
					origin: origin, 
					scale: 4,
					effects: SpriteEffects.None,
					layerDepth: Single.Epsilon * 2f);
				// Console.WriteLine(value: Transform.GlobalLocationByPivot);
				// Console.WriteLine(value: Transform.GlobalScale);
				// Console.WriteLine(value: TextureRectangle);
				// Console.WriteLine(value: origin);
			}
		}
	}
}