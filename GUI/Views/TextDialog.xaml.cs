using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;
using System.Globalization;

namespace GUI.Views
{
	public class TextDialog : Window, INotifyPropertyChanged
	{
		private readonly TextBlock DescriptionBlock;
		private readonly TextBox TextBox;
		private readonly Button AcceptButton;

		public string Description
		{
			get => DescriptionBlock.Text;
			set => DescriptionBlock.Text = value;
		}
		public string Placeholder
		{
			get => TextBox.Watermark;
			set => TextBox.Watermark = value;
		}
		public string AcceptPrompt
		{
			get => AcceptButton.Content as string;
			set => AcceptButton.Content = value;
		}

		public TextDialog()
		{
			this.InitializeComponent();

			DescriptionBlock = this.FindControl<TextBlock>("DescriptionBlock");
			TextBox = this.FindControl<TextBox>("TextBox");
			AcceptButton = this.FindControl<Button>("AcceptButton");

			TextBox.KeyDown += TextBox_KeyDown;
			AcceptButton.Tapped += AcceptButton_Tapped;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				Return();
			}
		}

		private void AcceptButton_Tapped(object sender, RoutedEventArgs e)
		{
			Return();
		}

		private void Return()
		{
			if (TextBox.Text is object && TextBox.Text.Length > 0)
			{
				Close(TextBox.Text);
			}
		}
	}

	public class TextToEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is string s && s.Length > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
