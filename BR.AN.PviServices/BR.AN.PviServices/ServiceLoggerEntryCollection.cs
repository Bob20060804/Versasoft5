namespace BR.AN.PviServices
{
	internal class ServiceLoggerEntryCollection : LoggerEntryCollection
	{
		public ServiceLoggerEntryCollection(Base parent, string name)
			: base(parent, name)
		{
		}

		protected override int GetArrayIndex(LoggerEntryBase eEntry)
		{
			return eEntry.propSArrayIndex;
		}

		protected override void SetArrayIndex(LoggerEntryBase eEntry)
		{
			eEntry.propSArrayIndex = arrayOfLoggerEntries.Count;
		}

		protected override void UpdateArrayIndices(int idxRemoved)
		{
			int num = 0;
			for (num = idxRemoved; num < arrayOfLoggerEntries.Count; num++)
			{
				((LoggerEntryBase)arrayOfLoggerEntries[num]).propSArrayIndex--;
			}
		}
	}
}
