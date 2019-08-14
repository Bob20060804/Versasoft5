using System;

namespace Prism.Regions
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class SyncActiveStateAttribute : Attribute
	{
	}
}
