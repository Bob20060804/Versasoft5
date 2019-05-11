namespace BR.AN.PviServices
{
	public enum CpuState
	{
		Run = 0,
		Service = 1,
		Stop = 2,
		Undefined = 0xFF,
		Offline = 0x100
	}
}
