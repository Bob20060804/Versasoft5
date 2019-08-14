using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.LeseSchreibgeraete
{
	public interface INF_CodePipelineDataAccess : INF_DataAccess
	{
		Task FUN_fdcCodeEinstellungenSetzenAsync(long i_i64MaschinenId, long i_i64ArrayIndex, bool i_blnAktiv, bool i_blnAlbAusElb, bool i_blnAlb, bool i_blnElb, long i_i64Timeout, long i_i64BenutzerId);

		Task<bool> FUN_fdcIstCodeKonfigurationAktivAsync(long i_i64MaschinenId, long i_i64ArrayIndex);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenFuerMaschineAsync(long i_i64MaschinenId);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenMitVerwendungAsync(long i_i64MaschinenId, ENUM_LsgVerwendung i_enmVerwendung);

		Task<EDC_CodeKonfigData> FUN_fdcHoleCodeKonfigurationAsync(long i_i64MaschinenId, long i_i64ArrayIndex, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenFuerMaschineAsync(long i_i64MaschinenId);

		Task FUN_fdcCodeKonfigurationSpeichernAsync(EDC_CodeKonfigData i_edcCodeKonfiguration, long i_i64BenutzerId);

		Task FUN_fdcCodeKonfigurationenSpeichernAsync(IEnumerable<EDC_CodeKonfigData> i_lstCodeKonfigurationen, long i_i64BenutzerId);

		Task FUN_fdcLoeschePipelineZweigAsync(long i_i64MaschinenId, long i_i64ArrayIndex, long i_i64Zweig, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoeschePipelineArrayAsync(long i_i64MaschinenId, long i_i64ArrayIndex, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_CodePipelineData> FUN_fdcHoleCodePipelineElementAsync(long i_i64MaschinenId, long i_i64ArrayIndex, ENUM_PipelineElement i_enuElement, long i_i64Zweig);

		Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleCodePipelineElementeAsync(long i_i64MaschinenId, long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineElementeSpeichernAsync(IEnumerable<EDC_CodePipelineData> i_lstPipelineElemente, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);
	}
}
