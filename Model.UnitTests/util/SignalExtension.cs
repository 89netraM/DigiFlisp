using System;
using System.Collections.Generic;

namespace Model.UnitTests.util
{
	public static class SignalExtension
	{
		public static void Update(this Signal signal, bool newValue)
		{
			signal.Update(new Stack<UpdateRecord>(), newValue);
		}
	}
}