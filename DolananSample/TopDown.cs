using System;
using Dolanan;
using Dolanan.Animation;
using Dolanan.Components;
using Dolanan.Components.UI;
using Dolanan.Controller;
using Dolanan.Core;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Scene;
using Dolanan.Scene.Object;
using Dolanan.ThirdParty;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DolananSample
{
	public class TopDown : GameMin
	{
		private ResTexturesTopDown _texturesTopDown;
		private Aseprite _uiAseprite;

		private Texture2D _uiTexture;
		private UIActor Canvas;
		private Player p;
		private UILayer UILayer;

		public TopDown()
		{
			Log.Print("Init");
		}

		protected override void Initialize()
		{
			base.Initialize();
			p = World.CreateActor<Player>("Player");
			p.Transform.GlobalLocation =
				new Vector2(GameSettings.RenderSize.X / 2f, GameSettings.RenderSize.Y / 2f);
			p.Sprite.Texture2D = GameMgr.Game.Content.Load<Texture2D>("Graphics/Aseprites/player");
			p.Sprite.FrameSize = new Point(32, 32);

			Aseprite aseprite = GameMgr.Game.Content.Load<Aseprite>("Graphics/Aseprites/player_ase");

			p.AnimationPlayer = p.AddComponent<AnimationPlayer>();
			foreach (var asepriteAnimationFrame in aseprite.AnimationFrames)
				p.AnimationPlayer.AddAnimationSequence(asepriteAnimationFrame.ToAnimationSequence(p.Sprite, "Frame"));


			var fd = World.CreateActor<SpriteActor>("r");
			fd.SetParent(p);
			fd.Sprite.Texture2D = GameMgr.Game.Content.Load<Texture2D>("square_64x64");
			fd.Location = Vector2.One * 64;
			fd.Rotation = MathHelper.ToRadians(45);

			_uiTexture = GameMgr.Game.Content.Load<Texture2D>("Graphics/UI/rpgItems");
			_uiAseprite = GameMgr.Game.Content.Load<Aseprite>("Graphics/UI/rpgitems_ase");
			Slice slice;
			Image img;
			// var ui = World.CreateActor<UIActor>("PlayerUI");
			// ui.RectTransform.Rectangle = new RectangleF(0, 0, 100, 25);
			// ui.RectTransform.Pivot = Pivot.Center;
			// img = ui.AddComponent<Image>();
			// if (_uiAseprite.TryGetSlice("Slice 1", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// ui.SetParent(p);
			// ui.RectTransform.GlobalLocationByPivot = p.GlobalLocation - Vector2.UnitY * 25;
			//ui.RectTransform.GlobalLocationByPivot = p.GlobalLocation - Vector2.UnitY * 25;

			World.Camera.FollowActor = p;

			if (_texturesTopDown.TryGet("square", out var texture2D))
			{
			}

			UILayer = World.CreateLayer<UILayer>(12);
			Canvas = UILayer.ScreenCanvas;
			Canvas.RectTransform.Size = new Vector2(GameSettings.RenderSize.X, GameSettings.RenderSize.Y);
			Canvas.Interactable = false;

			var vContAct = UILayer.CreateActor<UIActor>("VContainer");
			vContAct.Location += Vector2.UnitX * 50;
			vContAct.RectTransform.Size = new Vector2(400, 600);
			vContAct.Transform.Anchor = Anchor.FullStretch;
			var vCont = vContAct.AddComponent<VContainer>();
			vCont.Alignment = Container.ChildAlignment.TopRight;
			vCont.Padding = new Padding(6, 6);
			vCont.ChildStretchHorizontal = true;

			for (var y = 0; y < 3; y++)
			{
				var containerAct = UILayer.CreateActor<UIActor>("HContainer");
				containerAct.RectTransform.Size = new Vector2(vContAct.RectTransform.Width, 100);
				containerAct.RectTransform.Pivot = Pivot.TopRight;
				containerAct.SetParent(vContAct);
				var hCont = containerAct.AddComponent<HContainer>();
				hCont.Alignment = Container.ChildAlignment.TopRight;
				hCont.Padding = new Padding(4, 4);
				for (var i = 0; i < 4; i++)
				{
					var c = UILayer.CreateActor<UIActor>("img");
					img = c.AddComponent<Image>();
					img.Texture2D = _uiTexture;
					if (_uiAseprite.TryGetSlice("Slice 1", out slice))
						img.TextureRectangle = slice.Bounds;
					c.RectTransform.SetRectSize(new Vector2(64, 64));
					c.RectTransform.Pivot = Pivot.TopRight;
					c.SetParent(containerAct);
				}
			}

			var gridAct = UILayer.CreateActor<UIActor>("GridContainer");
			gridAct.RectTransform.Size = new Vector2(396, 150);
			gridAct.RectTransform.Pivot = Pivot.TopRight;
			gridAct.SetParent(vContAct);
			var gCont = gridAct.AddComponent<GridContainer>();
			gCont.Alignment = Container.ChildAlignment.TopLeft;
			gCont.AutoRectSize = true;
			for (var i = 0; i < 30; i++)
			{
				var c = UILayer.CreateActor<UIActor>("img");
				c.Interactable = true;
				img = c.AddComponent<Image>();
				img.Texture2D = _uiTexture;
				if (_uiAseprite.TryGetSlice("Slice 1", out slice))
					img.TextureRectangle = slice.Bounds;
				c.RectTransform.SetRectSize(new Vector2(64, 64));
				c.RectTransform.Pivot = Pivot.TopLeft;
				c.SetParent(gridAct);
				var b = c.AddComponent<Button>();
				b.OnPressed += () =>
				{
					c.Transform.Location += Input.GetMouseMotion().ToVector2();
				};
				Console.WriteLine(c.ZDepth);
			}


			// var topLeft = World.CreateActor<UIActor>("TopLeft", UILayer);
			// topLeft.SetParent(Canvas);
			// img = topLeft.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 1", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// topLeft.RectTransform.Rectangle = new RectangleF(0, 0, 64, 64);
			// topLeft.RectTransform.GlobalLocation = new Vector2(400, 0);
			// topLeft.RectTransform.FitAnchorToRect();
			// img.Stretch = false;
			// topLeft.Clip = true;

			// var topCenter = World.CreateActor<UIActor>("TopCenter", UILayer);
			// topCenter.SetParent(Canvas);
			// img = topCenter.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 4", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// topCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X * .25f, 0, GameSettings.ViewportSize.X * .75f, 100);
			// topCenter.RectTransform.FitAnchorToRect();
			// // topCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X / 2f - 50, 0, 100, 100);
			// // topCenter.RectTransform.Anchor = Anchor.TopCenter;
			//
			// topCenter.ReceiveMouseInput = true;

			// var topCenter2 = World.CreateActor<UIActor>("TopCenter2", UILayer);
			// img = topCenter2.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 1", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// topCenter2.SetParent(topCenter);
			// topCenter2.RectTransform.Anchor = Anchor.TopRight;
			// topCenter2.Transform.Location = Vector2.Zero;
			// topCenter2.RectTransform.SetRectSize(new Vector2(150, 150));
			// // topCenter2.RectTransform. = new RectangleF(0, 0, 150, 150);
			// topCenter2.ReceiveMouseInput = true;

			// topCenter2 = World.CreateActor<UIActor>("TopCenter3", UILayer);
			// img = topCenter2.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 1", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// topCenter2.SetParent(topCenter);
			// topCenter2.RectTransform.Pivot = Pivot.TopRight;
			// topCenter2.RectTransform.Anchor = Anchor.MiddleRight;
			// topCenter2.RectTransform.SetRectSize(new Vector2(150, 150));
			// topCenter2.RectTransform.LocationByPivot = new Vector2(0);
			// // topCenter2.RectTransform. = new RectangleF(0, 0, 150, 150);
			// topCenter2.ReceiveMouseInput = true;

			// var parentlabel = World.CreateActor<UIActor>("ParentLabel", UILayer);
			// parentlabel.SetParent(Canvas);
			// parentlabel.Transform.Anchor = Anchor.BottomLeft;
			// parentlabel.Transform.Location = new Vector2(0, Canvas.RectTransform.RectSize.Y - 400);
			// parentlabel.Transform.SetRectSize(new Vector2(240, 400));
			// parentlabel.Clip = true;
			//
			// var label = World.CreateActor<UIActor>("Label", UILayer);
			// label.SetParent(parentlabel);
			// label.Transform.Anchor = Anchor.BottomLeft;
			// label.Transform.Location = Vector2.Zero;
			// label.Transform.SetRectSize(new Vector2(240, 200));
			// label.Clip = false;
			// var l = label.AddComponent<Label>();
			// ResFont.Instance.TryGet("8px", out var f);
			// l.Font = f;
			// l.Text = "Hello there, this text automatically autoresize, no matter how much your text is. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
			// l.TextAlign = TextAlign.Left;
			// l.TextVAlign = TextVAlign.Top;

			// var topRight = World.CreateActor<UIActor>("TopRight", UILayer);
			// img = topRight.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 2", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// topRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X - 100, 0, 100, 100);
			// topRight.RectTransform.Anchor = Anchor.TopRight;
			// topRight.SetParent(Canvas);
			// topRight.ReceiveMouseInput = true;
			//
			// var middleLeft = World.CreateActor<UIActor>("MiddleLeft", UILayer);
			// img = middleLeft.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 3", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// middleLeft.RectTransform.Rectangle = new RectangleF(0, GameSettings.ViewportSize.Y / 2 - 50, 100, 100);
			// middleLeft.RectTransform.Anchor = Anchor.MiddleLeft;
			// middleLeft.SetParent(Canvas);
			// middleLeft.ReceiveMouseInput = true;
			//
			var middleCenter = World.CreateActor<UIActor>("MiddleCenter", UILayer);
			img = middleCenter.AddComponent<Image>();
			middleCenter.Transform.Pivot = Pivot.Center;
			middleCenter.RectTransform.Location = new Vector2(GameSettings.RenderSize.X / 2 - 50,
				GameSettings.RenderSize.Y / 2 - 50);
			middleCenter.RectTransform.Size = new Vector2(100, 100);
			middleCenter.RectTransform.Anchor = Anchor.MiddleCenter;
			middleCenter.SetParent(Canvas);
			middleCenter.Interactable = true;
			var btn = middleCenter.AddComponent<Button>();
			btn.OnPressed += () => { middleCenter.Transform.Location += Input.GetMouseMotion().ToVector2(); };
			middleCenter.Transform.Rotation = MathHelper.ToRadians(45);

			//
			// var middleRight = World.CreateActor<UIActor>("middleRight", UILayer);
			// img = middleRight.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 5", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// middleRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X - 100,
			// 	GameSettings.ViewportSize.Y / 2 - 50,
			// 	100, 100);
			// middleRight.RectTransform.Anchor = Anchor.MiddleRight;
			// middleRight.SetParent(Canvas);
			// middleRight.ReceiveMouseInput = true;
			//
			// var bottomLeft = World.CreateActor<UIActor>("bottomLeft", UILayer);
			// img = bottomLeft.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 6", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// bottomLeft.RectTransform.Rectangle = new RectangleF(0, GameSettings.ViewportSize.Y - 100, 100, 100);
			// bottomLeft.RectTransform.Anchor = Anchor.BottomLeft;
			// bottomLeft.SetParent(Canvas);
			// bottomLeft.ReceiveMouseInput = true;
			//
			// var bottomCenter = World.CreateActor<UIActor>("bottomCenter", UILayer);
			// img = bottomCenter.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 7", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// bottomCenter.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X / 2 - 50,
			// 	GameSettings.ViewportSize.Y - 100,
			// 	100, 100);
			// bottomCenter.RectTransform.Anchor = Anchor.BottomCenter;
			// bottomCenter.SetParent(Canvas);
			// bottomCenter.ReceiveMouseInput = true;
			//
			// var bottomRight = World.CreateActor<UIActor>("bottomRight", UILayer);
			// img = bottomRight.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 8", out slice))
			// 	img.TextureRectangle = slice.Bounds;
			// bottomRight.RectTransform.Rectangle = new RectangleF(GameSettings.ViewportSize.X - 100,
			// 	GameSettings.ViewportSize.Y - 100,
			// 	100, 100);
			// bottomRight.RectTransform.Anchor = Anchor.BottomRight;
			// bottomRight.SetParent(Canvas);
			// bottomRight.ReceiveMouseInput = true;
			//
			//
			// var stretchVertical = World.CreateActor<UIActor>("stretchVertical", UILayer);
			// img = stretchVertical.AddComponent<Image>();
			// img.Texture2D = _uiTexture;
			// if (_uiAseprite.TryGetSlice("Slice 9", out slice))
			// {
			// 	img.TextureRectangle = slice.Bounds;
			// 	stretchVertical.RectTransform.Location = new Vector2(GameSettings.ViewportSize.X * .2f, 0);
			// 	stretchVertical.RectTransform.Size = new Vector2(slice.Bounds.Width, slice.Bounds.Height);
			// }
			// stretchVertical.ReceiveMouseInput = true;
			// Log.Print(stretchVertical);
			//
			// // stretchVertical.RectTransform.Anchor = new Anchor(new Vector2(.2f, 0), new Vector2(.35f, 1));
			// stretchVertical.RectTransform.Anchor = Anchor.FullStretch;
			// stretchVertical.SetParent(Canvas);
			// //
			// //
			// var stretchHorizontal = UILayer.CreateActor<UIActor>("stretchHorizontal");
			// var ns = stretchHorizontal.AddComponent<NineSlice>();
			// var tNs = GameMgr.Game.Content.Load<Texture2D>("Graphics/UI/Colored/blue");
			// var asepriteNs = GameMgr.Game.Content.Load<Aseprite>("Graphics/UI/Colored/blue_ase");
			// ns.Texture2D = tNs;
			// if (asepriteNs.TryGetSlice("slice", out slice))
			// {
			// 	ns.TextureRectangle = slice.Bounds;
			// 	ns.Center = slice.Center;
			// 	stretchHorizontal.RectTransform.Location = new Vector2(50, GameSettings.ViewportSize.Y * 0.2f);
			// 	stretchHorizontal.RectTransform.Size = new Vector2(GameSettings.ViewportSize.X - 100, GameSettings.ViewportSize.Y * .5f);
			// }
			// //
			// stretchHorizontal.RectTransform.Anchor = new Anchor(new Vector2(0f, .2f), new Vector2(1, 0.5f));
			// ns.Mode = NineSlice.ResizeMode.Stretch;
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			_texturesTopDown = (ResTexturesTopDown) new ResTexturesTopDown().Load();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Keyboard.GetState().IsKeyDown(Keys.L))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.RightRect += 1f;
				else Canvas.RectTransform.RightRect -= 1f;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.K))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.BottomRect += 1f;
				else Canvas.RectTransform.BottomRect -= 1f;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.J))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.LeftRect += 1f;
				else Canvas.RectTransform.LeftRect -= 1f;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.I))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					Canvas.RectTransform.TopRect += 1f;
				else Canvas.RectTransform.TopRect -= 1f;
			}
		}

		protected override void Process(GameTime gameTime)
		{
			base.Process(gameTime);
		}

		/// <summary>
		///     Back Draw, occured after BackBufferRendering. Useful for drawing UI, debugging, etc. Always show in front.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="worldRect">BackBuffer Rectangle</param>
		protected override void BackDraw(GameTime gameTime, Rectangle worldRect)
		{
			base.BackDraw(gameTime, worldRect);
			GameMgr.SpriteBatch.Draw(ScreenDebugger.Pixel,
				new Rectangle(Camera.WorldToScreen(p.Transform.GlobalLocation), new Point(5, 5)), null, Color.Yellow);
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