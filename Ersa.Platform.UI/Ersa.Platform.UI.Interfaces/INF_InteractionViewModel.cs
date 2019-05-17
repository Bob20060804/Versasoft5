using Ersa.Platform.UI.Benutzerabfrage;
using Ersa.Platform.UI.Benutzereingabe;
using System;

namespace Ersa.Platform.UI.Interfaces
{
	public interface INF_InteractionViewModel
	{
		void SUB_LoginDialogAnzeigen(string i_strRechtNameKey = null, string i_strFehlertext = null);

		[Obsolete("INF_InteractionController.FUN_enmJaNeinAbbrechenAbfrageAnzeigen verwenden")]
		ENUM_BenutzerEingabe FUN_enmJaNeinAbbrechenAbfrageAnzeigen(EDC_JaNeinAbbrechenHinweis i_edcHinweis);

		[Obsolete("INF_InteractionController.FUN_enmOkAbbrechenAbfrageAnzeigen verwenden")]
		ENUM_BenutzerEingabe FUN_enmOkAbbrechenAbfrageAnzeigen(EDC_OkAbbrechenHinweis i_edcHinweis);

		[Obsolete("INF_InteractionController.SUB_OkHinweisAnzeigen verwenden")]
		void SUB_OkHinweisAnzeigen(EDC_OkHinweis i_edcHinweis);

		bool FUN_blnOfflineBetriebAbfrageAnzeigen();

		ENUM_LpDateiNichtGefundenEingabe FUN_enmLpDateiNichtGefundenHinweisAnzeigen(EDC_LpDateiNichtGefundenHinweis i_edcHinweis);

		ENUM_LpDateiNichtAutorisiertEingabe FUN_enmLpDateiNichtAutorisiertHinweisAnzeigen(EDC_LpDateiNichtAutorisiertHinweis i_edcHinweis);

		ENUM_LpDateiKorruptEingabe FUN_enmLpDateiKorruptHinweisAnzeigen(EDC_LpDateiKorruptHinweis i_edcHinweis);

		ENUM_LpInterpreterNichtGefundenEingabe FUN_enmLpInterpreterNichtGefundenHinweisAnzeigen(EDC_LpInterpreterNichtGefundenHinweis i_edcHinweis);

		[Obsolete("INF_InteractionController.FUN_strTextEingabeAbfrageAnzeigen verwenden")]
		string FUN_strTextEingabeAbfrageAnzeigen(EDC_TextEingabeHinweis i_edcHinweis);

		[Obsolete("INF_InteractionController.FUN_strAuswahlAbfrageAnzeigen verwenden")]
		string FUN_strAuswahlAbfrageAnzeigen(EDC_AuswahlAbfrage i_edcHinweis);

		object FUN_objOptionAbfrageAnzeigen(EDC_OptionAbfrage i_edcAbfrage);

		EDC_OffsetKorrekturEingabe FUN_edcOffsetKorrekturAbfrageAnzeigen(EDC_OffsetKorrekturAbfrage i_edcAbfrage);

		EDC_KennwortAendernEingabe FUN_edcKennwortAbfrageAnzeigen(EDC_KennwortAendernAbfrage i_edcHinweis);

		[Obsolete("INF_InteractionController.FUN_edcNeuesProgrammAbfrageAnzeigen verwenden")]
		EDC_NeuesProgrammEingabe FUN_edcNeuesProgrammAbfrageAnzeigen(EDC_NeuesProgrammAbfrage i_edcAbfrage);

		[Obsolete("INF_InteractionController.FUN_blnBibOderPrgImportAbfrageAnzeigen verwenden")]
		bool FUN_blnBibOderPrgImportAbfrageAnzeigen(EDC_BibOderPrgImportAbfrage i_edcAbfrage);
	}
}
