using Dolanan.Components;
using Dolanan.Engine;

namespace Dolanan.Scene
{
	public class UIActor : Actor
	{
		public RectTransform RectTransform;

		public UIActor(string name, Layer layer) : base(name, layer)
		{
		}

		public override void Start()
		{
			base.Start();
			RectTransform = base.AddComponent<RectTransform>();
			OnParentChange += parent => { RectTransform.RefreshParent(); };
		}

		public new T AddComponent<T>() where T : UIComponent
		{
			return base.AddComponent<T>();
		}
	}
}