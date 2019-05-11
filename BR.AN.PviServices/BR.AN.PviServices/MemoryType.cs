namespace BR.AN.PviServices
{
	public enum MemoryType
	{
		SystemRom = 0,
		SystemRam = 1,
		UserRom = 2,
		UserRam = 3,
		MemCard = 4,
		FixRam = 5,
		Dram = 65,
		Permanent = 240,
		SysInternal = 241,
		Remanent = 242,
		SystemSettings = 243,
		TransferModule = 244,
		Os = 61441,
		Tmp = 61442,
		Io = 61443,
		GlobalAnalog = 61444,
		GlobalDigital = 61445,
		NOTValid = 0xFFFF
	}
}
