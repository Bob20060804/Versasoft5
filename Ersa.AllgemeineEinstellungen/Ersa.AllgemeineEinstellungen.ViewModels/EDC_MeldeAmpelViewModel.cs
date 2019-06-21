using Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.MeldeAmpel;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.UI.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_MeldeAmpelViewModel : EDC_NavigationsViewModel
	{
		private readonly INF_CapabilityProvider m_edcCapabilityProvider;

		private readonly Lazy<INF_MeldeAmpelCapability> m_fdcCapability;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private IEnumerable<EDC_AmpelEinstellungen> m_enuAmpelEinstellungen;

		public DelegateCommand PRO_cmdLampenTestAnfordern
		{
			get;
			private set;
		}

		public bool PRO_blnDarfEinstellungenEditieren
		{
			get
			{
				if (FUN_blnIstPrimaer() && !base.PRO_edcShellViewModel.PRO_blnIstMaschineInProduktion)
				{
					return m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungProduktionSteuern");
				}
				return false;
			}
		}

		public IEnumerable<EDC_AmpelEinstellungen> PRO_enuAmpelEinstellungen => m_enuAmpelEinstellungen ?? (m_enuAmpelEinstellungen = m_fdcCapability.Value.FUN_enuAmpeleinstellungenLaden());

		public override bool PRO_blnHatAenderung => PRO_enuAmpelEinstellungen.Any((EDC_AmpelEinstellungen i_edcEinstellungen) => i_edcEinstellungen.PRO_blnHatAenderung);

		[ImportingConstructor]
		public EDC_MeldeAmpelViewModel(INF_CapabilityProvider i_edcCapabilityProvider, INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_ShellViewModel i_edcShellViewModel)
		{
			m_edcCapabilityProvider = i_edcCapabilityProvider;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			m_fdcCapability = new Lazy<INF_MeldeAmpelCapability>(() => m_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MeldeAmpelCapability>());
			PRO_cmdLampenTestAnfordern = new DelegateCommand(delegate
			{
				m_fdcCapability.Value.SUB_LampenTestAnfordern();
			}, FUN_blnKannLampenTestAnfordern);
			PropertyChangedEventManager.AddHandler(i_edcShellViewModel, delegate
			{
				SUB_BerechtigungenAuswerten();
			}, "PRO_blnIstMaschineInProduktion");
		}

		public override Task FUN_fdcAenderungenSpeichernAsync()
		{
			m_fdcCapability.Value.SUB_Speichern();
			return base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			m_fdcCapability.Value.SUB_Verwerfen();
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			m_enuAmpelEinstellungen = m_fdcCapability.Value.FUN_enuAmpeleinstellungenLaden();
			RaisePropertyChanged("PRO_enuAmpelEinstellungen");
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfEinstellungenEditieren");
			PRO_cmdLampenTestAnfordern.RaiseCanExecuteChanged();
		}

		private bool FUN_blnKannLampenTestAnfordern()
		{
			if (FUN_blnIstPrimaer() && m_fdcCapability.Value != null)
			{
				return m_fdcCapability.Value.FUN_blnKannLampenTestAnfordern();
			}
			return false;
		}
	}
}
