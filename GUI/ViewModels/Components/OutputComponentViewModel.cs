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

		public Signal InputSignal { get; private set; }

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
				if (InputSignal is Signal)
				{
					InputSignal.ValueChange -= InputSignal_ValueChange;
				}

				InputSignal = model.InputSignals[0];
				this.RaisePropertyChanged(nameof(InputSignal));

				if (InputSignal is Signal)
				{
					InputSignal.ValueChange += InputSignal_ValueChange;
				}
			}
		}

		private void InputSignal_ValueChange(object sender, EventArgs e)
		{
			this.RaisePropertyChanged(nameof(InputSignal));
		}
	}
}