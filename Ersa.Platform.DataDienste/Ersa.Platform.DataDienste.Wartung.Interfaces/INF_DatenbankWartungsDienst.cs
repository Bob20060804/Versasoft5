using System;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Wartung.Interfaces
{
	public interface INF_DatenbankWartungsDienst
	{
		Task FUN_fdcSpeicherAktuellesDatumAlsLetztesBackupDatumAsync();

		Task<string> FUN_fdcLeseLetztesBackupDatumAusDatenbankAsync();

		Task FUN_fdcSichereDieDatenbankAsync(string i_strSicherungspfad);

		Task FUN_fdcAuslieferungszustandFuerMaschineSetzenAsync();

		Task FUN_fdcLoescheAlteMeldungenVorStartdatumAsync(DateTime i_fdcStartdatum);

		Task<long> FUN_fdcErmittleDieAnzahlMeldungenVorStartdatumAsync(DateTime i_fdcStartdatum);

		Task FUN_fdcLoescheAlteProzessschreiberdatenVorStartdatumAsync(DateTime i_fdcStartdatum);

		Task<long> FUN_fdcErmittleDieAnzahProzessschreiberdatenVorStartdatumAsync(DateTime i_fdcStartdatum);

		Task FUN_fdcFuehreDatenbankReorganisierungDurchAsync();

		bool FUN_blnIstDatenbankLokalInstalliert();
	}
}
