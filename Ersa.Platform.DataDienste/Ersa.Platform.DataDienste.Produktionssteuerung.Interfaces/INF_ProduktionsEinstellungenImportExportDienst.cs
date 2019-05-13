using Ersa.Platform.Common.Produktionssteuerung;

namespace Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces
{
	public interface INF_ProduktionsEinstellungenImportExportDienst
	{
		EDC_ProduktionsEinstellungen FUN_edcImport(string i_strDateiPfad);

		void SUB_Export(EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen, string i_strDateiPfad);
	}
}
