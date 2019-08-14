using System;
using System.Runtime.Serialization;

namespace Prism.Modularity
{
	[Serializable]
	public class DuplicateModuleException : ModularityException
	{
		public DuplicateModuleException()
		{
		}

		public DuplicateModuleException(string message)
			: base(message)
		{
		}

		public DuplicateModuleException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public DuplicateModuleException(string moduleName, string message)
			: base(moduleName, message)
		{
		}

		public DuplicateModuleException(string moduleName, string message, Exception innerException)
			: base(moduleName, message, innerException)
		{
		}

		protected DuplicateModuleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
