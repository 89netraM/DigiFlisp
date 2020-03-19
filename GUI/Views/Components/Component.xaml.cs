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
		private readonly ComponentDataContext context = new ComponentDataContext();

		public new object Content
		{
			get => context.Content;
			set => context.Content = value;
		}
		public ComponentViewModel Model
		{
			get => context.Model;
			set => context.Model = value;
		}

		public Component()
		{
			this.InitializeComponent();
			DataContext = context;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void InputTapped(object sender, RoutedEventArgs e)
		{
			Model?.InputSignalTapped((e.Source as Rectangle)?.Tag as SignalViewModel);
		}
		private void OutputTapped(object sender, RoutedEventArgs e)
		{
			Model?.OutputSignalTapped((e.Source as Rectangle)?.Tag as SignalViewModel);
		}
	}

	class ComponentDataContext : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private object content;
		public object Content
		{
			get => content;
			set => SetAndNotify(ref content, value);
		}

		private ComponentViewModel model;
		public ComponentViewModel Model
		{
			get => model;
			set => SetAndNotify(ref model, value);
		}

		private void SetAndNotify<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
		{
			if (!Object.Equals(property, value))
			{
				property = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
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
