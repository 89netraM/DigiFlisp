namespace Model
{
	public readonly struct UpdateRecord
	{
		public string ComponentID { get; }
		public int InputIndex { get; }

		public UpdateRecord(string componentID, int inputIndex)
		{
			ComponentID = componentID;
			InputIndex = inputIndex;
		}
	}
}