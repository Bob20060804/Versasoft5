using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.CapabilityContracts.ProgrammVerwaltung
{
	public interface INF_ProgrammVerwaltungCapability
	{
		bool FUN_blnIstAktivesLoetprogramm(long i_i64ProgrammId);

		bool FUN_blnIstAktivesProgrammInBibliothek(long i_i64BibliothekId);

		void SUB_AktivesProgrammZuruecksetzen();

		Task FUN_fdcDefaultProgrammImportierenAsync();

		Func<Task<EDC_NeuesProgrammDaten>> FUN_delNeuesProgrammDatenErmitteln(IEnumerable<string> i_enuBibs, string i_strBibVorauswahl);

		Task FUN_fdcProgrammEditierenAsync(long i_i64PrgId, long i_i64BibId, bool i_blnWizardVerwenden);

		Task FUN_fdcProgrammEditierenAlsNeuesProgrammAsync(long i_i64PrgId, long i_i64BibId, bool i_blnWizardVerwenden, EDC_NeuesProgrammDaten i_edcDaten);

		Task FUN_fdcProgrammVersionBearbeitenAsync(long i_i64VersionId, bool i_blnWizardVerwenden);

		Task FUN_fdcProgrammVersionAnzeigenAsync(long i_i64VersionId);

		IEnumerable<INF_BibOderPrgImportHandler> FUN_enuSpezielleImportHandlerErmitteln();

		IEnumerable<EDC_MaschinenOperation> FUN_enuHoleMaschinenOperationen();

		bool FUN_blnProduktionsfreigabeAnzeigen();

		bool FUN_blnVersionenAnzeigen();
	}
}
