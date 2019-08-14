using Ersa.Platform.Common.Data.Prozessschreiber;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Prozessschreiber
{
	public interface INF_ProzessschreiberDataAccess : INF_DataAccess
	{
		Task FUN_fdcRegistriereProzessschreiberVariablenAsync(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen);

		Task FUN_fdcSpeichereProzessschreiberVariablenAsync(IEnumerable<EDC_SchreiberVariablenData> i_lstVariablen);

		Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleAlleRegistriertenVariablenFuerMaschineAsync(long i_i64MaschinenId);

		Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleProzessschreiberDatenAsync(long i_i64MaschinenId, DateTime i_sttVon, DateTime i_sttBis, IEnumerable<EDC_SchreiberVariablenData> i_lstSuchVariablen);

		Task<IEnumerable<EDC_SchreiberVariablenData>> FUN_fdcHoleProzessschreiberDatenAsync(long i_i64MaschinenId, DateTime i_sttVon, int i_i32AnzahlDatensaetze, IEnumerable<EDC_SchreiberVariablenData> i_lstSuchVariablen);

		Task<long> FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion);

		Task FUN_fdcLoescheAlleProzessschreiberDatenVorDatumAsync(long i_i64MaschinenId, DateTime i_fdcStartzeitpunkt, IDbTransaction i_fdcTransaktion);
	}
}
