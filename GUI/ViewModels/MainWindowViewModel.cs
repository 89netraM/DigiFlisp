using GUI.File;
using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ComponentListViewModel ComponentList { get; }
		private WorkspaceViewModel workspace;
		public WorkspaceViewModel Workspace
		{
			get => workspace;
			set
			{
				if (workspace != value)
				{
					workspace = value;
					ComponentList.WorkspaceItems = workspace?.Items;

					this.RaisePropertyChanged(nameof(Workspace));
					this.RaisePropertyChanged(nameof(IsWorkspaceAvalible));
				}
			}
		}

		public bool IsWorkspaceAvalible => Workspace != null;

		public ReactiveCommand<Unit, Unit> NewCommand { get; }
		public Func<Task<string>> NewAction { get; set; }

		public ReactiveCommand<Unit, Unit> OpenCommand { get; }
		public Func<Task<string>> OpenAction { get; set; }

		public ReactiveCommand<Unit, Unit> SaveCommand { get; }

		public ReactiveCommand<Unit, Unit> ExitCommand { get; }
		public Action CloseAction { get; set; }

		public MainWindowViewModel()
		{
			ComponentList = new ComponentListViewModel();
			ComponentList.AddComponent += ComponentList_AddComponent;

			NewCommand = ReactiveCommand.Create(NewCommandAction);

			OpenCommand = ReactiveCommand.Create(OpenCommandAction);
			SaveCommand = ReactiveCommand.Create(SaveCommandAction);

			ExitCommand = ReactiveCommand.Create(ExitCommandAction);

			PropertyChanged += MainWindowViewModel_PropertyChanged;
		}

		private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IsWorkspaceAvalible))
			{
				ComponentList.IsEnabled = IsWorkspaceAvalible;
			}
		}

		private async void NewCommandAction()
		{
			string newName = NewAction is object ? await NewAction.Invoke() : null;

			if (newName != null)
			{
				Workspace?.AddWorkspaceItem(new Blueprint(newName));
			}
		}

		private async void OpenCommandAction()
		{
			string folderPath = OpenAction is object ? await OpenAction.Invoke() : null;

			if (folderPath != null)
			{
				Workspace = new WorkspaceViewModel(folderPath);

				foreach (Blueprint blueprint in await ReaderWriter.ReadAll(folderPath))
				{
					Workspace.AddWorkspaceItem(blueprint);
				}
			}
		}

		private void SaveCommandAction()
		{
			Workspace?.SaveWorkspaceItems();
		}

		private void ExitCommandAction()
		{
			CloseAction?.Invoke();
		}

		private void ComponentList_AddComponent(object sender, string typeId)
		{
			Workspace?.AddComponent(typeId);
		}
	}
}