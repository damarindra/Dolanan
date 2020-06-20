using Dolanan.Animation;
using Dolanan.Collision;
using Dolanan.Components.UI;
using Dolanan.PipelineReader.Aseprite;
using Dolanan.Tools;

namespace Dolanan.ThirdParty
{
	public static class AsepriteHelper
	{
		
		public static AnimationSequence ToAnimationSequence(this AnimationFrame animationFrame, object sprite, string frameVariableName)
		{
			float totalDuration = 0;
		
			foreach (var i in animationFrame.Duration) totalDuration += i;
		
			var sequence = new AnimationSequence(animationFrame.Name, totalDuration);
			var frameTrack = sequence.CreateNewValueTrack<int>("frame", sprite, frameVariableName);
		
			float currentDuration = 0;
			for (var i = 0; i < animationFrame.FrameIndex.Length; i++)
			{
				frameTrack.AddKey(new Key<int>(currentDuration, animationFrame.FrameIndex[i]));
		
				currentDuration += animationFrame.Duration[i];
			}
		
			return sequence;
		}
		
		public static void ToBody(this Slice slice, ref Body body, bool usePivotAsCenter)
		{
			if (!slice.IsBoundValid)
			{
				Log.PrintWarning("Slice doesn't have a Bounds : " + slice.Name);
				return;
			}
		
			body.Size = slice.Bounds.Size.ToVector2();
			if (slice.IsPivotValid && usePivotAsCenter) body.Offset = slice.Pivot.ToVector2();
		}
		
		public static void ToNineSlice(this Slice slice, ref NineSlice nineSlice)
		{
			if (!slice.IsBoundValid || !slice.IsCenterValid)
			{
				Log.PrintWarning("Slice doesn't have a valid nineslice support :" + slice.Name);
				return;
			}
		
			nineSlice.Transform.SetRectSize(slice.Bounds.Size.ToVector2());
			nineSlice.Center = slice.Center;
		}
	}
}