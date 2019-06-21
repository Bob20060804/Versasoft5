using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Benutzer;
using Ersa.Platform.Common.Data.Codetabelle;
using Ersa.Platform.Common.Data.Maschinenkonfiguration;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Data.Produktionssteuerung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Betriebsfall;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Betriebsfall
{
	public class EDC_BetriebsfallImportExportDataAccess : EDC_DataAccess, INF_BetriebsfallmportExportIDataAccess, INF_DataAccess
	{
		private const string mC_strSequenceTabellenName = "SequenceTabelle";

		private const string mC_strSequenceTabelleSpalteName = "SequenceName";

		private const string mC_strSequenceTabelleSpalteWert = "SequenceWert";

		private readonly INF_IODienst m_edcIoDienst;

		public EDC_BetriebsfallImportExportDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter, INF_IODienst i_edcIoDienst)
			: base(i_edcDatenbankAdapter)
		{
			m_edcIoDienst = i_edcIoDienst;
		}

		public async Task FUN_fdcExportBetriebsdatenDataAsync(string i_strExportpfad, string i_strDateiname, long i_i64MaschinenId)
		{
			if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strExportpfad))
			{
				m_edcIoDienst.SUB_VerzeichnisErstellen(i_strExportpfad);
			}
			string strDateinameMitPfad = Path.Combine(i_strExportpfad, i_strDateiname);
			List<EDC_AbfrageContainer> list = new List<EDC_AbfrageContainer>();
			List<string> lstTabellenNamenAlleDaten = new List<string>();
			SUB_TabellenFuerExportErstellen(i_i64MaschinenId, list, lstTabellenNamenAlleDaten);
			DataSet fdcDataSet = new DataSet();
			foreach (EDC_AbfrageContainer item in list)
			{
				DataTable table = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(item.PRO_objSelectObjekt, null, item.PRO_dicDictionary).ConfigureAwait(continueOnCapturedContext: false);
				fdcDataSet.Tables.Add(table);
			}
			foreach (string item2 in lstTabellenNamenAlleDaten)
			{
				DataTable table2 = await m_edcDatenbankAdapter.FUN_fdcLeseTabelleVollstaendigAsync(item2).ConfigureAwait(continueOnCapturedContext: false);
				fdcDataSet.Tables.Add(table2);
			}
			DataTable table3 = await FUN_fdcSequenceTabelleErstellenAsync().ConfigureAwait(continueOnCapturedContext: false);
			fdcDataSet.Tables.Add(table3);
			await Task.Factory.StartNew(delegate
			{
				fdcDataSet.WriteXml(strDateinameMitPfad, XmlWriteMode.WriteSchema);
			}).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcImportBetriebsdatenDataAsync(string i_strImportDatei)
		{
			List<string> lstAlleTabellenNamen = new List<string>();
			List<string> lstTabellenNamenGeleertVorImport = new List<string>
			{
				"Parameter",
				"ActiveUsers",
				"Users",
				"UserMachineMapping",
				"UserTrackings",
				"MachineGroupMembers",
				"Machines",
				"MaschineGroups"
			};
			SUB_TabellenFuerImportErstellen(lstAlleTabellenNamen);
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (string item in lstTabellenNamenGeleertVorImport)
				{
					string i_strSql = $"delete {item}";
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				}
				DataSet fdcDataSet = await EDC_XmlZuDataSetReader.FUN_fdcLeseXmlDateiInDatasetAsync(i_strImportDatei).ConfigureAwait(continueOnCapturedContext: false);
				foreach (string item2 in lstAlleTabellenNamen)
				{
					DataTable dataTable = fdcDataSet.Tables[item2];
					if (dataTable != null)
					{
						await m_edcDatenbankAdapter.FUN_fdcSchreibeDatenInTabelleAsync(dataTable, item2, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				foreach (DataRow row in fdcDataSet.Tables["SequenceTabelle"].Rows)
				{
					string i_strSequenceName = (string)row["SequenceName"];
					uint.TryParse((string)row["SequenceWert"], out uint result);
					await m_edcDatenbankAdapter.FUN_fdcSetzeSequenceWertAsync(i_strSequenceName, result).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		private static void SUB_TabellenFuerExportErstellen(long i_i64MaschinenId, ICollection<EDC_AbfrageContainer> i_lstTabellenAbfragen, ICollection<string> i_lstTabellenNamenAlleDaten)
		{
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_MaschineData(EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_MaschinenGruppeData(EDC_MaschinenGruppeData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_MaschinenGruppenMitgliedData(EDC_MaschinenGruppenMitgliedData.FUN_strGruppeZuMaschinenWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_MaschinenkonfigurationData(EDC_MaschinenkonfigurationData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strWhereStatement = EDC_MeldungData.FUN_strDatumAufgetretenNachStartdatumWhereStatementErstellen(DateTime.Now - TimeSpan.FromDays(14.0), i_i64MaschinenId, dictionary);
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_MeldungData(i_strWhereStatement), dictionary));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_BenutzerData(EDC_BenutzerData.FUN_strBenutzerIdSunbselectErstellen(EDC_BenutzerMappingData.FUN_strBenutzerIdSunbselectErstellen(i_i64MaschinenId))), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_BenutzerMappingData(EDC_BenutzerMappingData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_AktiverBenutzerData(EDC_AktiverBenutzerData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_BenutzerTrackData(EDC_BenutzerTrackData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_ProduktionssteuerungData(EDC_ProduktionssteuerungData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_CodetabelleData(EDC_CodetabelleData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenAbfragen.Add(new EDC_AbfrageContainer(new EDC_CodetabelleneintragData(EDC_CodetabelleneintragData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId)), null));
			i_lstTabellenNamenAlleDaten.Add("CodeCache");
			i_lstTabellenNamenAlleDaten.Add("Nozzles");
			i_lstTabellenNamenAlleDaten.Add("Parameter");
			i_lstTabellenNamenAlleDaten.Add("CyclicMessageLib");
			i_lstTabellenNamenAlleDaten.Add("MessagesCyclicTemplates");
		}

		private static void SUB_TabellenFuerImportErstellen(ICollection<string> i_lstAlleTabellenNamen)
		{
			i_lstAlleTabellenNamen.Add("Machines");
			i_lstAlleTabellenNamen.Add("MaschineGroups");
			i_lstAlleTabellenNamen.Add("MachineGroupMembers");
			i_lstAlleTabellenNamen.Add("MachineConfigurations");
			i_lstAlleTabellenNamen.Add("Messages");
			i_lstAlleTabellenNamen.Add("Users");
			i_lstAlleTabellenNamen.Add("ActiveUsers");
			i_lstAlleTabellenNamen.Add("UserTrackings");
			i_lstAlleTabellenNamen.Add("ProductionControl");
			i_lstAlleTabellenNamen.Add("CodeTables");
			i_lstAlleTabellenNamen.Add("CodeTableMembers");
			i_lstAlleTabellenNamen.Add("CodeCache");
			i_lstAlleTabellenNamen.Add("Nozzles");
			i_lstAlleTabellenNamen.Add("Parameter");
			i_lstAlleTabellenNamen.Add("CyclicMessageLib");
			i_lstAlleTabellenNamen.Add("MessagesCyclicTemplates");
		}

		private async Task<DataTable> FUN_fdcSequenceTabelleErstellenAsync()
		{
			DataTable fdcSequenceTabelle = new DataTable("SequenceTabelle");
			DataColumn column = new DataColumn
			{
				DataType = Type.GetType("System.String"),
				ColumnName = "SequenceName"
			};
			fdcSequenceTabelle.Columns.Add(column);
			DataColumn column2 = new DataColumn
			{
				DataType = Type.GetType("System.Int32"),
				ColumnName = "SequenceWert"
			};
			fdcSequenceTabelle.Columns.Add(column2);
			DataRow fdcZeileSequence3 = fdcSequenceTabelle.NewRow();
			fdcZeileSequence3["SequenceName"] = "ess5_sequnique";
			DataRow dataRow = fdcZeileSequence3;
			object obj2 = dataRow["SequenceWert"] = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			fdcSequenceTabelle.Rows.Add(fdcZeileSequence3);
			DataRow fdcZeileSequence2 = fdcSequenceTabelle.NewRow();
			fdcZeileSequence2["SequenceName"] = "ess5_seprotokoll";
			dataRow = fdcZeileSequence2;
			obj2 = (dataRow["SequenceWert"] = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_seprotokoll").ConfigureAwait(continueOnCapturedContext: false));
			fdcSequenceTabelle.Rows.Add(fdcZeileSequence2);
			return fdcSequenceTabelle;
		}
	}
}
