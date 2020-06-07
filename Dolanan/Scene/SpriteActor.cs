using System;
using Dolanan.Collision;
using Dolanan.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Scene
{
	// TODO : Remove this
	public class SpriteActor : Actor
	{
		public Sprite Sprite { get; private set; }

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<Sprite>();
		}

		public SpriteActor(string name, Layer layer) : base(name, layer)
		{
		}
	}
}