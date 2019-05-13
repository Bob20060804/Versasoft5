using Ersa.Platform.Common.LeseSchreibGeraete;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Nutzen.Interfaces
{
	public interface INF_NutzenDatenVerwaltungsDienst
	{
		Task FUN_fdcNestDatenFuerNutzenSpeichernAsync(string i_strHash, IEnumerable<EDC_NestDaten> i_enuNestDaten, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_NestDaten> FUN_fdcNestDatenErmittelnAsync(string i_strHash, string i_strNestCode, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_NestDaten>> FUN_fdcNutzenDatenErmittelnAsync(string i_strHash, string i_strNutzenCode, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_NestDaten>> FUN_fdcDatenZuHashErmittelnAsync(string i_strHash, IDbTransaction i_fdcTransaktion = null);
	}
}
