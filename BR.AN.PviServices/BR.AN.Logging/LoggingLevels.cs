using System;

namespace BR.AN.Logging
{
	[Flags]
	public enum LoggingLevels
	{
		Information = 0x1,
		Warning = 0x2,
		Error = 0x4,
		Debug = 0x8
	}
}
