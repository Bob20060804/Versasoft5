using Ersa.Global.Mvvm;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;

namespace Ersa.Platform.Common.Loetprogramm
{
	[Serializable]
	public class EDC_ProgrammInfo : BindableBase, IEquatable<EDC_ProgrammInfo>
	{
		private string m_strProgrammName;

		private string m_strBibliotheksName;

		private bool m_blnIstNeuesProgramm;

		private bool m_blnIstVorlageProgramm;

		private string m_strFirmenname;

		private DateTime m_dtmDatum;

		private string m_strBenutzername;

		private string m_strKommentar;

		private string m_strEingangskontrolle;

		private string m_strAusgangskontrolle;

		public bool PRO_blnIstVorlageProgramm
		{
			get
			{
				return m_blnIstVorlageProgramm;
			}
			set
			{
				SetProperty(ref m_blnIstVorlageProgramm, value, "PRO_blnIstVorlageProgramm");
			}
		}

		public bool PRO_blnIstNeuesProgramm
		{
			get
			{
				return m_blnIstNeuesProgramm;
			}
			set
			{
				SetProperty(ref m_blnIstNeuesProgramm, value, "PRO_blnIstNeuesProgramm");
			}
		}

		public string PRO_strBibliotheksName
		{
			get
			{
				return m_strBibliotheksName;
			}
			set
			{
				SetProperty(ref m_strBibliotheksName, value, "PRO_strBibliotheksName");
			}
		}

		public bool PRO_blnIstFehlerhaft
		{
			get
			{
				if (PROa_enmFehlerhaft != null && PROa_enmFehlerhaft.Length != 0 && PROa_enmStatus != null && PROa_enmStatus.Length != 0)
				{
					int num = Array.IndexOf(PROa_enmStatus, ENUM_LoetprogrammStatus.Freigegeben);
					if (num >= 0 && num < PROa_enmFehlerhaft.Length)
					{
						return PROa_enmFehlerhaft[num];
					}
					int num2 = Array.IndexOf(PROa_enmStatus, ENUM_LoetprogrammStatus.Arbeitsversion);
					if (num2 >= 0 && num2 < PROa_enmFehlerhaft.Length)
					{
						return PROa_enmFehlerhaft[num2];
					}
				}
				return false;
			}
		}

		public string PRO_strProgrammName
		{
			get
			{
				return m_strProgrammName;
			}
			set
			{
				SetProperty(ref m_strProgrammName, value, "PRO_strProgrammName");
			}
		}

		public string PRO_strFirmenname
		{
			get
			{
				return m_strFirmenname;
			}
			set
			{
				SetProperty(ref m_strFirmenname, value, "PRO_strFirmenname");
			}
		}

		public string PRO_strAnwendungsVersionsInfo
		{
			get;
			set;
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

		public string PRO_strBenutzername
		{
			get
			{
				return m_strBenutzername;
			}
			set
			{
				SetProperty(ref m_strBenutzername, value, "PRO_strBenutzername");
			}
		}

		public ENUM_LoetprogrammStatus[] PROa_enmStatus
		{
			get;
			set;
		}

		public ENUM_LoetprogrammFreigabeStatus[] PROa_enmFreigabeStatus
		{
			get;
			set;
		}

		public bool[] PROa_enmFehlerhaft
		{
			get;
			set;
		}

		public int PRO_i32SetNummer
		{
			get;
			set;
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

		public string PRO_strEingangskontrolle
		{
			get
			{
				return m_strEingangskontrolle;
			}
			set
			{
				SetProperty(ref m_strEingangskontrolle, value, "PRO_strEingangskontrolle");
			}
		}

		public string PRO_strAusgangskontrolle
		{
			get
			{
				return m_strAusgangskontrolle;
			}
			set
			{
				SetProperty(ref m_strAusgangskontrolle, value, "PRO_strAusgangskontrolle");
			}
		}

		public long PRO_i64Id
		{
			get;
			set;
		}

		public long PRO_i64BibId
		{
			get;
			set;
		}

		public long PRO_i64VersionsId
		{
			get;
			set;
		}

		public int PRO_i32VersionsNummer
		{
			get;
			set;
		}

		public string PRO_strBildImportPfad
		{
			get;
			set;
		}

		public override bool Equals(object i_objVergleichsObjekt)
		{
			return Equals(i_objVergleichsObjekt as EDC_ProgrammInfo);
		}

		public bool Equals(EDC_ProgrammInfo i_edcPrgInfo)
		{
			return PRO_i64Id == i_edcPrgInfo?.PRO_i64Id;
		}

		public override int GetHashCode()
		{
			return PRO_i64Id.GetHashCode();
		}

		public bool FUN_blnPropertiesIdentisch(EDC_ProgrammInfo i_edcPrgInfo)
		{
			if (i_edcPrgInfo == null)
			{
				return false;
			}
			if (PRO_dtmDatum == i_edcPrgInfo.PRO_dtmDatum && PRO_strBibliotheksName == i_edcPrgInfo.PRO_strBibliotheksName && PRO_i32SetNummer == i_edcPrgInfo.PRO_i32SetNummer && PRO_strAnwendungsVersionsInfo == i_edcPrgInfo.PRO_strAnwendungsVersionsInfo && PRO_strBenutzername == i_edcPrgInfo.PRO_strBenutzername && PRO_strFirmenname == i_edcPrgInfo.PRO_strFirmenname && PRO_strKommentar == i_edcPrgInfo.PRO_strKommentar && PRO_strEingangskontrolle == i_edcPrgInfo.PRO_strEingangskontrolle && PRO_strAusgangskontrolle == i_edcPrgInfo.PRO_strAusgangskontrolle && PRO_strProgrammName == i_edcPrgInfo.PRO_strProgrammName && PRO_i64Id == i_edcPrgInfo.PRO_i64Id && PRO_i64BibId == i_edcPrgInfo.PRO_i64BibId && PRO_blnIstFehlerhaft == i_edcPrgInfo.PRO_blnIstFehlerhaft && PRO_i32VersionsNummer == i_edcPrgInfo.PRO_i32VersionsNummer)
			{
				return PRO_i64VersionsId == i_edcPrgInfo.PRO_i64VersionsId;
			}
			return false;
		}
	}
}
