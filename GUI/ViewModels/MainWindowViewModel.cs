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

		public ReactiveCommand<Unit, Unit> ExitCommand { get; }
		public Action CloseAction { get; set; }

		public MainWindowViewModel()
		{
			ComponentList = new ComponentListViewModel();

			ExitCommand = ReactiveCommand.Create(ExitCommandAction);
		}

		private void ExitCommandAction()
		{
			CloseAction?.Invoke();
		}
	}
}