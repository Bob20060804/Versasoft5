using Prism.Modularity;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;

namespace Prism.Mef.Modularity
{
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class DownloadedPartCatalogCollection
	{
		private Dictionary<ModuleInfo, ComposablePartCatalog> catalogs = new Dictionary<ModuleInfo, ComposablePartCatalog>();

		public void Add(ModuleInfo moduleInfo, ComposablePartCatalog catalog)
		{
			catalogs.Add(moduleInfo, catalog);
		}

		public ComposablePartCatalog Get(ModuleInfo moduleInfo)
		{
			return catalogs[moduleInfo];
		}

		public bool TryGet(ModuleInfo moduleInfo, out ComposablePartCatalog catalog)
		{
			return catalogs.TryGetValue(moduleInfo, out catalog);
		}

		public void Remove(ModuleInfo moduleInfo)
		{
			catalogs.Remove(moduleInfo);
		}

		public void Clear()
		{
			catalogs.Clear();
		}
	}
}
