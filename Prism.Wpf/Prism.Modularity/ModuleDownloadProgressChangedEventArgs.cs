using System;
using System.ComponentModel;

namespace Prism.Modularity
{
	public class ModuleDownloadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		public ModuleInfo ModuleInfo
		{
			get;
			private set;
		}

		public long BytesReceived
		{
			get;
			private set;
		}

		public long TotalBytesToReceive
		{
			get;
			private set;
		}

		public ModuleDownloadProgressChangedEventArgs(ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
			: base(CalculateProgressPercentage(bytesReceived, totalBytesToReceive), null)
		{
			if (moduleInfo == null)
			{
				throw new ArgumentNullException("moduleInfo");
			}
			ModuleInfo = moduleInfo;
			BytesReceived = bytesReceived;
			TotalBytesToReceive = totalBytesToReceive;
		}

		private static int CalculateProgressPercentage(long bytesReceived, long totalBytesToReceive)
		{
			if (bytesReceived == 0L || totalBytesToReceive == 0L || totalBytesToReceive == -1)
			{
				return 0;
			}
			return (int)(bytesReceived * 100 / totalBytesToReceive);
		}
	}
}
