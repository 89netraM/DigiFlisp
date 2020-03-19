using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UnitTests
{
	[TestClass]
	public class ComponentTests
	{
		[TestMethod]
		public void ShouldInvokeEventsOnInputAdd()
		{
			const int inputIndex = 0;

			Component component = new AndGate("testId", 1);
			Signal signal = new Signal("test", 0);
			bool hasEventBeenInvoked = false;

			component.InputSignalChanged += (sender, e) =>
			{
				hasEventBeenInvoked = true;
				Assert.AreEqual(inputIndex, e.Index, "The events index should be the correct index.");
				Assert.IsTrue(e.WasAdded, "It should be an add-event.");
			};

			component.SetInput(inputIndex, signal);

			Assert.IsTrue(hasEventBeenInvoked, "The event should have been invoked.");
		}

		[TestMethod]
		public void ShouldInvokeEventsOnInputRemove()
		{
			const int inputIndex = 0;

			Component component = new AndGate("testId", 1);
			Signal signal = new Signal("test", 0);
			bool hasEventBeenInvoked = false;

			component.SetInput(inputIndex, signal);

			component.InputSignalChanged += (sender, e) =>
			{
				hasEventBeenInvoked = true;
				Assert.AreEqual(inputIndex, e.Index, "The events index should be the correct index.");
				Assert.IsFalse(e.WasAdded, "It should be a remove-event.");
			};

			component.SetInput(inputIndex, null);

			Assert.IsTrue(hasEventBeenInvoked, "The event should have been invoked.");
		}
	}
}