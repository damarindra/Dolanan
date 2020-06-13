using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Engine
{
	public class Input
	{
		public static KeyboardState LastFrameKeyboardState;
		public static GamePadState LastFrameGamePadState;
		public static MouseState LastFrameMouseState;
		private static readonly Dictionary<string, InputAction> _inputActions = new Dictionary<string, InputAction>();
		private static readonly Dictionary<string, InputAxis> _inputAxises = new Dictionary<string, InputAxis>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">0 : left | 1 : right | 2 middle | 3 XButton1 | 4 XButton2</param>
		/// <returns></returns>
		public static bool IsMouseButtonJustPressed(int id = 0)
		{
			if (id == 0)
			{
				return LastFrameMouseState.LeftButton != ButtonState.Pressed &&
				       Mouse.GetState().LeftButton == ButtonState.Pressed;
			}else if (id == 1)
			{
				return LastFrameMouseState.RightButton != ButtonState.Pressed &&
				       Mouse.GetState().RightButton == ButtonState.Pressed;
			}else if (id == 2)
			{
				return LastFrameMouseState.MiddleButton != ButtonState.Pressed &&
				       Mouse.GetState().MiddleButton == ButtonState.Pressed;
			}else if (id == 3)
			{
				return LastFrameMouseState.XButton1 != ButtonState.Pressed &&
				       Mouse.GetState().XButton1 == ButtonState.Pressed;
			}else if (id == 4)
			{
				return LastFrameMouseState.XButton2 != ButtonState.Pressed &&
				       Mouse.GetState().XButton2 == ButtonState.Pressed;
			}

			return false;
		}
		public static bool IsMouseButtonJustUp(int id = 0)
		{
			if (id == 0)
			{
				return LastFrameMouseState.LeftButton == ButtonState.Pressed &&
				       Mouse.GetState().LeftButton != ButtonState.Pressed;
			}else if (id == 1)
			{
				return LastFrameMouseState.RightButton == ButtonState.Pressed &&
				       Mouse.GetState().RightButton != ButtonState.Pressed;
			}else if (id == 2)
			{
				return LastFrameMouseState.MiddleButton == ButtonState.Pressed &&
				       Mouse.GetState().MiddleButton != ButtonState.Pressed;
			}else if (id == 3)
			{
				return LastFrameMouseState.XButton1 == ButtonState.Pressed &&
				       Mouse.GetState().XButton1 != ButtonState.Pressed;
			}else if (id == 4)
			{
				return LastFrameMouseState.XButton2 == ButtonState.Pressed &&
				       Mouse.GetState().XButton2 != ButtonState.Pressed;
			}

			return false;
		}

		public static bool IsInputActionJustPressed(string action)
		{
			InputAction inputAction;
			if (_inputActions.TryGetValue(action, out inputAction))
				return inputAction.IsPressed() && !inputAction.IsPressed(LastFrameKeyboardState, LastFrameGamePadState);
			return false;
		}

		public static bool IsInputActionJustUp(string action)
		{
			InputAction inputAction;
			if (_inputActions.TryGetValue(action, out inputAction))
				return inputAction.IsUp() && !inputAction.IsUp(LastFrameKeyboardState, LastFrameGamePadState);
			return false;
		}

		public static bool IsInputActionPressed(string action)
		{
			InputAction inputAction;
			if (_inputActions.TryGetValue(action, out inputAction)) return inputAction.IsPressed();

			return false;
		}

		public static float GetAxis(string axis)
		{
			InputAxis inputAxis;
			if (_inputAxises.TryGetValue(axis, out inputAxis)) return inputAxis.Value();

			return 0;
		}

		public static void AddInputAction(string action, InputAction inputAction)
		{
			if (_inputActions.ContainsKey(action))
			{
				Console.WriteLine("Input Action with key : '" + action + "' already exist");
				return;
			}

			_inputActions.Add(action, inputAction);
		}

		public static void AddInputAxis(string axis, InputAxis inputAxis)
		{
			if (_inputAxises.ContainsKey(axis))
			{
				Console.WriteLine("Input Axis with key : '" + axis + "' already exist");
				return;
			}

			_inputAxises.Add(axis, inputAxis);
		}
	}

	/// <summary>
	///     Input Action
	/// </summary>
	public struct InputAction
	{
		public Keys Key, AltKey;
		public Buttons? Button, AltButton;
		private readonly int _controllerIndex;

		public InputAction(Keys? key = Keys.None,
			Keys? altKey = Keys.None,
			Buttons? btn = null,
			Buttons? altBtn = null,
			int controllerIndex = 0)
		{
			Key = key ?? Keys.None;
			AltKey = altKey ?? Keys.None;
			Button = btn;
			AltButton = altBtn;
			_controllerIndex = controllerIndex;
		}

		/// <summary>
		///     Get Key and GamePad key down status
		/// </summary>
		/// <param name="keyboardState">Leave it null to get the current state</param>
		/// <param name="gamePadState">Leave it null to get the current state</param>
		/// <returns></returns>
		public bool IsPressed(KeyboardState? keyboardState = null, GamePadState? gamePadState = null)
		{
			return IsKeyDown(keyboardState) || IsButtonDown(gamePadState);
		}

		/// <summary>
		///     Get Key and GamePad key up status
		/// </summary>
		/// <param name="keyboardState">Leave it null to get the current state</param>
		/// <param name="gamePadState">Leave it null to get the current state</param>
		/// <returns></returns>
		public bool IsUp(KeyboardState? keyboardState = null, GamePadState? gamePadState = null)
		{
			return IsKeyUp(keyboardState) || IsButtonUp(gamePadState);
		}

		private bool IsKeyDown(KeyboardState? ks = null)
		{
			if (!ks.HasValue) ks = Keyboard.GetState();
			return ks.Value.IsKeyDown(Key) || ks.Value.IsKeyDown(AltKey);
		}

		private bool IsKeyUp(KeyboardState? ks = null)
		{
			if (!ks.HasValue) ks = Keyboard.GetState();
			return ks.Value.IsKeyUp(Key) || ks.Value.IsKeyUp(AltKey);
		}

		private bool IsButtonDown(GamePadState? gps = null)
		{
			if (!gps.HasValue) gps = GamePad.GetState(_controllerIndex);
			var result = false;
			if (Button.HasValue)
				result = gps.Value.IsButtonDown((Buttons) Button);
			if (AltButton.HasValue)
				result = result || gps.Value.IsButtonDown((Buttons) AltButton);

			return result;
		}

		private bool IsButtonUp(GamePadState? gps = null)
		{
			if (!gps.HasValue) gps = GamePad.GetState(_controllerIndex);
			var result = false;
			if (Button.HasValue)
				result = gps.Value.IsButtonUp((Buttons) Button);
			if (AltButton.HasValue)
				result = result || gps.Value.IsButtonUp((Buttons) AltButton);

			return result;
		}
	}

	public struct InputAxis
	{
		public Keys PositiveKey, NegativeKey;
		public Buttons? PositiveButton, NegativeButton;
		public GamePadThumbStickDetail ThumbStick;

		private readonly int _controllerIndex;

		public InputAxis(Keys? positiveKey = Keys.None,
			Keys? negativeKey = Keys.None,
			Buttons? positiveButton = null,
			Buttons? negativeButton = null,
			GamePadThumbStickDetail? thumbStick = GamePadThumbStickDetail.None,
			int controllerIndex = 0)
		{
			PositiveKey = positiveKey ?? Keys.None;
			NegativeKey = negativeKey ?? Keys.None;

			PositiveButton = positiveButton;
			NegativeButton = negativeButton;

			ThumbStick = thumbStick ?? GamePadThumbStickDetail.None;

			_controllerIndex = controllerIndex;
		}

		public float Value()
		{
			float result = 0;
			var ks = Keyboard.GetState();
			if (ks.IsKeyDown(PositiveKey)) result += 1;
			if (ks.IsKeyDown(NegativeKey)) result -= 1;

			var gps = GamePad.GetState(_controllerIndex);
			if (PositiveButton.HasValue)
				if (gps.IsButtonDown((Buttons) PositiveButton))
					result += 1;
			if (NegativeButton.HasValue)
				if (gps.IsButtonDown((Buttons) NegativeButton))
					result -= 1;

			if (ThumbStick == GamePadThumbStickDetail.LeftHorizontal) result += gps.ThumbSticks.Left.X;
			else if (ThumbStick == GamePadThumbStickDetail.LeftVertical) result += gps.ThumbSticks.Left.Y;
			else if (ThumbStick == GamePadThumbStickDetail.RightHorizontal) result += gps.ThumbSticks.Right.X;
			else if (ThumbStick == GamePadThumbStickDetail.RightVertical) result += gps.ThumbSticks.Right.Y;

			return MathF.Sign(result);
		}
	}

	public enum GamePadThumbStickDetail
	{
		None,
		LeftHorizontal,
		LeftVertical,
		RightHorizontal,
		RightVertical
	}
}