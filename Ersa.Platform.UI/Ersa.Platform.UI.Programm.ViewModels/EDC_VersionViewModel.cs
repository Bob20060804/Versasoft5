using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;

namespace Ersa.Platform.UI.Programm.ViewModels
{
	public class EDC_VersionViewModel : EDC_ElementViewModel
	{
		private ENUM_LoetprogrammFreigabeArt m_enmFreigabeArt;

		private ENUM_LoetprogrammFreigabeStatus m_enmFreigabeStatus;

		private ENUM_LoetprogrammStatus m_enmStatus;

		private string m_strBenutzerName;

		private DateTime m_dtmDatum;

		private string m_strKommentar;

		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		public ENUM_LoetprogrammFreigabeArt PRO_enmFreigabeArt
		{
			get
			{
				return m_enmFreigabeArt;
			}
			set
			{
				SetProperty(ref m_enmFreigabeArt, value, "PRO_enmFreigabeArt");
			}
		}

		public ENUM_LoetprogrammFreigabeStatus PRO_enmFreigabeStatus
		{
			get
			{
				return m_enmFreigabeStatus;
			}
			set
			{
				SetProperty(ref m_enmFreigabeStatus, value, "PRO_enmFreigabeStatus");
			}
		}

		public ENUM_LoetprogrammStatus PRO_enmStatus
		{
			get
			{
				return m_enmStatus;
			}
			set
			{
				SetProperty(ref m_enmStatus, value, "PRO_enmStatus");
			}
		}

		public string PRO_strBenutzername
		{
			get
			{
				return m_strBenutzerName;
			}
			set
			{
				SetProperty(ref m_strBenutzerName, value, "PRO_strBenutzername");
			}
		}

		public DateTime PRO_dtmDatum
		{
			get
			{
				return m_dtmDatum;
			}
			set
			{
				SetProperty(ref m_dtmDatum, value, "PRO_dtmDatum");
			}
		}

		public string PRO_strKommentar
		{
			get
			{
				return m_strKommentar;
			}
			set
			{
				SetProperty(ref m_strKommentar, value, "PRO_strKommentar");
			}
		}

		public EDC_VersionViewModel(long i_i64Id, string i_strName, long i_i64ProgrammId)
			: base(i_i64Id, i_strName)
		{
			PRO_i64ProgrammId = i_i64ProgrammId;
		}
	}
}
