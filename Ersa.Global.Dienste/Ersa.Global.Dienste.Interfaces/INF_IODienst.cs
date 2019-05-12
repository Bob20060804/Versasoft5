using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ersa.Global.Dienste.Interfaces
{
	public interface INF_IODienst
	{
		bool FUN_blnDateiExistiert(string i_strPfad);

		bool FUN_blnIstDateiSchreibgeschuetzt(string i_strPfad);

		Stream FUN_fdcDateiOeffnen(string i_strDatei, FileMode i_fdcFileMode);

		Stream FUN_fdcDateiLesendOeffnen(string i_strDatei);

		void SUB_DateiKopieren(string i_strQuellPfad, string i_strDestinationPfad, bool i_blnUeberschreiben);

		void SUB_DateiVerschieben(string i_strQuellPfad, string i_strDestinationPfad, bool i_blnUeberschreiben);

		void SUB_DateiLoeschen(string i_strPfad);

		void SUB_DateiSchreibschutzEntfernen(string i_strDateiPfad);

		void SUB_DateiInhaltSchreiben(string i_strDateiPfad, IEnumerable<string> i_enuDateiInhalt, Encoding i_fdcEncoding = null);

		void SUB_DateiInhaltSchreiben(string i_strDateiPfad, string i_strDateiInhalt, Encoding i_fdcEncoding = null);

		IList<string> FUN_lstDateiInhaltLesen(string i_strDateiPfad);

		string FUN_strDateiInhaltLesen(string i_strDateiPfad);

		DateTime FUN_dtmDateiDatumErmitteln(string i_strDateipfad);

		string FUN_strValidiereUndBereinigeDateiNamen(string i_strDateiName);

		string FUN_strErmittleVerzeichnisName(string i_strVerzeichnisPfad);

		bool FUN_blnVerzeichnisExistiert(string i_strPfad);

		IEnumerable<string> FUN_lstHoleUnterverzeichnisse(string i_strPfad, bool i_blnKurzeVerzeichnisNamenZurueckgeben);

		IEnumerable<string> FUN_enuHoleDateiListeAusVerzeichnis(string i_strPfad, IEnumerable<string> i_lstDateiExtensions);

		void SUB_VerzeichnisVerschieben(string i_strPfad, string i_strNeuerPfad);

		void SUB_VerzeichnisRekursivLoeschen(string i_strPfad);

		void SUB_VerzeichnisLoeschen(string i_strPfad);

		void SUB_VerzeichnisErstellen(string i_strPfad);

		void SUB_VerzeichnisKopieren(string i_strPfad, string i_strNeuerPfad, bool i_blnUeberschreiben = false, string i_strDateiAusnahmenFilter = null);

		bool FUN_blnIstValiderOrdnerName(string i_strName);

		IEnumerable<string> FUN_enuDateienImVerzeichnisErmitteln(string i_strPfad);

		IEnumerable<string> FUN_enuHoleDateiListeAusVerzeichnis(string i_strPfad, string i_strSearchPattern);

		IEnumerable<int> FUNa_i32SerielleSchnittstellenNummernErmitteln();
	}
}
