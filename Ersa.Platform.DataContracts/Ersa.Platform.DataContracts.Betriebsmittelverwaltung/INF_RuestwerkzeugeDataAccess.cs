using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Betriebsmittelverwaltung
{
	public interface INF_RuestwerkzeugeDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_RuestwerkzeugeData>> FUN_fdcLeseRuestwerkzeugeFuerKomponentenIdAusDatenbankAsync(long i_i32RuestkomponentenId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_RuestwerkzeugeData> FUN_fdcLeseRuestwerkzeugFuerIdAusDatenbankAsync(long i_i64RuestwerkzeugId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_RuestwerkzeugeData> FUN_fdcRuestwerkzeugDatenSatzHinzufuegenAsync(EDC_RuestwerkzeugeData i_edcRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestwerkzeugDatenSatzAendernAsync(EDC_RuestwerkzeugeData i_edcRuestwerkzeugeData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestwerkzeugDatenSatzLoeschenAsync(long i_i64RuestwerkzeugId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRuestwerkzeugDatenLoeschenFuerKomponentenIdAsync(long i_i64RuestkomponentenId, IDbTransaction i_fdcTransaktion = null);
	}
}
