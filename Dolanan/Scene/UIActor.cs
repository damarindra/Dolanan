using Dolanan.Components;
using Dolanan.Engine;
using Microsoft.Xna.Framework;

namespace Dolanan.Scene
{
	public class UIActor : Actor
	{
		public UIActor(string name, Layer layer) : base(name, layer) { }

		public RectTransform RectTransform;

		public override void Start()
		{
			base.Start();
			RectTransform = base.AddComponent<RectTransform>();
			OnParentChange += parent =>
			{
				RectTransform.RefreshParent();
			};
		}

		public new T AddComponent<T>() where T : UIComponent
		{
			return base.AddComponent<T>();
		}
	}
}