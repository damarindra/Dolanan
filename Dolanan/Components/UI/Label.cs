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
		public bool AutoSize = true;

		public SpriteFont Font = null;
		public TextAlign TextAlign = TextAlign.Left;
		public TextVAlign TextVAlign = TextVAlign.Top;

		public Color TintColor = Color.White;

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

			GameMgr.SpriteBatch.DrawString(Font, _text, Transform.GlobalLocation, TintColor);
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