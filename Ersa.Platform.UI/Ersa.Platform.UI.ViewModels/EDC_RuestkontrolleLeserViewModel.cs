using Ersa.Global.Common;
using Ersa.Global.Mvvm;
using Ersa.Platform.CapabilityContracts.LeseSchreibeGeraete;
using Ersa.Platform.CapabilityContracts.Ruestkontrolle;
using Ersa.Platform.Common.Data.LeseSchreibgeraete;
using Ersa.Platform.Common.LeseSchreibGeraete.Ruestkontrolle;
using Ersa.Platform.DataDienste.Ruestkontrolle;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Logging;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.UI.Codeleser;
using Ersa.Platform.UI.Interfaces;
using Prism.Commands;
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
	[Export(typeof(INF_RuestkontrolleLeserViewModel))]
	public class EDC_RuestkontrolleLeserViewModel : BindableBase, INF_RuestkontrolleLeserViewModel
	{
		private readonly Lazy<INF_RuestkontrolleCapability> m_edcRuestkontrolleCapability;

		private readonly Lazy<INF_LsgRuestenCapability> m_edcLsgRuestenCapability;

		private readonly INF_RuestkontrolleEinstellungenDienst m_edcRuestkontrolleEinstellungenDienst;

		private readonly INF_LokalisierungsDienst m_edcLokalisierungsDienst;

		private readonly INF_Logger m_edcLogger;

		private readonly IEventAggregator m_fdcEventAggregator;

		private readonly IDictionary<ENUM_RuestWerkzeug, string> m_dicGeleseneDaten;

		private long? m_i64RuestkontrolleArrayIndex;

		private bool m_blnWirdGeradeGelesen;

		private bool m_blnRuestkontrolleLesenAenderungAboniert;

		public DelegateCommand<EDC_RuestkontrolleLeser> PRO_cmdDatenLesen
		{
			get;
		}

		public EDC_SmartObservableCollection<EDC_RuestkontrolleLeser> PRO_lstRuestkontrolleLeser
		{
			get;
		}

		public bool PRO_blnHatRuestkontrolleLeser => PRO_lstRuestkontrolleLeser.Any();

		[ImportingConstructor]
		public EDC_RuestkontrolleLeserViewModel(INF_CapabilityProvider i_edcCapabilityProvider, INF_RuestkontrolleEinstellungenDienst i_edcRuestkontrolleEinstellungenDienst, INF_LokalisierungsDienst i_edcLokalisierungsDienst, INF_Logger i_edcLogger, IEventAggregator i_fdcEventAggregator)
		{
			m_edcRuestkontrolleCapability = new Lazy<INF_RuestkontrolleCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_RuestkontrolleCapability>);
			m_edcLsgRuestenCapability = new Lazy<INF_LsgRuestenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_LsgRuestenCapability>);
			m_edcRuestkontrolleEinstellungenDienst = i_edcRuestkontrolleEinstellungenDienst;
			m_edcLokalisierungsDienst = i_edcLokalisierungsDienst;
			m_edcLogger = i_edcLogger;
			m_fdcEventAggregator = i_fdcEventAggregator;
			m_dicGeleseneDaten = new ConcurrentDictionary<ENUM_RuestWerkzeug, string>();
			PRO_cmdDatenLesen = new DelegateCommand<EDC_RuestkontrolleLeser>(SUB_DatenLesen);
			PRO_lstRuestkontrolleLeser = new EDC_SmartObservableCollection<EDC_RuestkontrolleLeser>();
		}

		public void SUB_RuestkontrolleLesenAenderungAbonieren()
		{
			if (m_blnRuestkontrolleLesenAenderungAboniert)
			{
				throw new InvalidOperationException("Setup control reading change event already subscribed");
			}
			m_fdcEventAggregator.GetEvent<EDC_CodeLesenEvent>().Subscribe(SUB_CodeLesenAenderungBehandeln);
			m_blnRuestkontrolleLesenAenderungAboniert = true;
		}

		public Task FUN_fdcInitialisierenAsync(params ENUM_RuestWerkzeug[] ia_enmWerkzeuge)
		{
			return FUN_fdcInitialisierenAsync(i_blnGruppeAlsNamenVerwenden: false, ia_enmWerkzeuge);
		}

		public Task FUN_fdcInitialisierenMitGruppenNamenAsync(params ENUM_RuestWerkzeug[] ia_enmWerkzeuge)
		{
			return FUN_fdcInitialisierenAsync(i_blnGruppeAlsNamenVerwenden: true, ia_enmWerkzeuge);
		}

		public void SUB_Aufraeumen()
		{
			m_i64RuestkontrolleArrayIndex = null;
			PRO_lstRuestkontrolleLeser.Clear();
			SUB_AktualisiereZustand();
		}

		private async Task FUN_fdcInitialisierenAsync(bool i_blnGruppeAlsNamenVerwenden, params ENUM_RuestWerkzeug[] ia_enmWerkzeuge)
		{
			_003C_003Ec__DisplayClass23_0 _003C_003Ec__DisplayClass23_;
			EDC_CodeKonfigData edcKonfig2 = _003C_003Ec__DisplayClass23_.edcKonfig;
			EDC_CodeKonfigData edcKonfig;
			EDC_CodeKonfigData eDC_CodeKonfigData = edcKonfig = await m_edcRuestkontrolleEinstellungenDienst.FUN_fdcErmittleAktiveRuestkontrolleKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: true);
			m_i64RuestkontrolleArrayIndex = edcKonfig?.PRO_i64ArrayIndex;
			if (!m_i64RuestkontrolleArrayIndex.HasValue || !ia_enmWerkzeuge.Any())
			{
				PRO_lstRuestkontrolleLeser.Clear();
				return;
			}
			if (!(await m_edcRuestkontrolleCapability.Value.FUN_fdcIstRuestkontrollePipelineAktivAsync(m_i64RuestkontrolleArrayIndex.Value)))
			{
				throw new InvalidOperationException("Cannot initialize manual setup control reading. The pipeline is not started");
			}
			IGrouping<string, EDC_WerkzeugElement>[] a_enuWerkzeugGruppen = m_edcLsgRuestenCapability.Value.FUN_enuErmittleWerkzeuge().ToArray();
			IEnumerable<EDC_RuestkontrolleLeser> i_enuElemente = from i_enmWerkzeug in ia_enmWerkzeuge
			select FUN_edcKonvertieren(edcKonfig?.PRO_i64ArrayIndex ?? 0, i_enmWerkzeug, a_enuWerkzeugGruppen, i_blnGruppeAlsNamenVerwenden) into i_edcLeser
			where i_edcLeser != null
			select i_edcLeser;
			PRO_lstRuestkontrolleLeser.SUB_Reset(i_enuElemente);
			foreach (EDC_RuestkontrolleLeser item in PRO_lstRuestkontrolleLeser)
			{
				if (m_dicGeleseneDaten.ContainsKey(item.PRO_enmRuestWerkzeug))
				{
					item.PRO_strGeleseneDaten = m_dicGeleseneDaten[item.PRO_enmRuestWerkzeug];
				}
				item.PRO_blnIstAktiv = m_blnWirdGeradeGelesen;
			}
			SUB_AktualisiereZustand();
		}

		private void SUB_DatenLesen(EDC_RuestkontrolleLeser i_edcLeser)
		{
			if (i_edcLeser != null)
			{
				m_edcLsgRuestenCapability.Value.SUB_TestLesenTriggern(i_edcLeser.PRO_enmRuestWerkzeug);
			}
		}

		private void SUB_CodeLesenAenderungBehandeln(EDC_CodeLesenEventPayload i_edcPayload)
		{
			long pRO_i64ArrayIndex = i_edcPayload.PRO_i64ArrayIndex;
			if (m_i64RuestkontrolleArrayIndex.HasValue && pRO_i64ArrayIndex == m_i64RuestkontrolleArrayIndex.Value)
			{
				m_blnWirdGeradeGelesen = (i_edcPayload.PRO_enmZustand == ENUM_CodeLesenZustand.LesenGestartet);
				SUB_CodeLesenFuerAktiveGeraeteBehandeln(i_edcPayload.PRO_enmZustand, i_edcPayload.PRO_edcRuestErgebnis);
			}
		}

		private void SUB_CodeLesenFuerAktiveGeraeteBehandeln(ENUM_CodeLesenZustand i_enmZustand, EDC_RuestErgebnis i_edcRuestErgebnis)
		{
			foreach (EDC_RuestkontrolleLeser item in PRO_lstRuestkontrolleLeser)
			{
				if (i_enmZustand == ENUM_CodeLesenZustand.LesenGestartet)
				{
					item.PRO_strGeleseneDaten = string.Empty;
					item.PRO_blnIstAktiv = true;
				}
				else
				{
					item.PRO_blnIstAktiv = false;
				}
			}
			if (i_edcRuestErgebnis != null)
			{
				string text = string.Empty;
				switch (i_enmZustand)
				{
				case ENUM_CodeLesenZustand.LeseErgebnisErmittelt:
					text = (i_edcRuestErgebnis.PRO_strRuestDaten ?? string.Empty);
					break;
				case ENUM_CodeLesenZustand.LeseErgebnisFehlerhaft:
				{
					text = m_edcLokalisierungsDienst.FUN_strText("13_724");
					Type reflectedType = MethodBase.GetCurrentMethod().ReflectedType;
					m_edcLogger.SUB_LogEintragSchreiben(ENUM_LogLevel.Fehler, "Error during code reading", reflectedType?.Namespace, reflectedType?.Name, MethodBase.GetCurrentMethod().Name, i_edcRuestErgebnis.PRO_fdcException);
					break;
				}
				}
				EDC_RuestkontrolleLeser eDC_RuestkontrolleLeser = PRO_lstRuestkontrolleLeser.FirstOrDefault((EDC_RuestkontrolleLeser i_edcLeser) => i_edcLeser.PRO_enmRuestWerkzeug == i_edcRuestErgebnis.PRO_enmWerkzeug);
				if (eDC_RuestkontrolleLeser != null)
				{
					eDC_RuestkontrolleLeser.PRO_strGeleseneDaten = text;
				}
				if (m_dicGeleseneDaten.ContainsKey(i_edcRuestErgebnis.PRO_enmWerkzeug))
				{
					m_dicGeleseneDaten[i_edcRuestErgebnis.PRO_enmWerkzeug] = text;
				}
				else
				{
					m_dicGeleseneDaten.Add(i_edcRuestErgebnis.PRO_enmWerkzeug, text);
				}
			}
		}

		private EDC_RuestkontrolleLeser FUN_edcKonvertieren(long i_i64ArrayIndex, ENUM_RuestWerkzeug i_enmWerkzeug, IGrouping<string, EDC_WerkzeugElement>[] ia_enuWerkzeugGruppen, bool i_blnGruppeAlsNamenVerwenden)
		{
			IGrouping<string, EDC_WerkzeugElement> grouping = ia_enuWerkzeugGruppen.FirstOrDefault((IGrouping<string, EDC_WerkzeugElement> i_enuWerkzeuge) => i_enuWerkzeuge.Any((EDC_WerkzeugElement i_edcElement) => i_edcElement.PRO_enmWerkzeug == i_enmWerkzeug));
			EDC_WerkzeugElement eDC_WerkzeugElement = grouping?.FirstOrDefault((EDC_WerkzeugElement i_edcElement) => i_edcElement.PRO_enmWerkzeug == i_enmWerkzeug);
			if (eDC_WerkzeugElement == null)
			{
				return null;
			}
			string key = grouping.Key;
			string pRO_strNameKey = eDC_WerkzeugElement.PRO_strNameKey;
			string i_strBezeichnung = i_blnGruppeAlsNamenVerwenden ? $"{m_edcLokalisierungsDienst.FUN_strText(key)}" : $"{m_edcLokalisierungsDienst.FUN_strText(key)} {m_edcLokalisierungsDienst.FUN_strText(pRO_strNameKey)}";
			return new EDC_RuestkontrolleLeser(i_i64ArrayIndex, i_enmWerkzeug, i_strBezeichnung);
		}

		private void SUB_AktualisiereZustand()
		{
			RaisePropertyChanged("PRO_blnHatRuestkontrolleLeser");
		}
	}
}
