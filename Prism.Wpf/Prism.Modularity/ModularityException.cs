using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Prism.Modularity
{
	[Serializable]
	public class ModularityException : Exception
	{
		public string ModuleName
		{
			get;
			set;
		}

		public ModularityException()
			: this(null)
		{
		}

		public ModularityException(string message)
			: this(null, message)
		{
		}

		public ModularityException(string message, Exception innerException)
			: this(null, message, innerException)
		{
		}

		public ModularityException(string moduleName, string message)
			: this(moduleName, message, null)
		{
		}

		public ModularityException(string moduleName, string message, Exception innerException)
			: base(message, innerException)
		{
			ModuleName = moduleName;
		}

		protected ModularityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ModuleName = (info.GetValue("ModuleName", typeof(string)) as string);
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ModuleName", ModuleName);
		}
	}
}
