using GUI.ViewModels;
using GUI.Views.Components;
using GUI.Views.Infinite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GUI.Views
{
	public class Blueprint : InfiniteCanvas
	{
		private BlueprintViewModel model;

		public Blueprint()
		{
			DataContextChanged += Blueprint_DataContextChanged;
		}

		private void Blueprint_DataContextChanged(object sender, EventArgs e)
		{
			if (model is BlueprintViewModel)
			{
				model.Components.CollectionChanged -= Components_CollectionChanged;
			}

			model = DataContext as BlueprintViewModel;

			Children.Clear();
			if (model is BlueprintViewModel)
			{
				model.Components.CollectionChanged += Components_CollectionChanged;

				foreach (var item in model.Components)
				{
					AddComponent(item);
				}
			}
		}

		private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach (var item in e.OldItems)
			{
				if (item is ComponentViewModel component)
				{
					RemoveComponent(component);
				}
			}
			foreach (var item in e.NewItems)
			{
				if (item is ComponentViewModel component)
				{
					AddComponent(component);
				}
			}
		}

		private void AddComponent(ComponentViewModel component)
		{
			Children.Add(ComponentFactory.Create(component));
		}

		private void RemoveComponent(ComponentViewModel component)
		{
			Children.Remove(Children.FirstOrDefault(x => x.Name == component.Id));
		}
	}
}