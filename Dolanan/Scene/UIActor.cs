using Dolanan.Engine;

namespace Dolanan.Scene
{
	// TODO : SetParent need to Update the Rectangle
	public class UIActor : Actor
	{
		public UIActor(string name, Layer layer) : base(name, layer)
		{
		}

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
	}
}