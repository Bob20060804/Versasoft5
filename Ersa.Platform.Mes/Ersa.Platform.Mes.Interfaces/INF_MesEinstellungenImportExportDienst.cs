using Ersa.Platform.Mes.Modell;

namespace Ersa.Platform.Mes.Interfaces
{
	public interface INF_MesEinstellungenImportExportDienst
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T FUN_edcImport<T>();

		/// <summary>
		/// ����
		/// </summary>
		/// <param name="i_edcMesEinstellungen"></param>
		void SUB_Export(EDC_MesTypEinstellung i_edcMesEinstellungen);
	}
}
