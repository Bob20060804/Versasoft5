using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ersa.Platform.Common.Mes
{
	[Serializable]
	public class EDC_MesMaschinenDaten
	{
		private readonly StringBuilder m_fdcStringBuilder = new StringBuilder();

		public Dictionary<ENUM_MesMaschinenDatenArgumente, object> PRO_dicArgumente
		{
			get;
			set;
		}

		public EDC_MesMaschinenDaten()
		{
			PRO_dicArgumente = new Dictionary<ENUM_MesMaschinenDatenArgumente, object>();
		}

		public override string ToString()
		{
			if (PRO_dicArgumente == null)
			{
				return string.Empty;
			}
			if (PRO_dicArgumente.Count <= 0)
			{
				return string.Empty;
			}
			m_fdcStringBuilder.Clear();
			m_fdcStringBuilder.AppendLine("Machine data:");
			foreach (ENUM_MesMaschinenDatenArgumente key in PRO_dicArgumente.Keys)
			{
				if (key.GetType() == typeof(IEnumerable))
				{
					string arg = string.Join("; ", (IEnumerable)PRO_dicArgumente[key]);
					m_fdcStringBuilder.AppendLine($"{key}|{arg}");
				}
				else
				{
					m_fdcStringBuilder.AppendLine($"{key}|{PRO_dicArgumente[key]}");
				}
			}
			return m_fdcStringBuilder.ToString();
		}
	}
}
