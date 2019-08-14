using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Prism.Mef
{
	public static class DefaultPrismServiceRegistrar
	{
		public static AggregateCatalog RegisterRequiredPrismServicesIfMissing(AggregateCatalog aggregateCatalog)
		{
			if (aggregateCatalog == null)
			{
				throw new ArgumentNullException("aggregateCatalog");
			}
			PrismDefaultsCatalog item = new PrismDefaultsCatalog(GetRequiredPrismPartsToRegister(aggregateCatalog));
			aggregateCatalog.Catalogs.Add(item);
			return aggregateCatalog;
		}

		private static IEnumerable<ComposablePartDefinition> GetRequiredPrismPartsToRegister(AggregateCatalog aggregateCatalog)
		{
			List<ComposablePartDefinition> list = new List<ComposablePartDefinition>();
			foreach (ComposablePartDefinition part in GetDefaultComposablePartCatalog().Parts)
			{
				foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
				{
					bool flag = false;
					foreach (ComposablePartDefinition part2 in aggregateCatalog.Parts)
					{
						foreach (ExportDefinition exportDefinition2 in part2.ExportDefinitions)
						{
							if (string.Compare(exportDefinition2.ContractName, exportDefinition.ContractName, StringComparison.Ordinal) == 0)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag && !list.Contains(part))
					{
						list.Add(part);
					}
				}
			}
			return list;
		}

		private static ComposablePartCatalog GetDefaultComposablePartCatalog()
		{
			return new AssemblyCatalog(Assembly.GetAssembly(typeof(MefBootstrapper)));
		}
	}
}
