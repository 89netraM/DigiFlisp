using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GUI.Views.Components
{
	public class IconComponent : UserControl
	{
		public IconComponent()
		{
			this.InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}

	public class TypeIdToSymbolConverter : IValueConverter
	{
		private static readonly IReadOnlyDictionary<string, string> typeIdIconPair = new Dictionary<string, string>
		{
			[AndGate.AndTypeId] = "&",
			[NotGate.NotTypeId] = "!",
			[OrGate.OrTypeId] = ">1"
		};

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string typeId && typeIdIconPair.TryGetValue(typeId, out string icon))
			{
				return icon;
			}
			else
			{
				throw new ArgumentException($"Can not convert ({value}) to an icon.", nameof(value));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
