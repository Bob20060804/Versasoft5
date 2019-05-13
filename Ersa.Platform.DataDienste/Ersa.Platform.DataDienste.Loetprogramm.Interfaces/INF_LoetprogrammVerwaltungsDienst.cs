using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Loetprogramm.Interfaces
{
	public interface INF_LoetprogrammVerwaltungsDienst
	{
		Task<IEnumerable<EDC_BibliothekInfo>> FUN_fdcBibliothekenAuslesenAsync(string i_strSuchbegriff = null);

		Task<EDC_BibliothekInfo> FUN_fdcBibliothekAuslesenAsync(long i_i64BibliotheksId, string i_strSuchbegriff = null);

		Task<IList<string>> FUN_fdcBibliothekNamenAuslesenAsync();

		Task<IEnumerable<KeyValuePair<long, string>>> FUN_fdcProgrammIdUndNamenInBibliothekErmittelnAsync(string i_strBibliotheksName);

		Task<IEnumerable<KeyValuePair<long, string>>> FUN_fdcProgrammIdUndNamenInBibliothekErmittelnAsync(long i_i64BibliotheksId);

		Task<IEnumerable<EDC_VersionsInfo>> FUN_fdcLoetprogrammVersionsStapelHolenAsync(long i_i64ProgrammId);

		Task<EDC_VersionsInfo> FUN_fdcLoetprogrammVersionHolenAsync(long i_i64VersionsId);

		Task FUN_fdcLoetprogrammVerschiebenAsync(long i_i64ProgrammId, long i_i64BibliotheksId);

		Task<long> FUN_fdcLoetprogrammDuplizierenAsync(long i_i64ProgrammId, string i_strNeuerName, long i_i64BibliotheksId);

		Task<long> FUN_fdcLoetprogrammDuplizierenAsync(long i_i64ProgrammId, long i_i64UrsprungsVerionsId, string i_strNeuerName, long i_i64BibliotheksId);

		Task FUN_fdcLoetprogrammLoeschenAsync(long i_i64ProgrammId);

		Task FUN_fdcLoetprogrammUmbenennenAsync(long i_i64ProgrammId, string i_strNeuerName);

		Task<long> FUN_fdcBibliothekErstellenAsync(string i_strBibliotheksName);

		Task FUN_fdcBibliothekLoeschenAsync(long i_i64BibliotheksId);

		Task FUN_fdcBibliothekUmbenennenAsync(long i_i64BibliotheksId, string i_strNeuerName);

		Task<long> FUN_fdcBibliothekDuplizierenAsync(long i_i64BibliotheksId, string i_strNeuerName);

		Task<EDC_ProgrammInfo> FUN_fdcDefaultProgrammInfoLesenAsync();

		Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoLesenAsync(long i_i64ProgrammId);

		Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoAusVersionsIdLesenAsync(long i_i64VersionsId);

		Task<EDC_ProgrammInfo> FUN_fdcProgrammInfoFuerAktuelleFreigegebeneVersionAusVersionsIdLesenAsync(long i_i64VersionsId, IDbTransaction i_fdcTransaktion = null);

		Task<long> FUN_fdcImportBibliothekAsync(string i_strBibliothekspfad, string i_strNeuerBibliothekName);

		Task<long> FUN_fdcImportiereLoetprogrammAsync(long i_i64ZielBibliotheksId, string i_strProgrammDatei, string i_strNeuerProgrammName);

		Task<bool> FUN_fdcExportBibliothekAsync(long i_i64ZielBibliotheksId, string i_strZielpfad);

		Task<bool> FUN_fdcExportProgrammAsync(long i_i64ProgrammId, string i_strZielpfad);

		Task<long> FUN_fdcArbeitsversionAnlegenAsync(long i_i64ProgrammId, long i_i64UrsprungVersionsId, string i_strKommentar);

		Task FUN_fdcArbeitsversionLoeschenAsync(long i_i64ProgrammId);

		Task FUN_fdcVersionFreigebenAsync(long i_i64ProgrammId, long i_i64FreigabeVersionsId, string i_strKommentar);

		Task FUN_fdcFreigabeWegnehmenAsync(long i_i64ProgrammId);

		Task FUN_fdcKommentarFuerArbeitsversionSetzenAsync(long i_i64VersionsId, string i_strKommentar);

		Task FUN_fdcVersionFreigebeStatusUndNotizenSetzenAsync(long i_i64ProgrammId, long i_i64VersionsId, ENUM_LoetprogrammFreigabeStatus i_enmFreigabeStatus, ENUM_LoetprogrammFreigabeArt i_enmFreigabeArt = ENUM_LoetprogrammFreigabeArt.Einstufig, string i_edcFreigabeKommentar = null);

		Task FUN_fdcVersionSichtbarkeitEntfernenAsync(long i_i64ProgrammId, long i_i64VersionsId);

		Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammArbeitsversionLadenAsync(long i_i64ProgrammId);

		Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammProduktionsFreigabeVersionLadenAsync(long i_i64ProgrammId);

		Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammLadenAsync(long i_i64ProgrammId, long i_i64VersionsId);

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		Task<long> FUN_fdcErmittleProgrammIdAusNamenAsync(string i_strBibliotheksName, string i_strProgrammName, IDbTransaction i_fdcTransaktion = null);

		[Obsolete("Laden anhand des Namens ist nicht eindeutig. Auf Methoden mit VersionsId umstellen!")]
		Task<EDC_LoetprogrammStand> FUN_fdcLoetprogrammLadenAsync(string i_strBibliotheksName, string i_strProgrammName, long i_i64VersionsId);

		Task FUN_fdcLoetprogrammBasisDatenSpeichernAsync(EDC_LoetprogrammData i_edcLoetprogramm, string i_strVersionsKommentar, string i_strBildImportPfad, IDictionary<ENUM_BildVerwendung, EDC_FiducialBilddaten> i_dicFiducialBilder = null);

		Task<long> FUN_fdcLoetprogrammSpeichernAsync(EDC_LoetprogrammStand i_edcLoetprogrammStand, string i_strBildImportPfad, IDbTransaction i_fdcTransaktion = null);

		Task<IEnumerable<EDC_ProgrammAenderung>> FUN_fdcErmittleParameterAenderungenZwischenZweiVersionenAsync(long i_i64AlteVersionsId, long i_i64NeueVersionsId);
	}
}
