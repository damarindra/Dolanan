using System;
using CoreGame.Collision;
using CoreGame.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreGame.Scene
{
	// TODO : Remove this
	public class SpriteActor : Actor
	{
		public Sprite Sprite { get; private set; }
		private Body _body;

		public SpriteActor(string name, Layer layer) : base(name, layer)
		{
			Sprite = AddComponent<Sprite>();
			_body = layer.GameWorld.Create(Transform, 64, 64, new Vector2(32, 32));
		}
	}
}