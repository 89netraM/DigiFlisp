﻿using GUI.File;
using Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
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
				}
			}
		}

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
		}

		private async void NewCommandAction()
		{
			string newName = NewAction is object ? await NewAction.Invoke() : null;

			if (newName != null)
			{
				Workspace?.AddWorkspaceItem(newName, new Blueprint());
			}
		}

		private async void OpenCommandAction()
		{
			string folderPath = OpenAction is object ? await OpenAction.Invoke() : null;

			if (folderPath != null)
			{
				Workspace = new WorkspaceViewModel(folderPath);

				foreach ((string, Blueprint) item in await ReaderWriter.ReadAll(folderPath))
				{
					Workspace.AddWorkspaceItem(item.Item1, item.Item2);
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