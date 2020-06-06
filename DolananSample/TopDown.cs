using Dolanan;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Scene;
using Dolanan.Scene.Object;
using Dolanan.Tools;
using Dolanan.Tools.GameHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolananSample
{
	public class TopDown : GameMin
	{
		private Player p;
		private ResTexturesTopDown _texturesTopDown;

		public TopDown() : base()
		{
			Log.Print("Init");
		}
		protected override void Initialize()
		{
			base.Initialize();
			Log.Print("Init");
			p = World.CreateActor<Player>("Player");
			p.Transform.GlobalPosition = new Vector2(GameSettings.ViewportSize.X / 2f, GameSettings.ViewportSize.Y / 2f);
			Log.Print(p.Transform.GlobalPosition.ToString());

			World.Camera.FollowActor = p;

			if (_texturesTopDown.TryGet("square", out var texture2D))
			{
				Actor c = World.CreateActor<SpriteActor>("Spr");
				c.GetComponent<Sprite>().Texture2D = texture2D;
				c.SetParent(p);
				c.Transform.Position = Vector2.One * 100;
				
				Actor d = World.CreateActor<SpriteActor>("Spr2");
				d.GetComponent<Sprite>().Texture2D = texture2D;
				d.SetParent(c);
				d.Transform.Position = Vector2.UnitX * 50;
				
				Actor wall = CollisionHelper.CreateColliderActor(new Vector2(0, GameSettings.ViewportSize.Y / 2f), new Vector2(64, GameSettings.ViewportSize.Y));
				wall.Transform.LocalScale = new Vector2(1, GameSettings.ViewportSize.Y / 64f);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Body>().Origin = wall.GetComponent<Renderer>().Origin;

				wall = CollisionHelper.CreateColliderActor(new Vector2( GameSettings.ViewportSize.X / 2f, 0), new Vector2(GameSettings.ViewportSize.X, 64));
				wall.Transform.LocalScale = new Vector2(GameSettings.ViewportSize.X / 64f, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Body>().Origin = wall.GetComponent<Renderer>().Origin;
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(GameSettings.ViewportSize.X , GameSettings.ViewportSize.Y / 2f), new Vector2(64, GameSettings.ViewportSize.Y));
				wall.Transform.LocalScale = new Vector2(1, GameSettings.ViewportSize.Y / 64f);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Body>().Origin = wall.GetComponent<Renderer>().Origin;
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(GameSettings.ViewportSize.X / 2f, GameSettings.ViewportSize.Y), new Vector2(GameSettings.ViewportSize.X, 64));
				wall.Transform.LocalScale = new Vector2(GameSettings.ViewportSize.X / 64f, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Body>().Origin = wall.GetComponent<Renderer>().Origin;
				Log.Print(wall.GetComponent<Body>().Origin.ToString());
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			_texturesTopDown = (ResTexturesTopDown)new ResTexturesTopDown().Load();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		protected override void Process(GameTime gameTime)
		{
			base.Process(gameTime);
		}
	}

	public class ResTexturesTopDown : ResourceBox<Texture2D>
	{
		protected override string ContentDirectory => "Graphics/";

		public override ResourceBox<Texture2D> Load()
		{
			ResourceHolder.Add("square", ContentManager.Load<Texture2D>(ContentDirectory + "square_64x64"));

			return this;
		}
	}
}