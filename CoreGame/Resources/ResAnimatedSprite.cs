using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;

namespace CoreGame.Resources
{
	public sealed class ResAnimatedSprite : ResourceBox<AnimatedSprite>
	{
		public static ResAnimatedSprite Instance;
		
		protected override string ContentDirectory => "Graphics/Aseprites/";

		public ResAnimatedSprite() : base(true)
		{
			if(Instance == null)
				Instance = this;
			else Unload(true);
		}
		
		public override ResourceBox<AnimatedSprite> Load()
		{
			base.Load();
			// TODO : all logic inside here

			Texture2D texture2D = ContentManager.Load<Texture2D>(ContentDirectory+"player");
			AnimationDefinition animationDefinition = ContentManager.Load<AnimationDefinition>(ContentDirectory+"player_ase");
			
			TryAddToResource("player", texture2D, animationDefinition);

			return this;
		}

		void TryAddToResource(string key, Texture2D texture, AnimationDefinition animationDefinition)
		{
			if(texture != null && animationDefinition != null)
				ResourceHolder.Add(key, new AnimatedSprite(texture, animationDefinition));
		}

		public override void Unload(bool safe = true)
		{
			base.Unload(safe);
			Instance = null;
		}
	}
}
