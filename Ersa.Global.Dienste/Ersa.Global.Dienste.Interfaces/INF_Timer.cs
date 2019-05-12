using System;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_Timer : IDisposable
	{
		void SUB_Initialisieren(Action i_delAktion, TimeSpan i_sttZeitPunkt, TimeSpan i_sttIntervall);

		void SUB_Initialisieren(Action i_delAktion, int i_i32ZeitPunkt, int i_i32Intervall);

		void SUB_SyncZeitpunktAendern(TimeSpan i_sttZeitPunkt, TimeSpan i_sttIntervall);
	}
}
