using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Resources
{
	public class ResFont : ResourceBox<SpriteFont>
	{
		public static ResFont Instance;

		public ResFont()
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

			TryAddToResource("8px", ContentManager.Load<SpriteFont>(ContentDirectory + "04B_03__8px"));
			TryAddToResource("16px", ContentManager.Load<SpriteFont>(ContentDirectory + "04B_03__16px"));

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