using Ersa.Platform.Common.Loetprogramm;
using System.Collections.Generic;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_ProgrammInfoInterpreter
	{
		bool FUN_blnKannInterpretieren(IList<string> i_lstDateiInhalt);

		EDC_ProgrammInfo FUN_edcProgrammInfoInterpretieren(IList<string> i_lstDateiInhalt);
	}
}
