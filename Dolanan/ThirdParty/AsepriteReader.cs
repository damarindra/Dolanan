using System.Drawing;
using Microsoft.Xna.Framework.Content;

namespace Dolanan.ThirdParty
{
	public class AsepriteReader : ContentTypeReader<Aseprite>
	{
		protected override Aseprite Read(ContentReader input, Aseprite existingInstance)
		{
			AnimationFrame[] animationFrames = new AnimationFrame[input.ReadInt32()];

			for (int i = 0; i < animationFrames.Length; i++)
			{
				string name = input.ReadString();

				int[] frameIndex = new int[input.ReadInt32()];
				int[] duration = new int[frameIndex.Length];

				for (int j = 0; j < frameIndex.Length; j++)
				{
					frameIndex[j] = input.ReadInt32();
					duration[j] = input.ReadInt32();
				}
				
				animationFrames[i] = new AnimationFrame(name, frameIndex, duration);
			}

			Slice[] Slices = new Slice[input.ReadInt32()];

			for (int i = 0; i < Slices.Length; i++)
			{
				string name = input.ReadString();
				Rectangle bounds = Rectangle.Empty;
				Rectangle center = Rectangle.Empty;
				Point pivot = Point.Empty;

				int bit = input.ReadInt32();

				// Bounds
				if ((bit & 1) != 0)
				{
					bounds = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());
				}
				
				if((bit & 2) != 0)
					center = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());
				
				if((bit & 4) != 0)
					pivot = new Point(input.ReadInt32(), input.ReadInt32());
				
				Slices[i] = new Slice(name, bit, bounds, center, pivot);
			}
			
			return new Aseprite(animationFrames, Slices);
		}
	}
}