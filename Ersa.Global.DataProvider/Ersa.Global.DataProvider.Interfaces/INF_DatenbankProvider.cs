using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Ersa.Global.DataProvider.Interfaces
{
	public interface INF_DatenbankProvider
	{
		bool PRO_blnIstDateibasierendeDatenbank
		{
			get;
		}

		bool PRO_blnKannMehrerTabellenSpaltenAufEinmalDroppen
		{
			get;
		}

		string PRO_strConnectionString
		{
			get;
		}

		string PRO_strDatenbankDienstName
		{
			get;
		}

		string PRO_strDatensicherungErweiterung
		{
			get;
		}

		string PRO_strDatenbankIdentifier
		{
			get;
		}

		INF_DatenbankDialekt PRO_edcDialekt
		{
			get;
		}

		string FUN_strHoleDatenbankName();

		string FUN_strHoleDatenbankBenutzerName();

		string FUN_strHoleDatenbankKennwort();

		string FUN_strHoleServerName();

		string FUN_strHoleVorhandeneTabellenAbfrage();

		NameValueCollection FUN_fdcHoleProviderEinstellungen();

		DbConnection FUN_fdcGetConnection();

		DbCommand FUN_fdcGetCommand();

		DbCommand FUN_fdcGetCommand(string i_strSql, Dictionary<string, object> i_dicParameter = null);

		DbCommand FUN_fdcGetCommand(string i_strSql, DbConnection i_fdcConnection, Dictionary<string, object> i_dicParameter = null);

		DbCommand FUN_fdcGetCommand(string i_strSql, IDbTransaction i_fdcDbTransaktion, Dictionary<string, object> i_dicParameter = null);

		void SUB_SetzeCommandParameter(DbCommand i_fdcCommand, Dictionary<string, object> i_dicParameter);

		DbDataAdapter FUN_fdcGetDataAdapter(DbCommand i_fdcCommand);

		DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, IDbTransaction i_fdcDdTransaktion);

		DbDataAdapter FUN_fdcGetDataAdapter(string i_strSql, DbConnection i_fdcConnection);

		DbCommandBuilder FUN_fdcGetCommandBuilder(DbDataAdapter i_fdcAdapter);

		string FUN_strSetzeAbfrageErgebnisLimit(string i_strSql, int i_i32Limit);

		bool FUN_blnExistiertTablespace(Func<string, object> i_delAbfrageFunc, string i_strTablespaceName);

		bool FUN_blnExistiertSequence(Func<string, object> i_delAbfrageFunc, string i_strSequenceName);

		uint FUN_u32HoleNaechstenSequenceWert(Func<string, object> i_delAbfrageFunc, Action<string> i_delUpdateFunc, string i_strSequenceName);

		Task<uint> FUN_u32HoleNaechstenSequenceWertAsync(Func<string, Task<object>> i_delAbfrageFuncAsync, Func<string, Task> i_delUpdateFuncAsync, string i_strSequenceName);

		void SUB_SetzeSequenceWert(Action<string> i_delUpdateFunc, string i_strSequenceName, uint i_i32Wert);

		Task FUN_fdcSetzeSequenceWertAsync(Func<string, Task> i_delUpdateFuncAsync, string i_strSequenceName, uint i_i32Wert);

		bool FUN_blnExistiertDieDatenbank(Func<string, object> i_delAbfrageFunc, string i_strDatenbankname);

		bool FUN_blnExistiertTabelle(Func<string, object> i_delAbfrageFunc, string i_strTabellenName);

		string FUN_strHoleTablespaceFuerTabelle(Func<string, object> i_delAbfrageFunc, string i_strTabellenName);

		void SUB_ErstelleDateibasierendeDatenbank();

		Task FUN_fdcKomprimiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName);

		Task FUN_fdcRepariereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName);

		Task FUN_fdcVerkleinereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName);

		Task FUN_fdcDatensicherungAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName, string i_strBenutzername, string i_strPasswort, string i_strSicherungspfad);

		Task FUN_fdcReorganisiereDatenbankAsync(Func<string, Task> i_delExecuteFuncAsync, string i_strDatenbankName);

		void SUB_TrenneDatenbankverbindungen(Action<string> i_delExecuteFunc, string i_strDatenbankName);

		void SUB_DropDatabase(Action<string> i_delExecuteFunc, string i_strDatenbankName);

		void SUB_LeereTabelle(Action<string> i_delUpdateFunc, string i_strDatenbankName);

		Task FUN_fdcLeereTabelleAsync(Func<string, Task> i_delUpdateFunc, string i_strDatenbankName);
	}
}
