using Ersa.Platform.Common;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_ApplikationsController
	{
		EDC_ElementVersion PRO_edcModulVersion
		{
			get;
		}

		void SUB_ZuServiceNavigieren();

		void SUB_ZuProgrammVerwaltungNavigieren();

		void SUB_ZuProgrammVerwaltungNavigieren(string i_strBibliotheksName);
	}
}
