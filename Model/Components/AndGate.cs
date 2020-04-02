using System.Collections.Generic;

namespace Model.Components
{
	public class AndGate : Component
	{
		public static readonly string AndTypeId = "AND";

		public AndGate(string id, int inputSize) : base(id, AndTypeId, inputSize, 1)
		{
			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			foreach (bool value in inputValues)
			{
				if (!value)
				{
					return new bool[] { false };
				}
			}

			return new bool[] { true };
		}

		public override Component Clone()
		{
			return new AndGate(Id, InputSignals.Count);
		}
	}
}