using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Ruestkontrolle
{
	public interface INF_RuestkontrolleEinstellungenDienst
	{
		Task<EDC_CodeKonfigData> FUN_fdcErmittleRuestkontrolleKonfigurationAsync();

		Task<EDC_CodeKonfigData> FUN_fdcErmittleAktiveRuestkontrolleKonfigurationAsync();

		Task<bool> FUN_fdcIstRuestkontrolleKonfiguriertAsync();

		Task FUN_fdcSpeichereRuestkontrolleKonfigurationAsync(EDC_CodeKonfigData i_edcKonfiguration);

		Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleGespeichertePipelineDatenAsync(long i_i64ArrayIndex);

		Task FUN_fdcPipelineDatenSpeichernAsync(long i_i64ArrayIndex, IEnumerable<EDC_CodePipelineData> i_enuPipelineDaten);
	}
}
