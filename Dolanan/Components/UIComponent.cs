using Dolanan.Engine;
using Dolanan.Scene;

namespace Dolanan.Components
{
	/// <summary>
	///     UIComponent has 2 types of Draw, the basic 'Draw' and the unique 'BackDraw'
	///     'Draw' will only active when Layer.UISpace == World
	///     'BackDraw' will only active when Layer.UISpace == Screen
	///     if Layer is not UILayer, so, it will always draw in world space
	/// </summary>
	public class UIComponent : Component
	{
		public UIActor UIActor;

		public new RectTransform Transform
		{
			get
			{
				if (UIActor == null)
					return null;
				return UIActor.RectTransform;
			}
		}

		public UIComponent(Actor owner) : base(owner)
		{
		}

		public override void Start()
		{
			base.Start();
			UIActor = (UIActor) Owner;
		}
	}
}