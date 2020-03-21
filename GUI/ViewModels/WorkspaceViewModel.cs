using Model;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
	public class WorkspaceViewModel : ViewModelBase
	{
		public ObservableCollection<BlueprintViewModel> Items { get; }
		public int SelectedIndex { get; set; } = -1;

		public WorkspaceViewModel()
		{
			Items = new ObservableCollection<BlueprintViewModel>();
		}

		public void AddWorkspaceItem(string name, Blueprint model)
		{
			Items.Add(new BlueprintViewModel(name, model));
		}

		public void AddComponent(string typeId)
		{
			if (0 <= SelectedIndex && SelectedIndex < Items.Count)
			{
				Items[SelectedIndex].AddComponent(typeId);
			}
		}
	}
}