using Model;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace GUI.ViewModels.Components
{
	public class OutputComponentViewModel : ComponentViewModel
	{
		private readonly OutputComponent model;

		private Signal inputSignal = null;
		public Signal InputSignal
		{
			get => inputSignal;
			private set
			{
				if (inputSignal is Signal)
				{
					inputSignal.ValueChange -= InputSignal_ValueChange;
				}

				inputSignal = value;
				this.RaisePropertyChanged(nameof(InputSignal));

				if (inputSignal is Signal)
				{
					inputSignal.ValueChange += InputSignal_ValueChange;
				}
			}
		}

		public OutputComponentViewModel(Component model) : base(model)
		{
			this.model = (model as OutputComponent) ?? throw new ArgumentException("The model of a OutputComponentViewModel must be OutputComponent", nameof(model));

			InputSignal = this.model.InputSignals[0];
			this.model.InputSignalChanged += Model_InputSignalChanged;
		}

		private void Model_InputSignalChanged(object sender, InputSignalChangeEvent e)
		{
			if (e.Index == 0)
			{
				InputSignal = model.InputSignals[0];
			}
		}

		private void InputSignal_ValueChange(object sender, EventArgs e)
		{
			this.RaisePropertyChanged(nameof(InputSignal));
		}
	}
}