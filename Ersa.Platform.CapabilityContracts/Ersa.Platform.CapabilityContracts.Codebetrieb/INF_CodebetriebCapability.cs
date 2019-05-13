using Ersa.Platform.Common.Codetabellen;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.Codebetrieb
{
	public interface INF_CodebetriebCapability
	{
		Task FUN_fdcCodePipelineStartenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineEinmaligLesenStartenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineLesenTestStartenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineSchreibenTestStartenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineBeendenAsync(long i_i64ArrayIndex);

		Task FUN_fdcAlleCodePipelinesBeendenAsync();

		Task<bool> FUN_fdcIstCodePipelineAktivAsync(long i_i64ArrayIndex);

		Task<IDisposable> FUN_fdcCodePipelinePausierenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineNeuErstellenAsync(long i_i64ArrayIndex);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodebetriebKonfigurationenAsync();

		Task<EDC_Codeeintrag> FUN_edcOeffneCodeEingabeDialogAsync(string i_strGelesenerCode);

		Task FUN_fdcOeffneCodeLesenDialogAsync(long i_i64ArrayIndex);

		Task FUN_fdcZeigeDoppelterCodeFehlerAsync(long i_i64ArrayIndex, string i_strCode);
	}
}
