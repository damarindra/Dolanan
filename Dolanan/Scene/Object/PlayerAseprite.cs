#nullable enable
using System;
using Dolanan.Animation;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Engine;
using Dolanan.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Scene.Object
{
	public class PlayerAseprite : Actor
	{
		private float _moveSpeed = 160;
		public Body Body { get; private set; }
		public AseSprite Sprite { get; private set; }

		public PlayerAseprite(string name, Layer layer) : base(name, layer) { }

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<AseSprite>();
			if(ResAnimatedSprite.Instance.TryGet("player", out var animatedSprite))
				Sprite.AnimatedSprite = animatedSprite;
			Sprite.SetOrigin(Pivot.Center);
			Sprite.AnimatedSprite.Play("Idle");
			
			Body = AddComponent<Body>();
			Body.BodyType = BodyType.Kinematic;
			Body.Size = Vector2.One * 20;
			Body.Offset = new Vector2(10, 4);
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

			Input.AddInputAxis("Horizontal", 
				new InputAxis(positiveKey: Keys.D, negativeKey: Keys.A, thumbStick: GamePadThumbStickDetail.LeftHorizontal));
			Input.AddInputAxis("Vertical", 
				new InputAxis(positiveKey: Keys.W, negativeKey: Keys.S, thumbStick: GamePadThumbStickDetail.LeftVertical));

		}

		#region CYCLE
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Move(gameTime, movementInput);

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