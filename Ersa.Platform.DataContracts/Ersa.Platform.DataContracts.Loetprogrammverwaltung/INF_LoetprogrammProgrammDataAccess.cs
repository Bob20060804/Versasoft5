using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataContracts.Loetprogrammverwaltung
{
	public interface INF_LoetprogrammProgrammDataAccess : INF_DataAccess
	{
		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		Task<long> FUN_fdcErmittleProgrammIdAusNamenAsync(string i_strBibliotheksName, string i_strProgrammName, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammData> FUN_fdcHoleLoetprogrammAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		Task<EDC_LoetprogrammData> FUN_fdcHoleLoetprogrammAsync(string i_strProgrammName, long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammData> FUN_fdcHoleDefaultLoetprogrammDataFuerMaschineAsync(long i_i64MaschinenId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleAlleProgrammeMitBibliothekIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null);

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleAlleProgrammeMitBibliothekNamenAsync(string i_strBibliotheksName, IDbTransaction i_fdcTransaktion = null);

		Task<DataTable> FUN_fdcHoleProgrammInDataTableAsync(long i_i64ProgrammId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammNeuAnlegenAsync(EDC_LoetprogrammData i_edcLoetprogramm, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammImportierenAsync(DataSet i_fdcDataSet, long i_i64ProgrammId, long i_i64BibliotheksId, long i_i64BenutzerId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammDataAktualisierenAsync(long i_i64ProgrammId, long i_i64BenutzerId, string i_strNotiz, string i_strEingangskontrolle, string i_strAusgangskontrolle, long i_i64Version, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammVerschiebenAsync(long i_i64ProgramId, long i_i64NeueBibiliotheksId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammGeloeschtSetzenAsync(long i_i64ProgrammId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcAlleLoetprogrammEinerBibliothekGeloeschtSetzenAsync(long i_i64BibliothekId, long i_i64BenutzerId, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcLoetprogrammUmbenennenAsync(long i_i64ProgrammId, string i_strNeuerName, IDbTransaction i_fdcTransaktion = null);

		Task FUN_fdcSetzeNeueProgrammVersion(IEnumerable<long> i_enuProgrammId, long i_i64NeueVersion, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcHoleProgrammAnzahlMitBibliotheksIdAsync(long i_i64BibliotheksId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammData>> FUN_fdcHoleProgrammeZuMaschinenGruppenAsync(IEnumerable<long> i_enuMaschinenGruppenIds, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleLoetprgInfoAbfrageVonProgrammIdAsync(long i_i64ProgrammId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleNichtGeloeschteLoetprgInfoListeVonBibliotheksIdAsync(long i_i64BibliothekdId, long i_i64MaschineId, string i_strSuchbegriff = null, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_LoetprogrammInfoDataAbfrage>> FUN_fdcHoleLoetprgInfoAbfrageVonVersionsIdAsync(long i_i64VersionsId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null);

		Task<EDC_LoetprogrammInfoDataAbfrage> FUN_fdcHoleAktuelleFreigegebeneLoetprgInfoAbfrageVonVersionsIdAsync(long i_i64VersionsId, long i_i64MaschineId, IDbTransaction i_fdcTransaktion = null);
	}
}
