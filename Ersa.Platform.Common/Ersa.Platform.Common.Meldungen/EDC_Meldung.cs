using Ersa.Platform.Common.Data.Meldungen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Meldungen
{
	/// <summary>
	/// Message
	/// </summary>
	public class EDC_Meldung : INF_Meldung, IEquatable<INF_Meldung>
	{
		public string PRO_strMeldungGuid
		{
			get;
			set;
		}

		public ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
			set;
		}

		public int PRO_i32ProduzentenCode
		{
			get;
			set;
		}

		public DateTime? PRO_sttAufgetreten
		{
			get;
			set;
		}

		public DateTime? PRO_sttQuittiert
		{
			get;
			set;
		}

		public DateTime? PRO_sttZurueckgestellt
		{
			get;
			set;
		}

		public ENUM_MeldungsTypen PRO_enmMeldungsTyp
		{
			get;
			set;
		}

		public int PRO_i32Betriebsart
		{
			get;
			set;
		}

		public int PRO_i32MeldungsNummer
		{
			get;
			set;
		}

		public int PRO_i32MeldungsOrt1
		{
			get;
			set;
		}

		public int PRO_i32MeldungsOrt2
		{
			get;
			set;
		}

		public int PRO_i32MeldungsOrt3
		{
			get;
			set;
		}

		public string PRO_strDetails
		{
			get;
			set;
		}

		public string PRO_strContext
		{
			get;
			set;
		}

		public IEnumerable<ENUM_ProzessAktionen> PRO_enuProzessAktionen
		{
			get;
			set;
		} = Enumerable.Empty<ENUM_ProzessAktionen>();


		public IEnumerable<ENUM_MeldungAktionen> PRO_enuMoeglicheAktionen
		{
			get;
			set;
		} = Enumerable.Empty<ENUM_MeldungAktionen>();


		public string PRO_strMeldungSortierKriterium
		{
			get
			{
				if (PRO_i32MeldungsNummer == 0)
				{
					return PRO_strMeldetext;
				}
				return PRO_i32MeldungsNummer.ToString();
			}
		}

		public string PRO_strMeldeort1
		{
			get;
			set;
		}

		public string PRO_strMeldeort2
		{
			get;
			set;
		}

		public string PRO_strMeldeort3
		{
			get;
			set;
		}

		public string PRO_strMeldetext
		{
			get;
			set;
		}

		public string PRO_strBenutzerName
		{
			get;
			set;
		}

		public bool PRO_blnEinlaufSperreAktiv
		{
			get
			{
				if (PRO_enuProzessAktionen != null)
				{
					return PRO_enuProzessAktionen.Contains(ENUM_ProzessAktionen.Einlausperre);
				}
				return false;
			}
		}

		public bool PRO_blnKannZurueckgestelltWerden
		{
			get
			{
				if (PRO_enuMoeglicheAktionen != null && PRO_enuMoeglicheAktionen.Contains(ENUM_MeldungAktionen.Zurueckstellen) && !PRO_sttQuittiert.HasValue)
				{
					return !PRO_sttZurueckgestellt.HasValue;
				}
				return false;
			}
		}

		public bool PRO_blnKannQuittiertWerden
		{
			get
			{
				if (PRO_enuMoeglicheAktionen != null && PRO_enuMoeglicheAktionen.Contains(ENUM_MeldungAktionen.Quittieren) && !PRO_sttQuittiert.HasValue)
				{
					return !PRO_sttZurueckgestellt.HasValue;
				}
				return false;
			}
		}

		public bool Equals(INF_Meldung i_edcAndereMeldung)
		{
			if (i_edcAndereMeldung == null)
			{
				return false;
			}
			return PRO_strMeldungGuid == i_edcAndereMeldung.PRO_strMeldungGuid;
		}

		public override int GetHashCode()
		{
			return PRO_strMeldungGuid.GetHashCode() ^ PRO_i32MeldungsNummer.GetHashCode() ^ PRO_sttAufgetreten.GetHashCode();
		}
	}
}
