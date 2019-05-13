using Ersa.Platform.Common.Codetabellen;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataDienste.Codetabelle.Interfaces;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle
{
	[Export(typeof(INF_CodetabellenValidierungsDienst))]
	public class EDC_CodetabellenValidierungsDienst : EDC_DataDienst, INF_CodetabellenValidierungsDienst
	{
		private readonly INF_LoetprogrammVerwaltungsDienst m_edcLpVerwaltungsDienst;

		[ImportingConstructor]
		public EDC_CodetabellenValidierungsDienst(INF_LoetprogrammVerwaltungsDienst i_edcLpVerwaltungsDienst, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcLpVerwaltungsDienst = i_edcLpVerwaltungsDienst;
		}

		public ENUM_CodeMaskeValidierungsErgebnis FUN_enmCodeMaskeValidieren(string i_strCodeMaske)
		{
			if (string.IsNullOrWhiteSpace(i_strCodeMaske))
			{
				return ENUM_CodeMaskeValidierungsErgebnis.enmCodeLeer;
			}
			if (i_strCodeMaske.Trim() != i_strCodeMaske)
			{
				return ENUM_CodeMaskeValidierungsErgebnis.enmCodeHatUngueltigeLeerzeichen;
			}
			if (i_strCodeMaske.Length > 399)
			{
				return ENUM_CodeMaskeValidierungsErgebnis.enmCodeZuLang;
			}
			return ENUM_CodeMaskeValidierungsErgebnis.enmValide;
		}

		public async Task<ENUM_CodeEintragProgrammValidierungsErgebnis> FUN_enmProgrammFehlerValidierenAsync(EDC_Codeeintrag i_edcCodeeintrag)
		{
			EDC_VersionsInfo eDC_VersionsInfo = (await m_edcLpVerwaltungsDienst.FUN_fdcLoetprogrammVersionsStapelHolenAsync(i_edcCodeeintrag.PRO_i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault((EDC_VersionsInfo i_edcItem) => ENUM_LoetprogrammStatus.Freigegeben.Equals(i_edcItem.PRO_enmVersionstatus));
			if (eDC_VersionsInfo == null)
			{
				return ENUM_CodeEintragProgrammValidierungsErgebnis.enmLoetprogrammNichtFreigegeben;
			}
			if (eDC_VersionsInfo.PRO_blnIstFehlerhaft)
			{
				return ENUM_CodeEintragProgrammValidierungsErgebnis.enmLoetprogrammIstFehlerhaft;
			}
			return ENUM_CodeEintragProgrammValidierungsErgebnis.enmValide;
		}
	}
}
