using Avalonia;
using System;
using ReactiveUI;
using Model;
using System.Linq;
using System.Collections.Generic;

namespace GUI.ViewModels
{
	public class ComponentViewModel : ViewModelBase
	{
		private readonly Component model;

		public string Id => model.Id;

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

		private readonly SignalViewModel[] inputSignals;
		private readonly SignalViewModel[] outputSignals;
		public IEnumerable<SignalViewModel> InputSignals => inputSignals;
		public IEnumerable<SignalViewModel> OutputSignals => outputSignals;

		public ComponentViewModel(Component model)
		{
			this.model = model ?? throw new ArgumentNullException(nameof(model));
			this.model.Position.ChangeEvent += Position_ChangeEvent;

			int maxNrConnections = Math.Max(this.model.InputSignals.Count, this.model.OutputSignals.Count);
			Size = new Size(5, 3 + 2 * (maxNrConnections - 1));

			inputSignals = this.model.InputSignals.Select((x, i) => new SignalViewModel(i, x)).ToArray();
			outputSignals = this.model.OutputSignals.Select((x, i) => new SignalViewModel(i, x)).ToArray();
			this.model.InputSignalChanged += Model_InputSignalChanged;
		}

		private void Position_ChangeEvent(object sender, PositionEvent e)
		{
			this.RaisePropertyChanged(nameof(Position));
		}

		private void Model_InputSignalChanged(object sender, InputSignalChangeEvent e)
		{
			inputSignals[e.Index].Signal = model.InputSignals[e.Index];
		}

		public void InputSignalTapped(SignalViewModel signal)
		{
			throw new NotImplementedException();
		}
		public void OutputSignalTapped(SignalViewModel signal)
		{
			throw new NotImplementedException();
		}
	}
}