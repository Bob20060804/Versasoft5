using System;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_SchedulerDienst
	{
		INF_TimerFactory PRO_edcTimerFactory
		{
			get;
			set;
		}

		IDisposable FUN_fdcAufgabeTerminieren(Action i_delAktion, Func<DateTime> i_delZeitpunkt, TimeSpan i_sttIntervall);

		IDisposable FUN_fdcAufgabeTerminieren(Action i_delAktion, int i_i32Zeitpunkt, int i_i32Intervall);
	}
}
