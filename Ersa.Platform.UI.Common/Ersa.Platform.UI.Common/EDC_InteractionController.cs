using Ersa.Global.Controls.Dialoge;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Common.Dialoge;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace Ersa.Platform.UI.Common
{
	[Export(typeof(INF_InteractionController))]
	public class EDC_InteractionController : INF_InteractionController
	{
		private readonly INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		[ImportingConstructor]
		public EDC_InteractionController(INF_LokalisierungsDienst i_edcLokalisierungsDienst)
		{
			m_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
		}

		public ENUM_Eingabe FUN_enmJaNeinAbbrechenAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strJaText = null, string i_strNeinText = null, string i_strAbbrechenText = null)
		{
			EDU_BenutzerAbfrageDialog eDU_BenutzerAbfrageDialog = new EDU_BenutzerAbfrageDialog(Application.Current.MainWindow)
			{
				PRO_strTitel = i_strTitel,
				PRO_strText = i_strText,
				PRO_strBestaetigenPositivText = (i_strJaText ?? m_edcLokalisierungsDienst.FUN_strText("13_80")),
				PRO_strBestaetigenNegativText = (i_strNeinText ?? m_edcLokalisierungsDienst.FUN_strText("13_79")),
				PRO_blnIstNegativeAuswahlSichtbar = true,
				PRO_blnIstAbbrechenSichtbar = true,
				PRO_strAbbrechenText = (i_strAbbrechenText ?? m_edcLokalisierungsDienst.FUN_strText("1_15"))
			};
			if (eDU_BenutzerAbfrageDialog.ShowDialog() != true)
			{
				return ENUM_Eingabe.Abbrechen;
			}
			if (!eDU_BenutzerAbfrageDialog.PRO_blnWurdeNegativBestatigt)
			{
				return ENUM_Eingabe.Ja;
			}
			return ENUM_Eingabe.Nein;
		}

		public ENUM_Eingabe FUN_enmOkAbbrechenAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strOkText = null, string i_strAbbrechenText = null)
		{
			EDU_BenutzerAbfrageDialog eDU_BenutzerAbfrageDialog = new EDU_BenutzerAbfrageDialog(Application.Current.MainWindow)
			{
				PRO_strTitel = i_strTitel,
				PRO_strText = i_strText,
				PRO_strBestaetigenPositivText = (i_strOkText ?? m_edcLokalisierungsDienst.FUN_strText("1_16")),
				PRO_blnIstNegativeAuswahlSichtbar = false,
				PRO_blnIstAbbrechenSichtbar = true,
				PRO_strAbbrechenText = (i_strAbbrechenText ?? m_edcLokalisierungsDienst.FUN_strText("1_15"))
			};
			if (eDU_BenutzerAbfrageDialog.ShowDialog() != true)
			{
				return ENUM_Eingabe.Abbrechen;
			}
			if (!eDU_BenutzerAbfrageDialog.PRO_blnWurdeNegativBestatigt)
			{
				return ENUM_Eingabe.Ok;
			}
			return ENUM_Eingabe.Abbrechen;
		}

		public void SUB_OkHinweisAnzeigen(string i_strTitel, string i_strText, string i_strOkText = null)
		{
			EDU_BenutzerAbfrageDialog eDU_BenutzerAbfrageDialog = new EDU_BenutzerAbfrageDialog(Application.Current.MainWindow);
			eDU_BenutzerAbfrageDialog.PRO_strTitel = i_strTitel;
			eDU_BenutzerAbfrageDialog.PRO_strText = i_strText;
			eDU_BenutzerAbfrageDialog.PRO_strBestaetigenPositivText = (i_strOkText ?? m_edcLokalisierungsDienst.FUN_strText("1_16"));
			eDU_BenutzerAbfrageDialog.PRO_blnIstNegativeAuswahlSichtbar = false;
			eDU_BenutzerAbfrageDialog.PRO_blnIstAbbrechenSichtbar = false;
			eDU_BenutzerAbfrageDialog.ShowDialog();
		}

		public string FUN_strTextEingabeAbfrageAnzeigen(string i_strTitel, string i_strText, string i_strInitialeEingabe = null, string i_strBestaetigenText = null, string i_strAbbrechenText = null, bool i_blnReturnErlaubt = false, Func<string, string> i_delValidierung = null)
		{
			EDU_TextEingabeDialog eDU_TextEingabeDialog = new EDU_TextEingabeDialog(Application.Current.MainWindow, i_strInitialeEingabe ?? string.Empty)
			{
				PRO_strTitel = i_strTitel,
				PRO_strText = i_strText,
				PRO_strBestaetigenText = (i_strBestaetigenText ?? m_edcLokalisierungsDienst.FUN_strText("1_16")),
				PRO_strAbbrechenText = (i_strAbbrechenText ?? m_edcLokalisierungsDienst.FUN_strText("1_15")),
				PRO_blnReturnErlaubt = i_blnReturnErlaubt,
				PRO_delValidierung = i_delValidierung
			};
			if (eDU_TextEingabeDialog.ShowDialog() != true)
			{
				return null;
			}
			return eDU_TextEingabeDialog.PRO_strEingabeText;
		}

		public string FUN_strAuswahlAbfrageAnzeigen(string i_strTitel, string i_strText, IList<string> i_lstAuswahlListe, string i_strInitialeAuswahl = null, string i_strBestaetigenText = null, Func<string, string> i_delBestaetigenText = null, string i_strAbbrechenText = null, bool i_blnBestaetigenVerhindernOhneAenderung = false, Func<string, string> i_delValidierung = null)
		{
			EDU_AuswahlDialog eDU_AuswahlDialog = new EDU_AuswahlDialog(Application.Current.MainWindow, i_strInitialeAuswahl)
			{
				PRO_strTitel = i_strTitel,
				PRO_strText = i_strText,
				PRO_strBestaetigenText = (i_strBestaetigenText ?? m_edcLokalisierungsDienst.FUN_strText("1_16")),
				PRO_delBestaetigenText = i_delBestaetigenText,
				PRO_strAbbrechenText = (i_strAbbrechenText ?? m_edcLokalisierungsDienst.FUN_strText("1_15")),
				PRO_lstAuswahlListe = i_lstAuswahlListe,
				PRO_blnBestaetigenVerhindernOhneAenderung = i_blnBestaetigenVerhindernOhneAenderung,
				PRO_delValidierung = i_delValidierung
			};
			if (eDU_AuswahlDialog.ShowDialog() != true)
			{
				return null;
			}
			return eDU_AuswahlDialog.PRO_strAuswahl;
		}

		public EDC_BibliothekUndProgrammEingabe FUN_edcNeuesProgrammAbfrageAnzeigen(string i_strTitel, IList<string> i_lstBibliotheken, string i_strInitialeBibliothekAuswahl = null, string i_strInitialierProgrammName = null, Func<string, string, string> i_delValidierung = null)
		{
			EDU_NeuesProgrammDialog eDU_NeuesProgrammDialog = new EDU_NeuesProgrammDialog(Application.Current.MainWindow)
			{
				PRO_strTitel = i_strTitel,
				PRO_lstBibliotheken = i_lstBibliotheken,
				PRO_strBibliothekName = i_strInitialeBibliothekAuswahl,
				PRO_strProgrammName = i_strInitialierProgrammName,
				PRO_delValidierung = i_delValidierung
			};
			if (eDU_NeuesProgrammDialog.ShowDialog() != true)
			{
				return null;
			}
			return new EDC_BibliothekUndProgrammEingabe
			{
				PRO_strBibliothekName = eDU_NeuesProgrammDialog.PRO_strBibliothekName,
				PRO_strProgrammName = eDU_NeuesProgrammDialog.PRO_strProgrammName
			};
		}

		public bool FUN_blnBibOderPrgImportAbfrageAnzeigen(string i_strTitel, string i_strStandardVerzeichnis, IEnumerable<EDC_BibliothekId> i_enuExistierendeBibs, IDictionary<ENUM_ImportFormat, EDC_BibOderPrgImportAbfrageItem> i_dicAbfrageItems)
		{
			EDU_BibOderPrgImportDialog eDU_BibOderPrgImportDialog = new EDU_BibOderPrgImportDialog(Application.Current.MainWindow);
			eDU_BibOderPrgImportDialog.PRO_strTitel = i_strTitel;
			eDU_BibOderPrgImportDialog.PRO_edcViewModel.SUB_Initialisieren(i_strStandardVerzeichnis, i_enuExistierendeBibs, i_dicAbfrageItems);
			return eDU_BibOderPrgImportDialog.ShowDialog() == true;
		}
	}
}
