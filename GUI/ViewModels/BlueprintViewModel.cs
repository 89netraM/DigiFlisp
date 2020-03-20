using GUI.ViewModels.Components;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI.ViewModels
{
	public class BlueprintViewModel
	{
		public string Name { get; }
		private readonly Blueprint blueprint;

		public ObservableCollection<ComponentViewModel> Components { get; }

		public BlueprintViewModel(string name, Blueprint blueprint)
		{
			Components = new ObservableCollection<ComponentViewModel>();

			Name = name;
			this.blueprint = blueprint;
			this.blueprint.ComponentEvent += Blueprint_ComponentEvent;

			var a = new Model.Components.AndGate("and", 2);
			a.Position.X = a.Position.Y = 5;
			this.blueprint.AddComponent(a);
			this.blueprint.AddComponent(new Model.Components.NotGate("not"));
		}

		private void Blueprint_ComponentEvent(object sender, ComponentEventArg e)
		{
			if (e.Added)
			{
				ComponentViewModel c = ComponentFactory.Create(e.AffectedComponent);
				c.Connect += Component_Connect;
				Components.Add(c);
			}
			else
			{
				ComponentViewModel c = Components.First(c => c.Id == e.AffectedComponent.Id);
				c.Connect -= Component_Connect;
				Components.Remove(c);
			}
		}

		private void Component_Connect(object sender, ConnectEvent e)
		{
			throw new NotImplementedException();
		}
	}
}