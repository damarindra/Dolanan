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
	public class HContainer : Container
	{
		public HContainer(Actor owner) : base(owner)
		{
		}

		public int Spacing = 4;

		protected override void RefreshChildsRectangle()
		{
			base.RefreshChildsRectangle();
			
			int lastX = (int)Transform.GlobalRectangle.X + Padding.Left;
			if (Alignment == ChildAlignment.TopRight || Alignment == ChildAlignment.BottomRight)
				lastX = (int) Transform.GlobalRectangle.Right - Padding.Right;
			int startY = (int) Transform.GlobalRectangle.Y + Padding.Top;
			if (Alignment == ChildAlignment.BottomLeft || Alignment == ChildAlignment.BottomRight)
				startY = (int) Transform.GlobalRectangle.Bottom - Padding.Bottom;

			int dir = (Alignment == ChildAlignment.TopRight || Alignment == ChildAlignment.BottomRight) ? -1 : 1;
			foreach (var transformChild in Transform.Childs)
			{
				Type ownerType = transformChild.Owner.GetType();
				if (ownerType == typeof(UIActor) || ownerType.IsSubclassOf(typeof(UIActor)))
				{
					RectTransform rt = (RectTransform) transformChild;
					rt.Anchor = _childAnchor;

					rt.GlobalLocationByPivot = new Vector2(lastX, startY);
					lastX += dir * ((int)rt.Size.X + Spacing);
				}
			}
		}
		
	}

}