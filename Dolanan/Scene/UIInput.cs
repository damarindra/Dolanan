using System.Collections.Generic;
using System.Linq;
using Dolanan.Components.UI;
using Dolanan.Engine;

namespace Dolanan.Scene
{
	/// <summary>
	/// 	Input for All UIButton.
	/// </summary>
	public static class UIInput
	{
		/// <summary>
		/// 	All Button (UIButton Component) that interacted by mouse will be registered here first, sorted by depth backwards,
		/// 	First index is bigger, last index is lower
		/// </summary>
		internal static List<Button> UIInteractedByMouseButtonJustPressed = new List<Button>();

		public static Button CurrentUIButtonPressed
		{
			get;
			internal set;
		}

		public static void Reset()
		{
			UIInteractedByMouseButtonJustPressed.Clear();
		}

		public static void RegisterButton(Button btn)
		{
			if(!UIInteractedByMouseButtonJustPressed.Contains(btn))
				UIInteractedByMouseButtonJustPressed.Add(btn);
		}

		internal static void HandleInput()
		{
			// Process the UIInput
			if (CurrentUIButtonPressed != null)
			{
				if (CurrentUIButtonPressed.State == Button.ButtonState.Pressed)
				{
					if (Input.IsMouseButtonJustUp())
					{
						CurrentUIButtonPressed.OnPressedUp?.Invoke();
						CurrentUIButtonPressed = null;
					}
					else
					{
						CurrentUIButtonPressed.OnPressed?.Invoke();
					}
				}
				return;
			}
			
			if (UIInteractedByMouseButtonJustPressed.Count > 0)
			{
				UIInteractedByMouseButtonJustPressed = UIInput.UIInteractedByMouseButtonJustPressed.OrderByDescending(ui => ui.Owner.ZDepth).ToList();
				CurrentUIButtonPressed = UIInput.UIInteractedByMouseButtonJustPressed[0];
				CurrentUIButtonPressed.OnPressedDown?.Invoke();
				CurrentUIButtonPressed.State = Button.ButtonState.Pressed;
			}
		}
	}
}