using Ersa.Platform.Common.Data.Produktionssteuerung;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Produktionssteuerung
{
	public interface INF_ProduktionssteuerungDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_ProduktionssteuerungData>> FUN_edcProduktionssteuerungDatenLadenAsync(long i_i64MaschinenId);

		Task<EDC_ProduktionssteuerungData> FUN_edcProduktionssteuerungDataLadenAsync(long i_i64ProduktionssteuerungId);

		Task<EDC_ProduktionssteuerungData> FUN_edcAktiveProduktionssteuerungDataLadenAsync(long i_i64MaschinenId);

		Task FUN_fdcProduktionssteuerungAktivSetzenAsync(long i_i64ProduktionssteuerungId, long i_i64MaschinenId, long i_i64BenutzerId);

		Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64ProduktionssteuerungId, long i_i64MaschinenId, string i_strBeschreibung, string i_strEinstellungen, bool i_blnIstAktiv, long i_i64BenutzerId);

		Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64MaschinenId, EDC_ProduktionssteuerungData i_edcProduktionssteuerungData, long i_i64BenutzerId);

		Task FUN_fdcProduktionssteuerungDataAendernAsync(long i_i64MaschinenId, IEnumerable<EDC_ProduktionssteuerungData> i_lstProduktionssteuerungData, long i_i64BenutzerId);

		Task<long> FUN_fdcProduktionssteuerungDataErstellenAsync(long i_i64MaschinenId, string i_strBeschreibung, string i_strEinstellungen, bool i_blnIstAktiv, long i_i64BenutzerId);

		Task<long> FUN_fdcProduktionssteuerungDataErstellenAsync(long i_i64MaschinenId, EDC_ProduktionssteuerungData i_edcProduktionssteuerungData, long i_i64BenutzerId);

		Task FUN_fdcProduktionssteuerungDataLoeschenAsync(long i_i64ProduktionssteuerungId);

		Task<bool> FUN_blnExportProduktionssteuerungDataAsync(long i_i64MaschinenId, string i_strExportpfad);

		Task<bool> FUN_blnImportProduktionssteuerungDataAsync(string i_strImportDatei, long i_i64BenutzerId);
	}
}
