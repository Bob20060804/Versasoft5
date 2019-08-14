using Prism.Modularity;
using System;
using System.ComponentModel;

namespace Prism.Mef.Modularity
{
	public interface IModuleExport
	{
		string ModuleName
		{
			get;
		}

		Type ModuleType
		{
			get;
		}

		[DefaultValue(InitializationMode.WhenAvailable)]
		InitializationMode InitializationMode
		{
			get;
		}

		[DefaultValue(null)]
		string[] DependsOnModuleNames
		{
			get;
		}
	}
}
