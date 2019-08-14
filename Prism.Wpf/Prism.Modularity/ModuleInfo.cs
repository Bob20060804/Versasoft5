using System;
using System.Collections.ObjectModel;

namespace Prism.Modularity
{
	[Serializable]
	public class ModuleInfo : IModuleCatalogItem
	{
		public string ModuleName
		{
			get;
			set;
		}

		public string ModuleType
		{
			get;
			set;
		}

		public Collection<string> DependsOn
		{
			get;
			set;
		}

		public InitializationMode InitializationMode
		{
			get;
			set;
		}

		public string Ref
		{
			get;
			set;
		}

		public ModuleState State
		{
			get;
			set;
		}

		public ModuleInfo()
			: this(null, null, new string[0])
		{
		}

		public ModuleInfo(string name, string type, params string[] dependsOn)
		{
			if (dependsOn == null)
			{
				throw new ArgumentNullException("dependsOn");
			}
			ModuleName = name;
			ModuleType = type;
			DependsOn = new Collection<string>();
			foreach (string item in dependsOn)
			{
				DependsOn.Add(item);
			}
		}

		public ModuleInfo(string name, string type)
			: this(name, type, new string[0])
		{
		}
	}
}
