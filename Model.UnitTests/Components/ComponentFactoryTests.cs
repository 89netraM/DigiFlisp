using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UnitTests.Components
{
	[TestClass]
	public class ComponentFactoryTests
	{
		[TestMethod]
		public void CanCreateAllComponents()
		{
			foreach (string typeId in ComponentFactory.ComponentTypeIds)
			{
				Component component = ComponentFactory.CreateComponent(typeId);

				Assert.IsNotNull(component);
			}
		}

		[TestMethod]
		public void CanCreateCorrectType()
		{
			Component component = ComponentFactory.CreateComponent(AndGate.AndTypeId);

			Assert.IsInstanceOfType(component, typeof(AndGate));
		}

		[TestMethod]
		public void CanCreateWithCustomId()
		{
			string customId = Guid.NewGuid().ToString();

			Component component = ComponentFactory.CreateComponent(AndGate.AndTypeId, customId);

			Assert.AreEqual(customId, component.Id);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowIfMissingTypeId()
		{
			_ = ComponentFactory.CreateComponent("MissingTypeId");
		}
	}
}
