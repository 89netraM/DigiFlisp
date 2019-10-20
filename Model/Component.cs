using System;
using System.Collections.Generic;

namespace Model
{
	public abstract class Component
	{
		public string Id { get; }
		public string TypeId { get; }

		private readonly Signal[] inputSignals;
		private readonly Signal[] outputSignals;

		public Component(string id, string typeId, int inputSize, int outputSize)
		{
			Id = id;
			TypeId = typeId;

			inputSignals = new Signal[inputSize];
			outputSignals = new Signal[outputSize];

			for (int i = 0; i < outputSignals.Length; i++)
			{
				outputSignals[i] = new Signal(Id, i);
			}
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

					Update(Guid.NewGuid().ToString());
				}
			}
		}

		public IReadOnlyCollection<Signal> GetInputSignals()
		{
			return inputSignals;
		}

		private bool[] GetInputValues()
		{
			bool[] values = new bool[inputSignals.Length];

			for (int i = 0; i < inputSignals.Length; i++)
			{
				values[i] = inputSignals[i].Value;
			}

			return values;
		}

		protected void Update(string updateId)
		{
			bool[] outputValues = Logic(GetInputValues());

			if (outputValues.Length != outputSignals.Length)
			{
				throw new Exception($"The `{nameof(Logic)}` method must return the correct amount of outputs.");
			}

			for (int i = 0; i < outputSignals.Length; i++)
			{
				outputSignals[i].Update(updateId, outputValues[i]);
			}
		}

		protected abstract bool[] Logic(bool[] inputValues);
	}
}