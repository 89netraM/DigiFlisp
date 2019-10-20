using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class Blueprint
	{
		private readonly IList<Component> componentList = new List<Component>();

		public int ComponentCount => componentList.Count;

		public Component GetComponent(int index)
		{
			return componentList[index];
		}

		public void AddComponent(Component component)
		{
			if (componentList.Contains(component))
			{
				throw new ArgumentException(nameof(component), "Can't add a component that's already included in this component.");
			}

			componentList.Add(component);
		}

		public void RemoveComponent(Component component)
		{
			if (!componentList.Contains(component))
			{
				throw new ArgumentException(nameof(component), "Can't remove a component unless it's included in this Blueprint.");
			}

			RemoveAllConnections(component);

			componentList.Remove(component);
		}

		public void Connect(Component fromComponent, int fromIndex, Component toComponent, int toIndex)
		{
			if (!componentList.Contains(fromComponent))
			{
				throw new ArgumentException(nameof(fromComponent), "Can't make a connection between two components unless both are included in this Blueprint.");
			}
			if (!componentList.Contains(toComponent))
			{
				throw new ArgumentException(nameof(toComponent), "Can't make a connection between two components unless both are included in this Blueprint.");
			}

			if (fromIndex < 0 || fromComponent.OutputSignals.Count <= fromIndex)
			{
				throw new ArgumentOutOfRangeException(nameof(fromComponent), "Can't make a connection from a Signal on an index out of range.");
			}
			if (toIndex < 0 || toComponent.InputSignals.Count <= toIndex)
			{
				throw new ArgumentOutOfRangeException(nameof(toComponent), "Can't make a connection to a Signal on an index out of range.");
			}

			toComponent.SetInput(toIndex, fromComponent.OutputSignals[fromIndex]);
		}

		public void Disconnect(Component fromComponent, int fromIndex, Component toComponent, int toIndex)
		{
			if (!componentList.Contains(fromComponent))
			{
				throw new ArgumentException(nameof(fromComponent), "Can't remove a connection between two components unless both are included in this Blueprint.");
			}
			if (!componentList.Contains(toComponent))
			{
				throw new ArgumentException(nameof(toComponent), "Can't remove a connection between two components unless both are included in this Blueprint.");
			}

			if (fromComponent.OutputSignals.Count <= fromIndex)
			{
				throw new ArgumentOutOfRangeException(nameof(fromComponent), "Can't remove a connection from a Signal on an index out of range.");
			}
			if (toComponent.InputSignals.Count <= toIndex)
			{
				throw new ArgumentOutOfRangeException(nameof(toComponent), "Can't remove a connection to a Signal on an index out of range.");
			}

			toComponent.SetInput(toIndex, null);
		}

		public IReadOnlyCollection<Connection> IncomingConnectionsFor(Component component)
		{
			List<Connection> connections = new List<Connection>();

			for (int i = 0; i < component.InputSignals.Count; i++)
			{
				Component connectingComponent = componentList.FirstOrDefault(x => x.Id == component.InputSignals[i]?.OwnerId);
				if (connectingComponent != null)
				{
					connections.Add(new Connection(
						connectingComponent,
						component.InputSignals[i].Index,
						i
					));
				}
			}

			return connections;
		}
		public IReadOnlyCollection<Connection> OutgoingConnectionsFor(Component component)
		{
			List<Connection> connections = new List<Connection>();

			foreach (Component other in componentList)
			{
				for (int i = 0; i < other.InputSignals.Count; i++)
				{
					if (other.InputSignals[i]?.OwnerId == component.Id)
					{
						connections.Add(new Connection(other, i, other.InputSignals[i].Index));
					}
				}
			}

			return connections;
		}

		public void RemoveAllConnections(Component component)
		{
			if (!componentList.Contains(component))
			{
				throw new ArgumentException(nameof(component), "Can't handle connections unless the component is included in this Blueprint.");
			}

			foreach (Connection connection in IncomingConnectionsFor(component))
			{
				Disconnect(
					connection.Other,
					connection.FromIndex,
					component,
					connection.ToIndex
				);
			}

			foreach (Connection connection in OutgoingConnectionsFor(component))
			{
				Disconnect(
					component,
					connection.FromIndex,
					connection.Other,
					connection.ToIndex
				);
			}
		}

		public void ReplaceComponent(Component oldComponent, Component newComponent)
		{
			if (!componentList.Contains(oldComponent))
			{
				throw new ArgumentException(nameof(oldComponent), "Can't remove a component unless it's included in this Blueprint.");
			}
			if (componentList.Contains(newComponent))
			{
				throw new ArgumentException(nameof(newComponent), "Can't add a component that's already included in this component.");
			}

			IReadOnlyCollection<Connection> oldIncoming = IncomingConnectionsFor(oldComponent);
			IReadOnlyCollection<Connection> oldOutgoing = OutgoingConnectionsFor(oldComponent);

			RemoveComponent(oldComponent);
			AddComponent(newComponent);

			for (int i = 0; i < oldIncoming.Count && i < newComponent.InputSignals.Count; i++)
			{
				Connection connection = oldIncoming.ElementAt(i);
				Connect(
					connection.Other,
					connection.FromIndex,
					newComponent,
					connection.ToIndex
				);
			}
			for (int i = 0; i < oldOutgoing.Count && i < newComponent.OutputSignals.Count; i++)
			{
				Connection connection = oldOutgoing.ElementAt(i);
				Connect(
					newComponent,
					connection.FromIndex,
					connection.Other,
					connection.ToIndex
				);
			}
		}
	}
}