using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using Ersa.Global.DataAdapter.Factories;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Maschinenverwaltung
{
	public class EDC_MaschinenDataAccess : EDC_DataAccess, INF_MaschinenDataAccess, INF_DataAccess
	{
		public EDC_MaschinenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<long> FUN_fdcRegistriereMaschineAsync(string i_strMaschinenTyp, string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				EDC_MaschineData edcGespeicherteMaschineData = await FUN_fdcHoleMaschinenDataAsync(i_strMaschinenNummer, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				if (edcGespeicherteMaschineData == null)
				{
					EDC_MaschineData eDC_MaschineData = new EDC_MaschineData();
					EDC_MaschineData eDC_MaschineData2 = eDC_MaschineData;
					eDC_MaschineData2.PRO_i64MaschinenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
					eDC_MaschineData.PRO_strMaschinenTyp = i_strMaschinenTyp;
					eDC_MaschineData.PRO_strMaschinenNummer = i_strMaschinenNummer;
					eDC_MaschineData.PRO_dtmAngelegt = DateTime.Now;
					edcGespeicherteMaschineData = eDC_MaschineData;
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcGespeicherteMaschineData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (!(await FUN_fdcHoleZugewieseneGruppenIdsAsync(edcGespeicherteMaschineData.PRO_i64MaschinenId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).Any())
				{
					EDC_MaschinenGruppeData edcDefaultGruppeData = (await FUN_edcHoleDefaultMaschinenGruppeAsync(i_strMaschinenTyp, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)) ?? (await FUN_fdcLegeDefaultMaschinenGruppeAnAsync(i_strMaschinenTyp, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false));
					if (!(await FUN_fdcPruefeObMaschineInGruppeIstAsync(edcDefaultGruppeData, edcGespeicherteMaschineData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)))
					{
						await FUN_fdcFuegeMaschineZuGruppeHinzuAsync(edcDefaultGruppeData, edcGespeicherteMaschineData, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
				return edcGespeicherteMaschineData.PRO_i64MaschinenId;
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public Task<IEnumerable<EDC_MaschineData>> FUN_fdcListeAllerMaschinenLadenAsync(IDbTransaction i_fdcTransaktion = null)
		{
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschineData(), i_fdcTransaktion);
		}

		public async Task<bool> FUN_fdcSindMehrereGleicheMaschinentypenRegistriertAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			string i_strSql = EDC_MaschineData.FUN_strSelectCountMaschinenTypErstellen((await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschineData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_strMaschinenTyp);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return false;
			}
			return Convert.ToInt32(obj) > 1;
		}

		public Task<EDC_MaschineData> FUN_fdcHoleMaschinenDataAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschineData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<EDC_MaschineData> FUN_fdcHoleMaschinenDataAsync(string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenNummerWhereStatementErstellen(i_strMaschinenNummer);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschineData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleMaschinenDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_MaschineData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<long> FUN_fdcImportiereMaschinenDatenAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion)
		{
			INF_ObjektAusDataRow<EDC_MaschineData> iNF_ObjektAusDataRow = EDC_ConverterFactory.FUN_edcErstelleObjektAusDataRowConverter<EDC_MaschineData>();
			DataRow i_fdcDataRow = i_fdcDataTable.Rows[0];
			EDC_MaschineData eDC_MaschineData = iNF_ObjektAusDataRow.FUN_edcLeseObjektAusDataRow(i_fdcDataRow);
			return FUN_fdcRegistriereMaschineAsync(eDC_MaschineData.PRO_strMaschinenTyp, eDC_MaschineData.PRO_strMaschinenNummer, i_fdcTransaktion);
		}

		public Task FUN_fdcUpdateMaschineAsync(EDC_MaschineData i_edcMaschineData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcMaschineData.PRO_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_edcMaschineData.PRO_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcMaschineData, i_fdcTransaktion);
		}

		public async Task<IEnumerable<long>> FUN_fdcHoleZugewieseneGruppenIdsAsync(long i_i64Maschinenid, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64Maschinenid == 0L)
			{
				return new List<long>
				{
					0L
				}.AsEnumerable();
			}
			string i_strWhereStatement = EDC_MaschinenGruppenMitgliedData.FUN_strGruppeZuMaschinenWhereStatementErstellen(i_i64Maschinenid);
			return (from i_edcEintrag in await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenGruppenMitgliedData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)
			select i_edcEintrag.PRO_i64GruppenId).ToList();
		}

		public Task<DataTable> FUN_fdcHoleGruppenMitgliedDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppenMitgliedData.FUN_strGruppeZuMaschinenWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_MaschinenGruppenMitgliedData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<long> FUN_fdcHoleAktiveCodetabellenIdAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64MaschinenId == 0L)
			{
				return 0L;
			}
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return (await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschineData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_i64CodetabellenId;
		}

		public async Task FUN_fdcSetzeAktiveCodetabellenIdAsync(long i_i64MaschinenId, long i_i64CodetabellenId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64MaschinenId != 0L && i_i64CodetabellenId != 0L)
			{
				string i_strSql = EDC_MaschineData.FUN_strUpdateCodetabellenIdStatementErstellen(i_i64MaschinenId, i_i64CodetabellenId);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcHoleDefaultLoetProgrammIdAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64MaschinenId == 0L)
			{
				return 0L;
			}
			string i_strWhereStatement = EDC_MaschineData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return (await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschineData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).PRO_i64DefaultProgramId;
		}

		public async Task FUN_fdcSetzeDefaultLoetProgrammIdAsync(long i_i64MaschinenId, long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_i64MaschinenId != 0L)
			{
				string i_strSql = EDC_MaschineData.FUN_strUpdateDefaultProgrammIdStatementErstellen(i_i64MaschinenId, i_i64ProgrammId);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task<long> FUN_fdcHoleDefaultMaschinenGruppenIdAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData = await FUN_edcHoleDefaultMaschinenGruppeAsync(i_strMaschinenTyp, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_MaschinenGruppeData == null)
			{
				return 0L;
			}
			long pRO_i64GruppenId = eDC_MaschinenGruppeData.PRO_i64GruppenId;
			if (pRO_i64GruppenId == 0L)
			{
				throw new ArgumentException("Machine group id=0", "i_strMaschinenTyp");
			}
			return pRO_i64GruppenId;
		}

		public async Task<long> FUN_i64LegeMaschinenGruppeAnAsync(string i_strGruppenName, string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData = new EDC_MaschinenGruppeData();
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData2 = eDC_MaschinenGruppeData;
			long num2 = eDC_MaschinenGruppeData2.PRO_i64GruppenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			eDC_MaschinenGruppeData.PRO_strGruppenName = i_strGruppenName;
			eDC_MaschinenGruppeData.PRO_strMaschinenTyp = i_strMaschinenTyp;
			EDC_MaschinenGruppeData edcNeueMaschinenGruppeData = eDC_MaschinenGruppeData;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcNeueMaschinenGruppeData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return edcNeueMaschinenGruppeData.PRO_i64GruppenId;
		}

		public Task<EDC_MaschinenGruppeData> FUN_fdcHoleMaschinenGruppeDataAsync(long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_MaschinenGruppeData i_edcSelectObjekt = new EDC_MaschinenGruppeData(EDC_MaschinenGruppeData.FUN_strGruppenIdWhereStatementErstellen(i_i64GruppenId));
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(i_edcSelectObjekt, null, i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleMaschinenGruppeInDataTableAsync(long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppeData.FUN_strGruppenIdWhereStatementErstellen(i_i64GruppenId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_MaschinenGruppeData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_enuHoleMaschinenGruppeDataAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppeData.FUN_strMaschinenTypeWhereStatmentErstellen(i_strMaschinenTyp);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenGruppeData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_fdcHoleMaschinenGruppeZugehoerigkeitenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppeData.FUN_strMaschinenIdWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenGruppeData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcUpdateMaschinenGruppeAsync(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcMaschinenGruppeData.PRO_strWhereStatement = EDC_MaschinenGruppeData.FUN_strGruppenIdWhereStatementErstellen(i_edcMaschinenGruppeData.PRO_i64GruppenId);
			return m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcMaschinenGruppeData, i_fdcTransaktion);
		}

		public Task FUN_fdcFuegeMaschineZuGruppeHinzuAsync(long i_i64MaschinenId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_MaschinenGruppenMitgliedData i_edcObjekt = new EDC_MaschinenGruppenMitgliedData
			{
				PRO_i64GruppenId = i_i64GruppenId,
				PRO_i64MaschinenId = i_i64MaschinenId
			};
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcObjekt, i_fdcTransaktion);
		}

		public Task FUN_fdcEntferneMaschineAusGruppeAsync(long i_i64MaschinenId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppenMitgliedData.FUN_strBestimmtesMitgliedWhereStatementErstellen(i_i64GruppenId, i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(new EDC_MaschinenGruppenMitgliedData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<IEnumerable<EDC_MaschinenBetriebsDatenWerteData>> FUN_fdcHoleAlleLetztenBetriebswerteAsync(long i_i64MaschinenId, DateTime i_fdcBisDatum, IDbTransaction i_fdcTransaktion = null)
		{
			List<EDC_MaschinenBetriebsDatenWerteData> lstBetriebsdaten = new List<EDC_MaschinenBetriebsDatenWerteData>();
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				foreach (ENUM_BetriebsdatenTyp value in Enum.GetValues(typeof(ENUM_BetriebsdatenTyp)))
				{
					IEnumerable<EDC_MaschinenBetriebsDatenWerteData> enumerable = await FUN_fdcHoleDieLetztenBetriebsDatenAsync(i_i64MaschinenId, i_fdcBisDatum, value, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					if (enumerable != null)
					{
						lstBetriebsdaten.AddRange(enumerable);
					}
				}
				if (i_fdcTransaktion != null)
				{
					return lstBetriebsdaten;
				}
				SUB_CommitTransaktion(fdcTransaktion);
				return lstBetriebsdaten;
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task FUN_fdcStelleBetriebsdatenAufAuslieferungszustandAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			IDbTransaction dbTransaction = i_fdcTransaktion;
			if (dbTransaction == null)
			{
				dbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			IDbTransaction fdcTransaktion = dbTransaction;
			try
			{
				List<long> lstMaxBetriebsdatenIds = new List<long>();
				foreach (ENUM_BetriebsdatenTyp value in Enum.GetValues(typeof(ENUM_BetriebsdatenTyp)))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					string i_strSql = EDC_MaschinenBetriebsDatenKopfData.FUN_strSelectMaxBetriebsdatenId(i_i64MaschinenId, DateTime.Now, value, dictionary);
					object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
					if (obj != null && obj != DBNull.Value)
					{
						lstMaxBetriebsdatenIds.Add(Convert.ToInt64(obj));
					}
				}
				if (lstMaxBetriebsdatenIds.Count > 0)
				{
					await FUN_fdcFuehreStatementListeAusAsync(new List<string>
					{
						EDC_MaschinenBetriebsDatenKopfData.FUN_strLoescheNichtEnthalteneIdsStatementErstellen(i_i64MaschinenId, lstMaxBetriebsdatenIds),
						EDC_MaschinenBetriebsDatenWerteData.FUN_strLoescheUngueltigeBetriebsdatenStatementErstellen(),
						EDC_MaschinenBetriebsDatenWerteData.FUN_strBetriebsdatenNullenStatementErstellen(lstMaxBetriebsdatenIds)
					}, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (i_fdcTransaktion == null)
				{
					SUB_CommitTransaktion(fdcTransaktion);
				}
			}
			catch (Exception)
			{
				if (i_fdcTransaktion == null)
				{
					SUB_RollbackTransaktion(fdcTransaktion);
				}
				throw;
			}
		}

		public async Task<EDC_MaschinenBetriebsdatenAbfrageData> FUN_fdcHoleHoechstenBetriebszeitDatenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion)
		{
			if (i_i64MaschinenId == 0L)
			{
				return null;
			}
			EDC_MaschinenBetriebsdatenAbfrageData eDC_MaschinenBetriebsdatenAbfrageData = new EDC_MaschinenBetriebsdatenAbfrageData();
			eDC_MaschinenBetriebsdatenAbfrageData.SUB_ErstelleMaxBetriebszeitAbfrageFuerMaschine(i_i64MaschinenId, ENUM_BetriebsdatenTyp.Betriebszeit);
			return await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(eDC_MaschinenBetriebsdatenAbfrageData, null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<bool> FUN_fdcBetriebsdatenSpeichernAsync(long i_i64MaschinenId, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp, IEnumerable<EDC_MaschinenBetriebsDatenWerteData> i_enuBetriebsdaten, IDbTransaction i_fdcTransaktion)
		{
			if (i_i64MaschinenId == 0L)
			{
				return false;
			}
			EDC_MaschinenBetriebsDatenKopfData eDC_MaschinenBetriebsDatenKopfData = new EDC_MaschinenBetriebsDatenKopfData();
			EDC_MaschinenBetriebsDatenKopfData eDC_MaschinenBetriebsDatenKopfData2 = eDC_MaschinenBetriebsDatenKopfData;
			long num2 = eDC_MaschinenBetriebsDatenKopfData2.PRO_i64BetriebsDatenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			eDC_MaschinenBetriebsDatenKopfData.PRO_i64MaschinenId = i_i64MaschinenId;
			eDC_MaschinenBetriebsDatenKopfData.PRO_dtmErstellt = DateTime.Now;
			eDC_MaschinenBetriebsDatenKopfData.PRO_enmBetriebsdatenTyp = i_enmBetriebsdatenTyp;
			EDC_MaschinenBetriebsDatenKopfData edcBetriebsdatenKopf = eDC_MaschinenBetriebsDatenKopfData;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcBetriebsdatenKopf, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			foreach (EDC_MaschinenBetriebsDatenWerteData item in i_enuBetriebsdaten)
			{
				item.PRO_i64BetriebsDatenId = edcBetriebsdatenKopf.PRO_i64BetriebsDatenId;
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(item, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			return true;
		}

		public async Task<IEnumerable<EDC_MaschinenBetriebsDatenWerteData>> FUN_fdcHoleDieLetztenBetriebsDatenAsync(long i_i64MaschinenId, DateTime i_fdcBisDatum, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp, IDbTransaction i_fdcTransaktion)
		{
			if (i_i64MaschinenId == 0L)
			{
				return null;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string i_strSql = EDC_MaschinenBetriebsDatenKopfData.FUN_strSelectMaxBetriebsdatenId(i_i64MaschinenId, i_fdcBisDatum, i_enmBetriebsdatenTyp, dictionary);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion, dictionary).ConfigureAwait(continueOnCapturedContext: false);
			if (obj == null || obj == DBNull.Value)
			{
				return null;
			}
			long i_i64BetriebsDatenId = Convert.ToInt64(obj);
			return await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenBetriebsDatenWerteData(EDC_MaschinenBetriebsDatenWerteData.FUN_strBetriebsdatenIdWhereStatementErstellen(i_i64BetriebsDatenId)), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		private bool FUN_blnPruefeObMaschineInGruppeIst(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, EDC_MaschineData i_edcMaschineData)
		{
			EDC_MaschinenGruppenMitgliedData i_edcSelectObjekt = new EDC_MaschinenGruppenMitgliedData(EDC_MaschinenGruppenMitgliedData.FUN_strBestimmtesMitgliedWhereStatementErstellen(i_edcMaschinenGruppeData.PRO_i64GruppenId, i_edcMaschineData.PRO_i64MaschinenId));
			return m_edcDatenbankAdapter.FUN_edcLeseObjekt(i_edcSelectObjekt) != null;
		}

		private async Task<bool> FUN_fdcPruefeObMaschineInGruppeIstAsync(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, EDC_MaschineData i_edcMaschineData, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppenMitgliedData.FUN_strBestimmtesMitgliedWhereStatementErstellen(i_edcMaschinenGruppeData.PRO_i64GruppenId, i_edcMaschineData.PRO_i64MaschinenId);
			return await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenGruppenMitgliedData(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false) != null;
		}

		private void SUB_FuegeMaschineZuGruppeHinzu(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, EDC_MaschineData i_edcMaschineData)
		{
			EDC_MaschinenGruppenMitgliedData i_edcObjekt = new EDC_MaschinenGruppenMitgliedData
			{
				PRO_i64GruppenId = i_edcMaschinenGruppeData.PRO_i64GruppenId,
				PRO_i64MaschinenId = i_edcMaschineData.PRO_i64MaschinenId
			};
			m_edcDatenbankAdapter.SUB_SpeichereObjekt(i_edcObjekt);
		}

		private Task FUN_fdcFuegeMaschineZuGruppeHinzuAsync(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, EDC_MaschineData i_edcMaschineData, IDbTransaction i_fdcTransaktion = null)
		{
			return FUN_fdcFuegeMaschineZuGruppeHinzuAsync(i_edcMaschineData.PRO_i64MaschinenId, i_edcMaschinenGruppeData.PRO_i64GruppenId, i_fdcTransaktion);
		}

		private EDC_MaschinenGruppeData FUN_edcHoleDefaultMaschinenGruppe(string i_strMaschinenTyp)
		{
			EDC_MaschinenGruppeData i_edcSelectObjekt = new EDC_MaschinenGruppeData(EDC_MaschinenGruppeData.FUN_strDefaultGruppenWhereStatementErstellen(i_strMaschinenTyp));
			return m_edcDatenbankAdapter.FUN_edcLeseObjekt(i_edcSelectObjekt);
		}

		private Task<EDC_MaschinenGruppeData> FUN_edcHoleDefaultMaschinenGruppeAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_MaschinenGruppeData.FUN_strDefaultGruppenWhereStatementErstellen(i_strMaschinenTyp);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenGruppeData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		private EDC_MaschinenGruppeData FUN_edcLegeDefaultMaschinenGruppeAn(string i_strMaschinenTyp)
		{
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData = new EDC_MaschinenGruppeData
			{
				PRO_i64GruppenId = m_edcDatenbankAdapter.FUN_u32HoleNaechstenSequenceWert("ess5_sequnique"),
				PRO_strGruppenName = EDC_MaschinenGruppeData.FUN_strHoleDefaultGruppenName(i_strMaschinenTyp),
				PRO_strMaschinenTyp = i_strMaschinenTyp
			};
			m_edcDatenbankAdapter.SUB_SpeichereObjekt(eDC_MaschinenGruppeData);
			return eDC_MaschinenGruppeData;
		}

		private async Task<EDC_MaschinenGruppeData> FUN_fdcLegeDefaultMaschinenGruppeAnAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData = new EDC_MaschinenGruppeData();
			EDC_MaschinenGruppeData eDC_MaschinenGruppeData2 = eDC_MaschinenGruppeData;
			long num2 = eDC_MaschinenGruppeData2.PRO_i64GruppenId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			eDC_MaschinenGruppeData.PRO_strGruppenName = EDC_MaschinenGruppeData.FUN_strHoleDefaultGruppenName(i_strMaschinenTyp);
			eDC_MaschinenGruppeData.PRO_strMaschinenTyp = i_strMaschinenTyp;
			EDC_MaschinenGruppeData edcNeueDefaultMaschinenGruppeData = eDC_MaschinenGruppeData;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcNeueDefaultMaschinenGruppeData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return edcNeueDefaultMaschinenGruppeData;
		}
	}
}
