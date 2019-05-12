using Ersa.Platform.Mes.Modell;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesEinstellungenImportExportDienst
	{
		T FUN_edcImport<T>();

		void SUB_Export(EDC_MesTypEinstellung i_edcMesEinstellungen);
	}
}
