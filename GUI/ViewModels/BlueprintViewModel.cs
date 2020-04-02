using Avalonia;
using GUI.ViewModels.Components;
using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace GUI.ViewModels
{
	public class BlueprintViewModel
	{
		public string Name => blueprint.Id;
		internal readonly Blueprint blueprint;

		public Point? Offset { get; set; } = null;
		public int? ZoomPoint { get; set; } = null;

		public ObservableCollection<ComponentViewModel> Components { get; }

		public ReactiveCommand<Unit, Unit> CancelConnectionCommand { get; }
		private ConnectEvent? connectionStart = null;

		public ObservableCollection<ConnectionViewModel> Connections { get; }

		public BlueprintViewModel(Blueprint blueprint)
		{
			Components = new ObservableCollection<ComponentViewModel>();
			Connections = new ObservableCollection<ConnectionViewModel>();

			this.blueprint = blueprint;
			this.blueprint.ComponentEvent += Blueprint_ComponentEvent;
			this.blueprint.ConnectionEvent += Blueprint_ConnectionEvent;

			for (int i = 0; i < this.blueprint.ComponentCount; i++)
			{
				Component component = this.blueprint.GetComponent(i);
				AddComponentViewModel(component);
				IReadOnlyCollection<Connection> connections = this.blueprint.OutgoingConnectionsFor(component);
				foreach (Connection connection in connections)
				{
					Connections.Add(new ConnectionViewModel(component.Id, connection.FromIndex, connection.Other.Id, connection.ToIndex));
				}
			}

			CancelConnectionCommand = ReactiveCommand.Create(CancelConnectionAction);
		}

		private void Blueprint_ConnectionEvent(object sender, ConnectionEventArg e)
		{
			ConnectionViewModel connection = new ConnectionViewModel(e.FromComponent.Id, e.FromIndex, e.ToComponent.Id, e.ToIndex);

			if (e.Connected)
			{
				Connections.Add(connection);
			}
			else
			{
				Connections.Remove(connection);
			}
		}

		public void AddComponent(string typeId)
		{
			blueprint.AddComponent(Model.Components.ComponentFactory.CreateComponent(typeId));
		}
		public void AddCustomComponent(Blueprint customBlueprintModel)
		{
			blueprint.AddComponent(Model.Components.ComponentFactory.CreateCustomComponent(customBlueprintModel));
		}

		private void Blueprint_ComponentEvent(object sender, ComponentEventArg e)
		{
			if (e.Added)
			{
				AddComponentViewModel(e.AffectedComponent);
			}
			else
			{
				ComponentViewModel c = Components.First(c => c.Id == e.AffectedComponent.Id);
				c.Connect -= Component_Connect;
				Components.Remove(c);
			}
		}

		private void AddComponentViewModel(Component component)
		{
			ComponentViewModel c = ComponentFactory.Create(component);
			c.Connect += Component_Connect;
			Components.Add(c);
		}

		private void Component_Connect(object sender, ConnectEvent e)
		{
			if (connectionStart is ConnectEvent s)
			{
				if (!s.Equals(e) && s.IsInputSide != e.IsInputSide)
				{
					if (s.IsInputSide)
					{
						blueprint.Connect(e.Component, e.Index, s.Component, s.Index);
					}
					else
					{
						blueprint.Connect(s.Component, s.Index, e.Component, e.Index);
					}
				}

				connectionStart = null;
			}
			else
			{
				if (e.IsInputSide && e.Component.InputSignals[e.Index] is object)
				{
					blueprint.Disconnect(e.Component, e.Index);
				}

				connectionStart = e;
			}
		}

		private void CancelConnectionAction()
		{
			connectionStart = null;
		}
	}
}