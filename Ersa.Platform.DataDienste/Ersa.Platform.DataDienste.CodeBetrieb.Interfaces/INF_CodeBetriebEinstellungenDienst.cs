using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.CodeBetrieb.Interfaces
{
	public interface INF_CodeBetriebEinstellungenDienst
	{
		Task FUN_fdcSpeichereCodeKonfigurationenAsync(IEnumerable<EDC_CodeKonfigData> i_enuKonfigurationen);

		Task FUN_fdcCodelesenKonfigurationSpeichernAsync(long i_i64ArrayIndex, bool i_blnIstKonfiguriert, ENUM_LsgOrt i_enmOrt, ENUM_LsgSpur i_enmSpur);

		Task FUN_fdcCodelesenKonfigurationSetzenAsync(long i_i64ArrayIndex, bool i_blnIstKonfiguriert);

		Task<EDC_CodeKonfigData> FUN_fdcHoleCodeKonfigurationAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodeEinstellungenSetzenAsync(long i_i64ArrayIndex, bool i_blnAktiv, bool i_blnAlbAusElb, bool i_blnAlb, bool i_blnElb, long i_i64Timeout);

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(ENUM_LsgVerwendung i_enmVerwendung);

		Task<IEnumerable<long>> FUN_fdcErstelleListeAktiverCodebetriebArraysAsync();

		Task<IEnumerable<long>> FUN_fdcErstelleListeAllerCodebetriebArraysAsync();

		Task<bool> FUN_fdcIstCodebetriebKonfiguriertUndAktivAsync();

		Task<bool> FUN_fdcIstCodebetriebKonfiguriertAsync();

		Task<IEnumerable<long>> FUN_fdcErstelleListeAktiveBenutzeranmeldungArraysAsync();

		Task<IEnumerable<long>> FUN_fdcErstelleListeAllerBenutzeranmeldungArraysAsync();

		Task<bool> FUN_fdcIstBenutzeranmeldungKonfiguriertUndAktivAsync();

		Task<bool> FUN_fdcIstBenutzeranmeldungKonfiguriertAsync();

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenAsync();

		Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenAsync();

		Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleGespeichertePipelineDatenAsync(long i_i64ArrayIndex);

		Task FUN_fdcCodePipelineAenderungenSpeichernAsync(long i_i64ArrayIndex, IList<long> i_lstEntfernteZweige, IList<IGrouping<long, EDC_CodePipelineData>> i_lstVorhandeneZweige, IList<IGrouping<long, EDC_CodePipelineData>> i_lstNeueZweige);
	}
}
