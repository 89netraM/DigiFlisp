using System;
using System.Collections.Generic;

namespace Model
{
	public delegate void SignalListener(string updateId);

	public sealed class Signal
	{
		public string OwnerId { get; }
		public int Index { get; }

		private readonly Dictionary<string, SignalListener> listeners = new Dictionary<string, SignalListener>();

		private string lastUpdateId;
		public bool Value { get; private set; }

		public Signal(string ownerId, int index)
		{
			OwnerId = ownerId;
			Index = index;
		}

		public void AddListener(string listenerId, SignalListener listener)
		{
			listeners.Add(listenerId, listener);
		}

		public void RemoveListener(string listenerId)
		{
			listeners.Remove(listenerId);
		}

		internal void Update(string updateId, bool newValue)
		{
			if (updateId == null)
			{
				throw new ArgumentNullException(nameof(updateId));
			}
			else if (updateId != lastUpdateId)
			{
				lastUpdateId = updateId;

				Value = newValue;
				InvokeListeners(updateId);
			}
		}

		private void InvokeListeners(string updateId)
		{
			foreach (SignalListener listener in listeners.Values)
			{
				listener?.Invoke(updateId);
			}
		}
	}
}