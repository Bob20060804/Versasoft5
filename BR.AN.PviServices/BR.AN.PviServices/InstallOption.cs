using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum InstallOption
	{
		Undefined = 0x0,
		Consistent = 0x8,
		ExitInit = 0x10,
		AdoptProcessVariable = 0x20,
		Reboot = 0x40,
		Cir = 0x80
	}
}
