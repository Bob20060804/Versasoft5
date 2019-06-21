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
	public class EDC_LoetprogrammVersionDataAccess : EDC_DataAccess, INF_LoetprogrammVersionDataAccess, INF_DataAccess
	{
		public EDC_LoetprogrammVersionDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public Task<EDC_LoetprogrammVersionData> FUN_fdcVersionLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammVersionData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task<EDC_LoetprogrammVersionAbfrageData> FUN_fdcVersionLadenAsync(long i_i64VersionsId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return FUN_enuFokusiereDatenAufMaschinenId(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammVersionAbfrageData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false), i_i64MaschinenId).FirstOrDefault();
		}

		public Task<IEnumerable<EDC_LoetprogrammVersionAbfrageData>> FUN_fdcHoleVersionenStapelAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strProgrammIdWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammVersionAbfrageData(i_strWhereStatement), i_fdcTransaktion);
		}

		public Task<DataTable> FUN_fdcHoleVersionInDataTableAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strVersionsIdWhereStatementErstellen(i_i64VersionsId);
			return m_edcDatenbankAdapter.FUN_fdcLeseInDataTableAsync(new EDC_LoetprogrammVersionData(i_strWhereStatement), i_fdcTransaktion);
		}

		public async Task<IEnumerable<EDC_LoetprogrammVersionAbfrageData>> FUN_fdcSichtbareVersionenLadenAsync(long i_i64ProgrammId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strSichtbareVersionenWhereStatementErstellen(i_i64ProgrammId);
			return FUN_enuFokusiereDatenAufMaschinenId(await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammVersionAbfrageData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false), i_i64MaschinenId);
		}

		public async Task<IEnumerable<ENUM_LoetprogrammStatus>> FUN_fdcErmittleSichtbareVersionenFuerProgrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammVersionData.FUN_strSelectDistinctStatusFuerProgrammIdStatement(i_i64ProgrammId);
			return (await m_edcDatenbankAdapter.FUN_fdcErstelleScalareObjektlisteAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList().Cast<ENUM_LoetprogrammStatus>().ToList();
		}

		public async Task<EDC_LoetprogrammVersionData> FUN_fdcArbeitsVersionLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strArbeitsversionWhereStatementErstellen(i_i64ProgrammId);
			return (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_LoetprogrammVersionData(i_strWhereStatement), i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
		}

		public Task<EDC_LoetprogrammVersionData> FUN_fdcFreigegebeneVersionLadenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strFreigegebeneVersionWhereStatementErstellen(i_i64ProgrammId);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_LoetprogrammVersionData(i_strWhereStatement), null, i_fdcTransaktion);
		}

		public async Task<EDC_LoetprogrammVersionData> FUN_fdcArbeitsversionErstellenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData edcArbeitsversion = await FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (edcArbeitsversion == null)
			{
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = new EDC_LoetprogrammVersionData
				{
					PRO_dtmAngelegtAm = DateTime.Now,
					PRO_i64AngelegtVon = i_i64BenutzerId,
					PRO_i64ProgrammId = i_i64ProgrammId,
					PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Arbeitsversion
				};
				EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData2 = eDC_LoetprogrammVersionData;
				eDC_LoetprogrammVersionData2.PRO_i32SetNummer = await FUN_fdcNeueSetNummerErmittelnAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				edcArbeitsversion = eDC_LoetprogrammVersionData;
			}
			edcArbeitsversion.PRO_strKommentar = i_strKommentar;
			await FUN_fdcVersionSpeichernAsync(edcArbeitsversion, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			return edcArbeitsversion;
		}

		public async Task FUN_fdcArbeitsversionLoeschenAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData != null)
			{
				eDC_LoetprogrammVersionData.PRO_strWhereStatement = EDC_LoetprogrammVersionData.FUN_strVersionsIdWhereStatementErstellen(eDC_LoetprogrammVersionData.PRO_i64VersionsId);
				await m_edcDatenbankAdapter.FUN_fdcLoescheObjektAsync(eDC_LoetprogrammVersionData, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcArbeitsversionAktualisierenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcArbeitsVersionLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData != null)
			{
				eDC_LoetprogrammVersionData.PRO_strKommentar = i_strKommentar;
				await FUN_fdcVersionSpeichernAsync(eDC_LoetprogrammVersionData, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcVersionFreigebenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, string i_strKommentar, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData edcVersion = await FUN_fdcVersionLadenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (edcVersion == null || edcVersion.PRO_i64ProgrammId != i_i64ProgrammId)
			{
				throw new InvalidDataException("no valid version data found");
			}
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcFreigegebeneVersionLadenAsync(edcVersion.PRO_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData != null)
			{
				eDC_LoetprogrammVersionData.PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Versioniert;
				await FUN_fdcVersionSpeichernAsync(eDC_LoetprogrammVersionData, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			edcVersion.PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Freigegeben;
			edcVersion.PRO_strKommentar = i_strKommentar;
			await FUN_fdcVersionSpeichernAsync(edcVersion, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcFreigeabeWegnehmenAsync(long i_i64ProgrammId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcFreigegebeneVersionLadenAsync(i_i64ProgrammId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData != null)
			{
				eDC_LoetprogrammVersionData.PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Versioniert;
				await FUN_fdcVersionSpeichernAsync(eDC_LoetprogrammVersionData, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcVersionFreigebeStatusUndNotizenSetzenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, ENUM_LoetprogrammFreigabeStatus i_enmFreigabeStatus, string i_strFreigabeNotiz, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcVersionLadenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData == null || eDC_LoetprogrammVersionData.PRO_i64ProgrammId != i_i64ProgrammId)
			{
				throw new InvalidDataException("no valid version data found");
			}
			eDC_LoetprogrammVersionData.PRO_enmFreigabeStatus = i_enmFreigabeStatus;
			eDC_LoetprogrammVersionData.PRO_strFreigabeNotizen = i_strFreigabeNotiz;
			await FUN_fdcVersionSpeichernAsync(eDC_LoetprogrammVersionData, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcVersionSichtbarkeitEntfernenAsync(long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = await FUN_fdcVersionLadenAsync(i_i64VersionsId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_LoetprogrammVersionData == null || eDC_LoetprogrammVersionData.PRO_i64ProgrammId != i_i64ProgrammId || !ENUM_LoetprogrammStatus.Versioniert.Equals(eDC_LoetprogrammVersionData.PRO_enmProgrammStatus))
			{
				throw new InvalidDataException("not possible to set version invisible");
			}
			eDC_LoetprogrammVersionData.PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Unsichtbar;
			await FUN_fdcVersionSpeichernAsync(eDC_LoetprogrammVersionData, i_i64BenutzerId, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcVersionImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, long i_i64VersionsId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			DataTable dataTable = i_fdcDataSet.Tables["ProgramHistory"];
			if (dataTable == null)
			{
				throw new InvalidDataException("no version data defined");
			}
			List<EDC_LoetprogrammVersionData> list = dataTable.FUN_enmErstelleObjektListeAusDataTable<EDC_LoetprogrammVersionData>().ToList();
			if (list.Count != 1)
			{
				throw new InvalidDataException("the target version data are not defined unambiguously");
			}
			EDC_LoetprogrammVersionData eDC_LoetprogrammVersionData = list[0];
			eDC_LoetprogrammVersionData.PRO_i64ProgrammId = i_i64ProgrammId;
			eDC_LoetprogrammVersionData.PRO_i64VersionsId = i_i64VersionsId;
			eDC_LoetprogrammVersionData.PRO_dtmAngelegtAm = DateTime.Now;
			eDC_LoetprogrammVersionData.PRO_dtmBearbeitetAm = DateTime.Now;
			eDC_LoetprogrammVersionData.PRO_i64AngelegtVon = i_i64BenutzerId;
			eDC_LoetprogrammVersionData.PRO_i64BearbeitetVon = i_i64BenutzerId;
			eDC_LoetprogrammVersionData.PRO_enmProgrammStatus = ENUM_LoetprogrammStatus.Arbeitsversion;
			return m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(eDC_LoetprogrammVersionData, i_fdcTransaktion);
		}

		public async Task FUN_fdcFuehreDatenMigrationDurchAsync()
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_LoetprogrammVersionData.FUN_strMigrationsEinstiegsUpdateStatementErstellen();
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				string i_strSql2 = EDC_LoetprogrammVersionData.FUN_strSelectDistinctProgrammIdOhneHistoryStatementErstellen();
				foreach (long item in (await m_edcDatenbankAdapter.FUN_fdcErstelleScalareObjektlisteAsync(i_strSql2, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList())
				{
					string i_strSql3 = EDC_LoetprogrammVersionData.FUN_strGroessteVersioneAlsFreigegebenUpdateStatementErstellen(item);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql3, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				string i_strSql4 = EDC_LoetprogrammVersionData.FUN_strSelectDistinctProgrammIdMitHistoryStatementErstellen();
				foreach (object i64ProgrammId in (await m_edcDatenbankAdapter.FUN_fdcErstelleScalareObjektlisteAsync(i_strSql4, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false)).ToList())
				{
					string i_strSql5 = EDC_LoetprogrammVersionData.FUN_strAlleVersionenAufVersioniertUpdateStatementErstellen((long)i64ProgrammId);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql5, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					string i_strSql6 = EDC_LoetprogrammVersionData.FUN_strGroessteVersioneAlsFreigegebenUndVersionBeibehaltenUpdateStatementErstellen((long)i64ProgrammId);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql6, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				string i_strSql7 = EDC_LoetprogrammVersionData.FUN_strVorlagenZuArbeitsversionenUpdateStatementErstellen();
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql7, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		public async Task FUN_fdcFuehreReleaseStateDatenMigrationDurchAsync()
		{
			IDbTransaction fdcTransaktion = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_LoetprogrammVersionData.FUN_strReleaseStateUpdateStatementErstellen();
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}

		private async Task FUN_fdcVersionSpeichernAsync(EDC_LoetprogrammVersionData i_edcVersion, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null)
		{
			i_edcVersion.PRO_dtmBearbeitetAm = DateTime.Now;
			i_edcVersion.PRO_i64BearbeitetVon = i_i64BenutzerId;
			bool blnInsert = false;
			if (i_edcVersion.PRO_i64VersionsId == 0L)
			{
				blnInsert = true;
				long num2 = i_edcVersion.PRO_i64VersionsId = await m_edcDatenbankAdapter.FUN_fdcHoleNaechstenSequenceWertAsync("ess5_sequnique").ConfigureAwait(continueOnCapturedContext: false);
			}
			if (blnInsert)
			{
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcVersion, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await m_edcDatenbankAdapter.FUN_fdcUpdateObjektAsync(i_edcVersion, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task<int> FUN_fdcNeueSetNummerErmittelnAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null)
		{
			string i_strSql = EDC_LoetprogrammVersionData.FUN_strSelectMaxSetNummerFuerProgrammIdStatement(i_i64ProgrammId);
			object obj = await m_edcDatenbankAdapter.FUN_fdcScalareAbfrageAsync(i_strSql, i_fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			if (obj != DBNull.Value && obj != null)
			{
				num = Convert.ToInt32(obj);
			}
			return num + 1;
		}

		private IEnumerable<EDC_LoetprogrammVersionAbfrageData> FUN_enuFokusiereDatenAufMaschinenId(IEnumerable<EDC_LoetprogrammVersionAbfrageData> i_enuAbfrageErgebnis, long i_i64MaschineId)
		{
			List<EDC_LoetprogrammVersionAbfrageData> source = i_enuAbfrageErgebnis.ToList();
			List<EDC_LoetprogrammVersionAbfrageData> list = new List<EDC_LoetprogrammVersionAbfrageData>();
			foreach (long i64VersionsId in (from i_edcItem in source
			select i_edcItem.PRO_i64VersionsId).Distinct())
			{
				EDC_LoetprogrammVersionAbfrageData eDC_LoetprogrammVersionAbfrageData = source.FirstOrDefault(delegate(EDC_LoetprogrammVersionAbfrageData i_edcItem)
				{
					if (i_edcItem.PRO_i64VersionsId == i64VersionsId)
					{
						return i_edcItem.PRO_i64MaschinenId == i_i64MaschineId;
					}
					return false;
				});
				if (eDC_LoetprogrammVersionAbfrageData == null)
				{
					eDC_LoetprogrammVersionAbfrageData = source.First((EDC_LoetprogrammVersionAbfrageData i_edcItem) => i_edcItem.PRO_i64VersionsId == i64VersionsId);
					eDC_LoetprogrammVersionAbfrageData.PRO_blnValide = true;
					eDC_LoetprogrammVersionAbfrageData.PRO_i64MaschinenId = i_i64MaschineId;
				}
				list.Add(eDC_LoetprogrammVersionAbfrageData);
			}
			return from i_edcItem in list
			orderby i_edcItem.PRO_i32ProgrammStatus descending, i_edcItem.PRO_i32SetNummer descending
			select i_edcItem;
		}
	}
}
