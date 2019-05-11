using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum ModuleNotifivcations
	{
		ModuleListChanges = 0x1,
		ModuleChanges = 0x2
	}
}
