using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UnitTests
{
	[TestClass]
	public class PositionTests
	{
		private Position position;

		[TestInitialize]
		public void Initialize()
		{
			position = new Position();
		}

		[TestMethod]
		public void ChangeX()
		{
			int changeTo = 5;

			position.X = changeTo;

			Assert.AreEqual(changeTo, position.X);
		}

		[TestMethod]
		public void ChangeXEvent()
		{
			bool hasEventListenerBeenInvoked = false;

			position.ChangeEvent += (sender, e) =>
			{
				Assert.IsTrue(e.IsXChange);
				Assert.IsFalse(e.IsYChange);
				hasEventListenerBeenInvoked = true;
			};
			position.X = 5;

			Assert.IsTrue(hasEventListenerBeenInvoked);
		}

		[TestMethod]
		public void ChangeY()
		{
			int changeTo = 5;

			position.Y = changeTo;

			Assert.AreEqual(changeTo, position.Y);
		}

		[TestMethod]
		public void ChangeYEvent()
		{
			bool hasEventListenerBeenInvoked = false;

			position.ChangeEvent += (sender, e) =>
			{
				Assert.IsFalse(e.IsXChange);
				Assert.IsTrue(e.IsYChange);
				hasEventListenerBeenInvoked = true;
			};
			position.Y = 5;

			Assert.IsTrue(hasEventListenerBeenInvoked);
		}
	}
}