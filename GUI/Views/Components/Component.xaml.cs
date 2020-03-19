using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using GUI.Views.Infinite;
using System;
using System.ComponentModel;
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
}
