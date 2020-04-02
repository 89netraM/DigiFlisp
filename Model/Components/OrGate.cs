using System.Collections.Generic;

namespace Model.Components
{
	public class OrGate : Component
	{
		public static readonly string OrTypeId = "OR";

		public OrGate(string id, int inputSize) : base(id, OrTypeId, inputSize, 1)
		{
			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			foreach (bool value in inputValues)
			{
				if (value)
				{
					return new bool[] { true };
				}
			}

			return new bool[] { false };
		}

		protected override Component InternalClone()
		{
			return new OrGate(Id, InputSignals.Count);
		}
	}
}