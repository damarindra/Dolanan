using System;
using Dolanan.Components;
using Dolanan.Engine;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Core
{
	/// <summary>
	///     RectTransform is used for UI Transforming stuff. Parent of UITransform can be Transform2D or RectTransform itself
	///     Anchoring only work when parent are RectTransform
	///     Location => Top Left of the rectangle.
	///     Use LocationByPivot to getting the local location of from parent location and pivot location
	///     All calculation is based on Rectangle.Location (Top Left). Pivot only to make developer easier to use this
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
				if (_rectangle.Location != value.Location)
				{
				}

				_rectangle = value;

				UpdateAnchorOffset();
				UpdateRectTransform();
				UpdateChildsRectTransform();
			}
		}

		public RectangleF GlobalRectangle => new RectangleF(GlobalLocation, Rectangle.Size);

		/// <summary>
		///     Anchor from the parent. Anchor will automatically adjusting the Rectangle, location and it size.
		///     Anchor doesn't act as Rectangle offset, so setting location do not relative to anchor
		/// </summary>
		public Anchor Anchor
		{
			get => _anchor;
			set
			{
				if(UIParent == null)
					Log.PrintWarning("Actor : "+ Owner.Name + ", Trying to modify anchor, but don't have Parent UIActor, it is useless! SetParent before modify anchor! ");
				
				_anchor.Min = new Vector2(MathEx.Clamp(value.Min.X, 0, 1), MathEx.Clamp(value.Min.Y, 0, 1));
				_anchor.Max = new Vector2(MathEx.Clamp(value.Max.X, 0, 1), MathEx.Clamp(value.Max.Y, 0, 1));

				UpdateAnchorOffset();
			}
		}

		/// <summary>
		///     Rectangle of the Anchor. If the Anchor is not stretch (Rectangle), will return empty rectangle with right location
		/// </summary>
		public RectangleF AnchorRect
		{
			get
			{
				if (UIParent == null)
				{
					Log.PrintError(
						"Actor : "+ Owner.Name + ", Trying to get Anchor Rect, but doesn't have any parent, returning default Rectangle");
					return _rectangle;
				}

				var min = Anchor.Min * UIParent.RectTransform.Rectangle.Size;
				var max = Anchor.Max * UIParent.RectTransform.Rectangle.Size;

				return new RectangleF(min, max - min);
			}
		}

		public UISpace UISpace
		{
			get
			{
				//just force to world ui if the layer is type`Layer`
				if (Owner.Layer.GetType() == typeof(Layer))
					return UISpace.World;

				var layer = (UILayer) Owner.Layer;
				return layer.UISpace;
			}
		}

		public Vector2 RectLocation => Rectangle.Location;
		public Vector2 RectSize => Rectangle.Size;

		/// <summary>
		///     Location is calculated from the TopLeft of the Rectangle. Location relative only to its parent location (also top
		///     left if UI)
		/// </summary>
		public override Vector2 Location
		{
			get => base.Location;
			set => SetRectLocation(value);
		}

		/// <summary>
		///     Location is calculated from the TopLeft of the Rectangle
		/// </summary>
		public override Vector2 GlobalLocation
		{
			get => base.GlobalLocation;
			set => Location = value - ParentGlobalLocation;
		}

		public Point ScreenLocation
		{
			get
			{
				if (UISpace == UISpace.World) Camera.WorldToScreen(GlobalLocation);

				return GlobalLocation.ToPoint();
			}
		}

		/// <summary>
		///     Location by Pivot, remember to set the Pivot and Rect Size before set the location.
		/// </summary>
		public Vector2 LocationByPivot
		{
			get => Rectangle.Location + Pivot * Rectangle.Size;
			set
			{
				var loc = value - Pivot * Rectangle.Size;
				SetRectLocation(loc);
			}
		}

		/// <summary>
		///     Global Location by Pivot, remember to set the Pivot and Rect Size before set the location.
		/// </summary>
		public Vector2 GlobalLocationByPivot
		{
			get => GlobalLocation + Pivot * Rectangle.Size;
			set
			{
				var loc = value - Pivot * Rectangle.Size;
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
				if (UISpace == UISpace.World) Camera.WorldToScreen(GlobalLocationByPivot);

				return GlobalLocationByPivot.ToPoint();
			}
		}

		public float LeftRect
		{
			get => Rectangle.Left;
			set
			{
				var delta = Rectangle.Left - value;
				Rectangle = new RectangleF(value, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		}

		public float RightRect
		{
			get => Rectangle.Right;
			set
			{
				var delta = value - Rectangle.Right;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width + delta, Rectangle.Height);
			}
		}

		public float TopRect
		{
			get => Rectangle.Top;
			set
			{
				var delta = Rectangle.Top - value;
				Rectangle = new RectangleF(Rectangle.X, value, Rectangle.Width, Rectangle.Height + delta);
			}
		}

		public float BottomRect
		{
			get => Rectangle.Bottom;
			set
			{
				var delta = value - Rectangle.Bottom;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height + delta);
			}
		}

		public UIActor UIParent { get; set; }

		#endregion

		#region Fields

		/// <summary>
		///     Pivot / Center of the rect location
		/// </summary>
		public Pivot Pivot = Pivot.TopLeft;

		/// <summary>
		///     Offset Left and Top from Anchor to Rectangle
		/// </summary>
		private Vector2 _offsetAnchorToRectMin;

		/// <summary>
		///     Offset Right and Bottom from Anchor to Rectangle
		/// </summary>
		private Vector2 _offsetAnchorToRectMax;

		private RectangleF _rectangle;
		private Anchor _anchor;

		#endregion

		#region Method

		/// <summary>
		/// 	Set Anchor Min and Max exactly the same as rectangle
		/// </summary>
		public void FitAnchorToRect()
		{
			if(UIParent == null)
				return;
			
			Anchor = new Anchor(Location / UIParent.RectTransform.RectSize, (Location + RectSize) / UIParent.RectTransform.RectSize);
		}

		public void RefreshParent()
		{
			Rectangle = _rectangle;
		}

		private void UpdateAnchorOffset()
		{
			if (UIParent != null)
			{
				var anchorRect = AnchorRect;

				_offsetAnchorToRectMin.X = _rectangle.X - anchorRect.Left;
				_offsetAnchorToRectMin.Y = _rectangle.Y - anchorRect.Top;
				_offsetAnchorToRectMax.X = _rectangle.Right - anchorRect.Right;
				_offsetAnchorToRectMax.Y = _rectangle.Bottom - anchorRect.Bottom;
			}
		}

		/// <summary>
		///     Updating rect transform including all child
		/// </summary>
		public void UpdateRectTransform()
		{
			// Updating rect transform
			if (UIParent != null)
			{
				var anchorRect = AnchorRect;

				var left = anchorRect.Left + _offsetAnchorToRectMin.X;
				var top = anchorRect.Top + _offsetAnchorToRectMin.Y;
				var right = anchorRect.Right + _offsetAnchorToRectMax.X;
				var bottom = anchorRect.Bottom + _offsetAnchorToRectMax.Y;
				_rectangle = new RectangleF(left, top, right - left, bottom - top);
			}

			base.Location = _rectangle.Location;
		}

		protected void UpdateChildsRectTransform()
		{
			foreach (var child in Childs)
				if (child.GetType() == typeof(RectTransform) || child.GetType().IsSubclassOf(typeof(RectTransform)))
					if (child.Owner.GetType() == typeof(UIActor) || child.Owner.GetType().IsSubclassOf(typeof(UIActor)))
					{
						var uiActor = (UIActor) child.Owner;

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
			OnParentChange += parent =>
			{
				if (parent != null)
					if (parent.Owner.GetType() == typeof(UIActor) ||
					    parent.Owner.GetType().IsSubclassOf(typeof(UIActor)))
						UIParent = (UIActor) parent.Owner;
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