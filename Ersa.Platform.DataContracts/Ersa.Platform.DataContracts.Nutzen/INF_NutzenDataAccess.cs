using Ersa.Platform.Common.Data.Nutzen;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Nutzen
{
	public interface INF_NutzenDataAccess : INF_DataAccess
	{
		Task FUN_fdcSchreibeNutzenDataInDatenbankAsync(string i_strHash, string i_strNutzenCode, string i_strNestCode, string i_strNestDaten, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_NutzenData>> FUN_fdcLeseNestDatenZuHashAusDatenbankAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_NutzenData>> FUN_fdcLeseNestDatenFuerNutzenCodeAusDatenbankAsync(string i_strHash, string i_strNutzenCode, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_NutzenData> FUN_fdcLeseNestDatenFuerNestCodeAusDatenbankAsync(string i_strHash, string i_strPcbCode, IDbTransaction i_fdcTransaktion = null);
	}
}
