using System;
using System.Runtime.Serialization;

namespace Prism.Modularity
{
	[Serializable]
	public class ModuleNotFoundException : ModularityException
	{
		public ModuleNotFoundException()
		{
		}

		public ModuleNotFoundException(string message)
			: base(message)
		{
		}

		public ModuleNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ModuleNotFoundException(string moduleName, string message)
			: base(moduleName, message)
		{
		}

		public ModuleNotFoundException(string moduleName, string message, Exception innerException)
			: base(moduleName, message, innerException)
		{
		}

		protected ModuleNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
