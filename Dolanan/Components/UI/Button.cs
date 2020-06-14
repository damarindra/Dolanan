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
		public ButtonAction OnPressedDown, OnPressedUp, OnPressed;

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
		public Easing.Functions Easing = Core.Easing.Functions.CubicEaseOut;
		public float EasingTime = .1f;

		private Image _image = null;
		private float _time = 0f;

		private enum ButtonState
		{
			None, Hovering, Pressed
		}

		private ButtonState _buttonState;
		private Color _startColor, _targetColor;

		public void SetImage(Image image)
		{
			Image = image;
		}

		public override void Start()
		{
			base.Start();
			Interactable = true;

			UIActor.OnMouseEnter += () =>
			{
				if (_buttonState != ButtonState.Pressed)
				{
					_buttonState = ButtonState.Hovering;
					// _startColor = Image?.TintColor ?? Color.White;
					// _targetColor = ColorTint.PressedColor;
				}
				//TODO Create await / task / async
			};
			UIActor.OnMouseExit += () =>
			{
				if (_buttonState == ButtonState.Hovering)
				{
					_buttonState = ButtonState.None;
					// _startColor = Image?.TintColor ?? Color.White;
					// _targetColor = ColorTint.NormalColor;
				}
			};
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Interactable)
			{
				if (UIActor.IsMouseInside)
				{
					if (Input.IsMouseButtonJustPressed())
					{
						OnPressedDown?.Invoke();
						_buttonState = ButtonState.Pressed;
					}
				}
			}

			if (_buttonState == ButtonState.Pressed)
			{
				if (Input.IsMouseButtonJustUp())
				{
					if (UIActor.IsMouseInside)
					{
						_buttonState = ButtonState.Hovering;
					}
					else
					{
						_buttonState = ButtonState.None;
					}
					OnPressedUp?.Invoke();
				}
				else
				{
					OnPressed?.Invoke();
				}
			}

			#region Interaction Effect
			//TODO Interaction still shit. Dunno better implementation for this things, maybe await task? dunno tho
			if (Image != null)
			{
				if (ButtonStyle.HasFlag(ButtonStyle.Tint))
				{
					if (!Interactable)
					{
						Image.TintColor = ColorTint.DisabledColor;
					}
					else
					{
						switch (_buttonState)
						{
							default:
							case ButtonState.None:
								if (Math.Abs(_time) > Single.Epsilon && _targetColor != ColorTint.NormalColor)
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
								if (Math.Abs(_time) > Single.Epsilon && _targetColor != ColorTint.HighlightedColor)
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
								if (Math.Abs(_time) > Single.Epsilon && _targetColor != ColorTint.PressedColor)
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
				}
			}
			#endregion
		}
	}

	public struct ColorTint
	{
		public Color NormalColor;
		public Color HighlightedColor;
		public Color PressedColor;
		public Color SelectedColor;
		public Color DisabledColor;

		public ColorTint(Color normalColor, Color highlightedColor, Color pressedColor, Color selectedColor, Color disabledColor)
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