using System;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	/// <summary>
	///     NineSlice texture. RectTransform stretching might be not working
	/// </summary>
	public class NineSlice : Image
	{
		/// <summary>
		///     Resize Mode, IMPORTANT, Tile mode only works when the texture size is Power of 2
		/// </summary>
		public enum ResizeMode
		{
			Tile,
			Stretch
		}

		private Rectangle _center = Rectangle.Empty;


		public ResizeMode Mode = ResizeMode.Tile;

		public NineSlice(Actor owner) : base(owner)
		{
		}

		public new Texture2D Texture2D
		{
			get => Texture;
			set
			{
				Texture = value;
				if (TextureRectangle == Rectangle.Empty)
				{
					TextureRectangle = Texture.Bounds;
					Center = new Rectangle(1, 1,
						TextureRectangle.Width - 2, TextureRectangle.Height - 2);
				}
			}
		}

		public Rectangle Center
		{
			get => _center;
			set =>
				_center = new Rectangle((int) MathF.Max(1, value.X), (int) MathF.Max(1, value.Y),
					(int) MathF.Min(TextureRectangle.Width - 2, value.Width),
					(int) MathF.Min(TextureRectangle.Height - 2, value.Height));
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			if (Texture2D == null)
				return;

			var transformRectangle = Transform.Rectangle.ToRectangle();

			// used for intersect to the slices, so we still get valid rectangle for the texture
			var transformToSrcRect = Transform.Rectangle.ToRectangle();
			transformToSrcRect.Location = TextureRectangle.Location;

			var drawLocation = transformRectangle.Location;

			var x1 = transformRectangle.Location.X;
			var x2 = transformRectangle.Location.X + Center.Location.X;
			var x3 = x1 + transformRectangle.Width - TopRightSlice.Width;
			var y1 = transformRectangle.Location.Y;
			var y2 = transformRectangle.Location.Y + Center.Location.Y;
			var y3 = y1 + transformRectangle.Height - BottomLeftSlice.Height;

			var widthLeft = transformRectangle.Width;
			var w1 = widthLeft >= x2 - x1 ? x2 - x1 : widthLeft;
			widthLeft -= w1;
			var w3 = TopRightSlice.Width;
			w3 = widthLeft >= w3 ? w3 : widthLeft;
			widthLeft -= w3;
			var w2 = widthLeft;

			var heightLeft = transformRectangle.Height;
			var h1 = heightLeft >= y2 - y1 ? y2 - y1 : heightLeft;
			heightLeft -= h1;
			var h3 = BottomLeftSlice.Height;
			h3 = heightLeft >= h3 ? h3 : heightLeft;
			heightLeft -= h3;
			var h2 = heightLeft;


			// Draw corner
			GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x1, y1, w1, h1), TopLeftSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x3, y1, w3, h1), TopRightSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x1, y3, w1, h3), BottomLeftSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x3, y3, w3, h3), BottomRightSlice, TintColor);

			// Draw resizeable side
			if (Mode == ResizeMode.Stretch)
			{
				GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x2, y1, w2, h1), TopCenterSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x1, y2, w1, h2), MiddleLeftSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x3, y2, w3, h2), MiddleRightSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x2, y3, w2, h3), BottomCenterSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, new Rectangle(x2, y2, w2, h2), MiddleCenterSlice, TintColor);
			}
			else
			{
				DrawTiling(new Point(x2, y1), new Point(w2, h1), TopCenterSlice);
				DrawTiling(new Point(x1, y2), new Point(w1, h2), MiddleLeftSlice);
				DrawTiling(new Point(x3, y2), new Point(w3, h2), MiddleRightSlice);
				DrawTiling(new Point(x2, y3), new Point(w2, h3), BottomCenterSlice);
				DrawTiling(new Point(x2, y2), new Point(w2, h2), MiddleCenterSlice);
			}
		}

		private void DrawTiling(Point start, Point size, Rectangle srcRect)
		{
			var x = start.X;
			var widthLeft = size.X;
			while (widthLeft > 0)
			{
				var y = start.Y;
				var heightLeft = size.Y;
				while (heightLeft > 0)
				{
					var destination = new Rectangle(x, y,
						widthLeft > srcRect.Width ? srcRect.Width : widthLeft,
						heightLeft > srcRect.Height ? srcRect.Height : heightLeft);
					var textureRect = new Rectangle(srcRect.X, srcRect.Y,
						widthLeft > srcRect.Width ? srcRect.Width : widthLeft,
						heightLeft > srcRect.Height ? srcRect.Height : heightLeft);

					GameMgr.SpriteBatch.Draw(Texture2D, destination, textureRect, TintColor);

					y += srcRect.Height;
					heightLeft -= srcRect.Height;
				}

				x += srcRect.Width;
				widthLeft -= srcRect.Width;
			}
		}

		#region GetSlice

		private Rectangle TopLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X, TextureRectangle.Y, Center.X, Center.Y);
			}
		}

		private Rectangle TopCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X, TextureRectangle.Y,
					Center.Width, Center.Y);
			}
		}

		private Rectangle TopRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X + Center.Width, TextureRectangle.Y,
					TextureRectangle.Width - (Center.X + Center.Width), Center.Y);
			}
		}

		private Rectangle MiddleLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X, TextureRectangle.Y + Center.Y,
					Center.X, Center.Height);
			}
		}

		private Rectangle MiddleCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X, TextureRectangle.Y + Center.Y,
					Center.Width, Center.Height);
			}
		}

		private Rectangle MiddleRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X + Center.Width, TextureRectangle.Y + Center.Y,
					TextureRectangle.Width - (Center.X + Center.Width), Center.Height);
			}
		}

		private Rectangle BottomLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X, TextureRectangle.Y + Center.Y + Center.Height,
					Center.X, TextureRectangle.Height - (_center.Y + Center.Height));
			}
		}

		private Rectangle BottomCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X, TextureRectangle.Y + Center.Y + Center.Height,
					Center.Width, TextureRectangle.Height - (_center.Y + Center.Height));
			}
		}

		private Rectangle BottomRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(TextureRectangle.X + Center.X + Center.Width,
					TextureRectangle.Y + Center.Y + Center.Height,
					TextureRectangle.Width - (Center.X + Center.Width),
					TextureRectangle.Height - (_center.Y + Center.Height));
			}
		}

		#endregion
	}
}