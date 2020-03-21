using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI.Views.Components
{
	public class OutputComponent : UserControl
	{
		public OutputComponent()
		{
			this.InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
