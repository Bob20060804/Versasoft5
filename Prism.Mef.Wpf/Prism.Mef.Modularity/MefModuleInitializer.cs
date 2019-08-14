using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Mef.Properties;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;

namespace Prism.Mef.Modularity
{
	[Export(typeof(IModuleInitializer))]
	public class MefModuleInitializer : ModuleInitializer
	{
		private DownloadedPartCatalogCollection downloadedPartCatalogs;

		private AggregateCatalog aggregateCatalog
		{
			get;
			set;
		}

		[ImportMany(AllowRecomposition = true)]
		private IEnumerable<Lazy<IModule, IModuleExport>> ImportedModules
		{
			get;
			set;
		}

		[ImportingConstructor]
		public MefModuleInitializer(IServiceLocator serviceLocator, ILoggerFacade loggerFacade, DownloadedPartCatalogCollection downloadedPartCatalogs, AggregateCatalog aggregateCatalog)
			: base(serviceLocator, loggerFacade)
		{
			if (downloadedPartCatalogs == null)
			{
				throw new ArgumentNullException("downloadedPartCatalogs");
			}
			if (aggregateCatalog == null)
			{
				throw new ArgumentNullException("aggregateCatalog");
			}
			this.downloadedPartCatalogs = downloadedPartCatalogs;
			this.aggregateCatalog = aggregateCatalog;
		}

		protected override IModule CreateModule(ModuleInfo moduleInfo)
		{
			if (downloadedPartCatalogs.TryGet(moduleInfo, out ComposablePartCatalog catalog))
			{
				if (!aggregateCatalog.Catalogs.Contains(catalog))
				{
					aggregateCatalog.Catalogs.Add(catalog);
				}
				downloadedPartCatalogs.Remove(moduleInfo);
			}
			if (ImportedModules != null && ImportedModules.Count() != 0)
			{
				Lazy<IModule, IModuleExport> lazy = ImportedModules.FirstOrDefault((Lazy<IModule, IModuleExport> x) => x.Metadata.ModuleName == moduleInfo.ModuleName);
				if (lazy != null)
				{
					return lazy.Value;
				}
			}
			throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, Resources.FailedToGetType, new object[1]
			{
				moduleInfo.ModuleType
			}));
		}
	}
}
