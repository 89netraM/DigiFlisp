using Avalonia;
using System;
using ReactiveUI;
using Model;
using System.Collections.Generic;

namespace GUI.ViewModels
{
	public class ComponentViewModel : ViewModelBase
	{
		private readonly Component model;

		public Point Position
		{
			get => new Point(model.Position.X, model.Position.Y);
			set
			{
				if (model.Position.X != value.X && model.Position.Y != value.Y)
				{
					model.Position.X = Convert.ToInt32(value.X);
					model.Position.Y = Convert.ToInt32(value.Y);
					this.RaisePropertyChanged();
				}
				else if (model.Position.X != value.X)
				{
					model.Position.X = Convert.ToInt32(value.X);
					this.RaisePropertyChanged();
				}
				else if (model.Position.Y != value.Y)
				{
					model.Position.Y = Convert.ToInt32(value.Y);
					this.RaisePropertyChanged();
				}
			}
		}

		public Size Size { get; }

		public IEnumerable<Signal> InputSignals => model.InputSignals;
		public IEnumerable<Signal> OutputSignals => model.OutputSignals;

		public ComponentViewModel(Component model)
		{
			this.model = model ?? throw new ArgumentNullException(nameof(model));
			this.model.Position.ChangeEvent += Position_ChangeEvent;

			int maxNrConnections = Math.Max(this.model.InputSignals.Count, this.model.OutputSignals.Count);
			Size = new Size(5, 3 + 2 * (maxNrConnections - 1));
		}

		private void Position_ChangeEvent(object sender, PositionEvent e)
		{
			this.RaisePropertyChanged(nameof(Position));
		}

		public void InputSignalTapped(Signal signal)
		{
			throw new NotImplementedException();
		}
		public void OutputSignalTapped(Signal signal)
		{
			throw new NotImplementedException();
		}
	}
}