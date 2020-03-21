using Model;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.ViewModels.Components
{
	public class OutputComponentViewModel : ComponentViewModel
	{
		private readonly OutputComponent model;

		public SignalViewModel InputSignal { get; }

		public OutputComponentViewModel(Component model) : base(model)
		{
			this.model = (model as OutputComponent) ?? throw new ArgumentException("The model of a OutputComponentViewModel must be OutputComponent", nameof(model));

			InputSignal = new SignalViewModel(0, this.model.InputSignals[0]);
			this.model.InputSignalChanged += Model_InputSignalChanged;
		}

		private void Model_InputSignalChanged(object sender, InputSignalChangeEvent e)
		{
			if (e.Index == 0)
			{
				InputSignal.Signal = model.InputSignals[0];
			}
		}
	}
}