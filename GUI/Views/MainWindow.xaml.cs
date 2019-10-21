using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using System;

namespace GUI.Views
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

			DataContext = new MainWindowViewModel
			{
				CloseAction = new Action(this.Close),
				NewAction = new Func<string>(NewDialog)
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private string NewDialog()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}
	}
}
