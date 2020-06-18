using Dolanan.Engine;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	/// <summary>
	/// 	ScrollContainer will automatically create Scroll Bar vertical and horizontal.
	/// 	ScrollContainer will not automatically layouting for you. Please use VContainer / HContainer / GridContainer
	/// 	as child
	/// </summary>
	public class ScrollContainer : UIComponent
	{
		public ScrollContainer(Actor owner) : base(owner)
		{
		}

		public ScrollBar ScrollBackgroundTexture;
		public ScrollBar ScrollHandleTexture;

		/// <summary>
		/// 	SrcActor is the source of the scroll container. If SrcActor Rectangle is bigger than ScrollContainer,
		/// 	it will automatically create ScrollBar
		/// </summary>
		public UIActor SrcActor
		{
			get
			{
				if (_srcActor == null && Transform.GetChilds.Length > 0)
					_srcActor = Transform.FirstChildUI;
				return _srcActor;
			}
			set
			{
				if (value.UIParent != Owner)
				{
					Log.PrintWarning(Owner.Name + " : Trying to set SrcActor, but not a children of this Actor. SrcActor must be a child of this Actor");
					return;
				}

				_srcActor = value;
			}
		}

		private UIActor _srcActor = null;

		public override void Start()
		{
			base.Start();
			Owner.Clip = true;
			
			
		}
	}
}