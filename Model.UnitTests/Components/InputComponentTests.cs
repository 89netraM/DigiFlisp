using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class InputComponentTests
	{
		[TestMethod]
		public void ShouldStartAsFalse()
		{
			Component inComponent = new InputComponent("testId");

			Assert.IsFalse(inComponent.OutputSignals[0].Value);
		}

		[TestMethod]
		public void TrueIsTrue()
		{
			InputComponent inComponent = new InputComponent("testId");
			inComponent.State = true;

			Assert.IsTrue(inComponent.OutputSignals[0].Value);
		}

		[TestMethod]
		public void FalseIsFalse()
		{
			InputComponent inComponent = new InputComponent("testId");
			inComponent.State = false;

			Assert.IsFalse(inComponent.OutputSignals[0].Value);
		}

		[TestMethod]
		public void ShouldUpdateListenersOnStateChange()
		{
			InputComponent inComponent = new InputComponent("testId");
			bool hasListenerBeenInvoked = false;

			inComponent.OutputSignals[0].AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);
			inComponent.State = true;

			Assert.IsTrue(hasListenerBeenInvoked, "The listeners of output should be called imeadietly after a state change.");
		}

		[TestMethod]
		public void ShouldNotUpdateListenersOnStateChangeToSame()
		{
			InputComponent inComponent = new InputComponent("testId");
			bool hasListenerBeenInvoked = false;

			inComponent.OutputSignals[0].AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);
			inComponent.State = false;

			Assert.IsFalse(hasListenerBeenInvoked, "The listeners of output should not be called if the state is \"changed\" to what it already is.");
		}
	}
}
