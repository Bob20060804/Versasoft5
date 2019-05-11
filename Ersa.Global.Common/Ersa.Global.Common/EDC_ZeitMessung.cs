using System;
using System.Diagnostics;

namespace Ersa.Global.Common
{
	public class EDC_ZeitMessung : IDisposable
	{
		private readonly string m_strName;

		private readonly Stopwatch m_fdcWatch;

		public TimeSpan PRO_fdcZeitDauer
		{
			get;
			set;
		}

		public EDC_ZeitMessung(string i_strName)
		{
			m_strName = i_strName;
			m_fdcWatch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			SUB_Stopp();
		}

		public void SUB_Stopp()
		{
			m_fdcWatch.Stop();
			PRO_fdcZeitDauer = m_fdcWatch.Elapsed;
			EDC_Utility.SUB_ConsoleLog(m_strName, m_fdcWatch.Elapsed.ToString());
		}
	}
}
