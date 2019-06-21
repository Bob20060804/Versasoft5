using Ersa.Global.Common.Helper;
using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Platform.Common.Bildverarbeitung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Selektiv;
using Ersa.Platform.DataContracts;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Data.DataAccess.Maschinenverwaltung
{
	public class EDC_MaschinenEinstellungenDataAccess : EDC_DataAccess, INF_MaschinenEinstellungenDataAccess, INF_DataAccess
	{
		public EDC_MaschinenEinstellungenDataAccess(INF_DatenbankAdapter i_edcDatenbankAdapter)
			: base(i_edcDatenbankAdapter)
		{
		}

		public async Task<Dictionary<ENUM_SelektivTiegel, byte[]>> FUN_fdcHoleAlleKamerakalibrierwerteFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypWhereStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VersacamKalibrationsDaten);
			return (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList().ToDictionary((EDC_MaschinenEinstellungenData i_edcEinstellung) => (ENUM_SelektivTiegel)i_edcEinstellung.PRO_i32SettingIndex, (EDC_MaschinenEinstellungenData i_edcEinstellung) => i_edcEinstellung.PRO_bytArrayWert);
		}

		public async Task FUN_fdcSpeichereKameraKalibrierwerteAsync(long i_i64MaschinenId, ENUM_SelektivTiegel i_enmSelektivTiegel, byte[] ia_bytKalibrierwerte)
		{
			EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.VersacamKalibrationsDaten,
				PRO_i32SettingIndex = (int)i_enmSelektivTiegel,
				PRO_bytArrayWert = ia_bytKalibrierwerte
			};
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VersacamKalibrationsDaten, (int)i_enmSelektivTiegel);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task FUN_fdcSpeichereKameraKalibrierwerteAsync(long i_i64MaschinenId, Dictionary<ENUM_SelektivTiegel, byte[]> i_dicKalibrierwerte)
		{
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (KeyValuePair<ENUM_SelektivTiegel, byte[]> item in i_dicKalibrierwerte)
				{
					EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
					{
						PRO_i64MaschinenId = i_i64MaschinenId,
						PRO_enmSettingsTyp = ENUM_EinstellungsTyp.VersacamKalibrationsDaten,
						PRO_i32SettingIndex = (int)item.Key,
						PRO_bytArrayWert = item.Value
					};
					string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VersacamKalibrationsDaten, (int)item.Key);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				}
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task<IDictionary<ENUM_SelektivTiegel, ENUM_VideoAufzeichnungEinstellung>> FUN_fdcHoleAlleVideoAufzeichnungEinstellungenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypWhereStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VideoAufzeichnungEinstellung);
			return (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList().ToDictionary((EDC_MaschinenEinstellungenData i_edcEinstellung) => (ENUM_SelektivTiegel)i_edcEinstellung.PRO_i32SettingIndex, (EDC_MaschinenEinstellungenData i_edcEinstellung) => (ENUM_VideoAufzeichnungEinstellung)i_edcEinstellung.PRO_i64LongWert);
		}

		public async Task FUN_fdcSpeichereVideoAufzeichnungEinstellungAsync(long i_i64MaschinenId, ENUM_SelektivTiegel i_enmSelektivTiegel, ENUM_VideoAufzeichnungEinstellung i_enmEinstellung)
		{
			EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.VideoAufzeichnungEinstellung,
				PRO_i32SettingIndex = (int)i_enmSelektivTiegel,
				PRO_i64LongWert = (long)i_enmEinstellung
			};
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VideoAufzeichnungEinstellung, (int)i_enmSelektivTiegel);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task FUN_fdcSpeichereEinstellungenDataAsync(EDC_MaschinenEinstellungenData i_edcDaten)
		{
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_edcDaten.PRO_i64MaschinenId, i_edcDaten.PRO_enmSettingsTyp);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(i_edcDaten, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public Task<IEnumerable<EDC_MaschinenEinstellungenData>> FUN_fdcHoleEinstellungenDataAsync(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungTyp)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypWhereStatementErstellen(i_i64MaschinenId, i_enmEinstellungTyp);
			return m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement));
		}

		public Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleEinstellungDataAsync(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungTyp, int i_i32EinstellungsIndex = 0)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(i_i64MaschinenId, i_enmEinstellungTyp, i_i32EinstellungsIndex);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement));
		}

		public Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKalibrationPosZ(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.FiducialKalibrationPosZ, (int)i_enmFiducialOrt);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement));
		}

		public async Task FUN_fdcSpeichereFiducialKalibrationPosZ(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt, float i_sngPosZ)
		{
			EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.FiducialKalibrationPosZ,
				PRO_i32SettingIndex = (int)i_enmFiducialOrt,
				PRO_sngRealWert = i_sngPosZ
			};
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.FiducialKalibrationPosZ, (int)i_enmFiducialOrt);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKorrekturwert(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.FiducialKorrekturwert, (int)i_enmFiducialOrt);
			return m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement));
		}

		public async Task FUN_fdcSpeichereFiducialKorrekturwert(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt, float i_sngKorrekturwert)
		{
			EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.FiducialKorrekturwert,
				PRO_i32SettingIndex = (int)i_enmFiducialOrt,
				PRO_sngRealWert = i_sngKorrekturwert
			};
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.FiducialKorrekturwert, (int)i_enmFiducialOrt);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task<T> FUN_fdcHoleEinstellungsObjektAsync<T>(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungsTyp, int i_i32EinstellungsIndex = 0)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypUndIndexWhereStatementErstellen(i_i64MaschinenId, i_enmEinstellungsTyp, i_i32EinstellungsIndex);
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = await m_edcDatenbankAdapter.FUN_edcLeseObjektAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_MaschinenEinstellungenData != null)
			{
				try
				{
					return eDC_MaschinenEinstellungenData.PRO_bytArrayWert.FUN_objDeserialize<T>();
				}
				catch (Exception)
				{
				}
			}
			return default(T);
		}

		public async Task FUN_fdcSpeichereEinstellungsObjektAsync<T>(T i_objEinstellungen, long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungsTyp, int i_i32EinstellungsIndex = 0)
		{
			EDC_MaschinenEinstellungenData edcEinstellungData = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = i_i64MaschinenId,
				PRO_enmSettingsTyp = i_enmEinstellungsTyp,
				PRO_i32SettingIndex = i_i32EinstellungsIndex,
				PRO_bytArrayWert = i_objEinstellungen.FUNa_bytSerialize()
			};
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, i_enmEinstellungsTyp, i_i32EinstellungsIndex);
				await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektAsync(edcEinstellungData, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}

		public async Task<Dictionary<ENUM_SelektivTiegel, string>> FUN_fdcHoleAlleKameraReferenzdatenFuerMaschineAsync(long i_i64MaschinenId)
		{
			string i_strWhereStatement = EDC_MaschinenEinstellungenData.FUN_strMaschinenIdMitTypWhereStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VersacamReferenzDaten);
			return (await m_edcDatenbankAdapter.FUN_fdcErstelleObjektlisteAsync(new EDC_MaschinenEinstellungenData(i_strWhereStatement)).ConfigureAwait(continueOnCapturedContext: false)).ToList().ToDictionary((EDC_MaschinenEinstellungenData i_fdcKvp) => (ENUM_SelektivTiegel)i_fdcKvp.PRO_i32SettingIndex, (EDC_MaschinenEinstellungenData i_fdcKvp) => i_fdcKvp.PRO_strMemoWert);
		}

		public async Task FUN_fdcSpeichereKameraReferenzDatenAsync(long i_i64MaschinenId, IDictionary<ENUM_SelektivTiegel, string> i_dicReferenzDaten)
		{
			IDbTransaction fdcDbTransaction = await FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<EDC_MaschinenEinstellungenData> lstEinstellungen = new List<EDC_MaschinenEinstellungenData>();
				foreach (KeyValuePair<ENUM_SelektivTiegel, string> fdcKvp in i_dicReferenzDaten)
				{
					string i_strSql = EDC_MaschinenEinstellungenData.FUN_strLoescheEinstellungStatementErstellen(i_i64MaschinenId, ENUM_EinstellungsTyp.VersacamReferenzDaten, (int)fdcKvp.Key);
					await m_edcDatenbankAdapter.FUN_fdcExecuteStatementAsync(i_strSql, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
					lstEinstellungen.Add(new EDC_MaschinenEinstellungenData
					{
						PRO_i64MaschinenId = i_i64MaschinenId,
						PRO_enmSettingsTyp = ENUM_EinstellungsTyp.VersacamReferenzDaten,
						PRO_i32SettingIndex = (int)fdcKvp.Key,
						PRO_strMemoWert = fdcKvp.Value
					});
				}
				await m_edcDatenbankAdapter.FUN_fdcSpeichereObjektListeAsync(lstEinstellungen, fdcDbTransaction).ConfigureAwait(continueOnCapturedContext: false);
				SUB_CommitTransaktion(fdcDbTransaction);
			}
			catch (Exception)
			{
				SUB_RollbackTransaktion(fdcDbTransaction);
				throw;
			}
		}
	}
}
