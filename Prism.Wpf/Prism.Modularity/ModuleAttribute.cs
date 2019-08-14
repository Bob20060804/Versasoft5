using System;

namespace Prism.Modularity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ModuleAttribute : Attribute
	{
		public string ModuleName
		{
			get;
			set;
		}

		public bool OnDemand
		{
			get;
			set;
		}
	}
}
