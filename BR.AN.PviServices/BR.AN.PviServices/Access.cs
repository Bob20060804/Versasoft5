using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum Access
	{
		No = 0x0,
		Read = 0x1,
		Write = 0x2,
		ReadAndWrite = 0x3,
		EVENT = 0x4,
		DIRECT = 0x8,
		FASTECHO = 0x10
	}
}
