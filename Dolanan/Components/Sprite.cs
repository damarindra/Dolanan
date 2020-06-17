using System;
using Dolanan.Scene;
using Microsoft.Xna.Framework;

namespace Dolanan.Components
{
	public class Sprite : Renderer
	{
		private int _frame;

		public Sprite(Actor owner) : base(owner)
		{
		}

		// TODO: Complete setter and getter event, such changing frame will automatically change the sprite image
		public int Frame
		{
			get => _frame;
			set
			{
				_frame = value;
				var rectPosition = new Point();
				rectPosition.Y = (int) MathF.Floor(_frame / (Texture2D.Width / FrameSize.X)) * FrameSize.Y;
				rectPosition.X = (int) MathF.Floor(_frame % (Texture2D.Width / FrameSize.X)) * FrameSize.X;
				SrcLocation = rectPosition;
			}
		}

		public Point FrameSize
		{
			get => SrcSize;
			set => SrcSize = value;
		}

		public override void Start()
		{
			base.Start();
			FrameSize = SrcRectangle.Size;
		}
	}
}