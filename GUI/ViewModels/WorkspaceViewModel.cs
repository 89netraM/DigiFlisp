using GUI.File;
using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI.ViewModels
{
	public class WorkspaceViewModel : ViewModelBase
	{
		private readonly string path;

		public ObservableCollection<BlueprintViewModel> Items { get; }
		public int SelectedIndex { get; set; } = -1;

		public WorkspaceViewModel(string path)
		{
			this.path = path;
			Items = new ObservableCollection<BlueprintViewModel>();
		}

		public void AddWorkspaceItem(Blueprint model)
		{
			Items.Add(new BlueprintViewModel(model));
		}

		public void AddComponent(string typeId)
		{
			if (0 <= SelectedIndex && SelectedIndex < Items.Count)
			{
				Items[SelectedIndex].AddComponent(typeId);
			}
		}

		public void SaveWorkspaceItems()
		{
			ReaderWriter.WriteAll(path, Items.Select(x => x.blueprint));
		}
	}
}