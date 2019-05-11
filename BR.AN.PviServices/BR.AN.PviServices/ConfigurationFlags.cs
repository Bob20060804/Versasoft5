using System;

namespace BR.AN.PviServices
{
	[Flags]
	public enum ConfigurationFlags
	{
		None = 0x0,
		Values = 0x1,
		ConnectionState = 0x4,
		ActiveState = 0x8,
		RefreshTime = 0x10,
		VariableMembers = 0x20,
		IOAttributes = 0x200,
		Scope = 0x400,
		All = 0x63D
	}
}
