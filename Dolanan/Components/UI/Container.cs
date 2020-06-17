using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	public abstract class Container : UIComponent
	{
		public enum ChildAlignment
		{
			TopLeft, TopRight,
			BottomLeft, BottomRight
		}
		
		protected Container(Actor owner) : base(owner)
		{
		}
		public ChildAlignment Alignment = ChildAlignment.TopLeft;
		public Padding Padding = new Padding(4);

		protected Anchor _childAnchor;

		/// <summary>
		/// Inner Rectangle, the location is on Global Space
		/// </summary>
		public Rectangle InnerRectangle
		{
			get
			{
				Rectangle innerRect = Transform.GlobalRectangle.ToRectangle();
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
			
		}
		
		#region Cycle
		public override void Start()
		{
			base.Start();
			Owner.Clip = true;

			Owner.OnChildChange += childs =>
			{
				RefreshChildsRectangle();
			};
		}
		
		#endregion
	}
}