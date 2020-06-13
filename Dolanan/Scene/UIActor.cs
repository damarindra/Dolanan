using Dolanan.Components;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UIActor : Actor
	{
		public RectTransform RectTransform;

		public new RectTransform Transform => RectTransform;

		public new UILayer Layer => (UILayer) base.Layer;

		public UIActor UIParent { get; private set; }
		
		// public Rectangle RectTransformToScreen => 

		protected Vector2 ParentLocation
		{
			get
			{
				Vector2 r = Vector2.Zero;
				if (UIParent != null)
					r += UIParent.ParentLocation + UIParent.RectTransform.Location;
				
				return r;
			}
		}
		
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