﻿﻿using System.Collections.Generic;
using Dolanan.Controller;
using Dolanan.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Components.UI
{
	// TODO, can be used for any UI Clip https://gamedev.stackexchange.com/questions/24697/how-to-clip-cut-off-text-in-a-textbox
	public class Label : UIComponent
	{
		public SpriteFont Font = null;
		public TextAlign TextAlign = TextAlign.Left;
		public TextVAlign TextVAlign = TextVAlign.Top;

		public Color TintColor = Color.White;
		public bool WordWrap = true;

		public Label(Actor owner) : base(owner)
		{
		}

		public string Text { get; set; } = "";

		private Vector2 TextLocation(string str, Vector2? size = null)
		{
			var result = Transform.GlobalLocation;
			if (size == null)
				size = Font.MeasureString(str);
			if (TextAlign == TextAlign.Right)
				result.X += Transform.RightRect - size.Value.X;
			else if (TextAlign == TextAlign.Center)
				result.X += Transform.Size.X / 2f - size.Value.X / 2f;

			if (TextVAlign == TextVAlign.Bottom)
				result.Y += Transform.BottomRect - size.Value.Y;
			else if (TextVAlign == TextVAlign.Middle)
				result.Y += Transform.Size.Y / 2f - size.Value.Y / 2f;

			return result;
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);
			if (Font == null)
				return;

			if (!WordWrap)
			{
				GameMgr.SpriteBatch.DrawString(Font,
					Text,
					TextLocation(Text),
					TintColor,
					Transform.GlobalRotation,
					Vector2.Zero,
					Owner.Transform.GlobalScaleRendering,
					SpriteEffects.None,
					layerZDepth);
			}
			else
			{
				var textArray = ParseText(Text);
				var textLocations = new Vector2[textArray.Length];
				float totalYOffset = 0;

				for (var i = 0; i < textArray.Length; i++)
				{
					textLocations[i] = Font.MeasureString(textArray[i]);
					totalYOffset += textLocations[i].Y;
				}

				if (TextVAlign == TextVAlign.Top)
					totalYOffset = 0;
				else if (TextVAlign == TextVAlign.Middle)
					totalYOffset = (totalYOffset - textLocations[0].Y) / 2f;
				else
					totalYOffset -= textLocations[0].Y;

				float yOffset = 0;
				for (var i = 0; i < textArray.Length; i++)
				{
					GameMgr.SpriteBatch.DrawString(Font,
						textArray[i],
						TextLocation(textArray[i], textLocations[i]) +
						new Vector2(0, -totalYOffset + yOffset),
						TintColor,
						Transform.GlobalRotation,
						Vector2.Zero,
						Owner.Transform.GlobalScaleRendering,
						SpriteEffects.None,
						layerZDepth);
					yOffset += textLocations[i].Y * Owner.Transform.GlobalScaleRendering.Y;
				}
			}
		}


		private string[] ParseText(string text)
		{
			if (Font == null)
				return new[] {text};
			var result = new List<string>();
			var line = string.Empty;
			var wordArray = text.Split(' ');

			foreach (var word in wordArray)
			{
				if (Font.MeasureString(line + word).Length() > Transform.Size.X)
				{
					line = line.Remove(line.Length - 1);
					result.Add(line);
					line = string.Empty;
				}

				line = line + word + ' ';
			}

			if (line.Length > 0)
				line = line.Remove(line.Length - 1);
			result.Add(line);

			return result.ToArray();
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