using Dolanan.Core;
using Dolanan.Scene;

namespace Dolanan.Components
{
	/// <summary>
	///     UIComponent has 2 types of Draw, the basic 'Draw' and the unique 'BackDraw'
	///     'Draw' will only active when Layer.UISpace == World
	///     'BackDraw' will only active when Layer.UISpace == Screen
	///     if Layer is not UILayer, so, it will always draw in world space
	/// </summary>
	public class UIComponent : Component
	{
		public new UIActor Owner;

		public UIComponent(Actor owner) : base(owner)
		{
		}

		public new RectTransform Transform
		{
			get
			{
				if (Owner == null)
					return null;
				return Owner.RectTransform;
			}
		}

		public bool Interactable { get; set; }

		public override void Start()
		{
			base.Start();
			Owner = (UIActor) base.Owner;
		}
	}

	public struct Padding
	{
		public int Left;
		public int Right;
		public int Top;
		public int Bottom;

		public Padding(int left, int right, int top, int bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public Padding(int val)
		{
			Left = val;
			Right = val;
			Top = val;
			Bottom = val;
		}

		public Padding(int horizontal, int vertical)
		{
			Left = horizontal;
			Right = horizontal;
			Top = vertical;
			Bottom = vertical;
		}

		public static Padding Zero => new Padding(0);
	}
}