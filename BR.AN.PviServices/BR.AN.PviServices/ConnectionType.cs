using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum ConnectionType
	{
		None = 0x0,
		Create = 0x1,
		Link = 0x2,
		CreateAndLink = 0x3
	}
}
