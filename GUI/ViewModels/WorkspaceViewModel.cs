using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GUI.ViewModels
{
	public class WorkspaceViewModel : ViewModelBase
	{
		public ObservableCollection<WorkspaceItem> Items { get; }

		public WorkspaceViewModel()
		{
			Items = new ObservableCollection<WorkspaceItem>();
		}

		public void AddWorkspaceItem(string name, Blueprint model)
		{
			Items.Add(new WorkspaceItem(name, model));
		}
	}

	public class WorkspaceItem
	{
		public string Name { get; }
		public Blueprint Blueprint { get; }

		public WorkspaceItem(string name, Blueprint blueprint)
		{
			Name = name;
			Blueprint = blueprint;
		}
	}
}