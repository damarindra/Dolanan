using System;
using System.Runtime.Serialization;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Component
{
	// TODO Create Frame feature
	public class Sprite : Renderer
	{
		// TODO: Complete setter and getter event, such changing frame will automatically change the sprite image
		public UInt16 Frame
		{
			get => _frame;
			set
			{
				_frame = value;
				Point rectPosition = new Point();
				rectPosition.Y = (int) MathF.Floor(_frame / (Texture2D.Width / FrameSize.X)) * FrameSize.Y; 
				rectPosition.X = (int) MathF.Floor(_frame % (Texture2D.Width / FrameSize.X)) * FrameSize.X;
				SrcLocation = rectPosition;
			}
		}

		private UInt16 _frame = 0;

		public Point FrameSize
		{
			get => SrcSize;
			set => SrcSize = value;
		}

		public Sprite() : base()
		{
			FrameSize = SrcRectangle.Size;
		}
	}
}