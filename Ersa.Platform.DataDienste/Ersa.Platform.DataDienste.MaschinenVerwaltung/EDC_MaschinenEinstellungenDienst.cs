using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Bildverarbeitung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.Common.Selektiv;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.MaschinenVerwaltung
{
	[Export(typeof(INF_MaschinenEinstellungenDienst))]
	public class EDC_MaschinenEinstellungenDienst : EDC_DataDienst, INF_MaschinenEinstellungenDienst
	{
		private const long mC_i64GlobaleMaschine = 0L;

		private readonly Lazy<INF_MaschinenEinstellungenDataAccess> m_edcMaschinenEinstellungenDataAccess;

		private readonly INF_JsonSerialisierungsDienst m_edcJsonSerialisierungsDienst;

		[ImportingConstructor]
		public EDC_MaschinenEinstellungenDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider, INF_JsonSerialisierungsDienst i_edcJsonSerialisierungsDienst)
			: base(i_edcCapabilityProvider)
		{
			m_edcMaschinenEinstellungenDataAccess = new Lazy<INF_MaschinenEinstellungenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenEinstellungenDataAccess>);
			m_edcJsonSerialisierungsDienst = i_edcJsonSerialisierungsDienst;
		}

		public async Task<Dictionary<ENUM_SelektivTiegel, byte[]>> FUN_fdcHoleAlleKamerakalibrierwerteFuerMaschineAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleAlleKamerakalibrierwerteFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereKameraKalibrierwerteAsync(ENUM_SelektivTiegel i_enmSelektivTiegel, byte[] ia_bytKalibrierwerte)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereKameraKalibrierwerteAsync(i_i64MaschinenId, i_enmSelektivTiegel, ia_bytKalibrierwerte).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereKameraKalibrierwerteAsync(Dictionary<ENUM_SelektivTiegel, byte[]> i_dicKalibrierwerte)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereKameraKalibrierwerteAsync(i_i64MaschinenId, i_dicKalibrierwerte).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereBenutzerverwaltungEinstellungenAsync(string i_strProvider, long i_i64AuslogDauer)
		{
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = new EDC_MaschinenEinstellungenData
			{
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.BenutzerVerwaltung
			};
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData2 = eDC_MaschinenEinstellungenData;
			long num2 = eDC_MaschinenEinstellungenData2.PRO_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			eDC_MaschinenEinstellungenData.PRO_i32SettingIndex = 0;
			eDC_MaschinenEinstellungenData.PRO_i64LongWert = i_i64AuslogDauer;
			eDC_MaschinenEinstellungenData.PRO_strTextWert = i_strProvider;
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereEinstellungenDataAsync(eDC_MaschinenEinstellungenData).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleBenutzerverwaltungEinstellungenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleEinstellungenDataAsync(i_i64MaschinenId, ENUM_EinstellungsTyp.BenutzerVerwaltung).ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
		}

		public async Task<string> FUN_fdcHoleGlobaleCadEinstellungenAsync()
		{
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleEinstellungDataAsync(0L, ENUM_EinstellungsTyp.GlobaleCadEinstellungen).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_MaschinenEinstellungenData != null)
			{
				return eDC_MaschinenEinstellungenData.PRO_strMemoWert;
			}
			return string.Empty;
		}

		public Task FUN_fdcSpeichereGlobaleCadEinstellungenAsync(string i_strEinstellungen)
		{
			EDC_MaschinenEinstellungenData i_edcDaten = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = 0L,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.GlobaleCadEinstellungen,
				PRO_i32SettingIndex = 0,
				PRO_strMemoWert = i_strEinstellungen
			};
			return m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereEinstellungenDataAsync(i_edcDaten);
		}

		public async Task<string> FUN_fdcHoleMesEinstellungenAsync(ENUM_MesTyp i_enmMesTyp)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleEinstellungDataAsync(i_i64MaschinenId, ENUM_EinstellungsTyp.MesEinstellungen, (int)i_enmMesTyp).ConfigureAwait(continueOnCapturedContext: false);
			return (eDC_MaschinenEinstellungenData != null) ? eDC_MaschinenEinstellungenData.PRO_strMemoWert : string.Empty;
		}

		public async Task FUN_fdcSpeichereMesEinstellungenAsync(string i_strEinstellungen, ENUM_MesTyp i_enmMesTyp)
		{
			long pRO_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_MaschinenEinstellungenData i_edcDaten = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = pRO_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.MesEinstellungen,
				PRO_i32SettingIndex = (int)i_enmMesTyp,
				PRO_strMemoWert = i_strEinstellungen
			};
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereEinstellungenDataAsync(i_edcDaten).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<string> FUN_fdcHoleMesKonfigurationAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_MaschinenEinstellungenData eDC_MaschinenEinstellungenData = await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleEinstellungDataAsync(i_i64MaschinenId, ENUM_EinstellungsTyp.MesKonfiguration).ConfigureAwait(continueOnCapturedContext: false);
			return (eDC_MaschinenEinstellungenData != null) ? eDC_MaschinenEinstellungenData.PRO_strMemoWert : string.Empty;
		}

		public async Task FUN_fdcSpeichereMesKonfigurationAsync(string i_strKonfiguration)
		{
			long pRO_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_MaschinenEinstellungenData i_edcDaten = new EDC_MaschinenEinstellungenData
			{
				PRO_i64MaschinenId = pRO_i64MaschinenId,
				PRO_enmSettingsTyp = ENUM_EinstellungsTyp.MesKonfiguration,
				PRO_i32SettingIndex = 0,
				PRO_strMemoWert = i_strKonfiguration
			};
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereEinstellungenDataAsync(i_edcDaten).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IDictionary<ENUM_SelektivTiegel, ENUM_VideoAufzeichnungEinstellung>> FUN_fdcHoleAlleVideoAufzeichnungEinstellungenFuerMaschineAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleAlleVideoAufzeichnungEinstellungenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereVideoAufzeichnungEinstellungAsync(ENUM_SelektivTiegel i_enmSelektivTiegel, ENUM_VideoAufzeichnungEinstellung i_enmEinstellung)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereVideoAufzeichnungEinstellungAsync(i_i64MaschinenId, i_enmSelektivTiegel, i_enmEinstellung).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKalibrationPosZ(ENUM_FiducialOrt i_enmFiducialOrt)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleFiducialKalibrationPosZ(i_i64MaschinenId, i_enmFiducialOrt).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereFiducialKalibrationPosZ(ENUM_FiducialOrt i_enmFiducialOrt, float i_sngPosZ)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereFiducialKalibrationPosZ(i_i64MaschinenId, i_enmFiducialOrt, i_sngPosZ).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKorrekturwert(ENUM_FiducialOrt i_enmFiducialOrt)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleFiducialKorrekturwert(i_i64MaschinenId, i_enmFiducialOrt).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcSpeichereFiducialKorrekturwert(ENUM_FiducialOrt i_enmFiducialOrt, float i_sngKorrekturwert)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereFiducialKorrekturwert(i_i64MaschinenId, i_enmFiducialOrt, i_sngKorrekturwert).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<Dictionary<ENUM_SelektivTiegel, Dictionary<int, byte[]>>> FUN_fdcHoleAlleKameraReferenzdatenFuerMaschineAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcHoleAlleKameraReferenzdatenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)).ToDictionary((KeyValuePair<ENUM_SelektivTiegel, string> i_fdcKvp) => i_fdcKvp.Key, (KeyValuePair<ENUM_SelektivTiegel, string> i_fdcKvp) => m_edcJsonSerialisierungsDienst.FUN_objDeserialisieren<Dictionary<int, byte[]>>(i_fdcKvp.Value));
		}

		public async Task FUN_fdcSpeichereKameraReferenzDatenAsync(Dictionary<ENUM_SelektivTiegel, Dictionary<int, byte[]>> i_dicReferenzDaten)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			Dictionary<ENUM_SelektivTiegel, string> i_dicReferenzDaten2 = i_dicReferenzDaten.ToDictionary((KeyValuePair<ENUM_SelektivTiegel, Dictionary<int, byte[]>> i_fdcKvp) => i_fdcKvp.Key, (KeyValuePair<ENUM_SelektivTiegel, Dictionary<int, byte[]>> i_fdcKvp) => m_edcJsonSerialisierungsDienst.FUN_strSerialisieren(i_fdcKvp.Value));
			await m_edcMaschinenEinstellungenDataAccess.Value.FUN_fdcSpeichereKameraReferenzDatenAsync(i_i64MaschinenId, i_dicReferenzDaten2).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
