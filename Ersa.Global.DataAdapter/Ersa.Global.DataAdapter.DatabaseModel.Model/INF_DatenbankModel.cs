using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Adapter.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.DatabaseModel.Model
{
	public interface INF_DatenbankModel
	{
		string PRO_strDatenbankDatenVerzeichnis
		{
			get;
		}

		int PRO_i32DatenbankVersion
		{
			get;
		}

		string PRO_strDatenbankName
		{
			get;
		}

		List<string> PRO_lstTabellenNamen
		{
			get;
			set;
		}

		string PRO_strDatenbankErstellungsString
		{
			get;
		}

		string PRO_strSequencesErstellungsString
		{
			get;
		}

		string PRO_strFunctionErstellungsString
		{
			get;
		}

		string PRO_strTriggerErstellungsString
		{
			get;
		}

		string PRO_strConstraintsErstellungsString
		{
			get;
		}

		List<string> PRO_lstTabellenErstellungsliste
		{
			get;
		}

		Dictionary<string, string> PRO_dicTablespacesErstellungsliste
		{
			get;
		}

		void SUB_ErstelleDatenbankUpdateliste();

		string FUN_strHoleDatenbankUpdateAnweisungen(int i_i32ZielVersion, int i_i32StartVersion);

		Func<INF_DatenbankAdapter, Task> FUN_delHoleDateMigrationsAction(int i_i32Version);

		string FUN_strHoleStandardErstellungsStringFuerModell(Type i_objModell);

		string FUN_strHoleStandardErstellungsStringFuerModell(object i_objModell);

		string FUN_strHoleAlterTableAddColumnStatement(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten);

		string FUN_strHoleAlterTableDropColumnStatement(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten);
	}
}
