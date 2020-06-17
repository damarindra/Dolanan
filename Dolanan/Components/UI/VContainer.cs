using System;
using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	/// <summary>
	/// Container will auto set the childs position. The event only trigger when an UIActor.SetParent is triggered.
	/// all child position is editable, don't change it's value if you want to keep it's position.
	/// </summary>
	public class VContainer : UIComponent
	{
		public VContainer(Actor owner) : base(owner)
		{
		}
		
		public ChildAlignment ChildAlignment = ChildAlignment.TopLeft;
		public Padding Padding;
		public int Spacing = 4;

		public void RefreshChildsLocation()
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
			
			int lastY = (int)Transform.GlobalRectangle.Y + Padding.Top;
			if (ChildAlignment == ChildAlignment.TopRight || ChildAlignment == ChildAlignment.BottomRight)
				lastY = (int) Transform.GlobalRectangle.Bottom - Padding.Bottom;
			int startX = (int) Transform.GlobalRectangle.X + Padding.Left;
			if (ChildAlignment == ChildAlignment.BottomLeft || ChildAlignment == ChildAlignment.BottomRight)
				startX = (int) Transform.GlobalRectangle.Right - Padding.Right;

			int dir = (ChildAlignment == ChildAlignment.BottomLeft || ChildAlignment == ChildAlignment.BottomRight) ? -1 : 1;
			foreach (var transformChild in Transform.Childs)
			{
				Type ownerType = transformChild.Owner.GetType();
				if (ownerType == typeof(UIActor) || ownerType.IsSubclassOf(typeof(UIActor)))
				{
					RectTransform rt = (RectTransform) transformChild;
					rt.Anchor = childAnchor;

					rt.LocationByPivot = new Vector2(startX, lastY);
					lastY += dir * ((int)rt.RectSize.Y + Spacing);
				}
			}
		}
		
		public override void Start()
		{
			base.Start();
			Owner.Clip = true;

			Owner.OnChildChange += childs =>
			{
				RefreshChildsLocation();
			};
		}
	}
}