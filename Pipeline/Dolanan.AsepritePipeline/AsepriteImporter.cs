using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TImport = System.String;

namespace Dolanan.AsepritePipeline
{
	[ContentImporter(".json", DisplayName = "Aseprite Importer", DefaultProcessor = "AsepriteProcessor")]
	public class AsepriteImporter : ContentImporter<string>
	{
		public override string Import(string filename, ContentImporterContext context)
		{
			return File.ReadAllText(filename);
		}
	}
}