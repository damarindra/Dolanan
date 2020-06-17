using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	/// <summary>
	///     Container is a container for UIActor. Container will automatically layouting all the childs
	/// </summary>
	public abstract class Container : UIComponent
	{
		public enum ChildAlignment
		{
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		protected Anchor _childAnchor;

		/// <summary>
		///     Alignment for the child (sub child not affected)
		///     what alignment do : set all of the childs Anchor.
		/// </summary>
		public ChildAlignment Alignment = ChildAlignment.TopLeft;

		/// <summary>
		///     Horizontal rectangle will following the parent.
		///     What it actually do : set the anchor Min.X = 0 and Max.X = 1
		/// </summary>
		public bool ChildStretchHorizontal = false;

		/// <summary>
		///     Vertical rectangle will following the parent
		///     What it actually do : set the anchor Min.Y = 0 and Max.Y = 1
		/// </summary>
		public bool ChildStretchVertical = false;

		/// <summary>
		///     Padding of the rectangle
		/// </summary>
		public Padding Padding = new Padding(4);

		protected Container(Actor owner) : base(owner)
		{
		}

		/// <summary>
		///     Inner Rectangle, the location is on Global Space.
		///     what it is : GlobalRectangle - Padding
		/// </summary>
		public Rectangle InnerRectangle
		{
			get
			{
				var innerRect = Transform.GlobalRectangle.ToRectangle();
				innerRect.X += Padding.Left;
				innerRect.Y += Padding.Top;
				innerRect.Width -= Padding.Right;
				innerRect.Height -= Padding.Bottom;
				return innerRect;
			}
		}

		protected virtual void RefreshChildsRectangle()
		{
			switch (Alignment)
			{
				default:
				case ChildAlignment.TopLeft:
					_childAnchor = Anchor.TopLeft;
					break;
				case ChildAlignment.TopRight:
					_childAnchor = Anchor.TopRight;
					break;
				case ChildAlignment.BottomLeft:
					_childAnchor = Anchor.BottomLeft;
					break;
				case ChildAlignment.BottomRight:
					_childAnchor = Anchor.BottomRight;
					break;
			}

			if (ChildStretchHorizontal)
			{
				_childAnchor.Min.X = 0;
				_childAnchor.Max.X = 1;
			}

			if (ChildStretchVertical)
			{
				_childAnchor.Min.Y = 0;
				_childAnchor.Max.Y = 0;
			}
		}

		#region Cycle

		public override void Start()
		{
			base.Start();
			Owner.Clip = true;

			Owner.OnChildChange += childs => { RefreshChildsRectangle(); };
		}

		#endregion
	}
}