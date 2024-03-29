﻿using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Visuals;
using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;

namespace GUI.Views
{
	public class Connection : Line
	{
		private const int defaultZIndex = 20;

		private const double defaultThickness = 2.0d;

		private readonly IControl parent;
		private readonly IControl from;
		private readonly IControl to;

		public Connection(IControl parent, IControl from, IControl to)
		{
			this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
			this.from = from ?? throw new ArgumentNullException(nameof(from));
			this.to = to ?? throw new ArgumentNullException(nameof(to));

			ZIndex = defaultZIndex;
			Stroke = new SolidColorBrush(Colors.Black);
			StrokeThickness = defaultThickness;
			StrokeLineCap = PenLineCap.Round;

			Layout();
		}

		public void SetScale(double scale)
		{
			StrokeThickness = scale * defaultThickness;
		}

		public void Layout()
		{
			StartPoint = parent.PointToClient(from.PointToScreen(new Point(from.Width / 2.0d, from.Height / 2.0d)));
			EndPoint = parent.PointToClient(to.PointToScreen(new Point(to.Width / 2.0d, to.Height / 2.0d)));
		}
	}
}