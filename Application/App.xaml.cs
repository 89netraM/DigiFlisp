using Avalonia;
using Avalonia.Markup.Xaml;

namespace Application
{
	public class App : Avalonia.Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
