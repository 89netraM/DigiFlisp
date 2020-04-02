using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using System;
using System.Threading.Tasks;

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
				NewAction = new Func<Task<string>>(NewDialog),
				OpenAction = new Func<Task<string>>(OpenDialog),
				ErrorAction = new Action<string>(ErrorDialog)
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private Task<string> NewDialog()
		{
			TextDialog dialog = new TextDialog
			{
				Title = "Add New Blueprint",
				Description = "Create a new blueprint with the following name:",
				Placeholder = "Name",
				AcceptPrompt = "Create"
			};
			return dialog.ShowDialog<string>(this);
		}

		private Task<string> OpenDialog()
		{
			OpenFolderDialog dialog = new OpenFolderDialog
			{
				Title = "Open a Workspace"
			};
			return dialog.ShowAsync(this);
		}

		private void ErrorDialog(string message)
		{
			ErrorDialog dialog = new ErrorDialog
			{
				Title = "An Error Occured",
				Message = message
			};
			dialog.ShowDialog(this);
		}
	}
}
