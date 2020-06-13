using Dolanan.Components;
using Dolanan.Engine;
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

		public override Vector2 Location
		{
			get => RectTransform.OriginLocation;
			set => RectTransform.SetRectLocation( value - RectTransform.OriginLocation);
		}

		public override Vector2 GlobalLocation
		{
			get; 
			set;
		}

		public RectangleF RectTransformToScreen => new RectangleF(ParentLocation + RectTransform.RectLocation, RectTransform.RectSize);

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
			RectTransform = base.AddComponent<RectTransform>();
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

		public new T AddComponent<T>() where T : UIComponent
		{
			return base.AddComponent<T>();
		}
	}
}