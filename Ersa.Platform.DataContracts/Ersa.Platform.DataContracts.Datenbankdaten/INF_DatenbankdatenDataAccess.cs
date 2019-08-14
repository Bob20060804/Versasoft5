using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Datenbankdaten
{
	public interface INF_DatenbankdatenDataAccess : INF_DataAccess
	{
		Task FUN_fdcSpeicherAktuellesDatumAlsLetztesBackupDatumAsync();

		Task<string> FUN_fdcLeseLetztesBackupDatumAusDatenbankAsync();

		Task<long> FUN_fdcLeseLoetprogrammVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleLoetprogrammVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcLeseProzessschreiberVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleProzessschreiberVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcLeseLoetprotokollVariablenVersionAusDatenbankAsync();

		Task FUN_fdcSpeichereAktuelleLoetprotokollVariablenVersionAsync(long i_i64Version, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSichereDieDatenbankAsync(string i_strSicherungspfad);

		Task FUN_fdcFuehreDatenbankWartungDurchAsync();

		bool FUN_blnIstDieDatenbankLokalInstalliert();
	}
}
