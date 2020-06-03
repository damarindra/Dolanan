#nullable enable
using System;
using CoreGame.Animation;
using CoreGame.Collision;
using CoreGame.Component;
using CoreGame.Engine;
using CoreGame.Resources;
using CoreGame.Tools;
using Humper;
using Humper.Base;
using Humper.Responses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;

namespace CoreGame.Scene.Object
{
	// TODO: Change to pure actor!
	public class Player : Actor
	{
		public AnimationSequence AnimationSequence { get; private set; }
		
		private float _moveSpeed = 60;
		public Body Body { get; private set; }
		public AseSprite Sprite { get; private set; }

		public Player(string name, Layer layer) : base(name, layer)
		{
			Sprite = AddComponent<AseSprite>();

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
				new InputAxis(positiveKey: Keys.S, negativeKey: Keys.W, thumbStick: GamePadThumbStickDetail.LeftVertical));

			Body = layer.GameWorld.Create(Transform, 32, 32, new Vector2(16, 16));
		}

		// TODO : Set Body Size in realtime, dunno if allowed
		public void SetBodySize(Point size)
		{
			
		}

		#region CYCLE
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Log.Print(Body.Bounds.ToString());

			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Move(gameTime, movementInput);
			
			AnimationSequence.UpdateAnimation(gameTime);
			
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
			if (Math.Abs(movementInput.LengthSquared()) < Single.Epsilon)
				return;
			movementInput.Normalize();
			
			Vector2 movement = new Vector2();
			movement += Transform.Right * movementInput.X;
			movement += Transform.Up * movementInput.Y;

			movement *= _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			Body.Move(Transform.Position.X + movement.X,Transform.Position.Y + movement.Y, CollisionRes);
		}

		public void Teleport(Vector2 position)
		{
			Body.Teleport(position);
		}

		protected virtual CollisionResponses CollisionRes(ICollision arg)
		{
			return CollisionResponses.Slide;
		}
	}
}