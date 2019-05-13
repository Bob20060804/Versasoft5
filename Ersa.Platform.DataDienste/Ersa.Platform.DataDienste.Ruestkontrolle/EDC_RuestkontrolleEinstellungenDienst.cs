using Ersa.Global.Common;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataDienste.CodeBetrieb.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Ruestkontrolle
{
	[Export(typeof(INF_RuestkontrolleEinstellungenDienst))]
	public class EDC_RuestkontrolleEinstellungenDienst : INF_RuestkontrolleEinstellungenDienst
	{
		private readonly INF_CodeBetriebEinstellungenDienst m_edcCodeBetriebEinstellungenDienst;

		[ImportingConstructor]
		public EDC_RuestkontrolleEinstellungenDienst(INF_CodeBetriebEinstellungenDienst i_edcCodeBetriebEinstellungenDienst)
		{
			m_edcCodeBetriebEinstellungenDienst = i_edcCodeBetriebEinstellungenDienst;
		}

		public async Task<EDC_CodeKonfigData> FUN_fdcErmittleRuestkontrolleKonfigurationAsync()
		{
			return (await m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList().SingleOrDefault((EDC_CodeKonfigData i_edcKonfig) => i_edcKonfig.PRO_enmCodeVerwendung == ENUM_LsgVerwendung.Ruestkontrolle);
		}

		public async Task<EDC_CodeKonfigData> FUN_fdcErmittleAktiveRuestkontrolleKonfigurationAsync()
		{
			return (await m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleAktiveCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList().SingleOrDefault((EDC_CodeKonfigData i_edcKonfig) => i_edcKonfig.PRO_enmCodeVerwendung == ENUM_LsgVerwendung.Ruestkontrolle);
		}

		public async Task<bool> FUN_fdcIstRuestkontrolleKonfiguriertAsync()
		{
			return (await FUN_fdcErmittleRuestkontrolleKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: false))?.PRO_blnIstKonfiguriert ?? false;
		}

		public Task FUN_fdcSpeichereRuestkontrolleKonfigurationAsync(EDC_CodeKonfigData i_edcKonfiguration)
		{
			EDC_CodeKonfigData[] i_enuKonfigurationen = new EDC_CodeKonfigData[1]
			{
				i_edcKonfiguration
			};
			return m_edcCodeBetriebEinstellungenDienst.FUN_fdcSpeichereCodeKonfigurationenAsync(i_enuKonfigurationen);
		}

		public Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleGespeichertePipelineDatenAsync(long i_i64ArrayIndex)
		{
			return m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleGespeichertePipelineDatenAsync(i_i64ArrayIndex);
		}

		public Task FUN_fdcPipelineDatenSpeichernAsync(long i_i64ArrayIndex, IEnumerable<EDC_CodePipelineData> i_enuPipelineDaten)
		{
			EDC_CodePipelineData[] array = i_enuPipelineDaten.ToArray();
			if (!array.Any())
			{
				return Task.CompletedTask;
			}
			return m_edcCodeBetriebEinstellungenDienst.FUN_fdcCodePipelineAenderungenSpeichernAsync(i_i64ArrayIndex, new List<long>(), new List<IGrouping<long, EDC_CodePipelineData>>
			{
				new EDC_Grouping<long, EDC_CodePipelineData>(array.First().PRO_i64Zweig, array)
			}, new List<IGrouping<long, EDC_CodePipelineData>>());
		}
	}
}
