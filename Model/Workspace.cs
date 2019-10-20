using System.Collections.Generic;

namespace Model
{
	public class Workspace
	{
		private readonly Dictionary<string, Blueprint> blueprintList = new Dictionary<string, Blueprint>();

		public IReadOnlyCollection<string> BlueprintIds => blueprintList.Keys;

		public Blueprint GetBlueprint(string id)
		{
			return blueprintList[id];
		}

		public void AddBlueprint(string id, Blueprint blueprint)
		{
			blueprintList.Add(id, blueprint);
		}

		public void RemoveBlueprint(string id)
		{
			blueprintList.Remove(id);
		}
	}
}