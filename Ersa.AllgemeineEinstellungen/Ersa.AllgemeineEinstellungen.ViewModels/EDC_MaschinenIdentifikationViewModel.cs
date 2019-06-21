using Ersa.Platform.Common.Data.Maschinenverwaltung;
using Ersa.Platform.DataDienste.MaschinenVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.Infrastructure.Events;
using Ersa.Platform.UI.ViewModels;
using Prism.Events;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_MaschinenIdentifikationViewModel : EDC_NavigationsViewModel
	{
		private readonly INF_MaschinenVerwaltungsDienst m_edcMaschinenVerwaltungsDienst;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private string m_strMaschinennummer;

		private EDC_MaschineData m_edcOriginalMaschinenDaten;

		private string m_strMaschinenbezeichnung;

		private string m_strMaschinenstandort;

		private string m_strMaschinenlinie;

		public override bool PRO_blnHatAenderung => FUN_blnHatMaschinenDatenAenderungen();

		public bool PRO_blnDarfEinstellungenEditieren => m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungProduktionSteuern");

		public string PRO_strMaschinennummer
		{
			get
			{
				return m_strMaschinennummer;
			}
			private set
			{
				SetProperty(ref m_strMaschinennummer, value, "PRO_strMaschinennummer");
			}
		}

		public string PRO_strMaschinenbezeichnung
		{
			get
			{
				return m_strMaschinenbezeichnung;
			}
			set
			{
				SetProperty(ref m_strMaschinenbezeichnung, value, "PRO_strMaschinenbezeichnung");
			}
		}

		public string PRO_strMaschinenstandort
		{
			get
			{
				return m_strMaschinenstandort;
			}
			set
			{
				SetProperty(ref m_strMaschinenstandort, value, "PRO_strMaschinenstandort");
			}
		}

		public string PRO_strMaschinenlinie
		{
			get
			{
				return m_strMaschinenlinie;
			}
			set
			{
				SetProperty(ref m_strMaschinenlinie, value, "PRO_strMaschinenlinie");
			}
		}

		[ImportingConstructor]
		public EDC_MaschinenIdentifikationViewModel(INF_MaschinenVerwaltungsDienst i_edcMaschinenVerwaltungsDienst, INF_AutorisierungsDienst i_edcAutorisierungsDienst, IEventAggregator i_fdcEventAggregator)
		{
			m_edcMaschinenVerwaltungsDienst = i_edcMaschinenVerwaltungsDienst;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			i_fdcEventAggregator.GetEvent<EDC_BenutzerGeaendertEvent>().Subscribe(delegate
			{
				EDC_Dispatch.SUB_AktionStarten(delegate
				{
					RaisePropertyChanged("PRO_blnDarfEinstellungenEditieren");
				});
			});
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			PRO_strMaschinennummer = await m_edcMaschinenVerwaltungsDienst.FUN_strMaschinenNummerErmittelnAsync();
			m_edcOriginalMaschinenDaten = await m_edcMaschinenVerwaltungsDienst.FUN_edcMaschinenDatenErmittelnAsync();
			PRO_strMaschinenbezeichnung = (m_edcOriginalMaschinenDaten.PRO_strKommentar ?? string.Empty);
			PRO_strMaschinenstandort = (m_edcOriginalMaschinenDaten.PRO_strOrt ?? string.Empty);
			PRO_strMaschinenlinie = (m_edcOriginalMaschinenDaten.PRO_strProduktionslinie ?? string.Empty);
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			m_edcOriginalMaschinenDaten.PRO_strKommentar = PRO_strMaschinenbezeichnung;
			m_edcOriginalMaschinenDaten.PRO_strOrt = PRO_strMaschinenstandort;
			m_edcOriginalMaschinenDaten.PRO_strProduktionslinie = PRO_strMaschinenlinie;
			await m_edcMaschinenVerwaltungsDienst.FUN_fdcMaschinenDatenSpeichernAsync(m_edcOriginalMaschinenDaten);
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			PRO_strMaschinenbezeichnung = (m_edcOriginalMaschinenDaten.PRO_strKommentar ?? string.Empty);
			PRO_strMaschinenstandort = (m_edcOriginalMaschinenDaten.PRO_strOrt ?? string.Empty);
			PRO_strMaschinenlinie = (m_edcOriginalMaschinenDaten.PRO_strProduktionslinie ?? string.Empty);
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		private bool FUN_blnHatMaschinenDatenAenderungen()
		{
			if (m_edcOriginalMaschinenDaten == null)
			{
				return false;
			}
			if (!(PRO_strMaschinenbezeichnung != (m_edcOriginalMaschinenDaten.PRO_strKommentar ?? string.Empty)) && !(PRO_strMaschinenstandort != (m_edcOriginalMaschinenDaten.PRO_strOrt ?? string.Empty)))
			{
				return PRO_strMaschinenlinie != (m_edcOriginalMaschinenDaten.PRO_strProduktionslinie ?? string.Empty);
			}
			return true;
		}
	}
}
