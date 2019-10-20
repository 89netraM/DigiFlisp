using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using Model.UnitTests.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class OutputComponentTests
	{
		[TestMethod]
		public void ShoudlStartAsFalse()
		{
			OutputComponent outComponent = new OutputComponent("testId");

			Assert.IsFalse(outComponent.State);
		}

		[TestMethod]
		public void FalseIsFalse()
		{
			Signal signal = new Signal("testA", 0);

			OutputComponent outComponent = new OutputComponent("testId");
			outComponent.SetInput(0, signal);

			Assert.IsFalse(outComponent.State);
		}

		[TestMethod]
		public void TrueIsTrue()
		{
			Signal signal = new Signal("testA", 0);
			signal.Update(true);

			OutputComponent outComponent = new OutputComponent("testId");
			outComponent.SetInput(0, signal);

			Assert.IsTrue(outComponent.State);
		}

		[TestMethod]
		public void ShouldUpdateListeners()
		{
			Signal signal = new Signal("testA", 0);
			OutputComponent outComponent = new OutputComponent("testId");
			outComponent.SetInput(0, signal);
			bool hasListenerBeenInvoked = false;

			outComponent.IndicatorSignal.AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);
			signal.Update(true);

			Assert.IsTrue(hasListenerBeenInvoked, "The listeners of output should be called imeadietly after a state change.");
		}
	}
}