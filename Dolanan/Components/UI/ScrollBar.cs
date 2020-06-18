using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	public class ScrollBar : UIComponent
	{
		public ScrollBar(Actor owner) : base(owner)
		{
		}
		
		public Texture2D Texture;
		/// <summary>
		/// 	Scroll SrcRectangle
		/// </summary>
		public Rectangle SrcRectangle;
		/// <summary>
		/// 	Nine slice inner rectangle
		/// </summary>
		public Rectangle SrcInnerRectangle;
		
		public float Value
		{
			get => _value;
			set => _value = MathEx.Clamp(value, 0f, 1f);
		}
		private float _value;

		public override void Start()
		{
			base.Start();
			Owner.Interactable = true;
		}
	}
}