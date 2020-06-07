using System.Drawing;
using Dolanan.Animation;

namespace Dolanan.ThirdParty
{
	public class Aseprite
	{
		public AnimationFrame[] AnimationFrames { get; }
		public Slice[] Slices { get; }

		public Aseprite(AnimationFrame[] animationFrames, Slice[] slices)
		{
			AnimationFrames = animationFrames;
			Slices = slices;
		}

		public bool TryGetAnimationFrame(string name, out AnimationFrame animation)
		{
			animation = default;
			foreach (var animationFrame in AnimationFrames)
			{
				if (animationFrame.Name == name)
				{
					animation = animationFrame;
					return true;
				}
			}
			return false;
		}
		public bool TryGetSlice(string name, out Slice slice)
		{
			slice = default;
			foreach (var s in Slices)
			{
				if (s.Name == name)
				{
					slice = s;
					return true;
				}
			}
			return false;
		}
	}

	public struct AnimationFrame
	{
		public AnimationFrame(string name, int[] frameIndex, int[] duration)
		{
			Name = name;
			FrameIndex = frameIndex;
			Duration = duration;
		}

		public string Name { get; }
		public int[] FrameIndex { get; }
		public int[] Duration { get; }

		public AnimationSequence ToAnimationSequence(object sprite, string frameVariableName)
		{
			float totalDuration = 0;

			foreach (var i in Duration)
			{
				totalDuration += i;
			}
			
			AnimationSequence sequence = new AnimationSequence(Name, totalDuration);
			ValueTrack<int> frameTrack = sequence.CreateNewValueTrack<int>("frame", sprite, frameVariableName);

			float currentDuration = 0;
			for (int i = 0; i < FrameIndex.Length; i++)
			{
				frameTrack.AddKey(new Key<int>(currentDuration, FrameIndex[i]));

				currentDuration += Duration[i];
			}

			return sequence;
		}
	}

	public struct Slice
	{
		public Slice(string name, int bit, Rectangle bounds, Rectangle center, Point pivot)
		{
			Name = name;
			Bounds = bounds;
			Center = center;
			Pivot = pivot;
			Bit = bit;
		}

		public string Name { get; }
		public Rectangle Bounds { get; }
		public Rectangle Center { get; }
		public Point Pivot { get; }
		private int Bit;

		public bool IsBoundValid => (Bit & 1) != 0;
		public bool IsCenterValid => (Bit & 2) != 0;
		public bool IsPivotValid => (Bit & 4) != 0;
	}
}