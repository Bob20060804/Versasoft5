namespace BR.AN.PviServices
{
	public enum BootMode
	{
		WarmRestart = 1,
		ColdRestart = 2,
		Reset = 4,
		Reconfig = 8,
		SYSSWCopy = 0x10,
		Diagnostics = 0x20,
		Error = 0x40,
		Boot = 0x80,
		NMI = 0x100
	}
}
