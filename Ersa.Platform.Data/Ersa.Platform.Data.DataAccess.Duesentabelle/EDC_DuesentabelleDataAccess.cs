using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Platform.Common.Data.Duesentabelle;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Duesentabelle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ersa.Platform.Data.DataAccess.Duesentabelle
{
	public class EDC_DuesentabelleDataAccess : EDC_DataAccess, INF_DuesentabelleDataAccess, INF_DataAccess
	{
		private const string mC_strParentElement = "EDC_TiegelDuesenTabelleZeile";

		private const string mC_strElementName = "Name";

		private const string mC_strElementBeschreibung = "Beschreibung";

		private const string mC_strElementPositionX = "PositionX";

		private const string mC_strElementPositionY = "PositionY";

		private const string mC_strElementAbmessungY = "AbmessungY";

		private const string mC_strElementAbmessungZ = "AbmessungZ";

		private const string mC_strElementBezugOffsetX = "BezugOffsetX";

		private const string mC_strElementBezugOffsetY = "BezugOffsetY";

		private const string mC_strLoetAggregatAbrev = "Lm";

		private const string mC_strFluxAggregatAbrev = "Fm";

		public EDC_DuesentabelleDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<IEnumerable<EDC_DuesenData>> FUN_fdcDuesenDatenLadenAsync(string i_strAggregatName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenData.FUN_strAggregatNameWhereStatementErstellen(i_strAggregatName, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleDuesentabellenDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenData.FUN_strHoleMaschinenIdWhereStatement(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_DuesenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcImportiereDuesentabellenDataAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion, long i_i64NeueMaschinenId = 0L)
		{
			INF_ObjektAusDataRow<EDC_DuesenData> edcConverter = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataRowConverter<EDC_DuesenData>();
			string i_strSql = EDC_DuesenData.FUN_strErstelleMaschinenIdLoeschenStatement(i_i64NeueMaschinenId);
			await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			foreach (DataRow row in i_fdcDataTable.Rows)
			{
				EDC_DuesenData edcDuesenData = edcConverter.FUN_edcLeseObjektAusDataRow(row);
				if (i_i64NeueMaschinenId > 0)
				{
					edcDuesenData.PRO_i64MaschinenId = i_i64NeueMaschinenId;
				}
				EDC_DuesenData eDC_DuesenData = edcDuesenData;
				eDC_DuesenData.PRO_i64DuesenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcDuesenData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcDuesenDatenSatzHinzufuegenAsync(EDC_DuesenData i_edcDuese, IDbTransaction i_fdcTransaktion = null)
		{
			long num2 = i_edcDuese.PRO_i64DuesenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcDuese, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return i_edcDuese.PRO_i64DuesenId;
		}

		public Task FUN_fdcDuesenDatenSatzAendernAsync(EDC_DuesenData i_edcDuese, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcDuese.PRO_strWhereStatement = EDC_DuesenData.FUN_strHoleDuesenIdWhereStatement(i_edcDuese.PRO_i64DuesenId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcDuese, i_fdcTransaktion);
		}

		public Task FUN_fdcDuesenDatenSatzLoeschenAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenData.FUN_strHoleDuesenIdWhereStatement(i_i64DuesenId);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_DuesenData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_DuesenData> FUN_fdcHoleDuesenDatenMitDuesenIdAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_DuesenData i_edcSelectObjekt = new EDC_DuesenData(EDC_DuesenData.FUN_strHoleDuesenIdWhereStatement(i_i64DuesenId));
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(i_edcSelectObjekt, null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_DuesenGeometrieUndSollwerteAbfrageData>> FUN_fdcHoleAlleDuesenGeometrienUndSollwerteAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenGeometrieUndSollwerteAbfrageData.FUN_strDuesenGeometrienIdSubselectWhereStaement(EDC_DuesenData.FUN_strGeometrieIdSubselectStatement(i_i64MaschinenId));
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenGeometrieUndSollwerteAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<EDC_DuesenGeometrieUndSollwerteAbfrageData> FUN_fdcHoleDuesenGeometrieUndSollwerteZuEinerGeometrieAsync(long i_i64GeometrieId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenGeometrieUndSollwerteAbfrageData.FUN_strDuesenGeometrienIdAbfrageWhereStaement(i_i64GeometrieId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_DuesenGeometrieUndSollwerteAbfrageData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task FUN_fdcDuesenSollwerteDatenSatzHinzufuegenAsync(EDC_DuesenSollwerteData i_edcDueseSollwerte, IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcDueseSollwerte, i_fdcTransaktion);
		}

		public Task FUN_fdcDuesenSollwerteDatenSatzAendernAsync(EDC_DuesenSollwerteData i_edcDueseSollwerte, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcDueseSollwerte.PRO_strWhereStatement = EDC_DuesenSollwerteData.FUN_strHoleGeometrieIdWhereStatement(i_edcDueseSollwerte.PRO_i64GeometrieId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcDueseSollwerte, i_fdcTransaktion);
		}

		public async Task<IEnumerable<EDC_Loetduese>> FUN_fdcHoleAlleLoetduesenAsync(long i_i64MaschinenId, string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenData.FUN_strAggregatTypWhereStatementErstellen("Lm", i_i64MaschinenId);
			return (from i_edcData in (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList()
			select FUN_edcExtrahiereLoetduese(i_edcData.PRO_strDuesenInhalt, i_i64MaschinenId, i_edcData.PRO_strAggregatName, i_strMaschinenNummer) into i_edcDuese
			where i_edcDuese != null
			select i_edcDuese).ToList();
		}

		public async Task<EDC_DuesenGeometrienData> FUN_fdcHoleDuesenGeometrieZuEinerDuesenParameterIdAsync(long i_i64DuesenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenGeometrieAbfrageData.FUN_strDuesenGeometrienAusDuesenIdAbfrageWhereStaement(i_i64DuesenId);
			return await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_DuesenGeometrieAbfrageData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleVonMaschineVerwendetenStandardGeometrienAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_DuesenGeometrienData.FUN_strGeometrienSubselectFilterWhereStatement(EDC_DuesenData.FUN_strGeometrieIdSubselectStatement(i_i64MaschinenId));
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenGeometrienData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_DuesenGeometrienData>> FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync(IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_DuesenGeometrienData(EDC_DuesenGeometrienData.FUN_strGeometrienSichtbarFilterWhereStatement()), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleAlleLoetduesenStandardGeometrienDataTableAsync()
		{
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_DuesenGeometrienData(EDC_DuesenGeometrienData.FUN_strGeometrienSichtbarFilterWhereStatement()));
		}

		public async Task FUN_fdcImportiereDuesenGeometrienDataAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion)
		{
			List<EDC_DuesenGeometrienData> lstGeometrien = (await FUN_fdcHoleAlleLoetduesenStandardGeometrienAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList();
			INF_ObjektAusDataRow<EDC_DuesenGeometrienData> edcConverter = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataRowConverter<EDC_DuesenGeometrienData>();
			foreach (DataRow row in i_fdcDataTable.Rows)
			{
				EDC_DuesenGeometrienData edcDuesenGeometrienData = edcConverter.FUN_edcLeseObjektAusDataRow(row);
				if (lstGeometrien.FirstOrDefault((EDC_DuesenGeometrienData i_edcData) => i_edcData.PRO_i64GeometrieId == edcDuesenGeometrienData.PRO_i64GeometrieId) == null)
				{
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcDuesenGeometrienData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		public async Task<bool> FUN_fdcAktualisiereDuesenGeometrienAusDateiAsync(string i_strImportDatei)
		{
			DataTable fdcTable = (await EDC_XmlZuDataSetReader.FUN_fdcLeseXmlDateiInDatasetAsync(i_strImportDatei).ConfigureAwait(continueOnCapturedContext: false)).Tables[0];
			if (fdcTable.Rows.Count == 0)
			{
				return true;
			}
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_DuesenGeometrienData.FUN_strErstelleLoescheTabellenInhaltAtatement();
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSchreibeDatenInTabelleAsync(fdcTable, "NozzleGeometries", fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
				return true;
			}
			catch
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				return false;
			}
		}

		private static EDC_Loetduese FUN_edcExtrahiereLoetduese(string i_strXml, long i_i64MaschinenId, string i_strAggregatName, string i_strMaschinenNummer)
		{
			return (from i_fdcNode in XDocument.Parse(i_strXml).Descendants()
			where i_fdcNode.Name.LocalName == "EDC_TiegelDuesenTabelleZeile"
			select FUN_edcErstelleLoetduese(i_i64MaschinenId, i_strAggregatName, i_strMaschinenNummer, i_fdcNode)).FirstOrDefault();
		}

		private static EDC_Loetduese FUN_edcErstelleLoetduese(long i_i64MaschinenId, string i_strAggregatName, string i_strMaschinenNummer, XElement i_fdcNode)
		{
			EDC_Loetduese eDC_Loetduese = new EDC_Loetduese(i_i64MaschinenId, i_strAggregatName, i_strMaschinenNummer);
			try
			{
				eDC_Loetduese.PRO_strName = (string)i_fdcNode.Element("Name");
				eDC_Loetduese.PRO_strBeschreibung = (string)i_fdcNode.Element("Beschreibung");
				eDC_Loetduese.PRO_dblPositionX = (double)i_fdcNode.Element("PositionX");
				eDC_Loetduese.PRO_dblPositionY = (double)i_fdcNode.Element("PositionY");
				eDC_Loetduese.PRO_dblAbmessungY = (double)i_fdcNode.Element("AbmessungY");
				eDC_Loetduese.PRO_dblAbmessungZ = (double)i_fdcNode.Element("AbmessungZ");
				eDC_Loetduese.PRO_dblBezugOffsetX = (double)i_fdcNode.Element("BezugOffsetX");
				eDC_Loetduese.PRO_dblBezugOffsetY = (double)i_fdcNode.Element("BezugOffsetY");
				return eDC_Loetduese;
			}
			catch (Exception)
			{
				return eDC_Loetduese;
			}
		}
	}
}
