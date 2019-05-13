using Ersa.Platform.Common.Bildverarbeitung;
using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.Common.Selektiv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces
{
	public interface INF_MaschinenEinstellungenDienst
	{
		Task<Dictionary<ENUM_SelektivTiegel, byte[]>> FUN_fdcHoleAlleKamerakalibrierwerteFuerMaschineAsync();

		Task FUN_fdcSpeichereKameraKalibrierwerteAsync(ENUM_SelektivTiegel i_enmSelektivTiegel, byte[] ia_bytKalibrierwerte);

		Task FUN_fdcSpeichereKameraKalibrierwerteAsync(Dictionary<ENUM_SelektivTiegel, byte[]> i_dicKalibrierwerte);

		Task FUN_fdcSpeichereBenutzerverwaltungEinstellungenAsync(string i_strProvider, long i_i64AuslogDauer);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleBenutzerverwaltungEinstellungenAsync();

		Task<string> FUN_fdcHoleGlobaleCadEinstellungenAsync();

		Task FUN_fdcSpeichereGlobaleCadEinstellungenAsync(string i_strEinstellungen);

		Task<string> FUN_fdcHoleMesEinstellungenAsync(ENUM_MesTyp i_enmMesTyp);

		Task FUN_fdcSpeichereMesEinstellungenAsync(string i_strEinstellungen, ENUM_MesTyp i_enmMesTyp);

		Task<string> FUN_fdcHoleMesKonfigurationAsync();

		Task FUN_fdcSpeichereMesKonfigurationAsync(string i_strKonfiguration);

		Task<IDictionary<ENUM_SelektivTiegel, ENUM_VideoAufzeichnungEinstellung>> FUN_fdcHoleAlleVideoAufzeichnungEinstellungenFuerMaschineAsync();

		Task FUN_fdcSpeichereVideoAufzeichnungEinstellungAsync(ENUM_SelektivTiegel i_enmSelektivTiegel, ENUM_VideoAufzeichnungEinstellung i_enmEinstellung);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKalibrationPosZ(ENUM_FiducialOrt i_enmFiducialOrt);

		Task FUN_fdcSpeichereFiducialKalibrationPosZ(ENUM_FiducialOrt i_enmFiducialOrt, float i_sngPosZ);

		Task<EDC_MaschinenEinstellungenData> FUN_fdcHoleFiducialKorrekturwert(ENUM_FiducialOrt i_enmFiducialOrt);

		Task FUN_fdcSpeichereFiducialKorrekturwert(ENUM_FiducialOrt i_enmFiducialOrt, float i_sngKorrekturwert);

		Task<Dictionary<ENUM_SelektivTiegel, Dictionary<int, byte[]>>> FUN_fdcHoleAlleKameraReferenzdatenFuerMaschineAsync();

		Task FUN_fdcSpeichereKameraReferenzDatenAsync(Dictionary<ENUM_SelektivTiegel, Dictionary<int, byte[]>> i_dicReferenzDaten);
	}
}
