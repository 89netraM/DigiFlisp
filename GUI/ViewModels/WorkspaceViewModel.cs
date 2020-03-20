using Model;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
	public class WorkspaceViewModel : ViewModelBase
	{
		public ObservableCollection<BlueprintViewModel> Items { get; }

		public WorkspaceViewModel()
		{
			Items = new ObservableCollection<BlueprintViewModel>();
		}

		public void AddWorkspaceItem(string name, Blueprint model)
		{
			Items.Add(new BlueprintViewModel(name, model));
		}
	}
}