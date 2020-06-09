using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using TInput = System.String;
using TOutput = Dolanan.AsepritePipeline.AsepriteModel;

namespace Dolanan.AsepritePipeline
{
	[ContentProcessor(DisplayName = "Aseprite Processor")]
	internal class AsepriteProcessor : ContentProcessor<string, AsepriteModel>
	{
		public override AsepriteModel Process(string input, ContentProcessorContext context)
		{
			return JsonConvert.DeserializeObject<AsepriteModel>(input);
		}
	}
}