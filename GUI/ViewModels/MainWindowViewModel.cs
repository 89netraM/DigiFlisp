using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

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
				}
			}
		}

		public ReactiveCommand<Unit, Unit> NewCommand { get; }
		public Func<string> NewAction { get; set; }

		public ReactiveCommand<Unit, Unit> ExitCommand { get; }
		public Action CloseAction { get; set; }

		public MainWindowViewModel()
		{
			ComponentList = new ComponentListViewModel();
			ComponentList.AddComponent += ComponentList_AddComponent;
			Workspace = new WorkspaceViewModel();

			NewCommand = ReactiveCommand.Create(NewCommandAction);

			ExitCommand = ReactiveCommand.Create(ExitCommandAction);
		}

		private void NewCommandAction()
		{
			string newName = NewAction?.Invoke();

			if (newName != null)
			{
				Workspace.AddWorkspaceItem(newName, new Blueprint());
			}
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