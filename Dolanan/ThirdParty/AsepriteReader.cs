using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Dolanan.ThirdParty
{
	public class AsepriteReader : ContentTypeReader<Aseprite>
	{
		protected override Aseprite Read(ContentReader input, Aseprite existingInstance)
		{
			var animationFrames = new AnimationFrame[input.ReadInt32()];

			for (var i = 0; i < animationFrames.Length; i++)
			{
				var name = input.ReadString();

				var frameIndex = new int[input.ReadInt32()];
				var duration = new int[frameIndex.Length];

				for (var j = 0; j < frameIndex.Length; j++)
				{
					frameIndex[j] = input.ReadInt32();
					duration[j] = input.ReadInt32();
				}

				animationFrames[i] = new AnimationFrame(name, frameIndex, duration);
			}

			var Slices = new Slice[input.ReadInt32()];

			for (var i = 0; i < Slices.Length; i++)
			{
				var name = input.ReadString();
				var bounds = Rectangle.Empty;
				var center = Rectangle.Empty;
				var pivot = Point.Zero;

				var bit = input.ReadInt32();

				// Bounds
				if ((bit & 1) != 0)
					bounds = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());

				if ((bit & 2) != 0)
					center = new Rectangle(input.ReadInt32(), input.ReadInt32(), input.ReadInt32(), input.ReadInt32());

				if ((bit & 4) != 0)
					pivot = new Point(input.ReadInt32(), input.ReadInt32());

				Slices[i] = new Slice(name, bit, bounds, center, pivot);
			}

			return new Aseprite(animationFrames, Slices);
		}
	}
}