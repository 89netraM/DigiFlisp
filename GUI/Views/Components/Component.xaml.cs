using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using GUI.ViewModels;
using GUI.Views.Infinite;
using Model;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GUI.Views.Components
{
	public class Component : InfiniteCanvasBlock
	{
		private readonly Decorator contentPresenter;

		public new IControl Content
		{
			get => contentPresenter.Child;
			set => contentPresenter.Child = value;
		}

		public Component()
		{
			this.InitializeComponent();

			contentPresenter = this.FindControl<Decorator>("contentPresenter");
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void InputTapped(object sender, RoutedEventArgs e)
		{
			(DataContext as ComponentViewModel)?.InputSignalTapped((e.Source as Rectangle)?.Tag as SignalViewModel);
		}
		private void OutputTapped(object sender, RoutedEventArgs e)
		{
			(DataContext as ComponentViewModel)?.OutputSignalTapped((e.Source as Rectangle)?.Tag as SignalViewModel);
		}
	}

	public class SignalToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Signal model)
			{
				return new SolidColorBrush(model.Value ? 0xFF00FF00 : 0xFFFF0000);
			}
			else
			{
				return new SolidColorBrush(Colors.Gray);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
