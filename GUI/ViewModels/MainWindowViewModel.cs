using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace GUI.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, Unit> ExitCommand { get; }
		public Action CloseAction { get; set; }

		public MainWindowViewModel()
		{
			ExitCommand = ReactiveCommand.Create(ExitCommandAction);
		}

		private void ExitCommandAction()
		{
			CloseAction?.Invoke();
		}
	}
}