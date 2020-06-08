using Dolanan.Components;
using Dolanan.Engine;

namespace Dolanan.Scene
{
	public class UIActor : Actor
	{
		public UIActor(string name, Layer layer) : base(name, layer) { }

		public RectTransform RectTransform;

		public override void Start()
		{
			base.Start();
			RectTransform = AddComponent<RectTransform>();
			OnParentChange += parent =>
			{
				RectTransform.RefreshParent();
			};
		}

		public T AddUIComponent<T>() where T : UIComponent
		{
			return AddComponent<T>();
		}
	}
}