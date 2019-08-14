using Ersa.Platform.Common.Data.Cad;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Cad
{
	public interface INF_CadDatenDataAccess : INF_DataAccess
	{
		Task<IEnumerable<EDC_CadVerbotenerBereichData>> FUN_fdcAlleVerbotenenBereicheLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcVerboteneBereicheSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadVerbotenerBereichData> i_enuVerboteneBereiche, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleVerboteneBereicheLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadAblaufSchrittData>> FUN_fdcAlleAblaufSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAblaufSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadAblaufSchrittData> i_enuAblaufschritte, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleAblaufSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadCncSchrittData>> FUN_fdcAlleCncSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcCncSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadCncSchrittData> i_enuCncSchritte, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleCncSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadRoutenSchrittData>> FUN_fdcAlleRoutenSchritteLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRoutenSchritteSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadRoutenSchrittData> i_enuRoutenSchritte, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleRoutenSchritteLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadRoutenData>> FUN_fdcAlleRoutenDatenLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcRoutenDatenSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadRoutenData> i_enuRoutenDaten, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleRoutenDatenLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_CadBewegungsGruppenData>> FUN_fdcAlleBewegungsGruppenLadenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcBewegungsGruppenSpeichernAsync(long i_i64VersionsId, IEnumerable<EDC_CadBewegungsGruppenData> i_enuGruppen, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleBewegungsGruppenLoeschenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<DataSet> FUN_fdcExportiereProjektDatenVersionAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcImportiereAusLoetprogrammAsync(DataSet i_fdcDataSet, long i_i64NeueVersionsId, IDbTransaction i_fdcTransaktion);
	}
}
