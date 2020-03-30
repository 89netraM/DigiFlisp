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

		public static async Task Write(Blueprint blueprint, string folderPath, string name)
		{
			BlueprintJSON blueprintJSON = BlueprintJSON.ToJSON(blueprint);

			using (Stream stream = System.IO.File.OpenWrite(CreateFilePath(folderPath, name)))
			{
				await JsonSerializer.SerializeAsync(stream, blueprintJSON);
			}
		}

		public static Task WriteAll(string folderPath, IEnumerable<(string, Blueprint)> blueprints)
		{
			return Task.WhenAll(blueprints.Select(x => Write(x.Item2, folderPath, x.Item1)));
		}

		public static async Task<(string, Blueprint)> Read(string path)
		{
			using (Stream stream = System.IO.File.OpenRead(path))
			{
				BlueprintJSON blueprintJSON = await JsonSerializer.DeserializeAsync<BlueprintJSON>(stream);

				return (Path.GetFileNameWithoutExtension(path), BlueprintJSON.FromJSON(blueprintJSON));
			}
		}

		public static Task<IEnumerable<(string, Blueprint)>> ReadAll(string folderPath)
		{
			return Task.WhenAll(Directory.GetFiles(folderPath, $"*.{fileExtension}").Select(x => Read(x)))
				.ContinueWith<IEnumerable<(string, Blueprint)>>(t => t.Result);
		}

		private static string CreateFilePath(string folderPath, string fileName)
		{
			return Path.Combine(folderPath, fileName + "." + fileExtension);
		}
	}
}