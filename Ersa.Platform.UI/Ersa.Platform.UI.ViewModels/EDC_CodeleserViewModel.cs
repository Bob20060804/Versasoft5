using Ersa.Global.Common;
using Ersa.Global.Common.Helper;
using Ersa.Global.Mvvm;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.CapabilityContracts.Codebetrieb;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete;
using Ersa.Platform.DataDienste.CodeBetrieb.Interfaces;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Codeleser;
using Ersa.Platform.UI.Interfaces;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ersa.Platform.UI.ViewModels
{
	[Export(typeof(INF_CodeleserViewModel))]
	public class EDC_CodeleserViewModel : BindableBase, INF_CodeleserViewModel
	{
		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly Lazy<INF_CodebetriebCapability> m_edcCodeBetriebCapability;

		private readonly INF_CodeBetriebEinstellungenDienst m_edcCodeBetriebEinstellungenDienst;

		private readonly INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		private readonly INF_Logger m_edcLogger;

		private readonly IDictionary<long, IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung>> m_dicGeleseneCodes;

		private readonly IList<long> m_lstAktiveArrayIndizes;

		private readonly IList<long> m_lstAktivierteArrayIndizes;

		private bool m_blnCodeLesenAenderungAboniert;

		public AsyncCommand<EDC_Codeleser> PRO_cmdCodeLesen
		{
			get;
			private set;
		}

		public EDC_SmartObservableCollection<EDC_Codeleser> PRO_lstCodeleser
		{
			get;
			private set;
		}

		public bool PRO_blnHatCodeleser => PRO_lstCodeleser.Any();

		public IEnumerable<long> PRO_enuAktiveArrayIndizes => m_lstAktiveArrayIndizes;

		[ImportingConstructor]
		public EDC_CodeleserViewModel(INF_CapabilityProvider i_edcCapabilityProvider, INF_CodeBetriebEinstellungenDienst i_edcCodeBetriebEinstellungenDienst, INF_LokalisierungsDienst i_edcLokalisierungsDienst, INF_Logger i_edcLogger, IEventAggregator i_fdcEventAggregator)
		{
			m_fdcEventAggregator = i_fdcEventAggregator;
			m_edcLogger = i_edcLogger;
			m_edcCodeBetriebEinstellungenDienst = i_edcCodeBetriebEinstellungenDienst;
			m_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
			m_edcCodeBetriebCapability = new Lazy<INF_CodebetriebCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_CodebetriebCapability>);
			m_dicGeleseneCodes = new ConcurrentDictionary<long, IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung>>();
			m_lstAktiveArrayIndizes = new List<long>();
			m_lstAktivierteArrayIndizes = new List<long>();
			PRO_cmdCodeLesen = new AsyncCommand<EDC_Codeleser>(FUN_fdcCodeLesenAsync);
			PRO_lstCodeleser = new EDC_SmartObservableCollection<EDC_Codeleser>();
		}

		public void SUB_CodeLesenAenderungAbonieren()
		{
			if (m_blnCodeLesenAenderungAboniert)
			{
				throw new InvalidOperationException("Code reading change event already subscribed");
			}
			m_fdcEventAggregator.GetEvent<EDC_CodeLesenEvent>().Subscribe(SUB_CodeLesenAenderungBehandeln);
			m_blnCodeLesenAenderungAboniert = true;
		}

		public async Task FUN_fdcInitialisierenAsync(ENUM_LsgOrt? i_enmOrt = default(ENUM_LsgOrt?), ENUM_LsgSpur? i_enmSpur = default(ENUM_LsgSpur?), ENUM_LsgVerwendung? i_enmVerwendung = default(ENUM_LsgVerwendung?))
		{
			IEnumerable<EDC_CodeKonfigData> source = await FUN_enuHoleLsgEinstellungenAsync(i_enmVerwendung).ConfigureAwait(continueOnCapturedContext: true);
			if (i_enmOrt.HasValue)
			{
				source = from i_edcLsg in source
				where i_edcLsg.PRO_enmOrt == i_enmOrt.Value
				select i_edcLsg;
			}
			if (i_enmSpur.HasValue)
			{
				source = from i_edcLsg in source
				where i_edcLsg.PRO_enmSpur == i_enmSpur.Value
				select i_edcLsg;
			}
			PRO_lstCodeleser.SUB_Reset(source.Select(FUN_edcKonvertieren).ToList());
			foreach (EDC_Codeleser item in PRO_lstCodeleser)
			{
				if (m_dicGeleseneCodes.ContainsKey(item.PRO_i64ArrayIndex))
				{
					if (m_lstAktiveArrayIndizes.Contains(item.PRO_i64ArrayIndex))
					{
						item.PRO_blnIstAktiv = true;
					}
					item.SUB_SetzeGeleseneWerte(m_dicGeleseneCodes[item.PRO_i64ArrayIndex]);
				}
			}
			SUB_AktualisiereZustand();
		}

		public void SUB_Aufraeumen()
		{
			SUB_PipelinesBeendenDieVonHierAktiviertWurden();
			PRO_lstCodeleser.Clear();
			SUB_AktualisiereZustand();
		}

		private void SUB_PipelinesBeendenDieVonHierAktiviertWurden()
		{
			foreach (long lstAktivierteArrayIndize in m_lstAktivierteArrayIndizes)
			{
				m_edcCodeBetriebCapability.Value.FUN_fdcCodePipelineBeendenAsync(lstAktivierteArrayIndize);
			}
			m_lstAktivierteArrayIndizes.Clear();
		}

		private async Task FUN_fdcCodeLesenAsync(EDC_Codeleser i_edcCodeleser)
		{
			if (i_edcCodeleser != null && m_edcCodeBetriebCapability.Value != null)
			{
				long pRO_i64ArrayIndex = i_edcCodeleser.PRO_i64ArrayIndex;
				if (!m_lstAktivierteArrayIndizes.Contains(pRO_i64ArrayIndex))
				{
					m_lstAktivierteArrayIndizes.Add(pRO_i64ArrayIndex);
				}
				await m_edcCodeBetriebCapability.Value.FUN_fdcCodePipelineLesenTestStartenAsync(pRO_i64ArrayIndex);
			}
		}

		private void SUB_CodeLesenAenderungBehandeln(EDC_CodeLesenEventPayload i_edcPayload)
		{
			long pRO_i64ArrayIndex = i_edcPayload.PRO_i64ArrayIndex;
			switch (i_edcPayload.PRO_enmZustand)
			{
			case ENUM_CodeLesenZustand.LesenGestartet:
				if (!m_lstAktiveArrayIndizes.Contains(pRO_i64ArrayIndex))
				{
					m_lstAktiveArrayIndizes.Add(pRO_i64ArrayIndex);
				}
				break;
			case ENUM_CodeLesenZustand.LesenBeendet:
				m_lstAktiveArrayIndizes.Remove(pRO_i64ArrayIndex);
				break;
			}
			SUB_CodeLesenFuerAktiveGeraeteBehandeln(i_edcPayload);
		}

		private void SUB_CodeLesenFuerAktiveGeraeteBehandeln(EDC_CodeLesenEventPayload i_edcPayload)
		{
			EDC_Codeleser eDC_Codeleser = PRO_lstCodeleser.FirstOrDefault((EDC_Codeleser i_edcCodeleser) => i_edcCodeleser.PRO_i64ArrayIndex == i_edcPayload.PRO_i64ArrayIndex);
			if (eDC_Codeleser != null)
			{
				switch (i_edcPayload.PRO_enmZustand)
				{
				case ENUM_CodeLesenZustand.LesenGestartet:
					eDC_Codeleser.PRO_blnIstAktiv = true;
					if (i_edcPayload.PRO_enmVerwendung != ENUM_LsgVerwendung.LesenFuerBenutzerAnmeldung)
					{
						eDC_Codeleser.SUB_SetzeGeleseneWerte(new List<EDC_CodeMitVerwendungUndBedeutung>());
					}
					break;
				case ENUM_CodeLesenZustand.LesenBeendet:
					eDC_Codeleser.PRO_blnIstAktiv = false;
					break;
				case ENUM_CodeLesenZustand.LeseErgebnisErmittelt:
				{
					EDC_Codeleser eDC_Codeleser2 = eDC_Codeleser;
					EDC_CodeLeseErgebnis pRO_edcLeseErgebnis = i_edcPayload.PRO_edcLeseErgebnis;
					eDC_Codeleser2.SUB_SetzeGeleseneWerte((pRO_edcLeseErgebnis != null) ? pRO_edcLeseErgebnis.PRO_dicCodes.FirstOrDefault().Value : null);
					SUB_PipelinesBeendenDieVonHierAktiviertWurden();
					break;
				}
				case ENUM_CodeLesenZustand.LeseErgebnisFehlerhaft:
				{
					string i_strFehler = m_edcLokalisierungsDienst.FUN_strText("13_724");
					eDC_Codeleser.SUB_SetzeFehler(i_strFehler);
					Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
					m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Error during code reading", reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, i_edcPayload.PRO_edcLeseErgebnis?.PRO_fdcException);
					SUB_PipelinesBeendenDieVonHierAktiviertWurden();
					break;
				}
				}
			}
			switch (i_edcPayload.PRO_enmZustand)
			{
			case ENUM_CodeLesenZustand.LesenBeendet:
				break;
			case ENUM_CodeLesenZustand.LesenGestartet:
			case ENUM_CodeLesenZustand.LeseErgebnisErmittelt:
			case ENUM_CodeLesenZustand.LeseErgebnisFehlerhaft:
				if (m_dicGeleseneCodes.ContainsKey(i_edcPayload.PRO_i64ArrayIndex))
				{
					IDictionary<long, IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung>> dicGeleseneCodes = m_dicGeleseneCodes;
					long pRO_i64ArrayIndex = i_edcPayload.PRO_i64ArrayIndex;
					EDC_CodeLeseErgebnis pRO_edcLeseErgebnis2 = i_edcPayload.PRO_edcLeseErgebnis;
					dicGeleseneCodes[pRO_i64ArrayIndex] = ((pRO_edcLeseErgebnis2 != null) ? pRO_edcLeseErgebnis2.PRO_dicCodes.FirstOrDefault().Value : null);
				}
				else
				{
					IDictionary<long, IReadOnlyList<EDC_CodeMitVerwendungUndBedeutung>> dicGeleseneCodes2 = m_dicGeleseneCodes;
					long pRO_i64ArrayIndex2 = i_edcPayload.PRO_i64ArrayIndex;
					EDC_CodeLeseErgebnis pRO_edcLeseErgebnis3 = i_edcPayload.PRO_edcLeseErgebnis;
					dicGeleseneCodes2.Add(pRO_i64ArrayIndex2, (pRO_edcLeseErgebnis3 != null) ? pRO_edcLeseErgebnis3.PRO_dicCodes.FirstOrDefault().Value : null);
				}
				break;
			}
		}

		private async Task<IEnumerable<EDC_CodeKonfigData>> FUN_enuHoleLsgEinstellungenAsync(ENUM_LsgVerwendung? i_enmVerwendung = default(ENUM_LsgVerwendung?))
		{
			IEnumerable<EDC_CodeKonfigData> source = (!i_enmVerwendung.HasValue) ? (await m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleAktiveCodeKonfigurationenAsync().ConfigureAwait(continueOnCapturedContext: false)) : (await m_edcCodeBetriebEinstellungenDienst.FUN_fdcHoleAktiveCodeKonfigurationenMitVerwendungAsync(i_enmVerwendung.Value).ConfigureAwait(continueOnCapturedContext: false));
			return from i_edcKonfig in source
			where i_edcKonfig.PRO_enmCodeVerwendung != ENUM_LsgVerwendung.Ruestkontrolle
			select i_edcKonfig;
		}

		private EDC_Codeleser FUN_edcKonvertieren(EDC_CodeKonfigData i_edcCodebetriebKonfiguration)
		{
			string arg = m_edcLokalisierungsDienst.FUN_strText(EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_edcCodebetriebKonfiguration.PRO_enmSpur));
			string arg2 = m_edcLokalisierungsDienst.FUN_strText(EDC_EnumBasisHelfer.FUN_strEnumWertBeschreibungErmitteln(i_edcCodebetriebKonfiguration.PRO_enmOrt));
			return new EDC_Codeleser(i_edcCodebetriebKonfiguration.PRO_i64ArrayIndex, $"{arg} ({arg2})")
			{
				PRO_blnDarfBedientWerden = (i_edcCodebetriebKonfiguration.PRO_enmCodeVerwendung != ENUM_LsgVerwendung.LesenFuerBenutzerAnmeldung)
			};
		}

		private void SUB_AktualisiereZustand()
		{
			RaisePropertyChanged("PRO_blnHatCodeleser");
		}
	}
}
