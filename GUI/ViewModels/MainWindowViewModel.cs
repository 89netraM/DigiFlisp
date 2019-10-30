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
		public WorkspaceViewModel Workspace { get; }

		public ReactiveCommand<Unit, Unit> NewCommand { get; }
		public Func<string> NewAction { get; set; }

		public ReactiveCommand<Unit, Unit> ExitCommand { get; }
		public Action CloseAction { get; set; }

		public MainWindowViewModel()
		{
			ComponentList = new ComponentListViewModel();
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
	}
}