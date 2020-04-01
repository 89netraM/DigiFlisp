using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GUI.File
{
	public static class ReaderWriter
	{
		private const string fileExtension = "df";

		public static async Task Write(Blueprint blueprint, string folderPath)
		{
			BlueprintJSON blueprintJSON = BlueprintJSON.ToJSON(blueprint);

			using (Stream stream = System.IO.File.OpenWrite(CreateFilePath(folderPath, blueprint.Id)))
			{
				await JsonSerializer.SerializeAsync(stream, blueprintJSON);
			}
		}

		public static Task WriteAll(string folderPath, IEnumerable<Blueprint> blueprints)
		{
			return Task.WhenAll(blueprints.Select(x => Write(x, folderPath)));
		}

		public static async Task<Blueprint> Read(string path)
		{
			using (Stream stream = System.IO.File.OpenRead(path))
			{
				BlueprintJSON blueprintJSON = await JsonSerializer.DeserializeAsync<BlueprintJSON>(stream);

				return BlueprintJSON.FromJSON(blueprintJSON, Path.GetFileNameWithoutExtension(path));
			}
		}

		public static Task<IEnumerable<Blueprint>> ReadAll(string folderPath)
		{
			return Task.WhenAll(Directory.GetFiles(folderPath, $"*.{fileExtension}").Select(x => Read(x)))
				.ContinueWith<IEnumerable<Blueprint>>(t => t.Result);
		}

		private static string CreateFilePath(string folderPath, string fileName)
		{
			return Path.Combine(folderPath, fileName + "." + fileExtension);
		}
	}
}