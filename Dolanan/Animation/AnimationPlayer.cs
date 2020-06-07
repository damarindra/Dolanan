using System;
using System.Collections.Generic;
using Dolanan.Components;
using Dolanan.Scene;
using Dolanan.Tools;
using Microsoft.Xna.Framework;

namespace Dolanan.Animation
{
	public class AnimationPlayer : Component
	{
		public AnimationPlayer(Actor owner) : base(owner) { }
		
		public List<AnimationSequence> Animation { get; private set; } = new List<AnimationSequence>();
		public bool IsPlaying { get; private set; }

		public AnimationSequence CurrentAnimation
		{
			get;
			set;
		}

		public float Speed { get; set; } = 1;
		
		/// <summary>
		/// Create new Animation Sequence
		/// </summary>
		/// <param name="name">The name of animation, must be unique</param>
		/// <param name="animationLength">Animation Length in Milisec</param>
		/// <param name="isReverse"></param>
		/// <param name="isLoop"></param>
		/// <returns></returns>
		public AnimationSequence CreateNewAnimationSequence(string name, float animationLength = 2000, bool isReverse = false,
			bool isLoop = true)
		{
			if (TryGetAnimationSequence(name, out var a))
			{
				Log.PrintWarning("Abort creating new Animation with name : " + name + ". Return existing animation instead");
				return a;
			}
			AnimationSequence sequence = new AnimationSequence(name, animationLength, isReverse, isLoop);
			Animation.Add(sequence);
			return sequence;
		}

		public void Resume()
		{
			if(Animation.Count == 0)
				return;
			if (CurrentAnimation == null)
				CurrentAnimation = Animation[0];
			
			IsPlaying = true;
		}

		public void Play(string name)
		{
			if (CurrentAnimation?.Name == name)
			{
				IsPlaying = true;
				return;
			}

			if (TryGetAnimationSequence(name, out var animationSequence))
			{
				CurrentAnimation = animationSequence;
				IsPlaying = true;
				return;
			}
			Log.PrintWarning("Animation with name : " + name +" is not available");
		}

		public void PlayAt(string name, float positionMilisec)
		{
			if (CurrentAnimation?.Name == name)
			{
				IsPlaying = true;
				CurrentAnimation?.Seek(positionMilisec);
				return;
			}

			if (TryGetAnimationSequence(name, out var animationSequence))
			{
				CurrentAnimation = animationSequence;
				CurrentAnimation.Seek(positionMilisec);
				IsPlaying = true;
				return;
			}
			Log.PrintWarning("Animation with name : " + name +" is not available");
		}

		public void Stop()
		{
			IsPlaying = false;

			CurrentAnimation?.Stop();
		}

		private bool TryGetAnimationSequence(string name, out AnimationSequence animationSequence)
		{
			animationSequence = null;
			for (int i = 0; i < Animation.Count; i++)
			{
				if (Animation[i].Name == name)
				{
					animationSequence = Animation[i];
					return true;
				}
			}
			return false;
		}

		public override void Update(GameTime gameTime)
		{
			// Console.WriteLine(IsPlaying);
			if(!IsPlaying || Animation.Count == 0)
				return;
			CurrentAnimation.UpdateAnimation((float) gameTime.ElapsedGameTime.TotalMilliseconds * Speed);
		}
	}
}