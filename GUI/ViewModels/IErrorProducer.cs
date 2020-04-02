using System;

namespace GUI.ViewModels
{
	interface IErrorProducer
	{
		public event EventHandler<Exception> Error;
	}
}