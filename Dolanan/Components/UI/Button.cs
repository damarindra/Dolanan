using System;
using Dolanan.Core;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	public delegate void ButtonAction();

	[Flags]
	public enum ButtonStyle
	{
		None = 			0,
		Tint = 			1,
		Scale	=		2,
		SwitchTexture =	4
	}
	
	public class Button : UIComponent
	{
		public Button(Actor owner) : base(owner)
		{
		}
		public ButtonAction OnPressedDown, OnPressed, OnPressedUp, OnHover;

		// This can be Image or NineSlice
		public Image Image
		{
			get
			{
				if(_image == null)
					_image = UIActor.GetComponent<Image>();
				return _image;
			}
			private set => _image = value;
		}

		public ButtonStyle ButtonStyle = ButtonStyle.Tint;
		public ColorTint ColorTint = ColorTint.Default;

		private Image _image = null;

		public void SetImage(Image image)
		{
			Image = image;
		}

		public override void Start()
		{
			base.Start();
		}
	}

	public struct ColorTint
	{
		public Color NormalColor;
		public Color HighlightedColor;
		public Color PressedColor;
		public Color SelectedColor;
		public Color DisabledColor;
		public Easing.Functions Easing;

		public ColorTint(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor, Color disabledColor, Easing.Functions easing)
		{
			NormalColor = normalColor;
			HighlightedColor = highlightedColor;
			PressedColor = pressedColor;
			SelectedColor = selectedColor;
			DisabledColor = disabledColor;
			Easing = easing;
		}

		public static ColorTint Default =>
			new ColorTint(Color.White, new Color(244, 244, 244, 255), new Color(200, 200, 200, 255),
				new Color(244, 244, 244, 255), new Color(200, 200, 200, 128), Core.Easing.Functions.CubicEaseInOut);
	}
}