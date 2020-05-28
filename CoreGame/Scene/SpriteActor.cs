using System;
using CoreGame.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Scene
{
	public class SpriteActor : Actor
	{
		public Sprite Sprite;

		public SpriteActor(Texture2D texture2D) : base()
		{
			if (texture2D == null)
			{
				Console.WriteLine("Texture is null! Abort creating sprite");
				return;
			}

			Sprite = AddComponent<Sprite>("SpriteRenderer");
			Sprite.Texture2D = texture2D;
			Sprite.SrcLocation = texture2D.Bounds.Location;
			Sprite.SrcSize = texture2D.Bounds.Size;
			Sprite.Pivot = RendererPivot.Center;
		}

		public SpriteActor(Texture2D texture2D, Vector2 position) : base()
		{
			Sprite = AddComponent<Sprite>("SpriteRenderer");
			Sprite.Texture2D = texture2D;
			Sprite.SrcLocation = texture2D.Bounds.Location;
			Sprite.SrcSize = texture2D.Bounds.Size;
			Sprite.Pivot = RendererPivot.Center;

			transform.Position = position;
			LateInit(null, transform.Position, transform.Rotation, transform.Scale);
		}

	}
}