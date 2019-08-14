using Prism.Properties;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Prism.Modularity
{
	[Serializable]
	public class ModuleInitializeException : ModularityException
	{
		public ModuleInitializeException()
		{
		}

		public ModuleInitializeException(string message)
			: base(message)
		{
		}

		public ModuleInitializeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ModuleInitializeException(string moduleName, string moduleAssembly, string message)
			: this(moduleName, message, moduleAssembly, null)
		{
		}

		public ModuleInitializeException(string moduleName, string moduleAssembly, string message, Exception innerException)
			: base(moduleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.FailedToLoadModule, new object[3]
			{
				moduleName,
				moduleAssembly,
				message
			}), innerException)
		{
		}

		public ModuleInitializeException(string moduleName, string message, Exception innerException)
			: base(moduleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.FailedToLoadModuleNoAssemblyInfo, new object[2]
			{
				moduleName,
				message
			}), innerException)
		{
		}

		protected ModuleInitializeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
