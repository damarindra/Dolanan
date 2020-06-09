using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Resources
{
	public class ResFont : ResourceBox<SpriteFont>
	{
		public static ResFont Instance;

		public ResFont() : base()
		{
			if (Instance == null)
				Instance = this;
			else Unload();
		}

		protected override string ContentDirectory => "Fonts/";

		public override ResourceBox<SpriteFont> Load()
		{
			base.Load();
			// TODO : all logic inside here

			TryAddToResource("bitty", ContentManager.Load<SpriteFont>(ContentDirectory + "bitty"));

			return this;
		}

		private void TryAddToResource(string key, SpriteFont font)
		{
			ResourceHolder.Add(key, font);
		}

		public override void Unload(bool safe = true)
		{
			base.Unload(safe);
			Instance = null;
		}
	}
}