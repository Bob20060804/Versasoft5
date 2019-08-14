using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Properties;
using System;
using System.Globalization;

namespace Prism.Modularity
{
	public class ModuleInitializer : IModuleInitializer
	{
		private readonly IServiceLocator serviceLocator;

		private readonly ILoggerFacade loggerFacade;

		public ModuleInitializer(IServiceLocator serviceLocator, ILoggerFacade loggerFacade)
		{
			if (serviceLocator == null)
			{
				throw new ArgumentNullException("serviceLocator");
			}
			if (loggerFacade == null)
			{
				throw new ArgumentNullException("loggerFacade");
			}
			this.serviceLocator = serviceLocator;
			this.loggerFacade = loggerFacade;
		}

		public void Initialize(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			IModule module = null;
			try
			{
				module = CreateModule(moduleInfo);
				module?.Initialize();
			}
			catch (Exception exception)
			{
				HandleModuleInitializationError(moduleInfo, module?.GetType().Assembly.FullName, exception);
			}
		}

		public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			Exception ex = (exception is ModuleInitializeException) ? exception : (string.IsNullOrEmpty(assemblyName) ? new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception) : new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception));
			loggerFacade.Log(ex.ToString(), Category.Exception, Priority.High);
			throw ex;
		}

		protected virtual IModule CreateModule(ModuleInfo moduleInfo)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			return CreateModule(moduleInfo.ModuleType);
		}

		protected virtual IModule CreateModule(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type == null)
			{
				throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.FailedToGetType, new object[1]
				{
					typeName
				}));
			}
			return (IModule)serviceLocator.GetInstance(type);
		}
	}
}
