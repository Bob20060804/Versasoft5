using Ersa.Global.Dienste.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Threading;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_Timer))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_Timer : INF_Timer, IDisposable
	{
		private bool m_blnIstDisposed;

		private Timer m_fdcTimer;

		~EDC_Timer()
		{
			Dispose(isDisposing: false);
		}

		public void SUB_Initialisieren(Action i_delAktion, TimeSpan i_sttZeitPunkt, TimeSpan i_sttIntervall)
		{
			m_fdcTimer = new Timer(delegate
			{
				i_delAktion();
			}, null, i_sttZeitPunkt, i_sttIntervall);
		}

		public void SUB_Initialisieren(Action i_delAktion, int i_i32ZeitPunkt, int i_i32Intervall)
		{
			m_fdcTimer = new Timer(delegate
			{
				i_delAktion();
			}, null, i_i32ZeitPunkt, i_i32Intervall);
		}

		public void SUB_SyncZeitpunktAendern(TimeSpan i_sttZeitPunkt, TimeSpan i_sttIntervall)
		{
			m_fdcTimer.Change(i_sttZeitPunkt, i_sttIntervall);
		}

		public void Dispose()
		{
			Dispose(isDisposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (!m_blnIstDisposed)
			{
				if (isDisposing && m_fdcTimer != null)
				{
					m_fdcTimer.Dispose();
				}
				m_blnIstDisposed = true;
			}
		}
	}
}
