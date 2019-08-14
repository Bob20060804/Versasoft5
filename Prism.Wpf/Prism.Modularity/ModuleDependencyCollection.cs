using System;
using System.Configuration;

namespace Prism.Modularity
{
	public class ModuleDependencyCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

		protected override string ElementName => "dependency";

		public ModuleDependencyConfigurationElement this[int index]
		{
			get
			{
				return (ModuleDependencyConfigurationElement)BaseGet(index);
			}
		}

		public ModuleDependencyCollection()
		{
		}

		public ModuleDependencyCollection(ModuleDependencyConfigurationElement[] dependencies)
		{
			if (dependencies == null)
			{
				throw new ArgumentNullException("dependencies");
			}
			foreach (ModuleDependencyConfigurationElement element in dependencies)
			{
				BaseAdd(element);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ModuleDependencyConfigurationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ModuleDependencyConfigurationElement)element).ModuleName;
		}
	}
}
