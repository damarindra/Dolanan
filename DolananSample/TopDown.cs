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
			p = World.CreateActor<Player>("Player");
			p.Transform.GlobalPosition = new Vector2(GameSettings.ViewportSize.X / 2f, GameSettings.ViewportSize.Y / 2f);
			Log.Print(p.Transform.GlobalPosition.ToString());

			World.Camera.FollowActor = p;

			if (_texturesTopDown.TryGet("square", out var texture2D))
			{
				Actor wall = CollisionHelper.CreateColliderActor(new Vector2(0, GameSettings.ViewportSize.Y / 2f), new Vector2(64, GameSettings.ViewportSize.Y));
				wall.Transform.LocalScale = new Vector2(1, GameSettings.ViewportSize.Y / 64f);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Body>().Offset = wall.GetComponent<Renderer>().Origin + new Vector2(0, (GameSettings.ViewportSize.Y / 2f) - 32f);

				wall = CollisionHelper.CreateColliderActor(new Vector2( GameSettings.ViewportSize.X / 2f, 0), new Vector2(GameSettings.ViewportSize.X, 64));
				wall.Transform.LocalScale = new Vector2(GameSettings.ViewportSize.X / 64f, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Body>().Offset = wall.GetComponent<Renderer>().Origin + new Vector2((GameSettings.ViewportSize.X / 2f) - 32f, 0);
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(GameSettings.ViewportSize.X , GameSettings.ViewportSize.Y / 2f), new Vector2(64, GameSettings.ViewportSize.Y));
				wall.Transform.LocalScale = new Vector2(1, GameSettings.ViewportSize.Y / 64f);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Body>().Offset = wall.GetComponent<Renderer>().Origin + new Vector2(0, (GameSettings.ViewportSize.Y / 2f) - 32f);
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(GameSettings.ViewportSize.X / 2f, GameSettings.ViewportSize.Y), new Vector2(GameSettings.ViewportSize.X, 64));
				wall.Transform.LocalScale = new Vector2(GameSettings.ViewportSize.X / 64f, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Body>().Offset = wall.GetComponent<Renderer>().Origin + new Vector2((GameSettings.ViewportSize.X / 2f) - 32f, 0);
				
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(64, 64), new Vector2(64, 64));
				wall.Transform.LocalScale = new Vector2(1, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;
				
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(254, 129), new Vector2(64, 64));
				wall.Transform.LocalScale = new Vector2(1, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(154, 229), new Vector2(64, 64));
				wall.Transform.LocalScale = new Vector2(1, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;
				
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(457, 365), Vector2.One * 64 * 2);
				wall.Transform.LocalScale = new Vector2(2, 2);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(763, 456), new Vector2(64, 64));
				wall.Transform.LocalScale = new Vector2(1, 1);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;
				
				
				wall = CollisionHelper.CreateColliderActor(new Vector2(765, 180), Vector2.One * 64 * 3);
				wall.Transform.LocalScale = new Vector2(3, 3);
				wall.AddComponent<Renderer>().Texture2D = texture2D;
				wall.GetComponent<Renderer>().ModulatedColor = Color.Red;
				wall.GetComponent<Renderer>().Pivot = Pivot.TopLeft;

				SpriteActor trigger = World.CreateActor<SpriteActor>("trigger");
				trigger.Transform.Position = new Vector2( 128, 400);
				Body body = trigger.AddComponent<Body>();
				body.IsTrigger = true;
				body.Tag = "trigger";
				body.Size = Vector2.One * 128;
				Sprite spr = trigger.GetComponent<Sprite>();
				spr.Pivot = Pivot.TopLeft;
				spr.Texture2D = texture2D;
				trigger.Transform.LocalScale *= 2;
				spr.ModulatedColor = new Color(Color.Aqua, .3f);
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