using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ersa.Platform.Plc
{
	public class EDC_PlcHelper
	{
		private const int mC_i32Anzahl = 1000;

		private int m_i32Eventcounter;

		private Dictionary<string, string> m_dic = new Dictionary<string, string>(1000);

		public void SUB_IncEventCounter(string i_strVar, string i_strWert)
		{
			m_i32Eventcounter++;
			if (m_i32Eventcounter % 1000 == 0)
			{
				SUB_Log(" 1000 Events vorbei");
				m_dic.Clear();
				SUB_DebugListenerKorrigieren();
			}
			else
			{
				string text = FUN_strZeitStempel(DateTime.Now) + i_strVar;
				if (!m_dic.ContainsKey(text))
				{
					m_dic.Add(text, i_strWert);
				}
				else
				{
					SUB_Log(" KeyÄrger = " + text);
				}
			}
			if (m_i32Eventcounter > 1000000)
			{
				m_i32Eventcounter = 0;
			}
		}

		private void SUB_DebugListenerKorrigieren()
		{
			if (Debug.Listeners.Count != 1)
			{
				TraceListener listener = Debug.Listeners[0];
				Debug.Listeners.Clear();
				Debug.Listeners.Add(listener);
			}
		}

		private string FUN_strZeitStempel(DateTime i_dtmZeitpunkt)
		{
			return $"{i_dtmZeitpunkt:yyyy.MM.dd hh:mm:ss}.{i_dtmZeitpunkt.Millisecond}.{i_dtmZeitpunkt.Ticks}";
		}

		private void SUB_Log(string i_strText)
		{
		}
	}
}
