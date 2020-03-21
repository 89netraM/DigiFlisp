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
		}

		public void AddComponent(string typeId)
		{
			blueprint.AddComponent(Model.Components.ComponentFactory.CreateComponent(typeId));
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