using System.Diagnostics;
using CoreGame.Controller;
using CoreGame.Scene;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;

namespace CoreGame.Components
{
	// TODO : AseSprite
	public class AseSprite : Component
	{
		public AnimatedSprite AnimatedSprite { get; set; }

		public override void Start()
		{
			base.Start();
		}

		public override void Update(GameTime gameTime)
		{
			if(AnimatedSprite == null)
				return;

			AnimatedSprite.Position = Owner.Transform.GlobalPosition;
			AnimatedSprite.RenderDefinition.Rotation = Owner.Transform.Rotation;
			AnimatedSprite.RenderDefinition.Scale = Owner.Transform.Scale;
			AnimatedSprite.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, float layerZDepth)
		{
			base.Draw(gameTime, layerZDepth);

			AnimatedSprite.RenderDefinition.LayerDepth = layerZDepth;
			AnimatedSprite.Render(GameMgr.SpriteBatch);
		}

		public AseSprite(Actor owner) : base(owner)
		{
		}
	}
}