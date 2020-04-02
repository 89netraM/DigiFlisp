using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.Views
{
	public class ErrorDialog : Window
	{
		private readonly TextBlock MessageBlock;

		public string Message
		{
			get => MessageBlock.Text;
			set => MessageBlock.Text = value;
		}

		public ErrorDialog()
		{
			this.InitializeComponent();

			MessageBlock = this.FindControl<TextBlock>("MessageBlock");
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
