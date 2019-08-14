using Ersa.Global.Common.Data.Filter;
using Ersa.Platform.Common.Data.Loetprotokoll;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprotokoll
{
	public interface INF_LoetprotokollDataAccess : INF_DataAccess
	{
		Task FUN_fdcRegistriereProtokollVariablenAsync(IEnumerable<EDC_LoetprotokollVariablenData> i_enuVariablen);

		Task<long> FUN_fdcFuegeProtokollEintragHinzuAsync(EDC_LoetprotokollKopfData i_edcProtokollKopf, IEnumerable<EDC_LoetprotokollVariablenData> i_enuVariablen, Dictionary<string, string> i_dicCodes, Dictionary<string, string> i_dicParameter);

		Task<IEnumerable<STRUCT_FilterBasisDefinition>> FUN_enuHoleMoeglicheFilterFuerLoetprotokollAbfragenAsync(long i_i64MaschinenId);

		Task<IEnumerable<EDC_LoetprotokollAbfrageErgebnis>> FUN_fdcFuehreLoetprotokollAbfrageAusAsync(IEnumerable<STRUCT_FilterKonkret> i_enuAbfrageFilter, IEnumerable<STRUCT_FilterBasisDefinition> i_enuVariablen, long i_i64MaschinenId, long i_i64MaschinenGruppenId, INF_LoetParameterIstSollConverter i_edcIstSollConverter);

		Task<EDC_LoetprotokollAbfrageErgebnis> FUN_fdcHoleLoetprotokollMitProtokollIdAsync(long i_i64ProtokollId, long i_i64MaschinenId, long i_i64MaschinenGruppenId, INF_LoetParameterIstSollConverter i_edcIstSollConverter);
	}
}
