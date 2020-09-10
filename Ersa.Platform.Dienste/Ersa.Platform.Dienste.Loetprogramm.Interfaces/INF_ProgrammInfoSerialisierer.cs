using Ersa.Platform.Common;
using Ersa.Platform.Common.Loetprogramm;
using System.Collections.Generic;

namespace Ersa.Platform.Dienste.Loetprogramm.Interfaces
{
	public interface INF_ProgrammInfoSerialisierer
	{
		bool FUN_blnKannSerialisieren(EDC_ProgrammInfo i_edcProgrammInfo);

		IList<string> FUN_lstProgrammInfoSerialisieren(EDC_ProgrammInfo i_edcProgrammInfo, EDC_ElementVersion i_edcVisuVersion);
	}
}
