using System;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	// TODO, can be used for any UI Clip https://gamedev.stackexchange.com/questions/24697/how-to-clip-cut-off-text-in-a-textbox
	public class Label : UIComponent
	{
		private string _text = "";

		/// <summary>
		///     Automatically resize whenever text is out of RectTransform
		/// </summary>
		[Obsolete]
		public bool AutoSize = true;

		public SpriteFont Font = null;
		public TextAlign TextAlign = TextAlign.Left;
		public TextVAlign TextVAlign = TextVAlign.Top;

		public Color TintColor = Color.White;

		private Vector2 TextLocation
		{
			get
			{
				Vector2 result = Transform.GlobalLocation;
				var size = Font.MeasureString(_text);
				if (TextAlign == TextAlign.Right)
					result.X += Transform.RightRect - size.X;
				else if (TextAlign == TextAlign.Center)
					result.X += Transform.RectSize.X / 2f - size.X / 2f;

				if (TextVAlign == TextVAlign.Bottom)
					result.Y += Transform.BottomRect - size.Y;
				else if (TextVAlign == TextVAlign.Middle)
					result.Y += Transform.RectSize.Y / 2f - size.Y / 2f;

				return result;
			}
		}

		public Label(Actor owner) : base(owner)
		{
		}

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				if (AutoSize && Font != null)
				{
					var size = Font.MeasureString(_text);
					Owner.Transform.SetRectSize(size);
				}
			}
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			if (Font == null)
				return;

			GameMgr.SpriteBatch.DrawString(Font, _text, TextLocation, TintColor);
		}
	}

	public enum TextAlign
	{
		Left,
		Center,
		Right
	}

	public enum TextVAlign
	{
		Top,
		Middle,
		Bottom
	}
}