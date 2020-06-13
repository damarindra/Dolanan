using Dolanan.Core;
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

		public UIComponent(Actor owner) : base(owner)
		{
		}

		public new RectTransform Transform
		{
			get
			{
				if (UIActor == null)
					return null;
				return UIActor.RectTransform;
			}
		}

		public override void Start()
		{
			base.Start();
			UIActor = (UIActor) Owner;
		}

		public bool Interactable
		{
			get
			{
				return _interactable;
			}
			set
			{
				_interactable = value;
			}
		}

		private bool _interactable;
	}
}