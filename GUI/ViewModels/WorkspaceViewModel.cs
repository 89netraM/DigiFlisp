using GUI.File;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GUI.ViewModels
{
	public class WorkspaceViewModel : ViewModelBase, IErrorProducer
	{
		private readonly string path;

		public event EventHandler<Exception> Error;

		public ObservableCollection<BlueprintViewModel> Items { get; }
		public int SelectedIndex { get; set; } = -1;

		public WorkspaceViewModel(string path)
		{
			this.path = path;
			Items = new ObservableCollection<BlueprintViewModel>();
		}

		public void AddWorkspaceItem(Blueprint model)
		{
			BlueprintViewModel bvm = new BlueprintViewModel(model);
			bvm.Error += Blueprint_Error;
			Items.Add(bvm);
		}

		public void AddComponent(string typeId)
		{
			if (0 <= SelectedIndex && SelectedIndex < Items.Count)
			{
				try
				{
					Items[SelectedIndex].AddComponent(typeId);
				}
				catch
				{
					Items[SelectedIndex].AddCustomComponent(Items.First(x => x.Name == typeId).blueprint);
				}
			}
		}

		public void SaveWorkspaceItems()
		{
			ReaderWriter.WriteAll(path, Items.Select(x => x.blueprint));
		}

		private void Blueprint_Error(object sender, Exception ex)
		{
			Error?.Invoke(sender, ex);
		}
	}
}