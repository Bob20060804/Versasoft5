using Ersa.Global.Common.Data.Model;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Global.DataProvider.Datenbanktypen;
using Ersa.Global.DataProvider.Factories.StrategieFactory;
using Ersa.Global.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Global.DataAdapter.DatabaseModel.Model
{
	public abstract class EDC_DatenbankModel : INF_DatenbankModel
	{
		private readonly Dictionary<string, string> m_dicTablespacesErstellungsliste = new Dictionary<string, string>();

		private readonly List<string> m_dicTabellenErstellungsliste = new List<string>();

		private readonly List<EDC_UpdateInformation> m_lstDatenbankUpdateliste = new List<EDC_UpdateInformation>();

		private readonly Dictionary<int, Func<INF_DatenbankAdapter, Task>> m_dicDatenbankMigrationliste = new Dictionary<int, Func<INF_DatenbankAdapter, Task>>();

		public abstract string PRO_strSequencesErstellungsString
		{
			get;
		}

		public abstract string PRO_strFunctionErstellungsString
		{
			get;
		}

		public abstract string PRO_strTriggerErstellungsString
		{
			get;
		}

		public abstract string PRO_strConstraintsErstellungsString
		{
			get;
		}

		public int PRO_i32DatenbankVersion
		{
			get;
			private set;
		}

		public string PRO_strDatenbankDatenVerzeichnis
		{
			get;
			private set;
		}

		public string PRO_strDatenbankName
		{
			get;
			set;
		}

		public List<string> PRO_lstTabellenNamen
		{
			get;
			set;
		} = new List<string>();


		public Dictionary<string, string> PRO_dicTablespacesErstellungsliste => m_dicTablespacesErstellungsliste;

		public List<string> PRO_lstTabellenErstellungsliste => m_dicTabellenErstellungsliste;

		public List<EDC_UpdateInformation> PRO_lstDatenbankUpdateliste => m_lstDatenbankUpdateliste;

		public Dictionary<int, Func<INF_DatenbankAdapter, Task>> PRO_dicDatenbankMigrationliste => m_dicDatenbankMigrationliste;

		public string PRO_strDatenbankErstellungsString => PRO_edcDatenbankDialekt.FUN_strHoleDatenbankErstellungsString(PRO_strDatenbankName);

		protected INF_DatenbankDialekt PRO_edcDatenbankDialekt
		{
			get;
			private set;
		}

		protected EDC_DatenbankModel(ENUM_DatenbankTyp i_enmDatenbankTyp, string i_strDatenbankName, string i_strDatenverzeichnis, int i_i32DatenbankVersion)
		{
			PRO_strDatenbankDatenVerzeichnis = i_strDatenverzeichnis;
			PRO_strDatenbankName = i_strDatenbankName;
			PRO_i32DatenbankVersion = i_i32DatenbankVersion;
			PRO_edcDatenbankDialekt = EDC_DatenbankDialektFactory.FUN_edcHoleDatenbankDialekt(i_enmDatenbankTyp);
		}

		public abstract void SUB_ErstelleDatenbankUpdateliste();

		public string FUN_strHoleDatenbankUpdateAnweisungen(int i_i32ZielVersion, int i_i32StartVersion)
		{
			foreach (EDC_UpdateInformation item in PRO_lstDatenbankUpdateliste)
			{
				if (item.PRO_i32Zielversion == i_i32ZielVersion && i_i32StartVersion >= item.PRO_i32Startversion)
				{
					return item.PRO_strScript;
				}
			}
			return string.Empty;
		}

		public Func<INF_DatenbankAdapter, Task> FUN_delHoleDateMigrationsAction(int i_i32Version)
		{
			if (PRO_dicDatenbankMigrationliste.ContainsKey(i_i32Version))
			{
				return PRO_dicDatenbankMigrationliste[i_i32Version];
			}
			return null;
		}

		public string FUN_strHoleAlterTableAddColumnStatement(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten)
		{
			List<EDC_DynamischeSpalte> list = i_lstSpalten.ToList();
			if (list.Count == 0)
			{
				return string.Empty;
			}
			if (list.Count == 1)
			{
				EDC_DynamischeSpalte eDC_DynamischeSpalte = list[0];
				return $"Alter Table {i_strTabellenName} {PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerEineSpalteStatement(eDC_DynamischeSpalte.PRO_strSpaltenName, eDC_DynamischeSpalte.PRO_strDatenTyp, eDC_DynamischeSpalte.PRO_i32DatenLaenge)}";
			}
			string text = string.Empty;
			bool i_blnIsErsteSpalte = true;
			foreach (EDC_DynamischeSpalte item in list)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += PRO_edcDatenbankDialekt.FUN_strHoleAlterTableAddColumnFuerMehrereSpaltenStatement(item.PRO_strSpaltenName, item.PRO_strDatenTyp, item.PRO_i32DatenLaenge, i_blnIsErsteSpalte);
				i_blnIsErsteSpalte = false;
			}
			return $"Alter Table {i_strTabellenName} {text}";
		}

		public string FUN_strHoleAlterTableDropColumnStatement(string i_strTabellenName, IEnumerable<EDC_DynamischeSpalte> i_lstSpalten)
		{
			List<EDC_DynamischeSpalte> list = i_lstSpalten.ToList();
			if (list.Count == 0)
			{
				return string.Empty;
			}
			if (list.Count == 1)
			{
				EDC_DynamischeSpalte eDC_DynamischeSpalte = list[0];
				return $"Alter Table {i_strTabellenName} {PRO_edcDatenbankDialekt.FUN_strHoleAlterTableDropColumnFuerEineSpalteStatement(eDC_DynamischeSpalte.PRO_strSpaltenName)}";
			}
			string text = string.Empty;
			bool i_blnIsErsteSpalte = true;
			foreach (EDC_DynamischeSpalte item in list)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += PRO_edcDatenbankDialekt.FUN_strHoleAlterTableDropColumnFuerMehrereSpaltenStatement(item.PRO_strSpaltenName, i_blnIsErsteSpalte);
				i_blnIsErsteSpalte = false;
			}
			return $"Alter Table {i_strTabellenName} {text}";
		}

		public string FUN_strHoleStandardErstellungsStringFuerModell(object i_objModell)
		{
			string i_strErstellungsstring = i_objModell.FUN_strHoleTabellenErstellungsString(PRO_edcDatenbankDialekt);
			string i_strIndexErstellungsString = i_objModell.FUN_lstHoleIndexErstellungsString(PRO_edcDatenbankDialekt);
			return PRO_edcDatenbankDialekt.FUN_strHoleStandardErstellungsString(i_strErstellungsstring, i_strIndexErstellungsString, i_objModell.FUN_strHoleTabellenName(), i_objModell.FUN_strHoleTabellenTablespace());
		}

		public string FUN_strHoleStandardErstellungsStringFuerModell(Type i_objModell)
		{
			string i_strErstellungsstring = EDC_TabellenErstellungsStringFactory.FUN_strHoleTabellenErstellungsString(PRO_edcDatenbankDialekt, i_objModell);
			string i_strIndexErstellungsString = EDC_IndexErstellungStringFactory.FUN_lstHoleIndexErstellungsString(PRO_edcDatenbankDialekt, i_objModell);
			return PRO_edcDatenbankDialekt.FUN_strHoleStandardErstellungsString(i_strErstellungsstring, i_strIndexErstellungsString, EDC_TabellenAttributHelfer.FUN_strHoleTabellenName(i_objModell), EDC_TablespaceHelfer.FUN_strHoleTabellenTablespace(i_objModell));
		}
	}
}
