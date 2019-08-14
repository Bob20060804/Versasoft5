using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Prism.Modularity
{
	public class ModuleConfigurationElementCollection : ConfigurationElementCollection
	{
		protected override bool ThrowOnDuplicate => true;

		public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

		protected override string ElementName => "module";

		public ModuleConfigurationElement this[int index]
		{
			get
			{
				return (ModuleConfigurationElement)BaseGet(index);
			}
		}

		public ModuleConfigurationElementCollection()
		{
		}

		public ModuleConfigurationElementCollection(ModuleConfigurationElement[] modules)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			foreach (ModuleConfigurationElement element in modules)
			{
				BaseAdd(element);
			}
		}

		public void Add(ModuleConfigurationElement module)
		{
			BaseAdd(module);
		}

		public bool Contains(string moduleName)
		{
			return BaseGet(moduleName) != null;
		}

		public IList<ModuleConfigurationElement> FindAll(Predicate<ModuleConfigurationElement> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			IList<ModuleConfigurationElement> list = new List<ModuleConfigurationElement>();
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ModuleConfigurationElement moduleConfigurationElement = (ModuleConfigurationElement)enumerator.Current;
					if (match(moduleConfigurationElement))
					{
						list.Add(moduleConfigurationElement);
					}
				}
				return list;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ModuleConfigurationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ModuleConfigurationElement)element).ModuleName;
		}
	}
}
