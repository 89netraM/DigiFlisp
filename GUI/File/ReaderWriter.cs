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

		public static async Task<Blueprint> Read(string path, IDictionary<string, Blueprint> blueprints)
		{
			using (Stream stream = System.IO.File.OpenRead(path))
			{
				BlueprintJSON blueprintJSON = await JsonSerializer.DeserializeAsync<BlueprintJSON>(stream);

				return BlueprintJSON.FromJSON(blueprintJSON, Path.GetFileNameWithoutExtension(path), blueprints);
			}
		}

		public static async Task<IEnumerable<Blueprint>> ReadAll(string folderPath)
		{
			IList<string> paths = Directory.GetFiles(folderPath, $"*.{fileExtension}").ToList();
			IDictionary<string, Blueprint> blueprints = new Dictionary<string, Blueprint>();

			if (paths.Count > 0)
			{
				int i = 0;
				while (paths.Count > 1)
				{
					try
					{
						Blueprint blueprint = await Read(paths[i], blueprints);

						paths.RemoveAt(i);
						blueprints.Add(blueprint.Id, blueprint);
						i = 0;
					}
					catch
					{
						i++;
					}
				}

				return blueprints.Values.Append(await Read(paths[0], blueprints));
			}
			else
			{
				return Enumerable.Empty<Blueprint>();
			}
		}

		private static string CreateFilePath(string folderPath, string fileName)
		{
			return Path.Combine(folderPath, fileName + "." + fileExtension);
		}
	}
}