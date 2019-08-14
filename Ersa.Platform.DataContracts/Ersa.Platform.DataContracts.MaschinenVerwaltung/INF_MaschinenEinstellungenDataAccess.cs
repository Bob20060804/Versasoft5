using Ersa.Platform.Common.Bildverarbeitung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Selektiv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.MaschinenVerwaltung
{
	public interface INF_MaschinenEinstellungenDataAccess : INF_DataAccess
	{
		Task<Dictionary<ENUM_SelektivTiegel, byte[]>> FUN_fdcHoleAlleKamerakalibrierwerteFuerMaschineAsync(long i_i64MaschinenId);

		Task FUN_fdcSpeichereKameraKalibrierwerteAsync(long i_i64MaschinenId, ENUM_SelektivTiegel i_enmSelektivTiegel, byte[] ia_bytKalibrierwerte);

		Task FUN_fdcSpeichereKameraKalibrierwerteAsync(long i_i64MaschinenId, Dictionary<ENUM_SelektivTiegel, byte[]> i_dicKalibrierwerte);

		Task<IDictionary<ENUM_SelektivTiegel, ENUM_VideoAufzeichnungEinstellung>> FUN_fdcHoleAlleVideoAufzeichnungEinstellungenFuerMaschineAsync(long i_i64MaschinenId);

		Task FUN_fdcSpeichereVideoAufzeichnungEinstellungAsync(long i_i64MaschinenId, ENUM_SelektivTiegel i_enmSelektivTiegel, ENUM_VideoAufzeichnungEinstellung i_enmEinstellung);

		Task FUN_fdcSpeichereEinstellungenDataAsync(EDC_MaschinenEinstellungenData i_edcDaten);

		Task<IEnumerable<EDC_MaschinenEinstellungenData>> FUN_fdcHoleEinstellungenDataAsync(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungTyp);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleEinstellungDataAsync(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungTyp, int i_i32EinstellungsIndex = 0);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKalibrationPosZ(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt);

		Task FUN_fdcSpeichereFiducialKalibrationPosZ(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt, float i_sngPosZ);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKorrekturwert(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt);

		Task FUN_fdcSpeichereFiducialKorrekturwert(long i_i64MaschinenId, ENUM_FiducialOrt i_enmFiducialOrt, float i_sngKorrekturwert);

		Task<T> FUN_fdcHoleEinstellungsObjektAsync<T>(long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungsTyp, int i_i32EinstellungsIndex = 0);

		Task FUN_fdcSpeichereEinstellungsObjektAsync<T>(T i_objEinstellungen, long i_i64MaschinenId, ENUM_EinstellungsTyp i_enmEinstellungsTyp, int i_i32EinstellungsIndex = 0);

		Task<Dictionary<ENUM_SelektivTiegel, string>> FUN_fdcHoleAlleKameraReferenzdatenFuerMaschineAsync(long i_i64MaschinenId);

		Task FUN_fdcSpeichereKameraReferenzDatenAsync(long i_i64MaschinenId, IDictionary<ENUM_SelektivTiegel, string> i_dicReferenzDaten);
	}
}
