﻿using System;
using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	/// <summary>
	///     NineSlice texture. RectTransform Scale and Rotation might not working very well. Scale only accept whole number (int)
	/// </summary>
	public class NineSlice : UIComponent
	{
		protected Texture2D Texture;
		public Rectangle TextureRectangle = Rectangle.Empty;
		public Color TintColor = Color.White;
		
		/// <summary>
		///     Resize Mode, IMPORTANT, Tile mode only works when the texture size is Power of 2
		/// </summary>
		public enum ResizeMode
		{
			Tile,
			Stretch
		}

		private Rectangle _center = Rectangle.Empty;


		public ResizeMode Mode = ResizeMode.Stretch;

		public NineSlice(Actor owner) : base(owner)
		{
		}

		public Texture2D Texture2D
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

		/// <summary>
		/// Inner rectangle nine slice
		/// </summary>
		public Rectangle Center
		{
			get
			{
				if (_center == Rectangle.Empty)
					_center = new Rectangle(1, 1,
						TextureRectangle.Width - 2,
						TextureRectangle.Height - 2);

				return _center;
			}
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

			var transformRectangle = Transform.GlobalRectangle.ToRectangle();

			Vector2 globalScale = Owner.Transform.GlobalScaleRendering;
			
			Point topLeftSize = TopLeftSlice.Size * globalScale.ToPoint();
			Point bottomRightSize = BottomRightSlice.Size * globalScale.ToPoint();
			Point middleCenterSize = new Point(transformRectangle.Size.X - topLeftSize.X - bottomRightSize.X, transformRectangle.Size.Y - topLeftSize.Y - bottomRightSize.Y);

			Point topLeftLoc = transformRectangle.Location;
			Point middleCenterLoc = transformRectangle.Location + topLeftSize;
			Point bottomRightLoc = transformRectangle.Location +
			                       (transformRectangle.Size - bottomRightSize);

			// Draw corner
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(topLeftLoc, topLeftSize), TopLeftSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(bottomRightLoc.X, topLeftLoc.Y, bottomRightSize.X, topLeftSize.Y), TopRightSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(topLeftLoc.X, bottomRightLoc.Y, topLeftSize.X, bottomRightSize.Y), BottomLeftSlice, TintColor);
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(bottomRightLoc, bottomRightSize), BottomRightSlice, TintColor);

			// Draw resizeable side
			if (Mode == ResizeMode.Stretch)
			{
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(middleCenterLoc.X, topLeftLoc.Y, middleCenterSize.X, topLeftSize.Y), TopCenterSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(topLeftLoc.X, middleCenterLoc.Y, topLeftSize.X, middleCenterSize.Y), MiddleLeftSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D,
					new Rectangle(bottomRightLoc.X, middleCenterLoc.Y, bottomRightSize.X, middleCenterSize.Y), MiddleRightSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(middleCenterLoc.X, bottomRightLoc.Y, middleCenterSize.X, bottomRightSize.Y), BottomCenterSlice, TintColor);
				GameMgr.SpriteBatch.Draw(Texture2D,
					new Rectangle(middleCenterLoc.X, middleCenterLoc.Y, middleCenterSize.X, middleCenterSize.Y), MiddleCenterSlice, TintColor);
			}
			else
			{
				DrawTiling(new Point(middleCenterLoc.X, topLeftLoc.Y), new Point(middleCenterSize.X, topLeftSize.Y), TopCenterSlice);
				DrawTiling(new Point(topLeftLoc.X, middleCenterLoc.Y), new Point(topLeftSize.X, middleCenterSize.Y), MiddleLeftSlice);
				DrawTiling(new Point(bottomRightLoc.X, middleCenterLoc.Y), new Point(bottomRightSize.X, middleCenterSize.Y), MiddleRightSlice);
				DrawTiling(new Point(middleCenterLoc.X, bottomRightLoc.Y), new Point(middleCenterSize.X, bottomRightSize.Y), BottomCenterSlice);
				DrawTiling(new Point(middleCenterLoc.X, middleCenterLoc.Y), new Point(middleCenterSize.X, middleCenterSize.Y), MiddleCenterSlice);
			}
		}

		private void DrawTiling(Point start, Point size, Rectangle srcRect)
		{
			Vector2 globalScale = Owner.Transform.GlobalScaleRendering;
			int srcWidthScaled = srcRect.Width * (int)globalScale.X;
			int srcHeightScaled = srcRect.Width * (int)globalScale.Y;
			var x = start.X;
			var widthLeft = size.X;
			while (widthLeft > 0)
			{
				var y = start.Y;
				var heightLeft = size.Y;
				while (heightLeft > 0)
				{
					var destination = new Rectangle(x, y,
						widthLeft > srcWidthScaled ? srcWidthScaled : widthLeft,
						heightLeft > srcHeightScaled ? srcHeightScaled : heightLeft);

					GameMgr.SpriteBatch.Draw(Texture2D, destination, srcRect, TintColor);

					y += srcHeightScaled;
					heightLeft -= srcHeightScaled;
				}

				x += srcWidthScaled;
				widthLeft -= srcWidthScaled;
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