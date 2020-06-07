using System;
using Dolanan.Animation;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Controller;
using Dolanan.Engine;
using Dolanan.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Scene.Object
{
	public class Player : Actor
	{
		private float _moveSpeed = 160;
		public Body Body { get; private set; }
		public Sprite Sprite { get; private set; }
		public AnimationPlayer AnimationPlayer { get; private set; }

		private Vector2 _movementInput;
		private Aseprite _aseprite;

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<Sprite>();
			Sprite.Texture2D = GameMgr.Game.Content.Load<Texture2D>("player");
			Sprite.FrameSize = new Point(32, 32);

			_aseprite = GameMgr.Game.Content.Load<Aseprite>("Graphics/Aseprites/player_ase");
			
			AnimationPlayer = AddComponent<AnimationPlayer>();
			foreach (var asepriteAnimationFrame in _aseprite.AnimationFrames)
			{
				AnimationPlayer.AddAnimationSequence(asepriteAnimationFrame.ToAnimationSequence(Sprite, "Frame"));
			}
			
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
			
			Input.AddInputAction("NextFrame", new InputAction(Keys.OemCloseBrackets));
			Input.AddInputAction("PrevFrame", new InputAction(Keys.OemOpenBrackets));
		}
		#region CYCLE

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Move(gameTime, _movementInput);

			if (Keyboard.GetState().IsKeyDown(Keys.R))
			{
				Transform.Rotation += 0.01f;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.T))
			{
				Transform.Rotation -= 0.01f;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.V))
			{
				Transform.LocalScale += new Vector2(0.01f);
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.C))
			{
				Transform.LocalScale -= new Vector2(0.01f);
			}

			Sprite.Update(gameTime);
			if (_movementInput == Vector2.Zero)
			{
				AnimationPlayer.Play("Idle");
			}
			else
			{
				AnimationPlayer.Play("Run");
			}
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

		public Player(string name, Layer layer) : base(name, layer)
		{
		}
	}
}