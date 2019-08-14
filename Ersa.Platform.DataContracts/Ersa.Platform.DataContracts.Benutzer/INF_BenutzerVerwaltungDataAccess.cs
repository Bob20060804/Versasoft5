using Ersa.Platform.Common.Data.Benutzer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Benutzer
{
	public interface INF_BenutzerVerwaltungDataAccess : INF_DataAccess
	{
		Task<EDC_BenutzerAbfrageData> FUN_fdcMaschinenBenutzerLadenAsync(long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenDefaultBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcGesamtBenutzerListeLadenAsync(IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenBenutzerListeLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcAnzahlAktiverMaschinenBenutzerErmittelnAsync(long i_i64MaschinenId);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcAktiveMaschinenBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BenutzerAbfrageData>> FUN_fdcMaschinenBenutzerMitNamenLadenAsync(string i_strName, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_BenutzerAbfrageData> FUN_fdcMaschinenBenutzerMitCodeLadenAsync(string i_strCode, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBenutzerHinzufuegenAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBenutzerAendernAsync(EDC_BenutzerAbfrageData i_edcBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBenutzerSynchronisierenAsync(EDC_BenutzerAbfrageData i_edcSynchronBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);
	}
}
