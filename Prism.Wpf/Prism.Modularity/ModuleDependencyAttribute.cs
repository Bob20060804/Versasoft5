using System;

namespace Prism.Modularity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ModuleDependencyAttribute : Attribute
	{
		private readonly string _moduleName;

		public string ModuleName => _moduleName;

		public ModuleDependencyAttribute(string moduleName)
		{
			_moduleName = moduleName;
		}
	}
}
