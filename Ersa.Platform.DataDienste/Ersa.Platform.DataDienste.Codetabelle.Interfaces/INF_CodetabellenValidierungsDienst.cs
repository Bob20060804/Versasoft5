using Ersa.Platform.Common.Codetabellen;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle.Interfaces
{
	public interface INF_CodetabellenValidierungsDienst
	{
		ENUM_CodeMaskeValidierungsErgebnis FUN_enmCodeMaskeValidieren(string i_strCodeMaske);

		Task<ENUM_CodeEintragProgrammValidierungsErgebnis> FUN_enmProgrammFehlerValidierenAsync(EDC_Codeeintrag i_edcCodeeintrag);
	}
}
