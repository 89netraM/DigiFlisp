using System;
using System.Collections.Generic;

namespace Model.Components
{
	public class InputComponent : Component
	{
		public static readonly string InputTypeId = "INPUT";

		private bool state;
		public bool State
		{
			get => state;
			set
			{
				if (state != value)
				{
					state = value;

					Update(Guid.NewGuid().ToString());
				}
			}
		}

		public InputComponent(string id) : base(id, InputTypeId, 0, 1) { }

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			return new bool[] { State };
		}
	}
}