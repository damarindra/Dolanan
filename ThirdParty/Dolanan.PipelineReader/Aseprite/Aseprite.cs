using Microsoft.Xna.Framework;

namespace Dolanan.PipelineReader.Aseprite
{
	public class Aseprite
	{
		public Aseprite(AnimationFrame[] animationFrames, Slice[] slices)
		{
			AnimationFrames = animationFrames;
			Slices = slices;
		}

		public AnimationFrame[] AnimationFrames { get; }
		public Slice[] Slices { get; }

		public bool TryGetAnimationFrame(string name, out AnimationFrame animation)
		{
			animation = default;
			foreach (var animationFrame in AnimationFrames)
				if (animationFrame.Name == name)
				{
					animation = animationFrame;
					return true;
				}

			return false;
		}

		public bool TryGetSlice(string name, out Slice slice)
		{
			slice = default;
			foreach (var s in Slices)
				if (s.Name == name)
				{
					slice = s;
					return true;
				}

			return false;
		}
	}
	//
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
		private readonly int Bit;
	
		public bool IsBoundValid => (Bit & 1) != 0;
		public bool IsCenterValid => (Bit & 2) != 0;
		public bool IsPivotValid => (Bit & 4) != 0;
	}
}