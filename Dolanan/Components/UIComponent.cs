using Dolanan.Scene;

namespace Dolanan.Components
{
	public class UIComponent : Component
	{
		public UIComponent(Actor owner) : base(owner) { }

		public UIActor UIActor;
		
		public override void Start()
		{
			base.Start();
			UIActor = (UIActor) Owner;
		}
	}
}