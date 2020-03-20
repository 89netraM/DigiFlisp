using Model;
using ReactiveUI;
using System;

namespace GUI.ViewModels
{
	public class SignalViewModel : ViewModelBase
	{
		public int Index { get; }

		private Signal signal;
		public Signal Signal
		{
			get => signal;
			set
			{
				if (signal is object)
				{
					signal.ValueChange -= Signal_ValueChange;
				}

				this.RaiseAndSetIfChanged(ref signal, value);

				if (signal is object)
				{
					signal.ValueChange += Signal_ValueChange;
				}
			}
		}

		public SignalViewModel(int index, Signal signal)
		{
			Index = index;
			Signal = signal;
		}

		private void Signal_ValueChange(object sender, EventArgs e)
		{
			this.RaisePropertyChanged(nameof(Signal));
		}
	}
}