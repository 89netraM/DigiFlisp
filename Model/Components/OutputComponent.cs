using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Components
{
	public class OutputComponent : Component
	{
		public static readonly string OutputTypeId = "OUTPUT";

		public event EventHandler<bool> IndicatorSignal;

		private bool state;
		public bool State
		{
			get => state;
			private set
			{
				if (state != value)
				{
					state = value;

					IndicatorSignal?.Invoke(this, value);
				}
			}
		}

		public OutputComponent(string id) : base(id, OutputTypeId, 1, 0)
		{
			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			State = inputValues.ElementAt(0);

			return Array.Empty<bool>();
		}
	}
}
