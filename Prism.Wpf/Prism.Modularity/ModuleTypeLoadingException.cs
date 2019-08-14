using Prism.Properties;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Prism.Modularity
{
	[Serializable]
	public class ModuleTypeLoadingException : ModularityException
	{
		public ModuleTypeLoadingException()
		{
		}

		public ModuleTypeLoadingException(string message)
			: base(message)
		{
		}

		public ModuleTypeLoadingException(string message, Exception exception)
			: base(message, exception)
		{
		}

		public ModuleTypeLoadingException(string moduleName, string message)
			: this(moduleName, message, null)
		{
		}

		public ModuleTypeLoadingException(string moduleName, string message, Exception innerException)
			: base(moduleName, string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.FailedToRetrieveModule, new object[2]
			{
				moduleName,
				message
			}), innerException)
		{
		}

		protected ModuleTypeLoadingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
