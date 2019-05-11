using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum CastModes
	{
		DEFAULT = 0x0,
		PG2000String = 0x1,
		DecimalConversion = 0x2,
		RangeCheck = 0x4,
		FloatConversion = 0x8,
		StringTermination = 0x10
	}
}
