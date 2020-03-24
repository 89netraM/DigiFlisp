using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using GUI.ViewModels;
using GUI.ViewModels.Components;
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

			KeyDown += Blueprint_KeyDown;
			Tapped += Blueprint_Tapped;
		}

		private void Blueprint_DataContextChanged(object sender, EventArgs e)
		{
			if (model is BlueprintViewModel)
			{
				model.Components.CollectionChanged -= Components_CollectionChanged;
				model.Connections.CollectionChanged -= Connections_CollectionChanged;
			}

			model = DataContext as BlueprintViewModel;

			Children.Clear();
			if (model is BlueprintViewModel)
			{
				model.Components.CollectionChanged += Components_CollectionChanged;
				model.Connections.CollectionChanged += Connections_CollectionChanged;

				foreach (var item in model.Components)
				{
					AddComponent(item);
				}
				foreach (var item in model.Connections)
				{
					AddConnection(item);
				}
			}
		}

		private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems is object)
			{
				foreach (var item in e.OldItems)
				{
					if (item is ComponentViewModel component)
					{
						RemoveComponent(component);
					}
				}
			}
			if (e.NewItems is object)
			{
				foreach (var item in e.NewItems)
				{
					if (item is ComponentViewModel component)
					{
						AddComponent(component);
					}
				}
			}
		}

		private void Connections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems is object)
			{
				foreach (var item in e.OldItems)
				{
					if (item is ConnectionViewModel connection)
					{
						RemoveConnection(connection);
					}
				}
			}
			if (e.NewItems is object)
			{
				foreach (var item in e.NewItems)
				{
					if (item is ConnectionViewModel connection)
					{
						AddConnection(connection);
					}
				}
			}
		}

		private void Blueprint_Tapped(object sender, RoutedEventArgs e)
		{
			if (e.Source == this)
			{
				model.CancelConnectionCommand.Execute().Subscribe();
			}
		}

		private void Blueprint_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				model.CancelConnectionCommand.Execute().Subscribe();
			}
		}

		private void AddComponent(ComponentViewModel component)
		{
			Children.Add(Components.ComponentFactory.Create(component));
		}

		private void RemoveComponent(ComponentViewModel component)
		{
			Children.Remove(Children.FirstOrDefault(x => x.Name == component.Id));
		}

		private void AddConnection(ConnectionViewModel connection)
		{
			if (Children.FirstOrDefault(x => x.Name == connection.FromId) is Component fromComponent)
			{
				IControl from = fromComponent.GetOutputSignal(connection.FromIndex);

				if (Children.FirstOrDefault(x => x.Name == connection.ToId) is Component toComponent)
				{
					IControl to = toComponent.GetInputSignal(connection.ToIndex);

					Children.Add(new Connection(this, from, to)
					{
						Name = connection.ToString()
					});
				}
				else
				{
					throw new Exception("Could not create connection " + connection.ToString());
				}
			}
			else
			{
				throw new Exception("Could not create connection " + connection.ToString());
			}
		}

		private void RemoveConnection(ConnectionViewModel connection)
		{
			Children.Remove(Children.FirstOrDefault(x => x.Name == connection.ToString()));
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Size outSize = base.ArrangeOverride(arrangeSize);

			foreach (IControl child in Children)
			{
				if (child is Connection childConnection)
				{
					childConnection.SetScale(ZoomFactor);
					childConnection.Layout();
				}
			}

			return outSize;
		}
	}
}