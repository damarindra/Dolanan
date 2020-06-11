using System;
using Dolanan.Scene;

namespace Dolanan.Components.UI
{
	public delegate void ButtonAction();
	
	public class Button : UIComponent
	{
		public Button(Actor owner) : base(owner)
		{
		}
		public ButtonAction OnPressedDown, OnPressed, OnPressedUp, OnHover;

		// This can be Image or NineSlice
		public Image Image { get; private set; }

		public void SetImage(Image image)
		{
			Image = image;
		}

		public override void Start()
		{
			base.Start();
			if (Image == null)
			{
				Image = UIActor.GetComponent<Image>();
			}
		}
	}
}