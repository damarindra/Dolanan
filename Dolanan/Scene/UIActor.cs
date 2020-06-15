﻿using System;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		/// <summary>
		/// Clipping all renderer type component (children affected)
		/// </summary>
		public bool Clip = false;

		/// <summary>
		/// If parent Clip is true?
		/// </summary>
		public bool IsGlobalClipped
		{
			get
			{
				bool result = Clip;
				
				
				UIActor p = UIParent;
				while (p != null)
				{
					if (p.Clip)
					{
						result = true;
						break;
					}
					
					p = p.UIParent;
				}

				return result;
			}
		}
		
		
		/// <summary>
		/// Used for clipping any type of renderer (Image, label, etc)
		/// </summary>
		public Rectangle GlobalRectangleClip
		{
			get
			{
				Rectangle result = Clip ? RectTransform.GlobalRectangle.ToRectangle() : GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle;

				UIActor p = UIParent;
				while (p != null)
				{
					if (p.Clip)
					{
						result = Rectangle.Intersect(result, p.RectTransform.Rectangle.ToRectangle());
					}

					p = p.UIParent;
				}
				
				return result;
			}
		}

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
					if (parent.GetType() == typeof(UIActor) || parent.GetType().IsSubclassOf(typeof(UIActor)))
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
				}
				else if(IsMouseInside && !isMouseEntering)
				{
					OnMouseExit?.Invoke();
					IsMouseInside = false;
				}
			}
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			if (IsGlobalClipped)
			{
				GameMgr.EndDraw();
				Rectangle currentRect = GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle;
				GameMgr.BeginDrawAuto(rasterizerState: GameMgr.RazterizerScissor, 
					blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate);
				GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle = GlobalRectangleClip;
				base.Draw(gameTime, layerZDepth);
				GameMgr.EndDraw();
				GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
				GameMgr.BeginDrawAuto();
			}
			else 
				base.Draw(gameTime, layerZDepth);
			
			
			GameMgr.SpriteBatch.DrawStroke(Transform.GlobalRectangle.ToRectangle(), Color.Yellow);

		}

		public new T AddComponent<T>() where T : UIComponent
		{
			return base.AddComponent<T>();
		}

	}
}