using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public delegate void UIMouseState();
	
	/// <summary>
	/// Rotation and scale will never work on UIActor!
	/// </summary>
	public class UIActor : Actor
	{
		public RectTransform RectTransform;

		public new RectTransform Transform => RectTransform;
		
		public UIActor UIParent { get; private set; }

		public RectangleF RectTransformToScreen => new RectangleF(GlobalLocation, RectTransform.RectSize);

		protected Vector2 ParentLocation
		{
			get
			{
				Vector2 r = Vector2.Zero;
				if (UIParent != null)
					r += UIParent.ParentLocation + UIParent.RectTransform.RectLocation;
				
				return r;
			}
		}

		public UIMouseState OnMouseEnter, OnMouseExit, OnMouseStay;
		
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