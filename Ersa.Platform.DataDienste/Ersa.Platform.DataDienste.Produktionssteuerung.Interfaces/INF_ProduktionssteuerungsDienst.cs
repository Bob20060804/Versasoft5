using Ersa.Platform.Common.Produktionssteuerung;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces
{
	public interface INF_ProduktionssteuerungsDienst
	{
		Task<IEnumerable<EDC_Produktionssteuerungsdaten>> FUN_edcProduktionssteuerungsDatenLadenAsync();

		Task<EDC_Produktionssteuerungsdaten> FUN_edcAktiveProduktionssteuerungsDatenLadenAsync();

		Task<EDC_Produktionssteuerungsdaten> FUN_edcProduktionssteuerungsDatenLadenAsync(long i_i64ProduktionssteuerungId);

		Task FUN_fdcProduktionssteuerungAktivSetzenAsync(long i_i64ProduktionssteuerungId);

		Task<long> FUN_fdcProduktionssteuerungsDatenErstellenAsync(EDC_Produktionssteuerungsdaten i_edcProduktionssteuerungsdaten);

		Task FUN_fdcProduktionssteuerungsDatenAendernAsync(long i_i64ProduktionssteuerungId, string i_strBeschreibung, EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen, bool i_blnIstAktiv);

		Task FUN_fdcProduktionssteuerungsDatenAendernAsync(EDC_Produktionssteuerungsdaten i_edcProduktionssteuerungsdaten);

		Task FUN_fdcProduktionssteuerungsDatenAendernAsync(IEnumerable<EDC_Produktionssteuerungsdaten> i_lstProduktionssteuerungsdaten);

		Task FUN_fdcProduktionssteuerungsDatenLoeschenAsync(long i_i64ProduktionssteuerungId);

		Task<bool> FUN_blnExportProduktionssteuerungsDatenAsync(string i_strExportpfad);

		Task<bool> FUN_blnImportProduktionssteuerungsDatenAsync(string i_strImportDatei);

		Task<long> FUN_i64AktiveDefaultLoetprogrammIdLadenAsync();

		Task FUN_fdcAktiveDefaultLoetprogrammIdSpeichernAsync(long i_i64ProgrammId);
	}
}
