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

		public void AddWorkspaceItem(string name)
		{
			Items.Add(new WorkspaceItem(name));
		}
	}

	public class WorkspaceItem
	{
		public string Name { get; }

		public WorkspaceItem(string name)
		{
			Name = name;
		}
	}
}