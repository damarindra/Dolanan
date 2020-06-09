using Dolanan.Components;

namespace Dolanan.Scene
{
	// TODO : Remove this
	public class SpriteActor : Actor
	{
		public SpriteActor(string name, Layer layer) : base(name, layer)
		{
		}

		public Sprite Sprite { get; private set; }

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<Sprite>();
		}
	}
}