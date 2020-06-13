using System;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Core
{
	/// <summary>
	/// RectTransform is used for UI Transforming stuff. Parent of UITransform can be Transform2D or RectTransform itself
	/// Anchoring only work when parent are RectTransform
	/// Location => Top Left of the rectangle.
	/// Use LocationByPivot to getting the local location of from parent location and pivot location
	/// </summary>
	public class RectTransform : Transform2D
	{
		public RectTransform(Actor owner) : base(owner)
		{
		}

		#region Properties

		public RectangleF Rectangle
		{
			get => _rectangle;
			set
			{
				// Snapping to parent rectangle if ui
				if (ParentUI != null)
				{
					_rectangle.X = MathF.Max(_rectangle.X, ParentUI.RectTransform.Left);
					_rectangle.Y = MathF.Max(_rectangle.Y, ParentUI.RectTransform.Top);
					_rectangle.Width = MathF.Min(_rectangle.Width, ParentUI.RectTransform.Rectangle.Width);
					_rectangle.Height = MathF.Min(_rectangle.Height, ParentUI.RectTransform.Rectangle.Height);
				}
				else
				{
					_rectangle = value;
				}

				UpdateAnchorOffset();
				UpdateRectTransform();
				UpdateChildsRectTransform();
			}
		}
		public RectangleF GlobalRectangle => new RectangleF(GlobalLocation, Rectangle.Size);

		public Anchor Anchor
		{
			get => _anchor;
			set
			{
				_anchor.Min = new Vector2(MathEx.Clamp(value.Min.X, 0, 1), MathEx.Clamp(value.Min.Y, 0, 1));
				_anchor.Max = new Vector2(MathEx.Clamp(value.Max.X, 0, 1), MathEx.Clamp(value.Max.Y, 0, 1));

				UpdateAnchorOffset();
			}
		}

		private void UpdateAnchorOffset()
		{
			if (ParentUI != null)
			{
				var anchorRect = AnchorRect;

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
					Log.PrintError(
						"Trying to get Anchor Rect, but doesn't have any parent, returning default Rectangle");
					return _rectangle;
				}

				var min = Anchor.Min * ParentUI.RectTransform.Rectangle.Size;
				var max = Anchor.Max * ParentUI.RectTransform.Rectangle.Size;

				return new RectangleF(ParentUI.RectTransform.Rectangle.Location + min, max - min);
			}
		}

		public UISpace UISpace
		{
			get
			{
				//just force to world ui if the layer is type`Layer`
				if (Owner.Layer.GetType() == typeof(Layer))
					return UISpace.World;
				
				UILayer layer = (UILayer) Owner.Layer;
				return layer.UISpace;
			}
		}

		public Vector2 RectLocation => Rectangle.Location;
		public Vector2 RectSize => Rectangle.Size;

		public Point ScreenLocation
		{
			get
			{
				if (UISpace == UISpace.World)
				{
					Camera.WorldToScreen(GlobalLocation);
				}

				return GlobalLocation.ToPoint();
			}
		}

		/// <summary>
		/// Location by Pivot
		/// </summary>
		public Vector2 LocationByPivot
		{
			get => Rectangle.Location + Pivot * Rectangle.Size;
			set
			{
				Vector2 loc = value - Pivot * Rectangle.Size;
				SetRectLocation(loc);
			}
		}
		/// <summary>
		/// Global Location by Pivot
		/// </summary>
		public Vector2 GlobalLocationByPivot
		{
			get => GlobalLocation + Pivot * Rectangle.Size;
			set
			{
				Vector2 loc = value - Pivot * Rectangle.Size;
				if (Parent != null)
					loc -= Parent.GlobalLocation;
				SetRectLocation(loc);
				//Rectangle = new RectangleF(, _rectangle.Size);
			}
		}

		public Point ScreenLocationByPivot
		{
			get
			{
				if (UISpace == UISpace.World)
				{
					Camera.WorldToScreen(GlobalLocationByPivot);
				}

				return GlobalLocationByPivot.ToPoint();
			}
		}

		public float Left
		{
			get => Rectangle.Left;
			set
			{
				var delta = Rectangle.Left - value;
				Rectangle = new RectangleF(value, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		}

		public float Right
		{
			get => Rectangle.Right;
			set
			{
				var delta = value - Rectangle.Right;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		}

		public float Top
		{
			get => Rectangle.Top;
			set
			{
				var delta = Rectangle.Top - value;
				Rectangle = new RectangleF(Rectangle.X, value, Rectangle.Width, Rectangle.Height + delta);
			}
		}

		public float Bottom
		{
			get => Rectangle.Bottom;
			set
			{
				var delta = value - Rectangle.Bottom;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height + delta);
			}
		}

		public UIActor ParentUI { get; set; }
		
		#endregion

		#region Fields

		/// <summary>
		/// Pivot / Center of the rect location
		/// </summary>
		public Pivot Pivot = Pivot.TopLeft;

		/// <summary>
		///     Offset Left and Top
		/// </summary>
		private Vector2 _offsetMin;

		/// <summary>
		///     Offset Right and Bottom
		/// </summary>
		private Vector2 _offsetMax;

		private RectangleF _rectangle;
		private Anchor _anchor;

		#endregion

		#region Method

		public void RefreshParent()
		{
			Rectangle = _rectangle;
		}

		/// <summary>
		///     Updating rect transform including all child
		/// </summary>
		public void UpdateRectTransform()
		{
			// Updating rect transform
			if (ParentUI != null)
			{
				var anchorRect = AnchorRect;

				var left = anchorRect.Left + _offsetMin.X;
				var top = anchorRect.Top + _offsetMin.Y;
				var right = anchorRect.Right + _offsetMax.X;
				var bottom = anchorRect.Bottom + _offsetMax.Y;
				_rectangle = new RectangleF(left - Parent.Location.X, top - Parent.Location.Y, right - left, bottom - top);
			}
			
			Location = _rectangle.Location;
		}

		protected void UpdateChildsRectTransform()
		{
			foreach (var child in Childs)
			{
				if (child.GetType() == typeof(RectTransform) || child.GetType().IsSubclassOf(typeof(RectTransform)))
				{
					if (child.Owner.GetType() == typeof(UIActor) || child.Owner.GetType().IsSubclassOf(typeof(UIActor)))
					{
						var uiActor = (UIActor) child.Owner;

						uiActor.RectTransform.UpdateRectTransform();
						uiActor.RectTransform.UpdateChildsRectTransform();
					}
				}
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
			OnParentChange += parent =>
			{
				if (parent != null)
				{
					if (parent.Owner.GetType() == (typeof(UIActor)) ||
					    parent.Owner.GetType().IsSubclassOf(typeof(UIActor)))
					{
						ParentUI = (UIActor) parent.Owner;
					}
				}
			};
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

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
		public static Anchor TopRight => new Anchor(new Vector2(1, 0));

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