using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum IOVariableTypes
	{
		PHYSICAL = 0x1,
		VALUE = 0x2,
		FORCE = 0x4
	}
}
