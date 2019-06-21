using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Produktionssteuerung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Produktionssteuerung;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Produktionssteuerung
{
	public class EDC_ProduktionssteuerungDataAccess : EDC_DataAccess, INF_ProduktionssteuerungDataAccess, INF_DataAccess
	{
		private readonly INF_IODienst m_edcIoDienst;

		public EDC_ProduktionssteuerungDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter, INF_IODienst i_edcIoDienst)
			: base(i_edcDatenbankAdapter)
		{
			m_edcIoDienst = i_edcIoDienst;
		}

		public Task<IEnumerable<EDC_ProduktionssteuerungData>> FUN_edcProduktionssteuerungDatenLadenAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_ProduktionssteuerungData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_ProduktionssteuerungData(i_strWhereStatement));
		}

		public Task<EDC_ProduktionssteuerungData> FUN_edcProduktionssteuerungDataLadenAsync(long i_i64ProduktionssteuerungId)
		{
			string i_strWhereStatement = EDC_ProduktionssteuerungData.FUN_strProduktionssteuerungIdWhereStatementErstellen(i_i64ProduktionssteuerungId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_ProduktionssteuerungData(i_strWhereStatement));
		}

		public Task<EDC_ProduktionssteuerungData> FUN_edcAktiveProduktionssteuerungDataLadenAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_ProduktionssteuerungData.FUN_strMaschinenIdUndAktivWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_ProduktionssteuerungData(i_strWhereStatement));
		}

		public async Task FUN_fdcProduktionssteuerungAktivSetzenAsync(long i_i64ProduktionssteuerungId, long i_i64MaschinenId, long i_i64BenutzerId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string strAktivUpdateStatement = EDC_ProduktionssteuerungData.FUN_strUpdateStatementAktivErstellen(i_i64ProduktionssteuerungId, i_i64BenutzerId);
				string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(i_i64ProduktionssteuerungId, i_i64BenutzerId, i_i64MaschinenId);
				Dictionary<string, object> dicParameter = new Dictionary<string, object>
				{
					{
						"pChangeDate",
						DateTime.Now
					}
				};
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion, dicParameter).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(strAktivUpdateStatement, fdcTransaktion, dicParameter).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64ProduktionssteuerungId, long i_i64MaschinenId, string i_strBeschreibung, string i_strEinstellungen, bool i_blnIstAktiv, long i_i64BenutzerId)
		{
			if (i_blnIstAktiv)
			{
				Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
				{
					{
						"pChangeDate",
						DateTime.Now
					}
				};
				string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(i_i64ProduktionssteuerungId, i_i64BenutzerId, i_i64MaschinenId);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, null, i_dicParameter).ConfigureAwait(continueOnCapturedContext: false);
			}
			EDC_ProduktionssteuerungData eDC_ProduktionssteuerungData = new EDC_ProduktionssteuerungData();
			EDC_ProduktionssteuerungData eDC_ProduktionssteuerungData2 = eDC_ProduktionssteuerungData;
			eDC_ProduktionssteuerungData2.PRO_i64ProduktionssteuerungId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			eDC_ProduktionssteuerungData.PRO_i64MaschinenId = i_i64MaschinenId;
			eDC_ProduktionssteuerungData.PRO_strBeschreibung = i_strBeschreibung;
			eDC_ProduktionssteuerungData.PRO_strEinstellungen = i_strEinstellungen;
			eDC_ProduktionssteuerungData.PRO_blnIstAktiv = i_blnIstAktiv;
			eDC_ProduktionssteuerungData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_ProduktionssteuerungData.PRO_dtmAngelegtAm = DateTime.Now;
			eDC_ProduktionssteuerungData.PRO_i64BearbeitetVon = i_i64BenutzerId;
			eDC_ProduktionssteuerungData.PRO_dtmBearbeitetAm = DateTime.Now;
			await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(eDC_ProduktionssteuerungData).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64MaschinenId, EDC_ProduktionssteuerungData i_edcProduktionssteuerungData, long i_i64BenutzerId)
		{
			return FUN_fdcProduktionssteuerungDataAendernAsync(i_edcProduktionssteuerungData, i_i64BenutzerId, i_i64MaschinenId);
		}

		public async Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64MaschinenId, IEnumerable<EDC_ProduktionssteuerungData> i_lstProduktionssteuerungData, long i_i64BenutzerId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (EDC_ProduktionssteuerungData i_lstProduktionssteuerungDatum in i_lstProduktionssteuerungData)
				{
					await FUN_fdcProduktionssteuerungDataAendernAsync(i_lstProduktionssteuerungDatum, i_i64BenutzerId, i_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcProduktionssteuerungDataErstellenAsync(long i_i64MaschinenId, string i_strBeschreibung, string i_strEinstellungen, bool i_blnIstAktiv, long i_i64BenutzerId)
		{
			long i64ProduktionssteuerungId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (i_blnIstAktiv)
				{
					Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
					{
						{
							"pChangeDate",
							DateTime.Now
						}
					};
					string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(i64ProduktionssteuerungId, i_i64BenutzerId, i_i64MaschinenId);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion, i_dicParameter).ConfigureAwait(continueOnCapturedContext: false);
				}
				EDC_ProduktionssteuerungData i_edcObjekt = new EDC_ProduktionssteuerungData
				{
					PRO_i64ProduktionssteuerungId = i64ProduktionssteuerungId,
					PRO_i64MaschinenId = i_i64MaschinenId,
					PRO_strBeschreibung = i_strBeschreibung,
					PRO_strEinstellungen = i_strEinstellungen,
					PRO_blnIstAktiv = i_blnIstAktiv,
					PRO_i64AngelegtVon = i_i64BenutzerId,
					PRO_dtmAngelegtAm = DateTime.Now,
					PRO_i64BearbeitetVon = i_i64BenutzerId,
					PRO_dtmBearbeitetAm = DateTime.Now
				};
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
				return i64ProduktionssteuerungId;
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task<long> FUN_fdcProduktionssteuerungDataErstellenAsync(long i_i64MaschinenId, EDC_ProduktionssteuerungData i_edcProduktionssteuerungData, long i_i64BenutzerId)
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
				if (i_edcProduktionssteuerungData.PRO_blnIstAktiv)
				{
					Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
					{
						{
							"pChangeDate",
							DateTime.Now
						}
					};
					string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId, i_i64BenutzerId, i_i64MaschinenId);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion, i_dicParameter).ConfigureAwait(continueOnCapturedContext: false);
				}
				i_edcProduktionssteuerungData.PRO_i64AngelegtVon = i_i64BenutzerId;
				i_edcProduktionssteuerungData.PRO_dtmAngelegtAm = DateTime.Now;
				i_edcProduktionssteuerungData.PRO_i64BearbeitetVon = i_i64BenutzerId;
				i_edcProduktionssteuerungData.PRO_dtmBearbeitetAm = DateTime.Now;
				i_edcProduktionssteuerungData.PRO_i64MaschinenId = i_i64MaschinenId;
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcProduktionssteuerungData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
			return i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId;
		}

		public Task FUN_fdcProduktionssteuerungDataLoeschenAsync(long i_i64ProduktionssteuerungId)
		{
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_ProduktionssteuerungData(EDC_ProduktionssteuerungData.FUN_strProduktionssteuerungIdWhereStatementErstellen(i_i64ProduktionssteuerungId)));
		}

		public async Task<bool> FUN_blnExportProduktionssteuerungDataAsync(long i_i64MaschinenId, string i_strExportpfad)
		{
			try
			{
				if (!m_edcIoDienst.FUN_blnVerzeichnisExistiert(i_strExportpfad))
				{
					m_edcIoDienst.SUB_VerzeichnisErstellen(i_strExportpfad);
				}
				string i_strWhereStatement = EDC_ProduktionssteuerungData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
				DataTable table = await m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_ProduktionssteuerungData(i_strWhereStatement));
				DataSet dataSet = new DataSet();
				dataSet.Tables.Add(table);
				string fileName = Path.Combine(i_strExportpfad, string.Format("{0}.xml", "ProductionControl"));
				dataSet.WriteXml(fileName, XmlWriteMode.WriteSchema);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> FUN_blnImportProduktionssteuerungDataAsync(string i_strImportDatei, long i_i64BenutzerId)
		{
			bool blnFehlerAufgetreten2 = false;
			bool blnHatAktiveSpalte = false;
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				DataTable edcProduktionssteuerungDataTabelle = (await EDC_XmlZuDataSetReader.FUN_fdcLeseXmlDateiInDatasetAsync(i_strImportDatei).ConfigureAwait(continueOnCapturedContext: false)).Tables["ProductionControl"];
				object objProduktionsstuerungId = -1;
				for (int i32Index = 0; i32Index < edcProduktionssteuerungDataTabelle.Rows.Count; i32Index++)
				{
					uint num = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
					edcProduktionssteuerungDataTabelle.Rows[i32Index]["ProductionControlId"] = num;
					edcProduktionssteuerungDataTabelle.Rows[i32Index]["ChangeUser"] = i_i64BenutzerId;
					edcProduktionssteuerungDataTabelle.Rows[i32Index]["ChangeDate"] = DateTime.Now;
					if (edcProduktionssteuerungDataTabelle.Rows[i32Index]["IsActive"].Equals("1"))
					{
						blnHatAktiveSpalte = true;
						objProduktionsstuerungId = edcProduktionssteuerungDataTabelle.Rows[i32Index]["ProductionControlId"];
					}
				}
				if (blnHatAktiveSpalte && edcProduktionssteuerungDataTabelle.Rows.Count > 0)
				{
					object value = edcProduktionssteuerungDataTabelle.Rows[0]["ChangeDate"];
					string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(Convert.ToInt32(objProduktionsstuerungId), i_i64BenutzerId, Convert.ToInt32(value));
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction, new Dictionary<string, object>
					{
						{
							"pChangeDate",
							DateTime.Now
						}
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				await m_edcDatenbankAdapter.FUN_fdcSchreibeDatenInTabelleAsync(edcProduktionssteuerungDataTabelle, edcProduktionssteuerungDataTabelle.TableName, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
				return blnFehlerAufgetreten2;
			}
			catch
			{
				blnFehlerAufgetreten2 = true;
				SUB_RollbackTransaktion(fdcDbTransaction);
				return blnFehlerAufgetreten2;
			}
		}

		private async Task FUN_fdcProduktionssteuerungDataAendernAsync(EDC_ProduktionssteuerungData i_edcProduktionssteuerungData, long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				if (i_edcProduktionssteuerungData.PRO_blnIstAktiv)
				{
					Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
					{
						{
							"pChangeDate",
							DateTime.Now
						}
					};
					string i_strSql = EDC_ProduktionssteuerungData.FUN_strUpdateStatementNichtAktivErstellen(i_edcProduktionssteuerungData.PRO_i64ProduktionssteuerungId, i_i64BenutzerId, i_i64MaschinenId);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion, i_dicParameter).ConfigureAwait(continueOnCapturedContext: false);
				}
				i_edcProduktionssteuerungData.PRO_i64BearbeitetVon = i_i64BenutzerId;
				i_edcProduktionssteuerungData.PRO_dtmBearbeitetAm = DateTime.Now;
				i_edcProduktionssteuerungData.PRO_i64MaschinenId = i_i64MaschinenId;
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcProduktionssteuerungData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}
	}
}
