using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.DatenbankVerwaltungsDienst.Interfaces
{
	public interface INF_DatenbankVerwaltungsDienst
	{
		Task<long> FUN_fdcLeseLoetprogrammVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(long i_i64Version);

		Task<long> FUN_fdcLeseProzessschreiberVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(long i_i64Version);

		Task<long> FUN_fdcLeseLoetprotokollVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(long i_i64Version);

		Task FUN_fdcUpdateLoetprogrammVariablenAsync(IDictionary<string, string> i_dicNamenMapping, IEnumerable<EDC_LoetprogrammParameterData> i_enuNeueVariablen, long i_i64NeueVersion);

		Task FUN_fdcUpdateProzessschreiberVariablenAsync(IDictionary<string, string> i_dicMapping, long i_i64NeueVersion);

		Task FUN_fdcUpdateLoetprotokollVariablenAsync(IDictionary<string, string> i_dicMapping, long i_i64NeueVersion);
	}
}
