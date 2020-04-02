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

					Update(new Stack<UpdateRecord>());
				}
			}
		}

		public InputComponent(string id) : base(id, InputTypeId, 0, 1)
		{
			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			return new bool[] { State };
		}

		public override Component Clone()
		{
			return new InputComponent(Id);
		}
	}
}