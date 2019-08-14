using System;

namespace Prism.Regions
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class RegionMemberLifetimeAttribute : Attribute
	{
		public bool KeepAlive
		{
			get;
			set;
		}

		public RegionMemberLifetimeAttribute()
		{
			KeepAlive = true;
		}
	}
}
