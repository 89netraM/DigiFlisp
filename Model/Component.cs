using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
	public abstract class Component
	{
		public string Id { get; }
		public string TypeId { get; }

		public Position Position { get; } = new Position();

		private readonly Signal[] inputSignals;
		private readonly Signal[] outputSignals;

		public IReadOnlyList<Signal> InputSignals => inputSignals;
		public IReadOnlyList<Signal> OutputSignals => outputSignals;

		public Component(string id, string typeId, int inputSize, int outputSize)
		{
			Id = id;
			TypeId = typeId;

			inputSignals = new Signal[inputSize];
			outputSignals = (new Signal[outputSize]).Select((ignored, index) => new Signal(Id, index)).ToArray();

			Update(new Stack<UpdateRecord>());
		}

		public void SetInput(int index, Signal input)
		{
			Signal oldInput = inputSignals[index];

			if (oldInput != input)
			{
				if (oldInput != null)
				{
					oldInput.RemoveListener(Id + index);
				}

				if (input != null)
				{
					inputSignals[index] = input;
					input.AddListener(Id + index, Update);

					Update(new Stack<UpdateRecord>());
				}
			}
		}

		protected void Update(Stack<UpdateRecord> updates)
		{
			IEnumerable<bool> outputValues = Logic(inputSignals.Select(x => x?.Value ?? false));

			if (outputValues.Count() != outputSignals.Length)
			{
				throw new Exception($"The `{nameof(Logic)}` method must return the correct amount of outputs.");
			}

			for (int i = 0; i < outputSignals.Length; i++)
			{
				outputSignals[i].Update(updates, outputValues.ElementAt(i));
			}
		}

		protected abstract IEnumerable<bool> Logic(IEnumerable<bool> inputValues);
	}
}