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
				NewAction = new Func<Task<string>>(NewDialog)
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private async Task<string> NewDialog()
		{
			TextDialog dialog = new TextDialog
			{
				Title = "Add New Blueprint",
				Description = "Create a new blueprint with the following name:",
				Placeholder = "Name",
				AcceptPrompt = "Create"
			};
			return await dialog.ShowDialog<string>(this);
		}
	}
}
