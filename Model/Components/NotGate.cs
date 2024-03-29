﻿using System.Linq;
using System.Collections.Generic;

namespace Model.Components
{
	public class NotGate : Component
	{
		public static readonly string NotTypeId = "NOT";

		public NotGate(string id) : base(id, NotTypeId, 1, 1)
		{
			Initialize();
		}

		protected override IEnumerable<bool> Logic(IEnumerable<bool> inputValues)
		{
			return new bool[] { !inputValues.ElementAt(0) };
		}

		protected override Component InternalClone()
		{
			return new NotGate(Id);
		}
	}
}