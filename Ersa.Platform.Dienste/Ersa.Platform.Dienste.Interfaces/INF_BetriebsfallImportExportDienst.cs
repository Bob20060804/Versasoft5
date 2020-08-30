using Ersa.Platform.DataDienste.MaschinenVerwaltung;
using System;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Interfaces
{
	public interface INF_BetriebsfallImportExportDienst
	{
		Task FUN_fdcImportierenAsync(string i_strImportDateiPfad, IProgress<STRUCT_MaschinenDatenLadenStatus> i_fdcFortschrittsEmpfaenger);

		Task FUN_fdcExportierenAsync(string i_strExportDateiPfad, int i_i32MaxDateiGroesseInKb);
	}
}
