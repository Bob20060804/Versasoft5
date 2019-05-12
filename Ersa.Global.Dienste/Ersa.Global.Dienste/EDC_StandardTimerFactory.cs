using Ersa.Global.Dienste.Interfaces;
using System.ComponentModel.Composition;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_TimerFactory))]
	public class EDC_StandardTimerFactory : INF_TimerFactory
	{
		public INF_Timer FUN_edcTimerErstellen()
		{
			return new EDC_Timer();
		}
	}
}
