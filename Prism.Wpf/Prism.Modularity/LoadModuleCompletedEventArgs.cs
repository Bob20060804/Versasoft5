using System;

namespace Prism.Modularity
{
	public class LoadModuleCompletedEventArgs : EventArgs
	{
		public ModuleInfo ModuleInfo
		{
			get;
			private set;
		}

		public Exception Error
		{
			get;
			private set;
		}

		public bool IsErrorHandled
		{
			get;
			set;
		}

		public LoadModuleCompletedEventArgs(ModuleInfo moduleInfo, Exception error)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			ModuleInfo = moduleInfo;
			Error = error;
		}
	}
}
