using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum TargetObjectType
	{
		Undefined = 0x0,
		File = 0x1,
		Directory = 0x2,
		Module = 0x4,
		Task = 0xC,
		EndOfEnum = 0xD
	}
}
