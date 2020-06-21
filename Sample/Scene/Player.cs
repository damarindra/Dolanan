using System;
using Dolanan.Animation;
using Dolanan.Collision;
using Dolanan.Components;
using Dolanan.Editor.Attribute;
using Dolanan.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dolanan.Scene.Object
{
	public class Player : Actor
	{
		[VisibleProperty] public Vector2 TestVecProp { get; set; }

		private readonly float _moveSpeed = 160;

		private Vector2 _movementInput;

		public Player(string name, Layer layer) : base(name, layer)
		{
		}

		public Body Body { get; private set; }
		public Sprite Sprite { get; private set; }
		public AnimationPlayer AnimationPlayer { get; private set; }

		public override void Start()
		{
			base.Start();
			Sprite = AddComponent<Sprite>();
			AnimationPlayer = AddComponent<AnimationPlayer>();

			Body = AddComponent<Body>();
			Body.BodyType = BodyType.Kinematic;
			Body.Size = Vector2.One * 20;
			Body.Offset = new Vector2(10, 4);
			Body.OnCollisionEnter += other =>
			{
				if (other.Tag == "wall")
					Console.WriteLine("Yay, I found Wall");
			};
			Body.OnTriggerEnter += other =>
			{
				if (other.Tag == "trigger")
					Console.WriteLine("haha, triggered!");
			};
			Body.OnCollisionExit += other =>
			{
				if (other.Tag == "wall")
					Console.WriteLine("By bye wall");
			};
			Body.OnTriggerExit += other =>
			{
				if (other.Tag == "trigger")
					Console.WriteLine("By Bye trigger!");
			};
			Body.OnCollisionStay += other =>
			{
				if (other.Tag == "wall")
					Console.WriteLine("The wall loves me");
			};
			Body.OnTriggerStay += other =>
			{
				if (other.Tag == "trigger")
					Console.WriteLine("I'm still Triggered!");
			};

			Input.AddInputAxis("Horizontal",
				new InputAxis(Keys.D, Keys.A, thumbStick: GamePadThumbStickDetail.LeftHorizontal));
			Input.AddInputAxis("Vertical",
				new InputAxis(Keys.W, Keys.S, thumbStick: GamePadThumbStickDetail.LeftVertical));

			Input.AddInputAction("NextFrame", new InputAction(Keys.OemCloseBrackets));
			Input.AddInputAction("PrevFrame", new InputAction(Keys.OemOpenBrackets));
		}

		private void Move(GameTime gameTime, Vector2 movementInput)
		{
			//movementInput.X = 1;
			if (Math.Abs(movementInput.LengthSquared()) < float.Epsilon)
				return;
			movementInput.Normalize();

			var movement = new Vector2();
			movement += Transform.Right * movementInput.X;
			movement += Transform.Up * movementInput.Y;

			if (movement != Vector2.Zero)
			{
				movement *= _moveSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;

				Body.Move(movement);
			}
		}

		#region CYCLE

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Move(gameTime, _movementInput);

			if (Keyboard.GetState().IsKeyDown(Keys.R))
				Transform.Rotation += 0.01f;
			else if (Keyboard.GetState().IsKeyDown(Keys.T)) Transform.Rotation -= 0.01f;

			if (Keyboard.GetState().IsKeyDown(Keys.V))
				Transform.LocalScale += new Vector2(0.01f);
			else if (Keyboard.GetState().IsKeyDown(Keys.C)) Transform.LocalScale -= new Vector2(0.01f);

			Sprite.Update(gameTime);
			if (_movementInput == Vector2.Zero)
				AnimationPlayer.Play("Idle");
			else
				AnimationPlayer.Play("Run");
		}


		public override void Draw(GameTime gameTime, float layerZDepth = 0)
		{
			base.Draw(gameTime, layerZDepth);

			Sprite.Draw(gameTime, layerZDepth);
		}

		#endregion
	}
}