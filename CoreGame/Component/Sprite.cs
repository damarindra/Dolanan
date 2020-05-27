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
			set => _frame = value;
		}


		private UInt16 _frame = 0;
		private Point _frameSize;

		public Sprite() : base()
		{
			_frameSize = SrcRectangle.Size;
		}
	}
}