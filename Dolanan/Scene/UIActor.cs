using System;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Scene
{
	public delegate void UIMouseState();
	
	/// <summary>
	/// UIActor, container for all UIComponent.
	/// Rotation and scale is not supported yet, so don't use it please.
	/// Why not supported yet? Are we actually use it, that was my question.
	/// Rotation actually useful tho, but, the problem is capturing mouse
	/// </summary>
	public class UIActor : Actor
	{
		public RectTransform RectTransform;

		public new RectTransform Transform => RectTransform;
		
		public UIActor UIParent { get; private set; }

		public bool ReceiveMouseInput = true;
		public UIMouseState OnMouseEnter, OnMouseExit;
		internal bool IsMouseInside = false;
		
		private enum MouseState
		{
			Inside, Outside
		}
		
		public UIActor(string name, Layer layer) : base(name, layer)
		{
		}

		public override void Start()
		{
			base.Start();
			base.Transform = RectTransform = base.AddComponent<RectTransform>();

			OnParentChange += parent =>
			{
				RectTransform.RefreshParent();
				if (parent != null)
				{
					if (parent.GetType().IsSubclassOf(typeof(UIActor)))
						UIParent = (UIActor) parent;
				}
			};
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (ReceiveMouseInput)
			{
				Vector2 mouseP = Mouse.GetState().Position.ToVector2()
					.RotateAround(Transform.ScreenLocationByPivot.ToVector2(), Transform.GlobalRotation);
				bool isMouseEntering = Transform.Rectangle.Contains(mouseP);
				if (!IsMouseInside && isMouseEntering)
				{
					OnMouseEnter?.Invoke();
					IsMouseInside = true;
					Console.WriteLine("Enter");
				}
				else if(IsMouseInside && !isMouseEntering)
				{
					OnMouseExit?.Invoke();
					IsMouseInside = false;
					Console.WriteLine("Exit");
				}
			}
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			base.Draw(gameTime, layerZDepth);
			GameMgr.SpriteBatch.DrawStroke(Transform.GlobalRectangle.ToRectangle(), Color.Yellow);

		}

		public new T AddComponent<T>() where T : UIComponent
		{
			return base.AddComponent<T>();
		}
	}
}