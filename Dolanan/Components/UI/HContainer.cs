using System;
using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	public class HContainer : UIComponent
	{
		public HContainer(Actor owner) : base(owner)
		{
		}

		public ChildAlignment ChildAlignment = ChildAlignment.TopLeft;
		public int HorizontalCount = 4;
		public Padding Padding;

		protected void RefreshChildsLocation()
		{
			Anchor childAnchor;
			switch (ChildAlignment)
			{
				default:
				case ChildAlignment.TopLeft:
					childAnchor = Anchor.TopLeft;
					break;
				case ChildAlignment.TopRight:
					childAnchor = Anchor.TopRight;
					break;
				case ChildAlignment.BottomLeft:
					childAnchor = Anchor.BottomLeft;
					break;
				case ChildAlignment.BottomRight:
					childAnchor = Anchor.BottomRight;
					break;
			}
			
			int lastX = (int)Transform.GlobalRectangle.X + Padding.Left;
			if (ChildAlignment == ChildAlignment.TopRight || ChildAlignment == ChildAlignment.BottomRight)
				lastX = (int) Transform.GlobalRectangle.Right - Padding.Right;
			int startY = (int) Transform.GlobalRectangle.Y + Padding.Top;
			if (ChildAlignment == ChildAlignment.BottomLeft || ChildAlignment == ChildAlignment.BottomRight)
				startY = (int) Transform.GlobalRectangle.Bottom - Padding.Bottom;

			int spacing = (int)(Transform.Rectangle.Size.X - (Padding.Left + Padding.Right)) / (HorizontalCount );
			// Console.WriteLine(spacing);
			
			int dir = (ChildAlignment == ChildAlignment.TopRight || ChildAlignment == ChildAlignment.BottomRight) ? -1 : 1;
			foreach (var transformChild in Transform.Childs)
			{
				Type ownerType = transformChild.Owner.GetType();
				if (ownerType == typeof(UIActor) || ownerType.IsSubclassOf(typeof(UIActor)))
				{
					RectTransform rt = (RectTransform) transformChild;
					rt.Anchor = childAnchor;

					rt.LocationByPivot = new Vector2(lastX, startY);
					lastX += dir * (spacing);
				}
			}
		}
		
		#region Cycle
		public override void Start()
		{
			base.Start();
			Owner.Clip = true;

			Owner.OnChildChange += childs =>
			{
				RefreshChildsLocation();
			};
		}
		
		#endregion
	}

	public enum ChildAlignment
	{
		TopLeft, TopRight,
		BottomLeft, BottomRight
	}
}