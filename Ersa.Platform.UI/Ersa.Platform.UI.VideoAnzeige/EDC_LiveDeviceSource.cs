using Ersa.Global.Common;
using Microsoft.Expression.Encoder.Live;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public class EDC_LiveDeviceSource : EDC_DisposableObject
	{
		public LiveDeviceSource PRO_fdcLiveDeviceSource
		{
			get;
			set;
		}

		protected override void SUB_InternalDispose()
		{
			if (PRO_fdcLiveDeviceSource != null)
			{
				PRO_fdcLiveDeviceSource.Dispose();
			}
		}
	}
}
