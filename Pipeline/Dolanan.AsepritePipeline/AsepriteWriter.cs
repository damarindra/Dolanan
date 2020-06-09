using System;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TWrite = Dolanan.AsepritePipeline.AsepriteModel;

namespace Dolanan.AsepritePipeline
{
	[ContentTypeWriter]
	public class AsepriteWriter : ContentTypeWriter<TWrite>
	{
		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			// Class Type, Main Project / namespace (? not sure about the last one)
			return "Dolanan.ThirdParty.AsepriteReader, Dolanan";
		}

		protected override void Write(ContentWriter output, TWrite value)
		{
			var frameValues = value.Frames.ToArray();

			// Animation Count
			output.Write(value.Meta.FrameTags.Length);

			// Animation
			foreach (var tag in value.Meta.FrameTags)
			{
				// Animation Name
				output.Write(tag.Name);

				// Total Frame
				output.Write((int) Math.Abs(tag.To - tag.From) + 1);

				if (tag.Direction == "forward")
					for (var i = (int) tag.From; i <= (int) tag.To; i++)
					{
						// Frame index
						output.Write(i);
						// duration
						output.Write((int) frameValues[i].Duration);
					}
				else
					for (var i = (int) tag.To; i <= (int) tag.From; i--)
					{
						// Frame index
						output.Write(i);
						// duration
						output.Write((int) frameValues[i].Duration);
					}
			}

			//Slices

			// Total Slice
			output.Write(value.Meta.Slices.Length);

			foreach (var slice in value.Meta.Slices)
			{
				// slice name
				output.Write(slice.Name);

				// Using bitmask to determine what slice type it is
				// 0 = Nothing, 1 = Bounds, 2 = Center (9slice), 4 = Pivot
				var bit = slice.Keys[0].Bounds != null ? 1 : 0;
				bit |= slice.Keys[0].Center != null ? 2 : 0;
				bit |= slice.Keys[0].Pivot != null ? 4 : 0;
				output.Write(bit);

				// Bounds
				if ((bit & 1) != 0)
				{
					output.Write((int) slice.Keys[0].Bounds.X);
					output.Write((int) slice.Keys[0].Bounds.Y);
					output.Write((int) slice.Keys[0].Bounds.W);
					output.Write((int) slice.Keys[0].Bounds.H);
				}

				if ((bit & 2) != 0)
				{
					output.Write((int) slice.Keys[0].Center.X);
					output.Write((int) slice.Keys[0].Center.Y);
					output.Write((int) slice.Keys[0].Center.W);
					output.Write((int) slice.Keys[0].Center.H);
				}

				if ((bit & 4) != 0)
				{
					output.Write((int) slice.Keys[0].Pivot.X);
					output.Write((int) slice.Keys[0].Pivot.Y);
				}
			}
		}
	}
}