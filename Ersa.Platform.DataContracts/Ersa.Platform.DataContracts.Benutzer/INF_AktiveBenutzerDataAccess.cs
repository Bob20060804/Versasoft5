using Ersa.Platform.Common.Data.Benutzer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Benutzer
{
	public interface INF_AktiveBenutzerDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_AktiverBenutzerData>> FUN_fdcAktiveBenutzerLadenAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAktiverBenutzerHinzufuegenAsync(EDC_AktiverBenutzerData i_edcAktiverBenutzer, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAktiverBenutzerEntfernenAsync(long i_i64BenutzerId, long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);
	}
}
