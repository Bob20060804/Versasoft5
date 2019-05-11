using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum ErrorMode
	{
		None = 0x0,
		Exception = 0x1,
		Event = 0x2
	}
}
