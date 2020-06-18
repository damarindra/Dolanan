using System;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components.UI
{
	public delegate void ButtonAction();

	[Flags]
	public enum ButtonStyle
	{
		None = 0,
		Tint = 1,
		Scale = 2,
		SwitchTexture = 4
	}

	public class Button : UIComponent
	{
		public ButtonState State { get; internal set; }

		private Image _image;
		private Color _startColor = Color.White, _targetColor = Color.White;
		private float _time;

		public ButtonStyle ButtonStyle = ButtonStyle.Tint;
		public ColorTint ColorTint = ColorTint.Default;
		public Easing.Functions Easing = Core.Easing.Functions.CubicEaseOut;
		public float EasingTime = .1f;
		public ButtonAction OnPressedDown, OnPressedUp, OnPressed;

		public Button(Actor owner) : base(owner)
		{
		}

		// This can be Image or NineSlice
		public Image Image
		{
			get
			{
				if (_image == null)
					_image = Owner.GetComponent<Image>();
				return _image;
			}
			private set => _image = value;
		}

		public void SetImage(Image image)
		{
			Image = image;
		}

		public override void Start()
		{
			base.Start();
			Interactable = true;

			Owner.OnMouseEnter += () =>
			{
				if (State != ButtonState.Pressed) State = ButtonState.Hovering;
				//TODO Create await / task / async
			};
			Owner.OnMouseExit += () =>
			{
				if (State == ButtonState.Hovering) State = ButtonState.None;
			};

			OnPressedDown += () =>
			{
				State = ButtonState.Pressed;
			};
			OnPressedUp += () =>
			{
				State = ButtonState.None;
			};
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Interactable && Owner.IsMouseInside && Input.IsMouseButtonJustPressed())
				UIInput.RegisterButton(this);


			#region Interaction Effect

			//TODO Interaction still shit. Dunno better implementation for this things, maybe await task? dunno tho
			if (Image != null)
				if (ButtonStyle.HasFlag(ButtonStyle.Tint))
				{
					if (!Interactable)
						Image.TintColor = ColorTint.DisabledColor;
					else
						switch (State)
						{
							default:
							case ButtonState.None:
								if (Math.Abs(_time) > float.Epsilon && _targetColor != ColorTint.NormalColor)
								{
									_targetColor = ColorTint.NormalColor;
									_startColor = Image.TintColor;
									_time = 0;
								}
								else if (Image.TintColor != ColorTint.NormalColor)
								{
									Image.TintColor = MathEx.Interpolate(_startColor, _targetColor, _time / EasingTime);
									_time += (float) gameTime.ElapsedGameTime.TotalSeconds;
								}

								break;
							case ButtonState.Hovering:
								if (Math.Abs(_time) > float.Epsilon && _targetColor != ColorTint.HighlightedColor)
								{
									_targetColor = ColorTint.HighlightedColor;
									_startColor = Image.TintColor;
									_time = 0;
								}
								else if (Image.TintColor != ColorTint.HighlightedColor)
								{
									Image.TintColor = MathEx.Interpolate(_startColor, _targetColor, _time / EasingTime);
									_time += (float) gameTime.ElapsedGameTime.TotalSeconds;
								}

								break;
							case ButtonState.Pressed:
								if (Math.Abs(_time) > float.Epsilon && _targetColor != ColorTint.PressedColor)
								{
									_targetColor = ColorTint.PressedColor;
									_startColor = Image.TintColor;
									_time = 0;
								}
								else if (Image.TintColor != ColorTint.PressedColor)
								{
									Image.TintColor = MathEx.Interpolate(_startColor, _targetColor, _time / EasingTime);
									_time += (float) gameTime.ElapsedGameTime.TotalSeconds;
								}

								break;
						}
				}

			#endregion
		}

		public enum ButtonState
		{
			None,
			Hovering,
			Pressed
		}
	}

	public struct ColorTint
	{
		public Color NormalColor;
		public Color HighlightedColor;
		public Color PressedColor;
		public Color SelectedColor;
		public Color DisabledColor;

		public ColorTint(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor,
			Color disabledColor)
		{
			NormalColor = normalColor;
			HighlightedColor = highlightedColor;
			PressedColor = pressedColor;
			SelectedColor = selectedColor;
			DisabledColor = disabledColor;
		}

		public static ColorTint Default =>
			new ColorTint(Color.White, new Color(244, 244, 244, 255), new Color(200, 200, 200, 255),
				new Color(244, 244, 244, 255), new Color(200, 200, 200, 128));
	}
}