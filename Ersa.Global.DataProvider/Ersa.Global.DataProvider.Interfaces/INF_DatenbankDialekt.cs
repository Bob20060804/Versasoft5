using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Ersa.Global.DataProvider.Interfaces
{
	public interface INF_DatenbankDialekt
	{
		bool PRO_blnHatSequences
		{
			get;
		}

		bool PRO_blnHatTablespaces
		{
			get;
		}

		Dictionary<string, string> FUN_dicDatenTypenMapping();

		string FUN_strHoleDatenbankDatenTypenMapping(string i_strDatentyp);

		DbParameter FUN_edcErstelleDbParameterMitDatentyp(DbCommand i_fdcCommand, string i_strDatentyp);

		string FUN_strHoleTabellenErstellungsStatement(DataTable i_fdcSchema, string i_strTabellenName, string i_strTablespace);

		string FUN_strHoleConstraintErstellungsString(string i_strTabellenName, string i_strConstraint);

		string FUN_strHoleNonUniqueHoleIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace);

		string FUN_strHoleUniqueIndexErstellungsString(string i_strIndexName, string i_strTabellenName, string i_strSpaltenName, string i_strTablespace);

		string FUN_strHoleStandardErstellungsString(string i_strErstellungsstring, string i_strIndexErstellungsString, string i_strTabellenName, string i_strTablespace);

		string FUN_strHoleDatenbankErstellungsString(string i_strDatenbankname);

		string FUN_strErstelleSequenceAnweisung(string i_strSequenceName);

		string FUN_strHoleTablespaceErstellungsString(string i_strTablespaceName, string i_strDatenbankDatenVerzeichnis);

		string FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge);

		string FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, string i_strDatenTyp, int i_i32DatenLaenge, bool i_blnIsErsteSpalte);

		string FUN_strHoleAlterTableDropColumnFuerEineSpalteStatement(string i_strSpaltenName);

		string FUN_strHoleAlterTableDropColumnFuerMehrereSpaltenStatement(string i_strSpaltenName, bool i_blnIsErsteSpalte);

		string FUN_strHoleAlterTableChangeCharacterColumnLength(string i_strSpaltenName, int i_i32NeueLaenge);

		string FUN_strHoleTabellenListeStatement(string i_strDatenbankName);

		string FUN_strHoleDatenbankExistiertStatement(string i_strDatenbankName);

		string FUN_strHoleTabelleExistiertStatement(string i_strTabellenName);

		string FUN_strHoleTablespaceExistenzAbfrage(string i_strTablespaceName);

		string FUN_strHoleTablespaceFuerTabelleStatement(string i_strTabellenName);

		string FUN_strHoleSequenceExistenzAbfrage(string i_strSequenceName);

		string FUN_strHoleSequenceAbfrage(string i_strSequenceName);

		string FUN_strHoleSequenceWertSetzenStatement(string i_strSequenceName, uint i_i32Wert);

		string FUN_strHoleTabellenUmbenennenStatement(string i_strAlterName, string i_strNeuerName);
	}
}
