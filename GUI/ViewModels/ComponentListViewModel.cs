using Model.Components;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Text;

namespace GUI.ViewModels
{
	public class ComponentListViewModel : ViewModelBase
	{
		public IEnumerable<string> StandardComponents => ComponentFactory.ComponentTypeIds;

		private ObservableCollection<BlueprintViewModel> workspaceItems;
		public ObservableCollection<BlueprintViewModel> WorkspaceItems
		{
			get => workspaceItems;
			set
			{
				if (workspaceItems is object)
				{
					workspaceItems.CollectionChanged -= WorkspaceItems_CollectionChanged;
				}

				this.RaiseAndSetIfChanged(ref workspaceItems, value);

				if (workspaceItems is object)
				{
					CustomComponents = new ObservableCollection<string>(workspaceItems.Select(x => x.Name));
					this.RaisePropertyChanged(nameof(CustomComponents));
					workspaceItems.CollectionChanged += WorkspaceItems_CollectionChanged;
				}
			}
		}
		public ObservableCollection<string> CustomComponents { get; private set; }

		private bool isEnabled = false;
		public bool IsEnabled
		{
			get => isEnabled;
			set => this.RaiseAndSetIfChanged(ref isEnabled, value);
		}

		public ReactiveCommand<string, Unit> AddComponentCommand { get; }
		public event EventHandler<string> AddComponent;

		public ComponentListViewModel()
		{
			AddComponentCommand = ReactiveCommand.Create<string, Unit>(AddComponentaction);
		}

		private void WorkspaceItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems is object)
			{
				foreach (var item in e.OldItems)
				{
					if (item is BlueprintViewModel viewModel)
					{
						CustomComponents.Remove(viewModel.Name);
					}
				}
			}
			if (e.NewItems is object)
			{
				foreach (var item in e.NewItems)
				{
					if (item is BlueprintViewModel viewModel)
					{
						CustomComponents.Add(viewModel.Name);
					}
				}
			}
		}

		private Unit AddComponentaction(string typeId)
		{
			AddComponent?.Invoke(this, typeId);

			return Unit.Default;
		}
	}
}