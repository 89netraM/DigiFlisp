using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using GUI.Views.Infinite;
using Model;
using System;
using System.Linq;
using System.Diagnostics;
using Avalonia.Media;

namespace GUI.Views
{
	public class Blueprint : UserControl
	{
		private readonly InfiniteCanvas canvas;

		private Model.Blueprint model;

		public Blueprint()
		{
			this.InitializeComponent();

			canvas = this.FindControl<InfiniteCanvas>("canvas");

			DataContextChanged += Blueprint_DataContextChanged;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void Blueprint_DataContextChanged(object sender, EventArgs e)
		{
			if (model != null)
			{
				model.ComponentEvent -= Model_ComponentEvent;
			}

			model = DataContext as Model.Blueprint;

			if (model != null)
			{
				model.ComponentEvent += Model_ComponentEvent;
			}

			UpdateView();
		}

		private void Model_ComponentEvent(object sender, ComponentEventArg e)
		{
			if (e.Added)
			{
				AddComponent(e.AffectedComponent);
			}
			else
			{
				RemoveComponent(e.AffectedComponent);
			}
		}

		private void UpdateView()
		{
			canvas.Children.Clear();

			if (model != null)
			{
				for (int i = 0; i < model.ComponentCount; i++)
				{
					AddComponent(model.GetComponent(i));
				}
			}
		}

		private void AddComponent(Component component)
		{
			// Temporary component representation
			InfiniteCanvasBlock block = new InfiniteCanvasBlock()
			{
				Name = component.Id
			};
			InfiniteCanvas.SetPosition(block, new Point(component.Position.X, component.Position.Y));
			InfiniteCanvas.SetSize(block, new Size(5, 5));
			block.Content = new TextBlock()
			{
				Text = component.Id,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
			};

			canvas.Children.Add(block);
		}

		private void RemoveComponent(Component component)
		{
			canvas.Children.Remove(canvas.Children.FirstOrDefault(x => x.Name == component.Id));
		}
	}
}
