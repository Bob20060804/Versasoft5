using Ersa.Platform.Common.Loetprogramm;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public interface INF_InteractionController
	{
		ENUM_Eingabe FUN_enmJaNeinAbbrechenAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strJaText = null, string i_strNeinText = null, string i_strAbbrechenText = null);

		ENUM_Eingabe FUN_enmOkAbbrechenAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strOkText = null, string i_strAbbrechenText = null);

		void SUB_OkHinweisAnzeigen(string i_strTitel, string i_strText, string i_strOkText = null);

		string FUN_strTextEingabeAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strInitialeEingabe = null, string i_strBestaetigenText = null, string i_strAbbrechenText = null, bool i_blnReturnErlaubt = false, Func<string, string> i_delValidierung = null);

		string FUN_strAuswahlAbfrageAnzeigen(string i_strTitel, string i_strText, IList<string> i_lstAuswahlListe, string i_strInitialeAuswahl = null, string i_strBestaetigenText = null, Func<string, string> i_delBestaetigenText = null, string i_strAbbrechenText = null, bool i_blnBestaetigenVerhindernOhneAenderung = false, Func<string, string> i_delValidierung = null);

		EDC_BibliothekUndProgrammEingabe FUN_edcNeuesProgrammAbfrageAnzeigen(string i_strTitel, IList<string> i_lstBibliotheken, string i_strInitialeBibliothekAuswahl = null, string i_strInitialierProgrammName = null, Func<string, string, string> i_delValidierung = null);

		bool FUN_blnBibOderPrgImportAbfrageAnzeigen(string i_strTitel, string i_strStandardVerzeichnis, IEnumerable<EDC_BibliothekId> i_enuExistierendeBibs, IDictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageItem> i_dicAbfrageItems);
	}
}
