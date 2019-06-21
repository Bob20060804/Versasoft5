using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.Helper;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Loetprogrammverwaltung
{
	public class EDC_LoetprogrammProgrammDataAccess : EDC_DataAccess, INF_LoetprogrammProgrammDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammProgrammDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<long> FUN_fdcErmittleProgrammIdAusNamenAsync(string i_strBibliotheksName, string i_strProgrammName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strErstelleProgrammIdAbfrageVonBibliotheksNameundProgrammNamen(i_strBibliotheksName, i_strProgrammName);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return (obj == null || obj == DBNull.Value) ? 0 : Convert.ToInt64(obj);
		}

		public Task<EDC_LoetprogrammData> FUN_fdcHoleLoetprogrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<EDC_LoetprogrammData> FUN_fdcHoleLoetprogrammAsync(string i_strProgrammName, long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strAbfrageProgrammVonNameUndBibliotheksIdWhereStatementErstellen(i_strProgrammName, i_i64BibliotheksId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<EDC_LoetprogrammData> FUN_fdcHoleDefaultLoetprogrammDataFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strDefaultProgrammeWhereStatementErstellen(i_i64MaschinenId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleAlleProgrammeMitBibliothekIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strBibliotheksIdNichtGeloeschtWhereStatementErstellen(i_i64BibliotheksId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleAlleProgrammeMitBibliothekNamenAsync(string i_strBibliotheksName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strBibliotheksNameNichtGeloeschtWhereStatementErstellen(i_strBibliotheksName);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleProgrammInDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task FUN_fdcLoetprogrammNeuAnlegenAsync(EDC_LoetprogrammData i_edcLoetprogramm, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			if (i_edcLoetprogramm.PRO_i64ProgrammId == 0L)
			{
				long num2 = i_edcLoetprogramm.PRO_i64ProgrammId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			}
			i_edcLoetprogramm.PRO_dtmAngelegtAm = DateTime.Now;
			i_edcLoetprogramm.PRO_dtmBearbeitetAm = DateTime.Now;
			i_edcLoetprogramm.PRO_i64AngelegtVon = i_i64BenutzerId;
			i_edcLoetprogramm.PRO_i64BearbeitetVon = i_i64BenutzerId;
			await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcLoetprogramm, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcLoetprogrammImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, long i_i64BibliotheksId, long i_i64BenutzerId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["SolderingPrograms"];
			if (dataTable == null)
			{
				throw new InvalidDataException("no program data defined");
			}
			List<EDC_LoetprogrammData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammData>().ToList();
			if (list.Count != 1)
			{
				throw new InvalidDataException("the target program is not defined unambiguously");
			}
			EDC_LoetprogrammData eDC_LoetprogrammData = list[0];
			eDC_LoetprogrammData.PRO_i64ProgrammId = i_i64ProgrammId;
			eDC_LoetprogrammData.PRO_i64BibliotheksId = i_i64BibliotheksId;
			if (!string.IsNullOrEmpty(i_strNeuerName))
			{
				eDC_LoetprogrammData.PRO_strName = i_strNeuerName;
			}
			eDC_LoetprogrammData.PRO_dtmAngelegtAm = DateTime.Now;
			eDC_LoetprogrammData.PRO_dtmBearbeitetAm = DateTime.Now;
			eDC_LoetprogrammData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_LoetprogrammData.PRO_i64BearbeitetVon = i_i64BenutzerId;
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_LoetprogrammData, i_fdcTransaktion);
		}

		public async Task FUN_fdcLoetprogrammDataAktualisierenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strNotiz, string i_strEingangskontrolle, string i_strAusgangskontrolle, long i_i64Version, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammData eDC_LoetprogrammData = await FUN_fdcHoleLoetprogrammAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			eDC_LoetprogrammData.PRO_strNotizen = i_strNotiz;
			eDC_LoetprogrammData.PRO_dtmBearbeitetAm = DateTime.Now;
			eDC_LoetprogrammData.PRO_i64BearbeitetVon = i_i64BenutzerId;
			eDC_LoetprogrammData.PRO_strEingangskontrolle = i_strEingangskontrolle;
			eDC_LoetprogrammData.PRO_strAusgangskontrolle = i_strAusgangskontrolle;
			eDC_LoetprogrammData.PRO_i32Version = (int)i_i64Version;
			eDC_LoetprogrammData.PRO_strWhereStatement = EDC_LoetprogrammData.FUN_strProgrammIdBibliotheksIdWhereStatementErstellen(i_i64ProgrammId, eDC_LoetprogrammData.PRO_i64BibliotheksId);
			await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(eDC_LoetprogrammData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_fdcHoleProgrammAnzahlMitBibliotheksIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strNichtGeloeschteProgrammeAnzahlAbfrageStatementErstellen(i_i64BibliotheksId);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return (obj == null || obj == DBNull.Value) ? 0 : Convert.ToInt64(obj);
		}

		public Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleProgrammeZuMaschinenGruppenAsync(IEnumerable<long> i_enuMaschinenGruppenIds, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammData.FUN_strMaschinenGruppenWhereStatementErstellen(i_enuMaschinenGruppenIds);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task FUN_fdcLoetprogrammVerschiebenAsync(long i_i64ProgramId, long i_i64NeueBibiliotheksId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strProgrammeZuNeuerBibliothekZuordnenUpdateStatementErstellen(i_i64ProgramId, i_i64NeueBibiliotheksId);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public Task FUN_fdcLoetprogrammGeloeschtSetzenAsync(long i_i64ProgrammId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strLoeschenUpdateStatementErstellen(i_i64ProgrammId, i_i64BenutzerId);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public Task FUN_fdcAlleLoetprogrammEinerBibliothekGeloeschtSetzenAsync(long i_i64BibliothekId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strProgrammeZuBibliothekLoeschenUpdateStatementErstellen(i_i64BibliothekId, i_i64BenutzerId);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public Task FUN_fdcLoetprogrammUmbenennenAsync(long i_i64ProgrammId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strProgrammeNeuerNameZuordnenUpdateStatementErstellen(i_i64ProgrammId, i_strNeuerName);
			Dictionary<string, object> i_dicParameter = new Dictionary<string, object>
			{
				{
					"pCreationDate",
					DateTime.Now
				}
			};
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion, i_dicParameter);
		}

		public Task FUN_fdcSetzeNeueProgrammVersion(IEnumerable<long> i_enuProgrammId, long i_i64NeueVersion, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strProgrammNeueVersionZuordnenUpdateStatementErstellen(i_enuProgrammId, i_i64NeueVersion);
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public Task FUN_fdcSetzeDefaultProgrammVersionsNummerAsync(IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammData.FUN_strDefaultProgrammVersionNummerStatementErstellen();
			return m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, i_fdcTransaktion);
		}

		public async Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleLoetprgInfoAbfrageVonProgrammIdAsync(long i_i64ProgrammId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammInfoDataAbfrage.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return FUN_enuFokusiereDatenAufMaschinenId(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammInfoDataAbfrage(i_strWhereStatement), i_fdcTransaktion, null, 3).ConfigureAwait(continueOnCapturedContext: false), i_i64MaschineId);
		}

		public async Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleNichtGeloeschteLoetprgInfoListeVonBibliotheksIdAsync(long i_i64BibliothekdId, long i_i64MaschineId, string i_strSuchbegriff = null, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = string.IsNullOrEmpty(i_strSuchbegriff) ? EDC_LoetprogrammInfoDataAbfrage.FUN_strBibliotheksIdUndNichtGeloeschtWhereStatementErstellen(i_i64BibliothekdId) : EDC_LoetprogrammInfoDataAbfrage.FUN_strBibliotheksIdUndNichtGeloeschtMitSuchbegriffWhereStatementErstellen(i_i64BibliothekdId, i_strSuchbegriff);
			return FUN_enuFokusiereDatenAufMaschinenId(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammInfoDataAbfrage(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false), i_i64MaschineId);
		}

		public async Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleLoetprgInfoAbfrageVonVersionsIdAsync(long i_i64VersionsId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammInfoDataAbfrage.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return FUN_enuFokusiereDatenAufMaschinenId(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammInfoDataAbfrage(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false), i_i64MaschineId);
		}

		public async Task<EDC_LoetprogrammInfoDataAbfrage> FUN_fdcHoleAktuelleFreigegebeneLoetprgInfoAbfrageVonVersionsIdAsync(long i_i64VersionsId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammInfoDataAbfrage.FUN_strAktuelleFreigegebeneVersionWhereStatementErstellen(i_i64VersionsId);
			EDC_LoetprogrammInfoDataAbfrage eDC_LoetprogrammInfoDataAbfrage = await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammInfoDataAbfrage(i_strWhereStatement), null, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammInfoDataAbfrage == null)
			{
				return null;
			}
			return FUN_enuFokusiereDatenAufMaschinenId(new List<EDC_LoetprogrammInfoDataAbfrage>
			{
				eDC_LoetprogrammInfoDataAbfrage
			}, i_i64MaschineId).FirstOrDefault();
		}

		private IEnumerable<EDC_LoetprogrammInfoDataAbfrage> FUN_enuFokusiereDatenAufMaschinenId(IEnumerable<EDC_LoetprogrammInfoDataAbfrage> i_enuAbfrageErgebnis, long i_i64MaschineId)
		{
			List<EDC_LoetprogrammInfoDataAbfrage> source = i_enuAbfrageErgebnis.ToList();
			List<EDC_LoetprogrammInfoDataAbfrage> list = new List<EDC_LoetprogrammInfoDataAbfrage>();
			foreach (long i64VersionsId in (from i_edcItem in source
			select i_edcItem.PRO_i64VersionsId).Distinct())
			{
				EDC_LoetprogrammInfoDataAbfrage eDC_LoetprogrammInfoDataAbfrage = source.FirstOrDefault(delegate(EDC_LoetprogrammInfoDataAbfrage i_edcItem)
				{
					if (i_edcItem.PRO_i64VersionsId == i64VersionsId)
					{
						return i_edcItem.PRO_i64MaschinenId == i_i64MaschineId;
					}
					return false;
				});
				if (eDC_LoetprogrammInfoDataAbfrage == null)
				{
					eDC_LoetprogrammInfoDataAbfrage = source.First((EDC_LoetprogrammInfoDataAbfrage i_edcItem) => i_edcItem.PRO_i64VersionsId == i64VersionsId);
					eDC_LoetprogrammInfoDataAbfrage.PRO_blnValide = true;
					eDC_LoetprogrammInfoDataAbfrage.PRO_i64MaschinenId = i_i64MaschineId;
				}
				list.Add(eDC_LoetprogrammInfoDataAbfrage);
			}
			return from i_edcItem in list
			orderby i_edcItem.PRO_i32ProgrammStatus descending, i_edcItem.PRO_i32SetNummer descending
			select i_edcItem;
		}
	}
}
