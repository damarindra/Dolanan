using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using TInput = System.String;
using TOutput = Dolanan.AsepritePipeline.AsepriteModel;

namespace Dolanan.AsepritePipeline
{
    [ContentProcessor(DisplayName = "Aseprite Processor")]
    class AsepriteProcessor : ContentProcessor<TInput, AsepriteModel>
    {
        public override AsepriteModel Process(TInput input, ContentProcessorContext context)
        {
            return JsonConvert.DeserializeObject<AsepriteModel>(input);
        }
    }
}
