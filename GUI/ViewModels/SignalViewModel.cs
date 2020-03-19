using Model;
using ReactiveUI;

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
				this.RaiseAndSetIfChanged(ref signal, value);
			}
		}

		public SignalViewModel(int index, Signal signal)
		{
			Index = index;
			this.signal = signal;
		}
	}
}