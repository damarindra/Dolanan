﻿#nullable enable
using System;
using Dolanan.Animation;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Engine;
using Dolanan.Resources;
using Dolanan.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using Hit = Dolanan.Collision.Hit;

namespace Dolanan.Scene.Object
{
	public class Player : Actor
	{
		public AnimationSequence AnimationSequence { get; private set; }
		
		private float _moveSpeed = 160;
		public Body Body { get; private set; }
		public AseSprite Sprite { get; private set; }

		public Player(string name, Layer layer) : base(name, layer) { }

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<AseSprite>();
			Body = AddComponent<Body>();
			Body.BodyType = BodyType.Kinematic;
			Body.OnCollisionEnter += other =>
			{
				if(other.Tag == "wall")
					Console.WriteLine("Yay, I found Wall");
			};
			Body.OnTriggerEnter += other =>
			{
				if(other.Tag == "trigger")
					Console.WriteLine("haha, triggered!");
			};
			Body.OnCollisionExit += other =>
			{
				if(other.Tag == "wall")
					Console.WriteLine("By bye wall");
			};
			Body.OnTriggerExit += other =>
			{
				if(other.Tag == "trigger")
					Console.WriteLine("By Bye trigger!");
			};
			Body.OnCollisionStay += other =>
			{
				if(other.Tag == "wall")
					Console.WriteLine("The wall loves me");
			};
			Body.OnTriggerStay += other =>
			{
				if(other.Tag == "trigger")
					Console.WriteLine("I'm still Triggered!");
			};

			AnimatedSprite animatedSprite;
			if(ResAnimatedSprite.Instance.TryGet("player", out animatedSprite))
				Sprite.AnimatedSprite = animatedSprite;
			
			Sprite.AnimatedSprite.Play("Idle");
			
			AnimationSequence = new AnimationSequence(1500);
			
			// Track<int> t = AnimationSequence.CreateNewValueTrack<int>("SpriteFrame", Sprite, "Frame");
			// t.AddKey(new Key<int>(0, 0));
			// t.AddKey(new Key<int>(300, 1));
			// t.AddKey(new Key<int>(600, 2));
			// t.AddKey(new Key<int>(900, 3));
			// t.AddKey(new Key<int>(1200, 4));
			// t.AddKey(new Key<int>(1500, 5));
			
			Input.AddInputAxis("Horizontal", 
				new InputAxis(positiveKey: Keys.D, negativeKey: Keys.A, thumbStick: GamePadThumbStickDetail.LeftHorizontal));
			Input.AddInputAxis("Vertical", 
				new InputAxis(positiveKey: Keys.W, negativeKey: Keys.S, thumbStick: GamePadThumbStickDetail.LeftVertical));

			//Body = Layer.GameWorld.CreateBody(Transform, new Vector2(0, 0), new Vector2(32,32));
			//Body = layer.GameWorld.CreateAABB(Transform.Position, Vector2.One * 32);
			//Body = layer.GameWorld.Create(Transform, 32, 32, new Vector2(16, 16));
			
		}

		#region CYCLE
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Move(gameTime, movementInput);
			
			AnimationSequence.UpdateAnimation(gameTime);

			if (Keyboard.GetState(0).IsKeyDown(Keys.R))
			{
				Transform.Rotation += 0.01f;
			}
			else if (Keyboard.GetState(0).IsKeyDown(Keys.T))
			{
				Transform.Rotation -= 0.01f;
			}
			if (Keyboard.GetState(0).IsKeyDown(Keys.V))
			{
				Transform.LocalScale += new Vector2(0.01f);
			}
			else if (Keyboard.GetState(0).IsKeyDown(Keys.C))
			{
				Transform.LocalScale -= new Vector2(0.01f);
			}
			
			Sprite.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

			Sprite.Draw(gameTime, layerZDepth);
		}

		#endregion
		private void Move(GameTime gameTime, Vector2 movementInput)
		{
			//movementInput.X = 1;
			if (Math.Abs(movementInput.LengthSquared()) < Single.Epsilon)
				return;
			movementInput.Normalize();
			
			Vector2 movement = new Vector2();
			movement += Transform.Right * movementInput.X;
			movement += Transform.Up * movementInput.Y;

			if (movement != Vector2.Zero)
			{
				movement *= _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

				Body.Move(movement);
			}
		}
	}
}