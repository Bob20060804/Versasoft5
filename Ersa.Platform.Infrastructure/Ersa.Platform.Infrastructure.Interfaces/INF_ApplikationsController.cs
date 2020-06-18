using Ersa.Platform.Common;

namespace Ersa.Platform.Infrastructure.Interfaces
{
    /// <summary>
    /// Ó¦ÓÃ¿ØÖÆÆ÷
    /// </summary>
	public interface INF_ApplikationsController
	{
		EDC_ElementVersion PRO_edcModulVersion
		{
			get;
		}

        /// <summary>
        /// Navigate to Service
        /// </summary>
		void SUB_ZuServiceNavigieren();

        /// <summary>
        /// Program Management Navigate
        /// </summary>
		void SUB_ZuProgrammVerwaltungNavigieren();
        /// <summary>
        /// Program Management Navigate
        /// </summary>
        /// <param name="i_strBibliotheksName"></param>
		void SUB_ZuProgrammVerwaltungNavigieren(string i_strBibliotheksName);
	}
}
