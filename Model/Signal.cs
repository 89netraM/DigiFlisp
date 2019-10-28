using System;
using System.Collections.Generic;

namespace Model
{
	public delegate void SignalListener(Stack<UpdateRecord> updates);

	public sealed class Signal
	{
		public string OwnerId { get; }
		public int Index { get; }

		private readonly Dictionary<string, SignalListener> listeners = new Dictionary<string, SignalListener>();

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

		internal void Update(Stack<UpdateRecord> updates, bool newValue)
		{
			UpdateRecord thisUpdate = new UpdateRecord(OwnerId, Index);

			if (!updates.Contains(thisUpdate))
			{
				Value = newValue;

				updates.Push(thisUpdate);
				InvokeListeners(updates);
				updates.Pop();
			}
		}

		private void InvokeListeners(Stack<UpdateRecord> updates)
		{
			foreach (SignalListener listener in listeners.Values)
			{
				listener?.Invoke(updates);
			}
		}
	}
}