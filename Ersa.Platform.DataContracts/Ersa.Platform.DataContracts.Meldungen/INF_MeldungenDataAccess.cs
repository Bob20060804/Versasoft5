using Ersa.Platform.Common.Data.Meldungen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Meldungen
{
	public interface INF_MeldungenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcAlleMeldungenImIntervallAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcQuittierteMeldungenImIntervallAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_MeldungenAbfrageData>> FUN_fdcAlleNichtQuittiertenMeldungenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcMeldungenHinzufuegenAsync(IEnumerable<EDC_MeldungData> i_enuNeueMeldung, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcMeldungHinzufuegenAsync(EDC_MeldungData i_edcMeldung, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcMeldungenAktualisierenAsync(IEnumerable<EDC_MeldungData> i_enuNeueMeldungen, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MeldungData> FUN_fdcMeldungenLadenAsync(string i_strMeldungId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcMeldungenContextHinzufuegenAsync(IEnumerable<EDC_MeldungContextData> i_enuMeldungenContext, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcMeldungContextHinzufuegenAsync(EDC_MeldungContextData i_edcMeldungenContext, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_MeldungContextData> FUN_fdcMeldungenContextLadenAsync(string i_strMeldungId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcZyklischeMeldungenHinzufuegenAsync(IEnumerable<Tuple<EDC_ZyklischeMeldungData, EDC_MeldungData>> i_enuNeueMeldungenTuple, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcZyklischeMeldungenVorlagenSchreibenAsync(IEnumerable<EDC_ZyklischeMeldungVorlageGruppeData> i_enuGruppen, IEnumerable<EDC_ZyklischeMeldungVorlageData> i_enuMeldungen, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_ZyklischeMeldungVorlageGruppeData>> FUN_fdcAlleZyklischeMeldungVorlagenGruppenAsync(IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_ZyklischeMeldungVorlageData>> FUN_fdcZyklischeMeldungenEinerVorlageGruppeAsync(long i_i64GruppeId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcErmittleDieAnzahlMeldungenVorStartdatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoescheAlleQuittiertenMeldungenVorDatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion = null);

		Task<DateTime> FUN_fdcErmittleDatumLetzteQuittierteMeldungAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);
	}
}
