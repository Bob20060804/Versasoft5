using Ersa.Global.Dienste.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_SchedulerDienst))]
	public class EDC_SchedulerDienst : INF_SchedulerDienst
	{
		[Import]
		public INF_TimerFactory PRO_edcTimerFactory
		{
			get;
			set;
		}

		public IDisposable FUN_fdcAufgabeTerminieren(Action i_delAktion, Func<DateTime> i_delZeitpunkt, TimeSpan i_sttIntervall)
		{
			INF_Timer edcTimer = PRO_edcTimerFactory.FUN_edcTimerErstellen();
			Action i_delAktion2 = delegate
			{
				i_delAktion();
				Task.Factory.StartNew((Func<Task>)async delegate
				{
					await Task.Delay(1000);
					edcTimer.SUB_SyncZeitpunktAendern(i_delZeitpunkt() - DateTime.Now, i_sttIntervall);
				});
			};
			edcTimer.SUB_Initialisieren(i_delAktion2, i_delZeitpunkt() - DateTime.Now, i_sttIntervall);
			return edcTimer;
		}

		public IDisposable FUN_fdcAufgabeTerminieren(Action i_delAktion, int i_i32Zeitpunkt, int i_i32Intervall)
		{
			INF_Timer iNF_Timer = PRO_edcTimerFactory.FUN_edcTimerErstellen();
			iNF_Timer.SUB_Initialisieren(i_delAktion, i_i32Zeitpunkt, i_i32Intervall);
			return iNF_Timer;
		}
	}
}
