using Ersa.Platform.Common.Data.Betriebsmittel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Betriebsmittelverwaltung
{
	public interface INF_BetriebsmittelDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_BetriebsmittelData>> FUN_fdcLeseBetriebsmittelFuerTypeAusDatenbankAsync(int i_i32BetriebsmittelTyp, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_BetriebsmittelData> FUN_fdcLeseBetriebsmittelDatenFuerIdAusDatenbankAsync(long i_i64BetriebsmittelId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_BetriebsmittelData> FUN_fdcBetriebsmittelDatenSatzHinzufuegenAsync(EDC_BetriebsmittelData i_edcBetriebsmittelData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBetriebsmittelDatenSatzAendernAsync(EDC_BetriebsmittelData i_edcBetriebsmittelData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBetriebsmittelDatenSatzLoeschenAsync(long i_i64BetriebsmittelId, IDbTransaction i_fdcTransaktion = null);
	}
}
