namespace Model
{
	public class Connection
	{
		public Component Other { get; }
		public int ToIndex { get; }
		public int FromIndex { get; }

		public Connection(Component other, int inIndex, int outIndex)
		{
			Other = other;
			ToIndex = inIndex;
			FromIndex = outIndex;
		}
	}
}