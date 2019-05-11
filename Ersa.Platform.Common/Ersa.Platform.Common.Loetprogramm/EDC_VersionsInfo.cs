using Ersa.Global.Mvvm;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;

namespace Ersa.Platform.Common.Loetprogramm
{
	public class EDC_VersionsInfo : BindableBase
	{
		private DateTime m_dtmVersionsDatum;

		private DateTime m_dtmBearbeitungsDatum;

		private ENUM_LoetprogrammStatus m_enmVersionsStatus;

		private string m_strBenutzer;

		private string m_strBearbeitungsBenutzer;

		private string m_strKommentar;

		private int m_i32SetNummer;

		private bool m_blnIstFehlerhaft;

		private ENUM_LoetprogrammFreigabeStatus m_enmFreigabeStatus;

		private EDC_FreigabeNotizen m_edcFreigabeNotizen;

		public long PRO_i64VersionsId
		{
			get;
			set;
		}

		public long PRO_i64ProgrammId
		{
			get;
			set;
		}

		public bool PRO_blnKannBearbeitetWerden
		{
			get
			{
				if (ENUM_LoetprogrammStatus.Arbeitsversion.Equals(PRO_enmVersionstatus))
				{
					return ENUM_LoetprogrammFreigabeStatus.Undefiniert.Equals(PRO_enmFreigabestatus);
				}
				return false;
			}
		}

		public DateTime PRO_dtmVersionsDatum
		{
			get
			{
				return m_dtmVersionsDatum;
			}
			set
			{
				SetProperty(ref m_dtmVersionsDatum, value, "PRO_dtmVersionsDatum");
			}
		}

		public DateTime PRO_dtmBearbeitungsDatum
		{
			get
			{
				return m_dtmBearbeitungsDatum;
			}
			set
			{
				SetProperty(ref m_dtmBearbeitungsDatum, value, "PRO_dtmBearbeitungsDatum");
			}
		}

		public ENUM_LoetprogrammStatus PRO_enmVersionstatus
		{
			get
			{
				return m_enmVersionsStatus;
			}
			set
			{
				if (SetProperty(ref m_enmVersionsStatus, value, "PRO_enmVersionstatus"))
				{
					RaisePropertyChanged("PRO_blnKannBearbeitetWerden");
				}
			}
		}

		public ENUM_LoetprogrammFreigabeStatus PRO_enmFreigabestatus
		{
			get
			{
				return m_enmFreigabeStatus;
			}
			set
			{
				if (SetProperty(ref m_enmFreigabeStatus, value, "PRO_enmFreigabestatus"))
				{
					RaisePropertyChanged("PRO_blnKannBearbeitetWerden");
				}
			}
		}

		public string PRO_strBenutzer
		{
			get
			{
				return m_strBenutzer;
			}
			set
			{
				SetProperty(ref m_strBenutzer, value, "PRO_strBenutzer");
			}
		}

		public string PRO_strBearbeitungsBenutzer
		{
			get
			{
				return m_strBearbeitungsBenutzer;
			}
			set
			{
				SetProperty(ref m_strBearbeitungsBenutzer, value, "PRO_strBearbeitungsBenutzer");
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

		public EDC_FreigabeNotizen PRO_edcFreigabeNotizen
		{
			get
			{
				return m_edcFreigabeNotizen;
			}
			set
			{
				SetProperty(ref m_edcFreigabeNotizen, value, "PRO_edcFreigabeNotizen");
			}
		}

		public int PRO_i32SetNummer
		{
			get
			{
				return m_i32SetNummer;
			}
			set
			{
				SetProperty(ref m_i32SetNummer, value, "PRO_i32SetNummer");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				return m_blnIstFehlerhaft;
			}
			set
			{
				SetProperty(ref m_blnIstFehlerhaft, value, "PRO_blnIstFehlerhaft");
			}
		}
	}
}
