using Model;
using System.Collections.Generic;

namespace GUI.File
{
	struct BlueprintJSON
	{
		public static BlueprintJSON ToJSON(Blueprint blueprint)
		{
			IDictionary<string, ComponentJSON> components = new Dictionary<string, ComponentJSON>();
			IList<ConnectionJSON> connections = new List<ConnectionJSON>();

			for (int i = 0; i < blueprint.ComponentCount; i++)
			{
				Component component = blueprint.GetComponent(i);
				components.Add(component.Id, ComponentJSON.ToJSON(component));

				foreach (Connection connection in blueprint.OutgoingConnectionsFor(component))
				{
					connections.Add(ConnectionJSON.ToJSON(component.Id, connection));
				}
			}

			return new BlueprintJSON
			{
				Components = components,
				Connections = connections
			};
		}

		public static Blueprint FromJSON(BlueprintJSON blueprintJSON, string id)
		{
			Blueprint blueprint = new Blueprint(id);

			foreach (KeyValuePair<string, ComponentJSON> item in blueprintJSON.Components)
			{
				blueprint.AddComponent(ComponentJSON.FromJSON(item.Value, item.Key));
			}

			foreach (ConnectionJSON connectionJSON in blueprintJSON.Connections)
			{
				blueprint.Connect(
					blueprint.GetComponent(connectionJSON.FromId),
					connectionJSON.FromIndex,
					blueprint.GetComponent(connectionJSON.ToId),
					connectionJSON.ToIndex);
			}

			return blueprint;
		}

		public IDictionary<string, ComponentJSON> Components { get; set; }
		public IEnumerable<ConnectionJSON> Connections { get; set; }
	}

	struct ConnectionJSON
	{
		public static ConnectionJSON ToJSON(string fromId, Connection connection)
		{
			return new ConnectionJSON
			{
				FromId = fromId,
				FromIndex = connection.FromIndex,
				ToId = connection.Other.Id,
				ToIndex = connection.ToIndex
			};
		}

		public string FromId { get; set; }
		public int FromIndex { get; set; }
		public string ToId { get; set; }
		public int ToIndex { get; set; }
	}
}