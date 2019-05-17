using Ersa.Global.Common;
using Microsoft.Expression.Encoder.Live;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public class EDC_LiveJob : EDC_DisposableObject
	{
		public LiveJob PRO_fdcLiveJob
		{
			get;
			set;
		}

		protected override void SUB_InternalDispose()
		{
			if (PRO_fdcLiveJob != null)
			{
				PRO_fdcLiveJob.Dispose();
			}
		}
	}
}
