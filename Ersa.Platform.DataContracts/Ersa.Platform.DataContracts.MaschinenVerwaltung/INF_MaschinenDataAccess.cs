using Ersa.Platform.Common.Data.Maschinenverwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.MaschinenVerwaltung
{
	public interface INF_MaschinenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_MaschineData>> FUN_fdcListeAllerMaschinenLadenAsync(IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcRegistriereMaschineAsync(string i_strMaschinenTyp, string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MaschineData> FUN_fdcHoleMaschinenDataAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MaschineData> FUN_fdcHoleMaschinenDataAsync(string i_strMaschinenNummer, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleMaschinenDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcImportiereMaschinenDatenAsync(DataTable i_fdcDataTable, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcUpdateMaschineAsync(EDC_MaschineData i_edcMaschineData, IDbTransaction i_fdcTransaktion = null);

		Task<bool> FUN_fdcSindMehrereGleicheMaschinentypenRegistriertAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<long>> FUN_fdcHoleZugewieseneGruppenIdsAsync(long i_i64Maschinenid, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleGruppenMitgliedDataTableFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcHoleAktiveCodetabellenIdAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSetzeAktiveCodetabellenIdAsync(long i_i64MaschinenId, long i_i64CodetabellenId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcHoleDefaultLoetProgrammIdAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSetzeDefaultLoetProgrammIdAsync(long i_i64MaschinenId, long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcHoleDefaultMaschinenGruppenIdAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_i64LegeMaschinenGruppeAnAsync(string i_strGruppenName, string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MaschinenGruppeData> FUN_fdcHoleMaschinenGruppeDataAsync(long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleMaschinenGruppeInDataTableAsync(long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_enuHoleMaschinenGruppeDataAsync(string i_strMaschinenTyp, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_MaschinenGruppeData>> FUN_fdcHoleMaschinenGruppeZugehoerigkeitenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcUpdateMaschinenGruppeAsync(EDC_MaschinenGruppeData i_edcMaschinenGruppeData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcFuegeMaschineZuGruppeHinzuAsync(long i_i64MaschinenId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcEntferneMaschineAusGruppeAsync(long i_i64MaschinenId, long i_i64GruppenId, IDbTransaction i_fdcTransaktion = null);

		Task<bool> FUN_fdcBetriebsdatenSpeichernAsync(long i_i64MaschinenId, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp, IEnumerable<EDC_MaschinenBetriebsDatenWerteData> i_enuBetriebsdaten, IDbTransaction i_fdcTransaktion);

		Task<IEnumerable<EDC_MaschinenBetriebsDatenWerteData>> FUN_fdcHoleDieLetztenBetriebsDatenAsync(long i_i64MaschinenId, DateTime i_fdcBisDatum, ENUM_BetriebsdatenTyp i_enmBetriebsdatenTyp, IDbTransaction i_fdcTransaktion);

		Task<IEnumerable<EDC_MaschinenBetriebsDatenWerteData>> FUN_fdcHoleAlleLetztenBetriebswerteAsync(long i_i64MaschinenId, DateTime i_fdcBisDatum, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcStelleBetriebsdatenAufAuslieferungszustandAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MaschinenBetriebsdatenAbfrageData> FUN_fdcHoleHoechstenBetriebszeitDatenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion);
	}
}
