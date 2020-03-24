using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GUI.ViewModels
{
	public class ConnectionViewModel : ViewModelBase
	{
		public string FromId { get; }
		public int FromIndex { get; }
		public string ToId { get; }
		public int ToIndex { get; }

		public ConnectionViewModel(string fromId, int fromIndex, string toId, int toIndex)
		{
			FromId = fromId ?? throw new ArgumentNullException(nameof(fromId));
			FromIndex = fromIndex;
			ToId = toId ?? throw new ArgumentNullException(nameof(toId));
			ToIndex = toIndex;
		}

		public override bool Equals(object obj)
		{
			return obj is ConnectionViewModel cvm && this == cvm;
		}

		public static bool operator ==(ConnectionViewModel a, ConnectionViewModel b)
		{
			return a is ConnectionViewModel &&
				b is ConnectionViewModel &&
				a.FromId == b.FromId &&
				a.FromIndex == b.FromIndex &&
				a.ToId == b.ToId &&
				a.ToIndex == b.ToIndex;
		}
		public static bool operator !=(ConnectionViewModel a, ConnectionViewModel b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FromId, FromIndex, ToId, ToIndex);
		}

		public override string ToString()
		{
			return $"{FromId}:{FromIndex}-{ToId}:{ToIndex}";
		}
	}
}