using Ersa.Platform.Common.Data.Meldungen;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Meldungen
{
	public interface INF_Meldung : IEquatable<INF_Meldung>
	{
		string PRO_strMeldungGuid
		{
			get;
		}

		ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
		}

		int PRO_i32ProduzentenCode
		{
			get;
		}

		DateTime? PRO_sttAufgetreten
		{
			get;
		}

		DateTime? PRO_sttQuittiert
		{
			get;
			set;
		}

		DateTime? PRO_sttZurueckgestellt
		{
			get;
			set;
		}

		ENUM_MeldungsTypen PRO_enmMeldungsTyp
		{
			get;
		}

		int PRO_i32Betriebsart
		{
			get;
		}

		int PRO_i32MeldungsNummer
		{
			get;
		}

		int PRO_i32MeldungsOrt1
		{
			get;
		}

		int PRO_i32MeldungsOrt2
		{
			get;
		}

		int PRO_i32MeldungsOrt3
		{
			get;
		}

		string PRO_strDetails
		{
			get;
			set;
		}

		string PRO_strContext
		{
			get;
			set;
		}

		IEnumerable<ENUM_ProzessAktionen> PRO_enuProzessAktionen
		{
			get;
			set;
		}

		IEnumerable<ENUM_MeldungAktionen> PRO_enuMoeglicheAktionen
		{
			get;
			set;
		}

		string PRO_strMeldungSortierKriterium
		{
			get;
		}

		string PRO_strMeldeort1
		{
			get;
			set;
		}

		string PRO_strMeldeort2
		{
			get;
			set;
		}

		string PRO_strMeldeort3
		{
			get;
			set;
		}

		string PRO_strMeldetext
		{
			get;
			set;
		}

		string PRO_strBenutzerName
		{
			get;
			set;
		}

		bool PRO_blnKannZurueckgestelltWerden
		{
			get;
		}

		bool PRO_blnKannQuittiertWerden
		{
			get;
		}
	}
}
