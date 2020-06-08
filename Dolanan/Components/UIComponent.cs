using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components
{
	/// <summary>
	/// UIComponent has 2 types of Draw, the basic 'Draw' and the unique 'BackDraw'
	/// 'Draw' will only active when Layer.UISpace == World
	/// 'BackDraw' will only active when Layer.UISpace == Screen
	/// if Layer is not UILayer, so, it will always draw in world space
	/// </summary>
	public class UIComponent : Component
	{
		public UIComponent(Actor owner) : base(owner) { }

		public UIActor UIActor;
		
		public override void Start()
		{
			base.Start();
			UIActor = (UIActor) Owner;
		}

		/// <summary>
		/// Draw right after BackBufferDraw. Only use this when you want to draw right in front of your screen.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="rectRender"></param>
		public virtual void BackDraw(GameTime gameTime, Rectangle rectRender) { }
	}
}