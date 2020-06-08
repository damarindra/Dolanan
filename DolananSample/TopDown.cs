using System;
using Dolanan;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Components.UI;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Scene;
using Dolanan.Scene.Object;
using Dolanan.Tools;
using Dolanan.Tools.GameHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DolananSample
{
	public class TopDown : GameMin
	{
		private Player p;
		private UILayer UILayer;
		private UIActor Canvas;
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

			UILayer = World.CreateLayer<UILayer>(3);
			Canvas = UILayer.ScreenCanvas;
			Canvas.RectTransform.Rectangle = new RectangleF(0,0,GameSettings.ViewportSize.X, GameSettings.ViewportSize.Y);

			Console.WriteLine("Valid");
			UIActor topLeft = World.CreateActor<UIActor>("TopLeft", UILayer);
			topLeft.AddComponent<Image>();
			topLeft.RectTransform.Rectangle = new RectangleF(0,0, 100, 100);
			topLeft.RectTransform.Anchor = Anchor.TopLeft;
			// Console.WriteLine(topLeft.RectTransform.Rectangle);
			topLeft.SetParent(Canvas);
			// Console.WriteLine(topLeft.RectTransform.Rectangle);

			UIActor topCenter = World.CreateActor<UIActor>("TopCenter", UILayer);
			topCenter.AddComponent<Image>();
			topCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X / 2f -  50, 0, 100, 100);
			topCenter.RectTransform.Anchor = Anchor.TopCenter;
			topCenter.SetParent(Canvas);
			
			UIActor topRight = World.CreateActor<UIActor>("TopRight", UILayer);
			topRight.AddComponent<Image>();
			topRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X -100, 0, 100, 100);
			topRight.RectTransform.Anchor = Anchor.TopRight;
			topRight.SetParent(Canvas);
			
			UIActor middleLeft = World.CreateActor<UIActor>("MiddleLeft", UILayer);
			middleLeft.AddComponent<Image>();
			middleLeft.RectTransform.Rectangle = new RectangleF(0, GameSettings.ViewportSize.Y / 2 - 50, 100, 100);
			middleLeft.RectTransform.Anchor = Anchor.MiddleLeft;
			middleLeft.SetParent(Canvas);
			
			UIActor middleCenter = World.CreateActor<UIActor>("MiddleCenter", UILayer);
			middleCenter.AddComponent<Image>();
			middleCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X / 2  - 50, GameSettings.ViewportSize.Y / 2 - 50, 
				100, 100);
			middleCenter.RectTransform.Anchor = Anchor.MiddleCenter;
			middleCenter.SetParent(Canvas);
			
			UIActor middleRight = World.CreateActor<UIActor>("middleRight", UILayer);
			middleRight.AddComponent<Image>();
			middleRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X -100, GameSettings.ViewportSize.Y / 2 - 50, 
				100, 100);
			middleRight.RectTransform.Anchor = Anchor.MiddleRight;
			middleRight.SetParent(Canvas);

			UIActor bottomLeft = World.CreateActor<UIActor>("bottomLeft", UILayer);
			bottomLeft.AddComponent<Image>();
			bottomLeft.RectTransform.Rectangle = new RectangleF(0, GameSettings.ViewportSize.Y - 100, 100, 100);
			bottomLeft.RectTransform.Anchor = Anchor.BottomLeft;
			bottomLeft.SetParent(Canvas);
			
			UIActor bottomCenter = World.CreateActor<UIActor>("bottomCenter", UILayer);
			bottomCenter.AddComponent<Image>();
			bottomCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X / 2 - 50, GameSettings.ViewportSize.Y - 100, 
				100, 100);
			bottomCenter.RectTransform.Anchor = Anchor.BottomCenter;
			bottomCenter.SetParent(Canvas);
			
			UIActor bottomRight = World.CreateActor<UIActor>("bottomRight", UILayer);
			bottomRight.AddComponent<Image>();
			bottomRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X -100, GameSettings.ViewportSize.Y - 100, 
				100, 100);
			bottomRight.RectTransform.Anchor = Anchor.BottomRight;
			bottomRight.SetParent(Canvas);

			
			UIActor stretchVertical = World.CreateActor<UIActor>("stretchVertical", UILayer);
			stretchVertical.AddComponent<Image>();
			stretchVertical.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X * .2f, 0, 
				GameSettings.ViewportSize.X * 0.15f, GameSettings.ViewportSize.Y);
			stretchVertical.RectTransform.Anchor = new Anchor(new Vector2(.2f, 0), new Vector2(.35f, 1));
			stretchVertical.SetParent(Canvas);
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

			if (Keyboard.GetState().IsKeyDown(Keys.L))
			{
				if(Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.Right += 1f;
				else Canvas.RectTransform.Right -= 1f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.K))
			{
				if(Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.Bottom += 1f;
				else Canvas.RectTransform.Bottom -= 1f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.J))
			{
				if(Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.Left += 1f;
				else Canvas.RectTransform.Left -= 1f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.I))
			{
				if(Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.Top += 1f;
				else Canvas.RectTransform.Top -= 1f;
			}
		}

		protected override void Process(GameTime gameTime)
		{
			base.Process(gameTime);
		}

		protected override void BackDraw(GameTime gameTime, Rectangle renderRect)
		{
			base.BackDraw(gameTime, renderRect);
			UILayer.BackDraw(gameTime, renderRect);
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