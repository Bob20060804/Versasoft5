using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Codetabellen;
using Ersa.Platform.Common.Data.Codetabelle;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.DataContracts.Codetabelle;
using Ersa.Platform.DataContracts.Loetprogrammverwaltung;
using Ersa.Platform.DataDienste.Codetabelle.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.Codetabelle
{
	[Export(typeof(INF_CodetabellenVerwaltungsDienst))]
	public class EDC_CodetabellenVerwaltungsDienst : EDC_DataDienst, INF_CodetabellenVerwaltungsDienst
	{
		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcMaschinenBasisDatenCapability;

		private readonly Lazy<INF_CodetabellenDataAccess> m_edcCodetabellenDataAccess;

		private readonly Lazy<INF_LoetprogrammBibliothekDataAccess> m_edcLoetprogrammBibliothekDataAccess;

		private readonly Lazy<INF_LoetprogrammProgrammDataAccess> m_edcLoetprogrammProgrammDataDataAccess;

		[Import]
		public INF_CodetabellenImportExportDienst PRO_edcImportExportDienst
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_CodetabellenVerwaltungsDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcMaschinenBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			m_edcLoetprogrammBibliothekDataAccess = new Lazy<INF_LoetprogrammBibliothekDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammBibliothekDataAccess>);
			m_edcLoetprogrammProgrammDataDataAccess = new Lazy<INF_LoetprogrammProgrammDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_LoetprogrammProgrammDataAccess>);
			m_edcCodetabellenDataAccess = new Lazy<INF_CodetabellenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CodetabellenDataAccess>);
		}

		public async Task<EDC_Codetabelle> FUN_edcAktiveCodetabelleErmittelnAsync()
		{
			long i_i64CodetabellenId = await m_edcMaschinenBasisDatenCapability.Value.FUN_fdcHoleAktiveCodetabellenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_CodetabelleData edcAktiveCodeTabelle = await m_edcCodetabellenDataAccess.Value.FUN_edcCodetabelleLadenAsync(i_i64CodetabellenId).ConfigureAwait(continueOnCapturedContext: false);
			if (edcAktiveCodeTabelle == null)
			{
				return null;
			}
			long obj = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (!edcAktiveCodeTabelle.PRO_i64GruppenId.Equals(obj))
			{
				return null;
			}
			return await FUN_edcCodeTabelleDataKonvertierenAsync(edcAktiveCodeTabelle, await m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabelleneintraegeLadenAsync(edcAktiveCodeTabelle.PRO_i64CodetabellenId).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcAktiveCodetabelleSetzenAsync(long i_i64IdAktiveCodetabelle)
		{
			return m_edcMaschinenBasisDatenCapability.Value.FUN_fdcSetzeAktiveCodetabellenIdAsync(i_i64IdAktiveCodetabelle);
		}

		public async Task<IEnumerable<EDC_Codetabelle>> FUN_enuCodetabellenErmittelnAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_Codetabelle> lstCodeTabellen = new List<EDC_Codetabelle>();
			foreach (EDC_CodetabelleData edcCodeTabelleData in await m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabellenLadenAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false))
			{
				lstCodeTabellen.Add(await FUN_edcCodeTabelleDataKonvertierenAsync(edcCodeTabelleData, await m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabelleneintraegeLadenAsync(edcCodeTabelleData.PRO_i64CodetabellenId).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false));
			}
			return lstCodeTabellen;
		}

		public async Task FUN_fdcNeueCodetabelleSpeichernAsync(EDC_Codetabelle i_edcCodetabelle)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64GruppenId = await FUN_i64HoleZugewieseneGruppenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege = await FUN_enuCodetabellenEintraegeKonvertierenAsync(i_edcCodetabelle.PRO_lstEintraege, i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			long num2 = i_edcCodetabelle.PRO_i64Id = await m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabelleMitEintraegenHinzufuegenAsync(i_edcCodetabelle.PRO_strName, i_enuCodetabelleneintraege, i64BenutzerId, i64GruppenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcGeaenderteCodetabelleSpeichernAsync(EDC_Codetabelle i_edcCodetabelle)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintraege = await FUN_enuCodetabellenEintraegeKonvertierenAsync(i_edcCodetabelle.PRO_lstEintraege, i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
			await m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabelleMitEintraegenAendernAsync(i_edcCodetabelle.PRO_i64Id, i_edcCodetabelle.PRO_strName, i_enuCodetabelleneintraege, i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public Task FUN_fdcCodetabelleLoeschenAsync(EDC_Codetabelle i_edcCodetabelle)
		{
			return m_edcCodetabellenDataAccess.Value.FUN_fdcCodetabelleMitEintraegenLoeschenAsync(i_edcCodetabelle.PRO_i64Id);
		}

		public Task FUN_fdcCodetabelleExportierenAsync(EDC_Codetabelle i_edcCodetabelle, string i_strDateiPfad)
		{
			PRO_edcImportExportDienst.SUB_Export(i_edcCodetabelle, i_strDateiPfad);
			return Task.FromResult(0);
		}

		public Task<EDC_Codetabelle> FUN_edcCodetabelleImportierenAsync(string i_strDateiPfad)
		{
			EDC_Codetabelle eDC_Codetabelle = PRO_edcImportExportDienst.FUN_edcImport(i_strDateiPfad);
			eDC_Codetabelle.PRO_strName = Path.GetFileNameWithoutExtension(i_strDateiPfad);
			return Task.FromResult(eDC_Codetabelle);
		}

		private async Task<EDC_Codetabelle> FUN_edcCodeTabelleDataKonvertierenAsync(EDC_CodetabelleData i_edcCodetabelleData, IEnumerable<EDC_CodetabelleneintragData> i_enuCodetabelleneintragData)
		{
			EDC_Codetabelle edcCodeTabelle = new EDC_Codetabelle
			{
				PRO_strName = i_edcCodetabelleData.PRO_strName,
				PRO_lstEintraege = new List<EDC_Codeeintrag>(),
				PRO_i64Id = i_edcCodetabelleData.PRO_i64CodetabellenId
			};
			foreach (EDC_CodetabelleneintragData edcCodeTabellenEintragData in i_enuCodetabelleneintragData)
			{
				EDC_LoetprogrammBibliothekData edcLoetprogrammBibliothek = await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitIdAsync(edcCodeTabellenEintragData.PRO_i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false);
				EDC_LoetprogrammData eDC_LoetprogrammData = await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(edcCodeTabellenEintragData.PRO_i64ProgrammId).ConfigureAwait(continueOnCapturedContext: false);
				EDC_Codeeintrag item = new EDC_Codeeintrag
				{
					PRO_strCode = edcCodeTabellenEintragData.PRO_strCode,
					PRO_strBibliothek = ((edcLoetprogrammBibliothek == null) ? string.Empty : edcLoetprogrammBibliothek.PRO_strName),
					PRO_strProgramm = ((eDC_LoetprogrammData == null) ? string.Empty : eDC_LoetprogrammData.PRO_strName),
					PRO_i64ProgrammId = edcCodeTabellenEintragData.PRO_i64ProgrammId
				};
				edcCodeTabelle.PRO_lstEintraege.Add(item);
			}
			return edcCodeTabelle;
		}

		private async Task<IEnumerable<EDC_CodetabelleneintragData>> FUN_enuCodetabellenEintraegeKonvertierenAsync(IEnumerable<EDC_Codeeintrag> i_enuCodeEintraege, long i_i64MaschinenId)
		{
			List<EDC_CodetabelleneintragData> lstCodetabellenEintraegeData = new List<EDC_CodetabelleneintragData>();
			foreach (EDC_Codeeintrag edcCodetabellenEintrag in i_enuCodeEintraege)
			{
				long i64BibliotheksId = await FUN_i64HoleBibliotheksIdZuNamenAsync(edcCodetabellenEintrag.PRO_strBibliothek, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
				long pRO_i64ProgrammId = await FUN_i64HoleProgrammIdZuNamenAsync(edcCodetabellenEintrag.PRO_strProgramm, i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false);
				lstCodetabellenEintraegeData.Add(new EDC_CodetabelleneintragData
				{
					PRO_i64BibliotheksId = i64BibliotheksId,
					PRO_i64ProgrammId = pRO_i64ProgrammId,
					PRO_strCode = edcCodetabellenEintrag.PRO_strCode
				});
			}
			return lstCodetabellenEintraegeData;
		}

		private async Task<long> FUN_i64HoleBibliotheksIdZuNamenAsync(string i_strBibliotheksName, long i_i64MaschinenId)
		{
			return (await m_edcLoetprogrammBibliothekDataAccess.Value.FUN_edcHoleBibliothekMitNamenAsync(i_strBibliotheksName, i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64BibliothekId ?? 0;
		}

		private async Task<long> FUN_i64HoleProgrammIdZuNamenAsync(string i_strProgrammName, long i_i64BibliotheksId)
		{
			if (i_i64BibliotheksId == 0L)
			{
				return 0L;
			}
			return (await m_edcLoetprogrammProgrammDataDataAccess.Value.FUN_fdcHoleLoetprogrammAsync(i_strProgrammName, i_i64BibliotheksId).ConfigureAwait(continueOnCapturedContext: false))?.PRO_i64ProgrammId ?? 0;
		}
	}
}
