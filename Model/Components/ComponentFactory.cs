using System;
using System.Collections.Generic;

namespace Model.Components
{
	public delegate Component ComponentFactoryMethod(string id);

	public class ComponentFactory
	{
		private readonly static IReadOnlyDictionary<string, ComponentFactoryMethod> componentCreatorMethods = new Dictionary<string, ComponentFactoryMethod>()
		{
			[AndGate.AndTypeId] = id => new AndGate(id, 2),
			[OrGate.OrTypeId] = id => new OrGate(id, 2),
			[NotGate.NotTypeId] = id => new NotGate(id),
			[InputComponent.InputTypeId] = id => new InputComponent(id),
			[OutputComponent.OutputTypeId] = id => new OutputComponent(id)
		};

		public static IEnumerable<string> ComponentTypeIds => componentCreatorMethods.Keys;

		public static Component CreateComponent(string typeId)
		{
			return CreateComponent(typeId, Guid.NewGuid().ToString());
		}
		public static Component CreateComponent(string typeId, string id)
		{
			if (!componentCreatorMethods.ContainsKey(typeId))
			{
				throw new ArgumentException(nameof(typeId), "The given type id ´does not exist.");
			}

			return componentCreatorMethods[typeId].Invoke(id);
		}
	}
}