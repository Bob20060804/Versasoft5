using Ersa.Platform.Common.Data.Bauteile;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Bauteile
{
	public interface INF_BauteilDataAccess : INF_DataAccess
	{
		Task FUN_fdcBauteilSpeichernAsync(EDC_BauteilData i_edcBauteildata, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBauteileSpeichernAsync(IEnumerable<EDC_BauteilData> i_enuBauteildata, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_BauteilData> FUN_fdcBauteilDatensatzLadenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerPackageNameLadenAsync(string i_strPackageName, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerPackageNamenLadenAsync(IEnumerable<string> i_strPackageNamen, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBauteilLoeschenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilData>> FUN_fdcAlleBauteileFuerLikePackageNameLadenAsync(string i_strLikePackageName, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBauteilMakrosSpeichernAsync(IEnumerable<EDC_BauteilMakroData> i_lstBauteilMacroDataListe, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBauteilMakroSpeichernAsync(EDC_BauteilMakroData i_edcBauteilMacroData, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBauteilMakroLoeschenAsync(EDC_BauteilMakroData i_edcBauteilMacroData, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdLadenAsync(string i_strBauteilId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdUndGeometrieIdLadenAsync(string i_strBauteilId, long i_i64GeometrieId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_BauteilMakroData>> FUN_fdcAlleBauteileMakrosFuerBauteilIdsLadenAsync(IEnumerable<string> i_strBauteilId, IDbTransaction i_fdcTransaktion = null);
	}
}
