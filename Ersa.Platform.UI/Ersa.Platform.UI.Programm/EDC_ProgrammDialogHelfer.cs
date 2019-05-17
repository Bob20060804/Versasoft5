using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Benutzereingabe;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace Ersa.Platform.UI.Programm
{
	[Export]
	public class EDC_ProgrammDialogHelfer
	{
		private readonly INF_InteractionController m_edcInteractionController;

		private readonly INF_LokalisierungsDienst m_edcLocDienst;

		[ImportingConstructor]
		public EDC_ProgrammDialogHelfer(INF_InteractionController i_edcInteractionController, INF_LokalisierungsDienst i_edcLocDienst)
		{
			m_edcInteractionController = i_edcInteractionController;
			m_edcLocDienst = i_edcLocDienst;
		}

		public string FUN_strKommentarEingabe(string i_strAnfangstext = null, bool i_blnMehrzeiligerText = false)
		{
			return m_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(m_edcLocDienst.FUN_strText("8_3436"), m_edcLocDienst.FUN_strText("13_240"), i_strAnfangstext, m_edcLocDienst.FUN_strText("9_2102"), null, i_blnMehrzeiligerText);
		}

		public void SUB_KeinGeeigneterSerialisiererGefundenDialogAnzeigen(string i_strDateiname)
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("13_250"), string.Format(m_edcLocDienst.FUN_strText("13_541"), i_strDateiname));
		}

		[Obsolete("FUN_enmUngueltigesProgrammSpeichernAnzeigen verwenden")]
		public ENUM_BenutzerEingabe FUN_enmUngueltigesProgrammSpeichernAbfrageAnzeigen()
		{
			if (FUN_enmUngueltigesProgrammSpeichernAnzeigen() != ENUM_Eingabe.Ok)
			{
				return ENUM_BenutzerEingabe.enmAbbrechen;
			}
			return ENUM_BenutzerEingabe.enmOk;
		}

		public ENUM_Eingabe FUN_enmUngueltigesProgrammSpeichernAnzeigen()
		{
			return m_edcInteractionController.FUN_enmOkAbbrechenAbfrageAnzeigen(m_edcLocDienst.FUN_strText("11_435"), m_edcLocDienst.FUN_strText("4_11061"), m_edcLocDienst.FUN_strText("1_1100"));
		}

		[Obsolete("FUN_enmVersionWiederherstellenAnzeigen verwenden")]
		public ENUM_BenutzerEingabe FUN_enmVersionWiederherstellenAbfrageAnzeigen()
		{
			if (FUN_enmVersionWiederherstellenAnzeigen() != ENUM_Eingabe.Ok)
			{
				return ENUM_BenutzerEingabe.enmAbbrechen;
			}
			return ENUM_BenutzerEingabe.enmOk;
		}

		public ENUM_Eingabe FUN_enmVersionWiederherstellenAnzeigen()
		{
			return m_edcInteractionController.FUN_enmOkAbbrechenAbfrageAnzeigen(m_edcLocDienst.FUN_strText("13_67"), m_edcLocDienst.FUN_strText("13_68"));
		}

		public void SUB_ProgrammUngueltigHinweisAnzeigen()
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("11_435"), m_edcLocDienst.FUN_strText("11_1047"));
		}

		public void SUB_KeineBibliothekAusgewaehltHinweisAnzeigen()
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("11_435"), m_edcLocDienst.FUN_strText("4_11804"));
		}

		public void SUB_KeinGueltigerBiblothekNameHinweisAnzeigen()
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("11_435"), string.Format(m_edcLocDienst.FUN_strText("13_234"), 31));
		}

		public void SUB_KeinGueltigerProgrammNameHinweisAnzeigen()
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("11_435"), string.Format(m_edcLocDienst.FUN_strText("13_236"), 31));
		}

		public void SUB_ProgrammNameDarfNichtLeerSeinHinweisAnzeigen()
		{
			m_edcInteractionController.SUB_OkHinweisAnzeigen(m_edcLocDienst.FUN_strText("11_435"), m_edcLocDienst.FUN_strText("13_235"));
		}
	}
}
