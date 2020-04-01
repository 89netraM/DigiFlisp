using Model.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
	public class Blueprint
	{
		public event EventHandler<ConnectionEventArg> ConnectionEvent;
		public event EventHandler<ComponentEventArg> ComponentEvent;

		private readonly IDictionary<string, Component> componentList = new Dictionary<string, Component>();

		public int ComponentCount => componentList.Count;

		public string Id { get; }

		public Blueprint(string id)
		{
			Id = id ?? throw new ArgumentNullException(nameof(id));
		}

		public Component GetComponent(int index)
		{
			return componentList.Values.ElementAt(index);
		}

		public Component GetComponent(string id)
		{
			return componentList[id];
		}

		public void AddComponent(Component component)
		{
			if (componentList.ContainsKey(component.Id))
			{
				throw new ArgumentException(nameof(component), "Can't add a component that's already included in this component.");
			}

			if (component is CustomComponent c && c.ContainsBlueprint(Id))
			{
				throw new Exception("A Blueprint can't contain a CustomComponent based on itself, directly or indirectly.");
			}

			componentList.Add(component.Id, component);

			ComponentEvent?.Invoke(this, (component, true));
		}

		public void RemoveComponent(Component component)
		{
			if (!componentList.ContainsKey(component.Id))
			{
				throw new ArgumentException(nameof(component), "Can't remove a component unless it's included in this Blueprint.");
			}

			RemoveAllConnections(component);

			componentList.Remove(component.Id);

			ComponentEvent?.Invoke(this, (component, false));
		}

		public void Connect(Component fromComponent, int fromIndex, Component toComponent, int toIndex)
		{
			if (!componentList.ContainsKey(fromComponent.Id))
			{
				throw new ArgumentException(nameof(fromComponent), "Can't make a connection between two components unless both are included in this Blueprint.");
			}
			if (!componentList.ContainsKey(toComponent.Id))
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

			ConnectionEvent?.Invoke(this, (fromComponent, fromIndex, toComponent, toIndex, true));
		}

		public void Disconnect(Component component, int index)
		{
			if (!componentList.ContainsKey(component.Id))
			{
				throw new ArgumentException(nameof(component), "Can't remove a connection between two components unless both are included in this Blueprint.");
			}

			if (component.InputSignals.Count <= index)
			{
				throw new ArgumentOutOfRangeException(nameof(component), "Can't remove a connection to a Signal on an index out of range.");
			}

			if (!(component.InputSignals[index] is object))
			{
				throw new ArgumentException(nameof(index), "Can't dissconnect a Signal that isn't connected to anything.");
			}

			Component fromComponent = GetComponent(component.InputSignals[index].OwnerId);
			// IReadOnlyList dosn't have IndexOf 😢
			int fromIndex = fromComponent.OutputSignals.Zip(Enumerable.Range(0, fromComponent.OutputSignals.Count)).First(x => x.First == component.InputSignals[index]).Second;

			component.SetInput(index, null);

			ConnectionEvent?.Invoke(this, (fromComponent, fromIndex, component, index, false));
		}

		public IReadOnlyCollection<Connection> IncomingConnectionsFor(Component component)
		{
			List<Connection> connections = new List<Connection>();

			for (int i = 0; i < component.InputSignals.Count; i++)
			{
				if (component.InputSignals[i] is object)
				{
					Component connectingComponent = GetComponent(component.InputSignals[i].OwnerId);
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

			foreach (Component other in componentList.Values)
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
			if (!componentList.ContainsKey(component.Id))
			{
				throw new ArgumentException(nameof(component), "Can't handle connections unless the component is included in this Blueprint.");
			}

			foreach (Connection connection in IncomingConnectionsFor(component))
			{
				Disconnect(
					component,
					connection.ToIndex
				);
			}

			foreach (Connection connection in OutgoingConnectionsFor(component))
			{
				Disconnect(
					connection.Other,
					connection.ToIndex
				);
			}
		}

		public void ReplaceComponent(Component oldComponent, Component newComponent)
		{
			if (!componentList.ContainsKey(oldComponent.Id))
			{
				throw new ArgumentException(nameof(oldComponent), "Can't remove a component unless it's included in this Blueprint.");
			}
			if (componentList.ContainsKey(newComponent.Id))
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

	public struct ConnectionEventArg
	{
		public Component FromComponent { get; }
		public int FromIndex { get; }
		public Component ToComponent { get; }
		public int ToIndex { get; }
		public bool Connected { get; }

		public ConnectionEventArg(Component fromComponent, int fromIndex, Component toComponent, int toIndex, bool connected)
		{
			FromComponent = fromComponent;
			FromIndex = fromIndex;
			ToComponent = toComponent;
			ToIndex = toIndex;
			Connected = connected;
		}

		public static implicit operator ConnectionEventArg(ValueTuple<Component, int, Component, int, bool> tupple)
		{
			return new ConnectionEventArg(tupple.Item1, tupple.Item2, tupple.Item3, tupple.Item4, tupple.Item5);
		}
	}

	public struct ComponentEventArg
	{
		public Component AffectedComponent { get; }
		public bool Added { get; }

		public ComponentEventArg(Component affectedComponent, bool added)
		{
			AffectedComponent = affectedComponent;
			Added = added;
		}

		public static implicit operator ComponentEventArg(ValueTuple<Component, bool> tupple)
		{
			return new ComponentEventArg(tupple.Item1, tupple.Item2);
		}
	}
}