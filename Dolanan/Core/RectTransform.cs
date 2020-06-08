using System;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Engine
{
	public class RectTransform : Component
	{
		public RectTransform(Actor owner) : base(owner) { }

		#region Properties

		public RectangleF Rectangle
		{
			get => _rectangle;
			set
			{
				// Snapping to parent rectangle if ui
				if (ParentUI != null)
				{
					_rectangle.X = MathF.Max(_rectangle.X, _parentUIActor.RectTransform.Left);
					_rectangle.Y = MathF.Max(_rectangle.Y, _parentUIActor.RectTransform.Top);
					_rectangle.Width = MathF.Min(_rectangle.Width, _parentUIActor.RectTransform.Rectangle.Width);
					_rectangle.Height = MathF.Min(_rectangle.Height, _parentUIActor.RectTransform.Rectangle.Height);
				}else
					_rectangle = value;

				UpdateAnchorOffset();
				UpdateRectTransform();
				UpdateChildsRectTransform();
			}
		}
		public Anchor Anchor
		{
			get => _anchor;
			set
			{
				_anchor.Min = new Vector2(MathEx.Clamp(value.Min.X,0, 1), MathEx.Clamp(value.Min.Y,0, 1));
				_anchor.Max = new Vector2(MathEx.Clamp(value.Max.X,0, 1), MathEx.Clamp(value.Max.Y,0, 1));

				UpdateAnchorOffset();
			}
		}

		private void UpdateAnchorOffset()
		{
			if (ParentUI != null)
			{
				RectangleF anchorRect = AnchorRect;
				
				_offsetMin.X = _rectangle.X - anchorRect.Left;
				_offsetMin.Y = _rectangle.Y - anchorRect.Top;
				_offsetMax.X = _rectangle.Right - anchorRect.Right;
				_offsetMax.Y = _rectangle.Bottom - anchorRect.Bottom;
			}
		}


		public RectangleF AnchorRect
		{
			get
			{
				if (ParentUI == null)
				{
					Log.PrintError("Trying to get Anchor Rect, but doesn't have any parent, returning default Rectangle");
					return _rectangle;
				}

				Vector2 min = Anchor.Min * _parentUIActor.RectTransform.Rectangle.Size;
				Vector2 max = Anchor.Max * _parentUIActor.RectTransform.Rectangle.Size;
				
				return new RectangleF(_parentUIActor.RectTransform.Rectangle.Location + min, max-min);
				
			}
		}

		public float Left
		{
			get => Rectangle.Left;
			set
			{
				float delta = Rectangle.Left - value;
				Rectangle = new RectangleF(value, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		} 
		public float Right
		{
			get => Rectangle.Right;
			set
			{
				float delta = value - Rectangle.Right;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		} 
		public float Top
		{
			get => Rectangle.Top;
			set
			{
				float delta = Rectangle.Top - value;
				Rectangle = new RectangleF(Rectangle.X, value, Rectangle.Width, Rectangle.Height + delta);
			}
		} 
		public float Bottom 
		{
			get => Rectangle.Bottom;
			set
			{
				float delta = value - Rectangle.Bottom;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height + delta);
			}
		}

		public UIActor ParentUI
		{
			get
			{
				if(_parentUIActor == null && Owner.Parent != null)
					_parentUIActor = (UIActor) Owner.Parent;
				return _parentUIActor;
			}
		}

		#endregion

		#region Fields

		public Pivot Pivot;

		/// <summary>
		/// Offset Left and Top
		/// </summary>
		private Vector2 _offsetMin;
		/// <summary>
		/// Offset Right and Bottom
		/// </summary>
		private Vector2 _offsetMax;

		private UIActor _parentUIActor;
		private RectangleF _rectangle;
		private Anchor _anchor;

		#endregion
				
		#region Method

		public void RefreshParent()
		{
			Rectangle = _rectangle;
		}
		
		/// <summary>
		/// Updating rect transform including all child
		/// </summary>
		public void UpdateRectTransform()
		{
			// Updating rect transform
			if (ParentUI != null)
			{
				RectangleF anchorRect = AnchorRect;
				// Console.WriteLine(anchorRect);

				float left = anchorRect.Left + _offsetMin.X;
				float top = anchorRect.Top + _offsetMin.Y;
				float right = anchorRect.Right + _offsetMax.X;
				float bottom = anchorRect.Bottom + _offsetMax.Y;
				// Console.WriteLine(_offsetMin.X);
				// Console.WriteLine(_offsetMin.Y);
				// Console.WriteLine(_offsetMax.X);
				// Console.WriteLine(_offsetMax.Y);
				//
				_rectangle = new RectangleF(left,top, right-left, bottom-top);
				// Console.WriteLine(Owner.Name);
				// Console.WriteLine(_rectangle);
			}
		}

		protected void UpdateChildsRectTransform()
		{
			foreach (Actor child in Owner.GetChilds)
			{
				UIActor uiActor = (UIActor) child;
				
				if(uiActor == null)
					continue;

				uiActor.RectTransform.UpdateRectTransform();
				uiActor.RectTransform.UpdateChildsRectTransform();
			}
		}

		public void SetRectLocation(Vector2 location)
		{
			Rectangle = new RectangleF(location, _rectangle.Size);
		}
		public void SetRectSize(Vector2 size)
		{
			Rectangle = new RectangleF(_rectangle.Location, size);
		}
		
		#endregion

		#region Cycle

		public override void Start()
		{
			base.Start();
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			
			GameMgr.SpriteBatch.DrawStroke(_rectangle.ToRectangle(), Color.Yellow);
		}

		#endregion
		
		
	}

	public struct Anchor
	{
		public Vector2 Min;
		public Vector2 Max;

		public Anchor(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}
		public Anchor(Vector2 anchor)
		{
			Min = Max = anchor;
		}
		
		public static Anchor TopLeft => new Anchor(Vector2.Zero); 
		public static Anchor TopCenter => new Anchor(new Vector2(0.5f, 0)); 
		public static Anchor TopRight => new Anchor(new Vector2(1,0)); 
		
		public static Anchor MiddleLeft => new Anchor(new Vector2(0, 0.5f)); 
		public static Anchor MiddleCenter => new Anchor(new Vector2(0.5f, 0.5f)); 
		public static Anchor MiddleRight => new Anchor(new Vector2(1f, 0.5f));
		
		public static Anchor BottomLeft => new Anchor(new Vector2(0, 1f)); 
		public static Anchor BottomCenter => new Anchor(new Vector2(0.5f, 1f)); 
		public static Anchor BottomRight => new Anchor(new Vector2(1f, 1f));
		
		public static Anchor FullStretch => new Anchor(Vector2.Zero, Vector2.One);
		
		public static Anchor TopHorizontalStretch => new Anchor(Vector2.Zero, Vector2.UnitX);
		public static Anchor MiddleHorizontalStretch => new Anchor(new Vector2(0, 0.5f), new Vector2(1, 0.5f));
		public static Anchor BottomHorizontalStretch => new Anchor(new Vector2(0, 1f), new Vector2(1, 1f));
		
		public static Anchor LeftVerticalStretch => new Anchor(Vector2.Zero, Vector2.UnitY);
		public static Anchor CenterVerticalStretch => new Anchor(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
		public static Anchor RightVerticalStretch => new Anchor(new Vector2(1, 0f), new Vector2(1f, 1f));
	}
}