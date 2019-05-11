using System;

namespace BR.AN.PviServices
{
	[Flags]
	internal enum VariableAttribute
	{
		None = 0x0,
		Input = 0x1,
		Output = 0x2,
		Constant = 0x4,
		Variable = 0x8
	}
}
