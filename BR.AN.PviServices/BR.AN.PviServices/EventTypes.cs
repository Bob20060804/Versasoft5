namespace BR.AN.PviServices
{
	internal enum EventTypes
	{
		Error = 3,
		Disconnect = 9,
		Connection = 10,
		Data = 11,
		Status = 12,
		Dataform = 13,
		Proceeding = 0x80,
		UserTag = 129,
		ServiceConnect = 240,
		ServiceDisconnect = 241,
		ServiceArrange = 242,
		EVENT_LINEBASE = 0xFF,
		ModuleChanged = 0x100,
		EventDebugger = 257,
		ModuleDeleted = 258,
		ModuleListChangedXML = 403,
		RedundancyCtrlEventXML = 440,
		TracePointsDataChanged = 452,
		BondChanged = 453,
		LicenseChnaged = 454
	}
}
