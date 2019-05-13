using System;

namespace Ersa.Platform.Infrastructure
{
	public class EDC_GlobalSetting : EDC_NotificationObjectMitSprachUmschaltung
	{
		private string m_strWert;

		private Action<EDC_GlobalSetting> m_delWertGeaendertAktion;

		public string PRO_strKey
		{
			get;
			private set;
		}

		public string PRO_strWert
		{
			get
			{
				return m_strWert;
			}
			set
			{
				SetProperty(ref m_strWert, value, "PRO_strWert");
			}
		}

		public string PRO_strDefaultWert
		{
			get;
			set;
		}

		public string PRO_strLokalisierungsKey
		{
			get;
			set;
		}

		public bool PRO_blnIstSchreibgeschuetzt
		{
			get;
			set;
		}

		public bool PRO_blnIstAusgeblendet
		{
			get;
			set;
		}

		public ENUM_GlobalSettingBereich PRO_enmBereich
		{
			get;
			set;
		}

		public Action<EDC_GlobalSetting> PRO_delWertGeaendertAktion
		{
			get
			{
				return m_delWertGeaendertAktion ?? ((Action<EDC_GlobalSetting>)delegate
				{
				});
			}
			set
			{
				m_delWertGeaendertAktion = value;
			}
		}

		public EDC_GlobalSetting(string i_strKey)
		{
			PRO_strKey = i_strKey;
		}
	}
}
