using Prism.Modularity;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Prism.Mef.Modularity
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ModuleExportAttribute : ExportAttribute, IModuleExport
	{
		public string ModuleName
		{
			get;
			private set;
		}

		public Type ModuleType
		{
			get;
			private set;
		}

		public InitializationMode InitializationMode
		{
			get;
			set;
		}

		[CLSCompliant(false)]
		[DefaultValue(new string[]
		{

		})]
		public string[] DependsOnModuleNames
		{
			get;
			set;
		}

		public ModuleExportAttribute(Type moduleType)
			: base(typeof(IModule))
		{
			if (moduleType == null)
			{
				throw new ArgumentNullException("moduleType");
			}
			ModuleName = moduleType.Name;
			ModuleType = moduleType;
		}

		public ModuleExportAttribute(string moduleName, Type moduleType)
			: base(typeof(IModule))
		{
			ModuleName = moduleName;
			ModuleType = moduleType;
		}
	}
}
