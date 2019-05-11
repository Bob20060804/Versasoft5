namespace BR.AN.PviServices
{
	internal enum PVIEventTypes
	{
		Error = 3,
		Connect = 10,
		Data = 11,
		State = 12,
		DataFormat = 13,
		Proceeding = 0x80,
		UserTag = 129,
		LineEventsBase = 0xFF
	}
}
