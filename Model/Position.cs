using System;

namespace Model
{
	public class Position
	{
		public event EventHandler<PositionEvent> ChangeEvent;

		private int x = 0;
		public int X
		{
			get => x;
			set
			{
				if (x != value)
				{
					x = value;
					ChangeEvent?.Invoke(this, true);
				}
			}
		}

		private int y = 0;
		public int Y
		{
			get => y;
			set
			{
				if (y != value)
				{
					y = value;
					ChangeEvent?.Invoke(this, false);
				}
			}
		}
	}

	public struct PositionEvent
	{
		public bool IsXChange { get; }
		public bool IsYChange { get => !IsXChange; }

		public PositionEvent(bool isXChange)
		{
			IsXChange = isXChange;
		}

		public static implicit operator PositionEvent(bool isXChange) => new PositionEvent(isXChange);
	}
}