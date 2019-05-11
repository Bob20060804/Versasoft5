using System;

namespace BR.AN.Logging
{
	[Flags]
	public enum LoggingOutputs
	{
		FlatFile = 0x1,
		EventLog = 0x2,
		VisualStudioOutputWindow = 0x4
	}
}
