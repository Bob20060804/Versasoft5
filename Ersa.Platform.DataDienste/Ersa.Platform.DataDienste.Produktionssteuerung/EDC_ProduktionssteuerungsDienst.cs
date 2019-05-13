using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.Produktionssteuerung;
using Ersa.Platform.Common.Produktionssteuerung;
using Ersa.Platform.DataContracts.MaschinenVerwaltung;
using Ersa.Platform.DataContracts.Produktionssteuerung;
using Ersa.Platform.DataDienste.Produktionssteuerung.Helfer;
using Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Produktionssteuerung
{
	[Export(typeof(INF_ProduktionssteuerungsDienst))]
	public class EDC_ProduktionssteuerungsDienst : EDC_DataDienst, INF_ProduktionssteuerungsDienst
	{
		private readonly Lazy<INF_ProduktionssteuerungDataAccess> m_edcProduktionssteuerungDataAccess;

		private readonly Lazy<INF_MaschinenDataAccess> m_edcMaschinenDataAccess;

		[Import("Ersa.JsonSerialisierer")]
		public INF_SerialisierungsDienst PRO_edcSerialisierer
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_ProduktionssteuerungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcProduktionssteuerungDataAccess = new Lazy<INF_ProduktionssteuerungDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_ProduktionssteuerungDataAccess>);
			m_edcMaschinenDataAccess = new Lazy<INF_MaschinenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_MaschinenDataAccess>);
		}

		public async Task<IEnumerable<EDC_Produktionssteuerungsdaten>> FUN_edcProduktionssteuerungsDatenLadenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return (from i_edcProduktionssteuerungData in await m_edcProduktionssteuerungDataAccess.Value.FUN_edcProduktionssteuerungDatenLadenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false)
			select EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, i_edcProduktionssteuerungData)).ToList();
		}

		public async Task<EDC_Produktionssteuerungsdaten> FUN_edcAktiveProduktionssteuerungsDatenLadenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_ProduktionssteuerungData eDC_ProduktionssteuerungData = await m_edcProduktionssteuerungDataAccess.Value.FUN_edcAktiveProduktionssteuerungDataLadenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_ProduktionssteuerungData != null)
			{
				return EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, eDC_ProduktionssteuerungData);
			}
			return null;
		}

		public async Task<EDC_Produktionssteuerungsdaten> FUN_edcProduktionssteuerungsDatenLadenAsync(long i_i64ProduktionssteuerungId)
		{
			EDC_ProduktionssteuerungData eDC_ProduktionssteuerungData = await m_edcProduktionssteuerungDataAccess.Value.FUN_edcProduktionssteuerungDataLadenAsync(i_i64ProduktionssteuerungId).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_ProduktionssteuerungData != null)
			{
				return EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, eDC_ProduktionssteuerungData);
			}
			return null;
		}

		public async Task FUN_fdcProduktionssteuerungAktivSetzenAsync(long i_i64ProduktionssteuerungId)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungAktivSetzenAsync(i_i64ProduktionssteuerungId, i64MaschinenId, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_fdcProduktionssteuerungsDatenErstellenAsync(string i_strBeschreibung, EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen, bool i_blnIstAktiv)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			string i_strEinstellungen = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcProduktionsEinstellungen);
			return await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataErstellenAsync(i64MaschinenId, i_strBeschreibung, i_strEinstellungen, i_blnIstAktiv, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_fdcProduktionssteuerungsDatenErstellenAsync(EDC_Produktionssteuerungsdaten i_edcProduktionssteuerungsdaten)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataErstellenAsync(i64MaschinenId, EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, i_edcProduktionssteuerungsdaten), i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcProduktionssteuerungsDatenAendernAsync(long i_i64ProduktionssteuerungId, string i_strBeschreibung, EDC_ProduktionsEinstellungen i_edcProduktionsEinstellungen, bool i_blnIstAktiv)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			string i_strEinstellungen = PRO_edcSerialisierer.FUN_strSerialisieren(i_edcProduktionsEinstellungen);
			await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataAendernAsync(i_i64ProduktionssteuerungId, i64MaschinenId, i_strBeschreibung, i_strEinstellungen, i_blnIstAktiv, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcProduktionssteuerungsDatenAendernAsync(EDC_Produktionssteuerungsdaten i_edcProduktionssteuerungsdaten)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataAendernAsync(i64MaschinenId, EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, i_edcProduktionssteuerungsdaten), i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcProduktionssteuerungsDatenAendernAsync(IEnumerable<EDC_Produktionssteuerungsdaten> i_lstProduktionssteuerungsdaten)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_ProduktionssteuerungData> i_lstProduktionssteuerungData = (from i_edcProduktionssteuerungsdaten in i_lstProduktionssteuerungsdaten
			select EDC_ProduktionssteuerungsKonvertierungsHelfer.FUN_edcKonvertieren(PRO_edcSerialisierer, i_edcProduktionssteuerungsdaten)).ToList();
			await m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataAendernAsync(i64MaschinenId, i_lstProduktionssteuerungData, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcProduktionssteuerungsDatenLoeschenAsync(long i_i64ProduktionssteuerungId)
		{
			return m_edcProduktionssteuerungDataAccess.Value.FUN_fdcProduktionssteuerungDataLoeschenAsync(i_i64ProduktionssteuerungId);
		}

		public async Task<bool> FUN_blnExportProduktionssteuerungsDatenAsync(string i_strExportpfad)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcProduktionssteuerungDataAccess.Value.FUN_blnExportProduktionssteuerungDataAsync(i_i64MaschinenId, i_strExportpfad).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<bool> FUN_blnImportProduktionssteuerungsDatenAsync(string i_strImportDatei)
		{
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcProduktionssteuerungDataAccess.Value.FUN_blnImportProduktionssteuerungDataAsync(i_strImportDatei, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<long> FUN_i64AktiveDefaultLoetprogrammIdLadenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcMaschinenDataAccess.Value.FUN_fdcHoleDefaultLoetProgrammIdAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcAktiveDefaultLoetprogrammIdSpeichernAsync(long i_i64ProgrammId)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcMaschinenDataAccess.Value.FUN_fdcSetzeDefaultLoetProgrammIdAsync(i_i64MaschinenId, i_i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
