using System;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	/// <summary>
	/// NineSlice texture. RectTransform stretching might be not working
	/// </summary>
	public class NineSlice : UIComponent
	{
		public enum ResizeMode
		{
			Tile, Stretch
		}
		
		public NineSlice(Actor owner) : base(owner)
		{
		}

		public Texture2D Texture2D
		{
			get => _texture;
			set
			{
				_texture = value;
				if (SrcTextureRectangle == Rectangle.Empty)
				{
					SrcTextureRectangle = _texture.Bounds;
					Center = new Rectangle(1, 1, 
						SrcTextureRectangle.Width - 2, SrcTextureRectangle.Height - 2);
				}
			}
		}
		public ResizeMode Mode = ResizeMode.Stretch;
		public Rectangle SrcTextureRectangle = Rectangle.Empty;
		public Color TintColor = Color.White;

		public Rectangle Center
		{
			get => _center;
			set
			{
				_center = new Rectangle((int)MathF.Max(1, value.X), (int)MathF.Max(1, value.Y),
					(int)MathF.Min(SrcTextureRectangle.Width - 2, value.Width),
					(int)MathF.Min(SrcTextureRectangle.Height - 2, value.Height));
			}
		}

		private Rectangle _center = Rectangle.Empty;

		private Texture2D _texture;

		#region GetSlice
		private Rectangle TopLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X, SrcTextureRectangle.Y, Center.X, Center.Y);
			}
		}
		private Rectangle TopCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X,SrcTextureRectangle.Y, 
					Center.Width, Center.Y);
			}
		}
		private Rectangle TopRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X + Center.Width, SrcTextureRectangle.Y, 
					SrcTextureRectangle.Width - (Center.X + Center.Width), Center.Y);
			}
		}

		private Rectangle MiddleLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X, SrcTextureRectangle.Y + Center.Y, 
					Center.X, Center.Height);
			}
		}
		private Rectangle MiddleCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X, SrcTextureRectangle.Y + Center.Y, 
					Center.Width, Center.Height);
			}
		}
		private Rectangle MiddleRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X + Center.Width, SrcTextureRectangle.Y + Center.Y, 
					SrcTextureRectangle.Width - (Center.X + Center.Width), Center.Height);
			}
		}
		private Rectangle BottomLeftSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X, SrcTextureRectangle.Y + Center.Y + Center.Height, 
					Center.X, SrcTextureRectangle.Height - (_center.Y + Center.Height));
			}
		}
		private Rectangle BottomCenterSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X, SrcTextureRectangle.Y + Center.Y + Center.Height, 
					Center.Width, SrcTextureRectangle.Height - (_center.Y + Center.Height));
			}
		}
		private Rectangle BottomRightSlice
		{
			get
			{
				if (Texture2D == null)
					return default;
				return new Rectangle(SrcTextureRectangle.X + Center.X + Center.Width, SrcTextureRectangle.Y + Center.Y + Center.Height, 
					SrcTextureRectangle.Width - (Center.X + Center.Width), SrcTextureRectangle.Height - (_center.Y + Center.Height));
			}
		}
		#endregion

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			if(Texture2D == null)
				return;

			Rectangle transformRectangle = Transform.Rectangle.ToRectangle();
			
			// used for intersect to the slices, so we still get valid rectangle for the texture
			Rectangle transformToSrcRect = Transform.Rectangle.ToRectangle();
			transformToSrcRect.Location = SrcTextureRectangle.Location;

			Point drawLocation = transformRectangle.Location;
			
			

			#region DrawCornerSide

			//Corner slice, if the rectangle is to small, it might stretch. Just be careful when setup the Transform.Rectangle.
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(drawLocation, Rectangle.Intersect(transformToSrcRect, TopLeftSlice).Size),
				TopLeftSlice, TintColor);
			
			drawLocation = transformRectangle.Location + new Point(transformRectangle.Width - TopRightSlice.Width, 0);
			drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(drawLocation, Rectangle.Intersect(transformToSrcRect, TopRightSlice).Size), 
					TopRightSlice, TintColor);

			drawLocation = transformRectangle.Location + new Point(0, transformRectangle.Height - BottomLeftSlice.Height);
			drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
			GameMgr.SpriteBatch.Draw(Texture2D, 
				new Rectangle(drawLocation, Rectangle.Intersect(transformToSrcRect, BottomLeftSlice).Size), 
					BottomLeftSlice, TintColor);
			
			drawLocation = transformRectangle.Location + new Point(transformRectangle.Width - TopRightSlice.Width, transformRectangle.Height - BottomLeftSlice.Height);
			drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
			GameMgr.SpriteBatch.Draw(Texture2D,
				new Rectangle(drawLocation,Rectangle.Intersect(transformToSrcRect, BottomRightSlice).Size),
					BottomRightSlice, TintColor);
			
			#endregion
			
			if (Mode == ResizeMode.Stretch)
			{
				//Middle Center Edge
				drawLocation = transformRectangle.Location + new Point(TopLeftSlice.Width, 0);
				drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(drawLocation, new Point(transformRectangle.Width - TopLeftSlice.Width - TopRightSlice.Width, TopLeftSlice.Height)),
					TopCenterSlice, TintColor);
				drawLocation = transformRectangle.Location + new Point(0, TopLeftSlice.Height);
				drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(drawLocation, new Point(TopLeftSlice.Width, transformRectangle.Height - TopLeftSlice.Height - BottomLeftSlice.Height)), 
					MiddleLeftSlice, TintColor);
				drawLocation = transformRectangle.Location + new Point(transformRectangle.Width - TopRightSlice.Width, TopLeftSlice.Height);
				drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
				GameMgr.SpriteBatch.Draw(Texture2D, 
					new Rectangle(drawLocation, new Point(TopRightSlice.Width, transformRectangle.Height - TopRightSlice.Height - BottomRightSlice.Height)),
					MiddleRightSlice, TintColor);
				drawLocation = transformRectangle.Location + new Point(TopLeftSlice.Width, transformRectangle.Height - BottomLeftSlice.Height);
				drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
				GameMgr.SpriteBatch.Draw(Texture2D,
					new Rectangle(drawLocation, new Point(transformRectangle.Width - BottomLeftSlice.Width - BottomRightSlice.Width, BottomLeftSlice.Height)), 
					BottomCenterSlice, TintColor);
				
				// Middle Center
				drawLocation = transformRectangle.Location + TopLeftSlice.Size;
				drawLocation = MathEx.Min(transformRectangle.Location, drawLocation);
				GameMgr.SpriteBatch.Draw(Texture2D,
					new Rectangle(drawLocation, Rectangle.Intersect(transformToSrcRect, TopCenterSlice).Size),
					TopCenterSlice, TintColor);
				
			}
			else
			{
				int totalX = SrcTextureRectangle.Width / Center.Width;
				int totalY = SrcTextureRectangle.Height / Center.Height;

				for (int x = 0; x <= totalX; x++)
				{
					for (int y = 0; y <= totalY; y++)
					{
						GameMgr.SpriteBatch.Draw(Texture2D,
							new Rectangle(drawLocation, Rectangle.Intersect(transformToSrcRect, TopCenterSlice).Size),
							TopCenterSlice, TintColor);
					}
				}
			}
		}
	}
}