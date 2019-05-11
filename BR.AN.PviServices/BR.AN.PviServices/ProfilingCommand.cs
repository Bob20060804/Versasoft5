using System;

namespace BR.AN.PviServices
{
	[CLSCompliant(false)]
	public enum ProfilingCommand : uint
	{
		ExtendedStart,
		Stop,
		Deinstall,
		GetStack,
		GetInfo,
		Install,
		Start,
		Default
	}
}
