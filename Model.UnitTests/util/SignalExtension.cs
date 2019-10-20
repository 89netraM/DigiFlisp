using System;

namespace Model.UnitTests.util
{
	public static class SignalExtension
	{
		public static void Update(this Signal signal, bool newValue)
		{
			signal.Update(Guid.NewGuid().ToString(), newValue);
		}
	}
}