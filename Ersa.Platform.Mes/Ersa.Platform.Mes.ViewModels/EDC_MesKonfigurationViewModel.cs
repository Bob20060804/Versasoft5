using Ersa.Global.Common;
using Ersa.Platform.CapabilityContracts.Mes;
using Ersa.Platform.Common.Mes;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.Lokalisierung.Interfaces;
using Ersa.Platform.Mes.Interfaces;
using Ersa.Platform.Mes.Konfiguration;
using Ersa.Platform.UI.BreadCrumb;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.Platform.Mes.ViewModels
{
	[Export]
	public class EDC_MesKonfigurationViewModel : EDC_NavigationsViewModel, INF_VerlassenCommandSource
	{
		public ENUM_ZusatzprotokollTyp m_enuOrigZusatzprotokollTyp;

		public ENUM_ZusatzprotokollTyp m_enmAusgewaehlterZusatzprotokollTyp;

		public ENUM_MesTyp m_enmAusgewaehlteMesTyp;

		private readonly INF_MesKonfigurationsManager m_edcKonfigurationsManager;

		private readonly Lazy<INF_MesMaschinenDatenCapability> m_edcMesMaschinenDatenCapability;

		private readonly IEventAggregator m_fdcEventAggregator;

		private EDC_MesKonfiguration m_edcMesKonfigurationOrig;

		private EDC_XamlListeFunktionsKonfiguration m_lstAktuelleFunk;

		public DelegateCommand PRO_cmdBreadCrumbEintragAusgewaehlt
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdMesTypGewechselt
		{
			get;
			private set;
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				bool flag = false;
				if (m_edcMesKonfigurationOrig != null)
				{
					flag = (m_edcMesKonfigurationOrig.PRO_enuMesTyp != PRO_enmAusgewaehlteMesTyp);
					flag |= !m_edcMesKonfigurationOrig.FUN_blnGleich(PRO_edcMesKonfiguration);
				}
				return flag | (m_enuOrigZusatzprotokollTyp != PRO_enmAusgewaehlterZusatzprotokollTyp);
			}
		}

		public EDC_XamlListeFunktionsKonfiguration PRO_lstAktuelleFunk
		{
			get
			{
				return m_lstAktuelleFunk;
			}
			set
			{
				SetProperty(ref m_lstAktuelleFunk, value, "PRO_lstAktuelleFunk");
			}
		}

		public ENUM_MesTyp PRO_enmAusgewaehlteMesTyp
		{
			get
			{
				return m_enmAusgewaehlteMesTyp;
			}
			set
			{
				SetProperty(ref m_enmAusgewaehlteMesTyp, value, "PRO_enmAusgewaehlteMesTyp");
			}
		}

		public ENUM_ZusatzprotokollTyp PRO_enmAusgewaehlterZusatzprotokollTyp
		{
			get
			{
				return m_enmAusgewaehlterZusatzprotokollTyp;
			}
			set
			{
				SetProperty(ref m_enmAusgewaehlterZusatzprotokollTyp, value, "PRO_enmAusgewaehlterZusatzprotokollTyp");
			}
		}

		public IEnumerable<ENUM_MesTyp> PRO_enmVerfuegbareMesTypen
		{
			get
			{
				List<ENUM_MesTyp> list = new List<ENUM_MesTyp>();
				list.Add(ENUM_MesTyp.KeinMes);
				list.AddRange(from i_edcMesKommunikationsDienst in m_edcKonfigurationsManager.PRO_enuKommunikationsDienste
				select i_edcMesKommunikationsDienst.PRO_enmMesTyp);
				return list;
			}
		}

		public EDC_SmartObservableCollection<EDC_BreadCrumbEintrag> PRO_lstBreadCrumbEintraege
		{
			get;
			private set;
		}

		public EDC_MesKonfiguration PRO_edcMesKonfiguration
		{
			get;
			set;
		}

		public EDC_SmartObservableCollection<ENUM_ZusatzprotokollTyp> PRO_lstZusatzprotokollTyp
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_MesKonfigurationViewModel(INF_CapabilityProvider i_edcCapabilityProvider, INF_MesKonfigurationsManager i_edcKonfigurationsManager, IEventAggregator i_fdcEventAggregator)
		{
			m_edcMesMaschinenDatenCapability = new Lazy<INF_MesMaschinenDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MesMaschinenDatenCapability>);
			m_edcKonfigurationsManager = i_edcKonfigurationsManager;
			m_fdcEventAggregator = i_fdcEventAggregator;
			PRO_lstZusatzprotokollTyp = new EDC_SmartObservableCollection<ENUM_ZusatzprotokollTyp>();
			PRO_lstBreadCrumbEintraege = new EDC_SmartObservableCollection<EDC_BreadCrumbEintrag>();
			PRO_cmdBreadCrumbEintragAusgewaehlt = new DelegateCommand(SUB_Verlassen);
			PRO_cmdMesTypGewechselt = new DelegateCommand(SUB_MesTypGewechselt);
		}

		public void SUB_Verlassen()
		{
			m_edcMesMaschinenDatenCapability.Value.SUB_MesKonfigurationAnsichtVerlassen();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			await FUN_fdcKonfigHolenAsync().ConfigureAwait(continueOnCapturedContext: true);
			if (!PRO_lstBreadCrumbEintraege.Any())
			{
				SUB_BreadCrumbAufbauen();
			}
		}

		public override Task FUN_fdcOnNavigatedFromAsync(NavigationContext i_fdcNavigationContext)
		{
			return base.FUN_fdcOnNavigatedFromAsync(i_fdcNavigationContext);
		}

		public override async Task FUN_fdcAenderungenVerwerfenAsync()
		{
			await FUN_fdcKonfigHolenAsync().ConfigureAwait(continueOnCapturedContext: true);
			await base.FUN_fdcAenderungenVerwerfenAsync().ConfigureAwait(continueOnCapturedContext: true);
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			using (FUN_fdcFortschrittsAnzeigeEinblenden(i_blnUeberdeckendeAnzeige: true, PRO_edcLokalisierungsDienst.FUN_strText("13_844", "...")))
			{
				try
				{
					m_edcMesMaschinenDatenCapability.Value.SUB_AktivesZusatzProtokolleSetzen(PRO_enmAusgewaehlterZusatzprotokollTyp);
					m_enuOrigZusatzprotokollTyp = PRO_enmAusgewaehlterZusatzprotokollTyp;
					await m_edcKonfigurationsManager.FUN_fdcMesInaktivSetzenAsync().ConfigureAwait(continueOnCapturedContext: true);
					if (PRO_edcMesKonfiguration != null)
					{
						PRO_edcMesKonfiguration.PRO_enuMesTyp = PRO_enmAusgewaehlteMesTyp;
						await m_edcKonfigurationsManager.FUN_fdcSpeichereMesKonfigurationAsync(PRO_edcMesKonfiguration).ConfigureAwait(continueOnCapturedContext: true);
						m_edcMesKonfigurationOrig = PRO_edcMesKonfiguration.FUN_edcClone();
						List<EDC_SoftwareFeatureGeaendertPayload> payload = new List<EDC_SoftwareFeatureGeaendertPayload>
						{
							new EDC_SoftwareFeatureGeaendertPayload(ENUM_SoftwareFeatures.MesKonfiguriertFeature, !ENUM_MesTyp.KeinMes.Equals(PRO_enmAusgewaehlteMesTyp))
						};
						m_fdcEventAggregator.GetEvent<EDC_SoftwareFeatureGeaendertEvent>().Publish(payload);
					}
				}
				catch (Exception i_fdcException)
				{
					SUB_OkHinweisAnzeigen("13_250", "13_251", i_fdcException);
				}
				await base.FUN_fdcAenderungenSpeichernAsync();
			}
		}

		private void SUB_MesTypGewechselt()
		{
			PRO_lstAktuelleFunk = FUN_edcAktuelleFunkListe(PRO_enmAusgewaehlteMesTyp);
			SUB_AktualisiereZustand();
		}

		private async Task FUN_fdcKonfigHolenAsync()
		{
			EDC_SmartObservableCollection<ENUM_ZusatzprotokollTyp> pRO_lstZusatzprotokollTyp = PRO_lstZusatzprotokollTyp;
			pRO_lstZusatzprotokollTyp.SUB_Reset(await m_edcMesMaschinenDatenCapability.Value.FUN_fdcImplementierteZusatzProtokolleHolenAsync().ConfigureAwait(continueOnCapturedContext: true));
			PRO_edcMesKonfiguration = await m_edcKonfigurationsManager.FUN_fdcHoleMesKonfigurationAsync().ConfigureAwait(continueOnCapturedContext: true);
			RaisePropertyChanged("PRO_edcMesKonfiguration");
			m_edcMesKonfigurationOrig = PRO_edcMesKonfiguration.FUN_edcClone();
			PRO_enmAusgewaehlteMesTyp = PRO_edcMesKonfiguration.PRO_enuMesTyp;
			PRO_enmAusgewaehlterZusatzprotokollTyp = m_edcMesMaschinenDatenCapability.Value.FUN_enmAktivesZusatzProtokolleHolen();
			m_enuOrigZusatzprotokollTyp = PRO_enmAusgewaehlterZusatzprotokollTyp;
			PRO_lstAktuelleFunk = FUN_edcAktuelleFunkListe(PRO_enmAusgewaehlteMesTyp);
		}

		private EDC_XamlListeFunktionsKonfiguration FUN_edcAktuelleFunkListe(ENUM_MesTyp i_enmAusgewaehlteMesTyp)
		{
			List<EDC_MesFunktionenKonfiguration> list = PRO_edcMesKonfiguration.FUN_lstHoleMesFunktionenListeFuerMesTyp(i_enmAusgewaehlteMesTyp);
			if (list == null)
			{
				return null;
			}
			return new EDC_XamlListeFunktionsKonfiguration(list);
		}

		private void SUB_OkHinweisAnzeigen(string i_strTitelKey, string i_strTextKey, Exception i_fdcException = null)
		{
			string i_strText = (i_fdcException == null) ? base.PRO_edcLokalisierungsDienst.FUN_strText(i_strTextKey) : string.Format("{0}{1}{1}{2}", base.PRO_edcLokalisierungsDienst.FUN_strText(i_strTextKey), Environment.NewLine, i_fdcException);
			base.PRO_edcInteractionController.SUB_OkHinweisAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText(i_strTitelKey), i_strText);
		}

		private void SUB_BreadCrumbAufbauen()
		{
			INF_LokalisierungsDienst pRO_edcLokalisierungsDienst = base.PRO_edcLokalisierungsDienst;
			Func<string, string> delLokalisierungsAktion = pRO_edcLokalisierungsDienst.FUN_strText;
			EDC_BreadCrumbEintrag eDC_BreadCrumbEintrag = new EDC_BreadCrumbEintrag
			{
				PRO_i32BreadCrumbEbene = 0,
				PRO_delLokalisierungsAktion = (() => delLokalisierungsAktion("1_86")),
				PRO_objTag = null
			};
			EDC_BreadCrumbEintrag eDC_BreadCrumbEintrag2 = new EDC_BreadCrumbEintrag
			{
				PRO_i32BreadCrumbEbene = 1,
				PRO_delLokalisierungsAktion = (() => delLokalisierungsAktion("7_101")),
				PRO_blnIstKlickbar = true,
				PRO_objTag = null
			};
			EDC_BreadCrumbEintrag eDC_BreadCrumbEintrag3 = new EDC_BreadCrumbEintrag
			{
				PRO_i32BreadCrumbEbene = 1,
				PRO_delLokalisierungsAktion = (() => delLokalisierungsAktion("18_134")),
				PRO_objTag = null
			};
			EDC_BreadCrumbEintrag[] i_enuElemente = new EDC_BreadCrumbEintrag[3]
			{
				eDC_BreadCrumbEintrag,
				eDC_BreadCrumbEintrag2,
				eDC_BreadCrumbEintrag3
			};
			PRO_lstBreadCrumbEintraege.SUB_Reset(i_enuElemente);
		}
	}
}
