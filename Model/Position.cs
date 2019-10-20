namespace Model
{
	public class Position
	{
		private int x;
		public int X
		{
			get => x;
			set
			{
				if (x != value)
				{
					x = value;
				}
			}
		}

		private int y;
		public int Y
		{
			get => y;
			set
			{
				if (y != value)
				{
					y = value;
				}
			}
		}
	}
}