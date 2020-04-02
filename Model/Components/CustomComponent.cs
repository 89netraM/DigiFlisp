using System.Collections.Generic;
using System.Linq;

namespace Model.Components
{
	public class CustomComponent : Component
	{
		private static IEnumerable<InputComponent> GetInputs(Blueprint blueprint)
		{
			for (int i = 0; i < blueprint.ComponentCount; i++)
			{
				if (blueprint.GetComponent(i) is InputComponent inputComponent)
				{
					yield return inputComponent;
				}
			}
		}

		private static IEnumerable<OutputComponent> GetOutputs(Blueprint blueprint)
		{
			for (int i = 0; i < blueprint.ComponentCount; i++)
			{
				if (blueprint.GetComponent(i) is OutputComponent outputComponent)
				{
					yield return outputComponent;
				}
			}
		}

		private readonly Blueprint blueprint;

		public CustomComponent(string id, Blueprint blueprint) :
			base(id, blueprint.Id, GetInputs(blueprint).Count(), GetOutputs(blueprint).Count())
		{
			this.blueprint = blueprint;

			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			foreach ((InputComponent, bool) input in GetInputs(blueprint).Zip(inputValues))
			{
				input.Item1.State = input.Item2;
			}

			foreach (OutputComponent output in GetOutputs(blueprint))
			{
				yield return output.State;
			}
		}

		protected override Component InternalClone()
		{
			return new CustomComponent(Id, blueprint.Clone());
		}

		public bool ContainsBlueprint(string blueprintId)
		{
			if (TypeId == blueprintId)
			{
				return true;
			}

			for (int i = 0; i < blueprint.ComponentCount; i++)
			{
				if (blueprint.GetComponent(i) is CustomComponent c && c.ContainsBlueprint(blueprintId))
				{
					return true;
				}
			}

			return false;
		}
	}
}