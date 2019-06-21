using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.Adapter.Interface
{
	public interface INF_OrganisationsAdapter
	{
		void SUB_SetzeNeuenDatenbankProvider(INF_DatenbankProvider i_edcDatenbankProvider);

		bool FUN_blnPruefeVerbindung(int i_i16Timeout);

		bool FUN_blnPruefeVerbindung(int i_i16Timeout, int i_i16AnzahlVeruche);

		string FUN_strHoleLetzteVerbindungsFehlermeldung();

		void SUB_ErstelleDieDatenbank();

		void SUB_ErstelleDieDatenbankKomponenten();

		void SUB_FuehreDatenbankUpdatesDurch(int i_i16AktuelleVersion, int i_i32EndVersion, Action<long> i_delParameterUpdateAction);

		void SUB_ErstelleEineZusatztabelle(object i_objModell);

		void SUB_FuegeTabellenSpaltenHinzu(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten);

		void SUB_LoescheTabellenSpalten(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten);

		Task FUN_fdcKomprimiereDatenbankAsync();

		Task FUN_fdcRepariereDatenbankAsync();

		Task FUN_fdcVerkleinereDatenbankAsync();

		Task FUN_fdcDatensicherungAsync(string i_strSicherungsverzeichnis);

		Task FUN_fdcReorganisiereDatenbankAsync();

		string FUN_strHoleServerName();

		NameValueCollection FUN_fdcHoleProviderEinstellungen();

		bool FUN_blnIstDatenbankDateibasiert();

		bool FUN_blnExistiertDieTabelle(string i_strTabellenName);

		bool FUN_blnExistiertDieDatenbank(string i_strDatenbankname);

		bool FUN_blnExistiertTablespace(string i_strTablespaceName);

		bool FUN_blnExistiertSequence(string i_strSequenceName);

		void SUB_TrenneDatenbankverbindungen(string i_strDatenbankname);

		void SUB_DropDatabase(string i_strDatenbankname);

		void SUB_LeereTabelle(string i_strTabellenName);

		Task FUN_fdcLeereTabelleAsync(string i_strTabellenName);

		IEnumerable<string> FUN_enuHoleListeAllerTabellen();

		Task<DataTable> FUN_fdcLeseTabellenSchemaAsync(string i_strTabelle);

		string FUN_strErstelleCreateTableScriptAusSchema(DataTable i_fdcSchema, string i_strTabelle);
	}
}
