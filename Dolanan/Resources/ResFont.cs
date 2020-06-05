using Dolanan.Controller;
using Microsoft.Xna.Framework.Graphics;

namespace Dolanan.Resources
{
	public class ResFont : ResourceBox<SpriteFont>
	{
		public static ResFont Instance;
		
		protected override string ContentDirectory => "Fonts/";

		public ResFont() : base(true)
		{
			if(Instance == null)
				Instance = this;
			else Unload(true);
		}
		
		public override ResourceBox<SpriteFont> Load()
		{
			base.Load();
			// TODO : all logic inside here

			TryAddToResource("bitty", ContentManager.Load<SpriteFont>(ContentDirectory + "bitty"));

			return this;
		}

		void TryAddToResource(string key, SpriteFont font)
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