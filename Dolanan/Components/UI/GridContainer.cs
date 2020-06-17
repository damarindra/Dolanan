using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	/// <summary>
	///     Container will auto set the childs position. The event only trigger when an UIActor.SetParent is triggered.
	///     all child position is editable, don't change it's value if you want to keep it's position.
	/// </summary>
	public class GridContainer : Container
	{
		/// <summary>
		///     Automatically set child rect size same as GridSize when child added to this Container
		/// </summary>
		public bool AutoRectSize = false;

		public Point GridSize = new Point(32, 32);

		/// <summary>
		///     If true, grid will go vertically first, if false, otherwise
		/// </summary>
		public bool IsVerticalPriority = false;

		public GridContainer(Actor owner) : base(owner)
		{
		}

		protected override void RefreshChildsRectangle()
		{
			base.RefreshChildsRectangle();

			var innerRect = InnerRectangle;

			var lastX = innerRect.X;
			if (Alignment == ChildAlignment.TopRight || Alignment == ChildAlignment.BottomRight)
				lastX = (int) Transform.GlobalRectangle.Right - Padding.Right;
			var lastY = innerRect.Y;
			if (Alignment == ChildAlignment.BottomLeft || Alignment == ChildAlignment.BottomRight)
				lastY = (int) Transform.GlobalRectangle.Bottom - Padding.Bottom;

			var dir = Point.Zero;
			dir.X = Alignment == ChildAlignment.TopRight || Alignment == ChildAlignment.BottomRight
				? -1
				: 1;
			dir.Y = Alignment == ChildAlignment.BottomLeft || Alignment == ChildAlignment.BottomRight
				? -1
				: 1;

			foreach (var transformChild in Transform.Childs)
			{
				var ownerType = transformChild.Owner.GetType();
				if (ownerType == typeof(UIActor) || ownerType.IsSubclassOf(typeof(UIActor)))
				{
					var rt = (RectTransform) transformChild;
					rt.Anchor = _childAnchor;
					if (AutoRectSize)
						rt.Size = GridSize.ToVector2();

					rt.GlobalLocationByPivot = new Vector2(lastX, lastY);

					if (IsVerticalPriority)
					{
						lastY += dir.Y * GridSize.Y;
						if (dir.Y > 0 && lastY + GridSize.Y > innerRect.Bottom)
						{
							lastY = innerRect.Y;
							lastX += GridSize.X;
						}
						else if (lastY - GridSize.Y < innerRect.Top)
						{
							lastY = innerRect.Y + innerRect.Height;
							lastX += GridSize.X;
						}
					}
					else
					{
						lastX += dir.X * GridSize.X;
						if (dir.X > 0 && lastX + GridSize.X > innerRect.Right)
						{
							lastX = innerRect.X;
							lastY += GridSize.Y;
						}
						else if (lastX - GridSize.X < innerRect.Left)
						{
							lastX = innerRect.X + innerRect.Width;
							lastY += GridSize.Y;
						}
					}
				}
			}
		}
	}
}