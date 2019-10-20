using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using Model.UnitTests.util;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class OrGateTests
	{
		[TestMethod]
		public void ShouldStartAsFalse()
		{
			Component orGate = new OrGate("testId", 2);

			Assert.IsFalse(orGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void FalseAndFalseIsFalse()
		{
			Signal a = new Signal("testA", 0);

			Signal b = new Signal("testB", 0);

			Component orGate = new OrGate("testId", 2);
			orGate.SetInput(0, a);
			orGate.SetInput(1, b);

			Assert.IsFalse(orGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void TrueAndFalseIsTrue()
		{
			Signal a = new Signal("testA", 0);
			a.Update(true);

			Signal b = new Signal("testB", 0);

			Component orGate = new OrGate("testId", 2);
			orGate.SetInput(0, a);
			orGate.SetInput(1, b);

			Assert.IsTrue(orGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void TrueAndTrueIsTrue()
		{
			Signal a = new Signal("testA", 0);
			a.Update(true);

			Signal b = new Signal("testB", 0);
			b.Update(true);

			Component orGate = new OrGate("testId", 2);
			orGate.SetInput(0, a);
			orGate.SetInput(1, b);

			Assert.IsTrue(orGate.OutputSignals[0].Value);
		}

		[TestMethod]
		public void FalseAndTrueIsTrue()
		{
			Signal a = new Signal("testA", 0);

			Signal b = new Signal("testB", 0);
			a.Update(true);

			Component orGate = new OrGate("testId", 2);
			orGate.SetInput(0, a);
			orGate.SetInput(1, b);

			Assert.IsTrue(orGate.OutputSignals[0].Value);
		}
	}
}