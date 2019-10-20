using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using Model.UnitTests.util;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class NotGateTests
	{
		[TestMethod]
		public void ShouldStartAsTrue()
		{
			Component notGate = new NotGate("testId");

			Assert.IsTrue(notGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void FalseIsTrue()
		{
			Signal signal = new Signal("testA", 0);

			Component notGate = new NotGate("testId");
			notGate.SetInput(0, signal);

			Assert.IsTrue(notGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void TrueIsFalse()
		{
			Signal signal = new Signal("testA", 0);
			signal.Update(true);

			Component notGate = new NotGate("testId");
			notGate.SetInput(0, signal);

			Assert.IsFalse(notGate.OutputSignals[0].Value);
		}
	}
}