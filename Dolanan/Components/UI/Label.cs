using Dolanan.Controller;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	// TODO, can be used for any UI Clip https://gamedev.stackexchange.com/questions/24697/how-to-clip-cut-off-text-in-a-textbox
	public class Label : UIComponent
	{
		public Label(Actor owner) : base(owner)
		{
		}

		public SpriteFont Font = null;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				if (AutoSize && Font != null)
				{
					Vector2 size = Font.MeasureString(_text);
					Owner.Transform.SetRectSize(size);
				}
			}
		}
		public TextAlign TextAlign = TextAlign.Left;
		public TextVAlign TextVAlign = TextVAlign.Top;
		/// <summary>
		/// Automatically resize whenever text is out of RectTransform
		/// </summary>
		public bool AutoSize = true;

		public Color TintColor = Color.White;

		private string _text = "";
		private RasterizerState _rasterizer = new RasterizerState() { ScissorTestEnable = true };

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			if(Font == null)
				return;
			GameMgr.EndDraw();
			
			Rectangle currentRect = GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle;
			GameMgr.BeginDrawAuto(rasterizerState: _rasterizer, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate);

			GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle = Transform.GlobalRectangle.ToRectangle();
			GameMgr.SpriteBatch.DrawString(Font, _text, Transform.GlobalLocation, TintColor);

			GameMgr.EndDraw();
			GameMgr.SpriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
			
			GameMgr.BeginDrawAuto();
		}
	}

	public enum TextAlign
	{
		Left, Center, Right
	}
	public enum TextVAlign
	{
		Top, Middle, Bottom
	}
}