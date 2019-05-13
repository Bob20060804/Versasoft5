using Ersa.Global.Dienste.Interfaces;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataContracts.Datenbankdaten;
using Ersa.Platform.DataContracts.LeseSchreibgeraete;
using Ersa.Platform.DataDienste.CodeBetrieb.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.DataDienste.CodeBetrieb
{
	[Export(typeof(INF_CodeBetriebEinstellungenDienst))]
	public class EDC_CodeBetriebEinstellungenDienst : EDC_DataDienst, INF_CodeBetriebEinstellungenDienst
	{
		private readonly Lazy<INF_CodePipelineDataAccess> m_edcCodePipelineAccess;

		private readonly Lazy<INF_DatenbankdatenDataAccess> m_edcDatenbankDataAccess;

		[Import("Ersa.XmlSerialisierer", typeof(INF_SerialisierungsDienst))]
		public INF_SerialisierungsDienst PRO_edcSerialisierungsDienst
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_CodeBetriebEinstellungenDienst(INF_DataAccessProvider i_edcDataAccessProvider, INF_CapabilityProvider i_edcCapabilityProvider)
			: base(i_edcCapabilityProvider)
		{
			m_edcCodePipelineAccess = new Lazy<INF_CodePipelineDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_CodePipelineDataAccess>);
			m_edcDatenbankDataAccess = new Lazy<INF_DatenbankdatenDataAccess>(i_edcDataAccessProvider.FUN_edcDataAccessInterfaceHolen<INF_DatenbankdatenDataAccess>);
		}

		public async Task FUN_fdcSpeichereCodeKonfigurationenAsync(IEnumerable<EDC_CodeKonfigData> i_enuKonfigurationen)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			List<EDC_CodeKonfigData> list = i_enuKonfigurationen.ToList();
			foreach (EDC_CodeKonfigData item in list)
			{
				item.PRO_i64MaschinenId = i64MaschinenId;
			}
			await m_edcCodePipelineAccess.Value.FUN_fdcCodeKonfigurationenSpeichernAsync(list, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcCodelesenKonfigurationSpeichernAsync(long i_i64ArrayIndex, bool i_blnIstKonfiguriert, ENUM_LsgOrt i_enmOrt, ENUM_LsgSpur i_enmSpur)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_CodeKonfigData eDC_CodeKonfigData = await m_edcCodePipelineAccess.Value.FUN_fdcHoleCodeKonfigurationAsync(i64MaschinenId, i_i64ArrayIndex).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_CodeKonfigData == null)
			{
				eDC_CodeKonfigData = new EDC_CodeKonfigData
				{
					PRO_i64MaschinenId = i64MaschinenId,
					PRO_i64ArrayIndex = i_i64ArrayIndex,
					PRO_enmCodeVerwendung = ENUM_LsgVerwendung.LesenProLoetgut,
					PRO_blnWirdVerwendet = true,
					PRO_blnAlbVerwenden = false,
					PRO_blnElbVerwenden = false,
					PRO_i64Timeout = 0L
				};
			}
			eDC_CodeKonfigData.PRO_blnIstKonfiguriert = i_blnIstKonfiguriert;
			eDC_CodeKonfigData.PRO_enmOrt = i_enmOrt;
			eDC_CodeKonfigData.PRO_enmSpur = i_enmSpur;
			await m_edcCodePipelineAccess.Value.FUN_fdcCodeKonfigurationSpeichernAsync(eDC_CodeKonfigData, i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcCodelesenKonfigurationSetzenAsync(long i_i64ArrayIndex, bool i_blnIstKonfiguriert)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			EDC_CodeKonfigData eDC_CodeKonfigData = await m_edcCodePipelineAccess.Value.FUN_fdcHoleCodeKonfigurationAsync(i64MaschinenId, i_i64ArrayIndex).ConfigureAwait(continueOnCapturedContext: false);
			if (eDC_CodeKonfigData != null)
			{
				eDC_CodeKonfigData.PRO_blnIstKonfiguriert = i_blnIstKonfiguriert;
				await m_edcCodePipelineAccess.Value.FUN_fdcCodeKonfigurationSpeichernAsync(eDC_CodeKonfigData, i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task FUN_fdcCodeEinstellungenSetzenAsync(long i_i64ArrayIndex, bool i_blnAktiv, bool i_blnAlbAusElb, bool i_blnAlb, bool i_blnElb, long i_i64Timeout)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i_i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			await m_edcCodePipelineAccess.Value.FUN_fdcCodeEinstellungenSetzenAsync(i64MaschinenId, i_i64ArrayIndex, i_blnAktiv, i_blnAlbAusElb, i_blnAlb, i_blnElb, i_i64Timeout, i_i64BenutzerId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<EDC_CodeKonfigData> FUN_fdcHoleCodeKonfigurationAsync(long i_i64ArrayIndex)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcCodePipelineAccess.Value.FUN_fdcHoleCodeKonfigurationAsync(i_i64MaschinenId, i_i64ArrayIndex).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(ENUM_LsgVerwendung i_enmVerwendung)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcCodePipelineAccess.Value.FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(i_i64MaschinenId, i_enmVerwendung).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IEnumerable<long>> FUN_fdcErstelleListeAktiverCodebetriebArraysAsync()
		{
			return from edcKonfig in (await FUN_fdcHoleAktiveCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList().Where(delegate(EDC_CodeKonfigData edcKonfig)
			{
				if (!edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenBeiProduktionsstart))
				{
					return edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenProLoetgut);
				}
				return true;
			})
			select edcKonfig.PRO_i64ArrayIndex;
		}

		public async Task<IEnumerable<long>> FUN_fdcErstelleListeAllerCodebetriebArraysAsync()
		{
			return from edcKonfig in (await FUN_fdcHoleCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList().Where(delegate(EDC_CodeKonfigData edcKonfig)
			{
				if (!edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenBeiProduktionsstart))
				{
					return edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenProLoetgut);
				}
				return true;
			})
			select edcKonfig.PRO_i64ArrayIndex;
		}

		public async Task<bool> FUN_fdcIstCodebetriebKonfiguriertUndAktivAsync()
		{
			return (await FUN_fdcErstelleListeAktiverCodebetriebArraysAsync().ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		public async Task<bool> FUN_fdcIstCodebetriebKonfiguriertAsync()
		{
			return (await FUN_fdcErstelleListeAllerCodebetriebArraysAsync().ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		public async Task<IEnumerable<long>> FUN_fdcErstelleListeAktiveBenutzeranmeldungArraysAsync()
		{
			return from edcKonfig in (await FUN_fdcHoleAktiveCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList()
			where edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenFuerBenutzerAnmeldung)
			select edcKonfig.PRO_i64ArrayIndex;
		}

		public async Task<IEnumerable<long>> FUN_fdcErstelleListeAllerBenutzeranmeldungArraysAsync()
		{
			return from edcKonfig in (await FUN_fdcHoleCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)).ToList()
			where edcKonfig.PRO_enmCodeVerwendung.Equals(ENUM_LsgVerwendung.LesenFuerBenutzerAnmeldung)
			select edcKonfig.PRO_i64ArrayIndex;
		}

		public async Task<bool> FUN_fdcIstBenutzeranmeldungKonfiguriertUndAktivAsync()
		{
			return (await FUN_fdcErstelleListeAktiveBenutzeranmeldungArraysAsync().ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		public async Task<bool> FUN_fdcIstBenutzeranmeldungKonfiguriertAsync()
		{
			return (await FUN_fdcErstelleListeAllerBenutzeranmeldungArraysAsync().ConfigureAwait(continueOnCapturedContext: false)).Any();
		}

		public async Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleCodeKonfigurationenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcCodePipelineAccess.Value.FUN_fdcHoleCodeKonfigurationenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IEnumerable<EDC_CodeKonfigData>> FUN_fdcHoleAktiveCodeKonfigurationenAsync()
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcCodePipelineAccess.Value.FUN_fdcHoleAktiveCodeKonfigurationenFuerMaschineAsync(i_i64MaschinenId).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<IEnumerable<EDC_CodePipelineData>> FUN_fdcHoleGespeichertePipelineDatenAsync(long i_i64ArrayIndex)
		{
			long i_i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			return await m_edcCodePipelineAccess.Value.FUN_fdcHoleCodePipelineElementeAsync(i_i64MaschinenId, i_i64ArrayIndex).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task FUN_fdcCodePipelineAenderungenSpeichernAsync(long i_i64ArrayIndex, IList<long> i_lstEntfernteZweige, IList<IGrouping<long, EDC_CodePipelineData>> i_lstVorhandeneZweige, IList<IGrouping<long, EDC_CodePipelineData>> i_lstNeueZweige)
		{
			long i64MaschinenId = await FUN_i64HoleAktuelleMaschinenIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			long i64BenutzerId = await FUN_i64HoleAktuelleBenutzerIdAsync().ConfigureAwait(continueOnCapturedContext: false);
			IDbTransaction fdcTransaktion = await m_edcDatenbankDataAccess.Value.FUN_fdcStarteTransaktionAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				foreach (long item in i_lstEntfernteZweige)
				{
					await m_edcCodePipelineAccess.Value.FUN_fdcLoeschePipelineZweigAsync(i64MaschinenId, i_i64ArrayIndex, item, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				foreach (IGrouping<long, EDC_CodePipelineData> enuVorhandeneElemente in i_lstVorhandeneZweige)
				{
					long pRO_i64Zweig = enuVorhandeneElemente.First().PRO_i64Zweig;
					long key = enuVorhandeneElemente.Key;
					foreach (EDC_CodePipelineData item2 in enuVorhandeneElemente)
					{
						item2.PRO_i64MaschinenId = i64MaschinenId;
						item2.PRO_i64ArrayIndex = i_i64ArrayIndex;
						item2.PRO_i64Zweig = key;
					}
					await m_edcCodePipelineAccess.Value.FUN_fdcLoeschePipelineZweigAsync(i64MaschinenId, i_i64ArrayIndex, pRO_i64Zweig, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
					await m_edcCodePipelineAccess.Value.FUN_fdcCodePipelineElementeSpeichernAsync(enuVorhandeneElemente, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				foreach (IGrouping<long, EDC_CodePipelineData> item3 in i_lstNeueZweige)
				{
					long key2 = item3.Key;
					List<EDC_CodePipelineData> list = item3.ToList();
					foreach (EDC_CodePipelineData item4 in list)
					{
						item4.PRO_i64MaschinenId = i64MaschinenId;
						item4.PRO_i64ArrayIndex = i_i64ArrayIndex;
						item4.PRO_i64Zweig = key2;
					}
					await m_edcCodePipelineAccess.Value.FUN_fdcCodePipelineElementeSpeichernAsync(list, i64BenutzerId, fdcTransaktion).ConfigureAwait(continueOnCapturedContext: false);
				}
				m_edcDatenbankDataAccess.Value.SUB_CommitTransaktion(fdcTransaktion);
			}
			catch (Exception)
			{
				m_edcDatenbankDataAccess.Value.SUB_RollbackTransaktion(fdcTransaktion);
				throw;
			}
		}
	}
}
