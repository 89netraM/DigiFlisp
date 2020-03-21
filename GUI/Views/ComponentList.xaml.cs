using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;

namespace GUI.Views
{
	public class ComponentList : UserControl
	{
		public ComponentList()
		{
			this.InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}

	public class CapitalizationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string str)
			{
				return char.ToUpper(str[0]) + str.Substring(1).ToLower();
			}
			else
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}