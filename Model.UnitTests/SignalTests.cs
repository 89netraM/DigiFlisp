using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Model.UnitTests
{
	[TestClass]
	public class SignalTests
	{
		[TestMethod]
		public void ShouldCacheTheValue()
		{
			Signal signal = new Signal("testOwnerId", 0);

			bool sentValue = true;
			signal.Update("testUpdateId1", sentValue);
			Assert.AreEqual(sentValue, signal.Value, "The signal should have cached the value sent in the last update.");

			sentValue = false;
			signal.Update("testUpdateId2", sentValue);
			Assert.AreEqual(sentValue, signal.Value, "The signal should have cached the value sent in the last update.");
		}

		[TestMethod]
		public void ShouldInvokeListenersOnUpdate()
		{
			Signal signal = new Signal("testOwnerId", 0);
			bool hasListenerBeenInvoked = false;

			signal.AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);

			signal.Update("testUpdateId", false);

			Assert.IsTrue(hasListenerBeenInvoked, "The listener should have been invoked.");
		}

		[TestMethod]
		public void ShouldInvokeAllListenersOnUpdate()
		{
			Signal signal = new Signal("testOwnerId", 0);
			bool[] hasListenerBeenInvoked = new bool[10];

			for (int i = 0; i < hasListenerBeenInvoked.Length; i++)
			{
				int index = i;

				signal.AddListener(
					"testListenerId" + index,
					updateId =>
					{
						hasListenerBeenInvoked[index] = true;
					}
				);
			}

			signal.Update("testUpdateId", false);

			Assert.IsTrue(hasListenerBeenInvoked.All(x => x), "The all listeners should have been invoked.");
		}

		[TestMethod]
		public void ShouldPassSamUpdatId()
		{
			Signal signal = new Signal("testOwnerId", 0);
			string sentUpdateId = Guid.NewGuid().ToString();
			string recivedUpdateId = null;

			signal.AddListener(
				"testListenerId",
				updateId =>
				{
					recivedUpdateId = updateId;
				}
			);

			signal.Update(sentUpdateId, false);

			Assert.AreSame(sentUpdateId, recivedUpdateId);
		}

		[TestMethod]
		public void ShouldNotInvokeRemovedListeners()
		{
			Signal signal = new Signal("testOwnerId", 0);
			string listenerId = "testListenerId";
			bool hasListenerBeenInvoked = false;

			signal.AddListener(
				listenerId,
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);
			signal.RemoveListener(listenerId);

			signal.Update("testUpdateId", false);

			Assert.IsFalse(hasListenerBeenInvoked, "The listener should not have been invoked.");
		}
	}
}