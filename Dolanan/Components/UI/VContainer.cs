using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	/// <summary>
	///     Container will auto set the childs position. The event only trigger when an UIActor.SetParent is triggered.
	///     all child position is editable, don't change it's value if you want to keep it's position.
	/// </summary>
	public class VContainer : Container
	{
		public int Spacing = 4;

		public VContainer(Actor owner) : base(owner)
		{
		}

		protected override void RefreshChildsRectangle()
		{
			base.RefreshChildsRectangle();

			var lastY = (int) Transform.GlobalRectangle.Y + Padding.Top;
			if (Alignment == ChildAlignment.BottomLeft || Alignment == ChildAlignment.BottomRight)
				lastY = (int) Transform.GlobalRectangle.Bottom - Padding.Bottom;
			var startX = (int) Transform.GlobalRectangle.X + Padding.Left;
			if (Alignment == ChildAlignment.TopRight || Alignment == ChildAlignment.BottomRight)
				startX = (int) Transform.GlobalRectangle.Right - Padding.Right;

			var dir = Alignment == ChildAlignment.BottomLeft || Alignment == ChildAlignment.BottomRight ? -1 : 1;
			foreach (var transformChild in Transform.Childs)
			{
				var ownerType = transformChild.Owner.GetType();
				if (ownerType == typeof(UIActor) || ownerType.IsSubclassOf(typeof(UIActor)))
				{
					var rt = (RectTransform) transformChild;
					rt.Anchor = _childAnchor;

					rt.GlobalLocationByPivot = new Vector2(startX, lastY);
					lastY += dir * ((int) rt.Size.Y + Spacing);
				}
			}
		}
	}
}