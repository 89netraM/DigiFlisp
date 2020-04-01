using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using Model.UnitTests.util;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class CustomComponentTest
	{
		[TestMethod]
		public void SimpleAdd()
		{
			Blueprint bp = new Blueprint("testBP");

			Component i1 = new InputComponent("B");
			bp.AddComponent(i1);
			Component i2 = new InputComponent("C");
			bp.AddComponent(i2);
			Component a = new AndGate("A", 2);
			bp.AddComponent(a);
			Component o = new OutputComponent("D");
			bp.AddComponent(o);

			bp.Connect(i1, 0, a, 0);
			bp.Connect(i2, 0, a, 1);
			bp.Connect(a, 0, o, 0);

			Component customComponent = new CustomComponent("1", bp);
			Signal signal1 = new Signal("test1", 0);
			Signal signal2 = new Signal("test2", 0);
			customComponent.SetInput(0, signal1);
			customComponent.SetInput(1, signal2);

			signal1.Update(true);
			Assert.IsFalse(customComponent.OutputSignals[0].Value);

			signal2.Update(true);
			Assert.IsTrue(customComponent.OutputSignals[0].Value);

			signal1.Update(false);
			Assert.IsFalse(customComponent.OutputSignals[0].Value);
		}

		[TestMethod]
		public void ComplicatedWithSelfCalling()
		{
			Blueprint bp = new Blueprint("testBP");

			Component i1 = new InputComponent("B");
			bp.AddComponent(i1);
			Component i2 = new InputComponent("C");
			bp.AddComponent(i2);
			Component A = new AndGate("1", 2);
			bp.AddComponent(A);
			Component B = new AndGate("2", 2);
			bp.AddComponent(B);
			Component C = new NotGate("3");
			bp.AddComponent(C);
			Component D = new NotGate("4");
			bp.AddComponent(D);
			Component @out = new OutputComponent("5");
			bp.AddComponent(@out);

			bp.Connect(i1, 0, A, 1);
			bp.Connect(i2, 0, B, 1);
			bp.Connect(A, 0, C, 0);
			bp.Connect(B, 0, D, 0);
			bp.Connect(C, 0, B, 0);
			bp.Connect(D, 0, A, 0);
			bp.Connect(C, 0, @out, 0);

			Component customComponent = new CustomComponent("1", bp);
			Signal signal1 = new Signal("test1", 0);
			Signal signal2 = new Signal("test2", 0);
			customComponent.SetInput(0, signal1);
			customComponent.SetInput(1, signal2);

			// Setting value of next state to 0
			signal1.Update(true);
			signal2.Update(false);

			// Getting value of state
			signal1.Update(true);
			signal2.Update(true);
			Assert.IsFalse(customComponent.OutputSignals[0].Value);

			// Setting value of next state to 1
			signal1.Update(false);
			signal2.Update(true);

			// Check values of state
			signal1.Update(true);
			signal2.Update(true);
			Assert.IsTrue(customComponent.OutputSignals[0].Value);
		}
	}
}