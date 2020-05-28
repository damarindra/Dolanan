#nullable enable
using System;
using System.Collections.Generic;
using CoreGame.Engine;
using CoreGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoreGame.Scene.Object
{
	public class Player : SpriteActor
	{
		public Animation Animation;
		
		private float _moveSpeed = 60;
		
		public Player(Texture2D texture2D) : base(texture2D)
		{
			Sprite.SrcLocation = Point.Zero;
			Sprite.SrcSize = new Point(32, 32);
			Animation = new Animation(1500, true, false);
			
			Track<int> t = Animation.CreateNewValueTrack<int>("SpriteFrame", Sprite, "Frame");
			Animation.Seek(1500);
			t.AddKey(new TKey<int>(0, 0));
			t.AddKey(new TKey<int>(300, 1));
			t.AddKey(new TKey<int>(600, 2));
			t.AddKey(new TKey<int>(900, 3));
			t.AddKey(new TKey<int>(1200, 4));
			t.AddKey(new TKey<int>(1500, 5));
			t.PrintALl();
		}

		public Player(Texture2D texture2D, Vector2 position) : base(texture2D, position)
		{
			Sprite.SrcLocation = Point.Zero;
			Sprite.SrcSize = new Point(32, 32);
		}

		protected override void LateInit(Actor? parent, Vector2 position, float rotation, Vector2 scale)
		{
			base.LateInit(parent, position, rotation, scale);
			
			Input.AddInputAxis("Horizontal", new InputAxis(positiveKey: Keys.D, negativeKey: Keys.A, thumbStick: GamePadThumbStickDetail.LeftHorizontal));
			Input.AddInputAxis("Vertical", new InputAxis(positiveKey: Keys.S, negativeKey: Keys.W, thumbStick: GamePadThumbStickDetail.LeftVertical));
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if(movementInput.LengthSquared() > 0)
				movementInput.Normalize();
			Vector2 movement = new Vector2();
			movement += transform.Right * movementInput.X;
			movement += transform.Up * movementInput.Y;

			movement *= _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			transform.Position += movement;
			
			Animation.UpdateAnimation(gameTime);
		}

		public void WhatsUpDude(string thisIsStr)
		{
			Log.Print(thisIsStr);
		}
	}
}