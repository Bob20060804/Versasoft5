using Ersa.AllgemeineEinstellungen.Zeitschaltuhr;
using Ersa.Platform.CapabilityContracts.AllgemeineEinstellungen.Zeitschaltuhr;
using Ersa.Platform.Common;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_ZeitschaltuhrViewModel : EDC_NavigationsViewModel, IDataErrorInfo
	{
		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		private readonly INF_CapabilityProvider m_edcCapabilityProvider;

		private DateTime m_sttAktuelleZeit;

		private INF_ZeitschaltuhrCapability m_edcZeitschaltuhrCapability;

		private string m_strUrlaubStart;

		private string m_strUrlaubEnde;

		public INF_ZeitschaltuhrCapability PRO_edcZeitschaltuhrCapability
		{
			get
			{
				return m_edcZeitschaltuhrCapability;
			}
			set
			{
				m_edcZeitschaltuhrCapability = value;
				RaisePropertyChanged("PRO_edcZeitschaltuhrCapability");
			}
		}

		public string PRO_strUrlaubStart
		{
			get
			{
				return m_strUrlaubStart;
			}
			set
			{
				if (SetProperty(ref m_strUrlaubStart, value, "PRO_strUrlaubStart"))
				{
					SUB_Aktualisieren();
				}
			}
		}

		public string PRO_strUrlaubEnde
		{
			get
			{
				return m_strUrlaubEnde;
			}
			set
			{
				if (SetProperty(ref m_strUrlaubEnde, value, "PRO_strUrlaubEnde"))
				{
					SUB_Aktualisieren();
				}
			}
		}

		public bool PRO_blnDarfZeitschaltuhrEditieren
		{
			get
			{
				if (FUN_blnIstPrimaer() && m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungZeitschaltuhr") && PRO_edcZeitschaltuhrCapability != null)
				{
					return PRO_edcZeitschaltuhrCapability.PRO_blnDarfZeitschaltuhrEditieren;
				}
				return false;
			}
		}

		public ObservableCollection<INF_WochenuhrZeile> PRO_colNichtGeloeschteWochenuhrZeilen
		{
			get;
			set;
		}

		public IEnumerable<EDC_Wochentag> PRO_enuWochentage
		{
			get;
			private set;
		}

		public DateTime PRO_sttAktuelleZeit
		{
			get
			{
				return m_sttAktuelleZeit;
			}
			set
			{
				m_sttAktuelleZeit = value;
				RaisePropertyChanged("PRO_sttAktuelleZeit");
			}
		}

		public override bool PRO_blnHatAenderung
		{
			get
			{
				if (!PRO_edcZeitschaltuhrCapability.FUN_blnAenderungenVorhanden())
				{
					return FUN_blnDatumsAenderungVorhanden();
				}
				return true;
			}
		}

		public DelegateCommand PRO_cmdPcZeitInSpsUebernehmen
		{
			get;
			private set;
		}

		public DelegateCommand<INF_WochenuhrZeile> PRO_cmdWochenuhrZeileLoeschen
		{
			get;
			private set;
		}

		public DelegateCommand PRO_cmdWochenuhrZeileHinzufuegen
		{
			get;
			private set;
		}

		public string Error => string.Empty;

		private DateTime? PRO_dtmUrlaubStart
		{
			get
			{
				if (!DateTime.TryParse(PRO_strUrlaubStart, out DateTime result))
				{
					return null;
				}
				return result;
			}
		}

		private DateTime? PRO_dtmUrlaubEnde
		{
			get
			{
				if (!DateTime.TryParse(PRO_strUrlaubEnde, out DateTime result))
				{
					return null;
				}
				return result;
			}
		}

		public string this[string i_strPropertyName]
		{
			get
			{
				if (i_strPropertyName == "PRO_strUrlaubStart" && !PRO_dtmUrlaubStart.HasValue)
				{
					return "4_2121";
				}
				if (i_strPropertyName == "PRO_strUrlaubEnde" && !PRO_dtmUrlaubEnde.HasValue)
				{
					return "4_2121";
				}
				return string.Empty;
			}
		}

		[ImportingConstructor]
		public EDC_ZeitschaltuhrViewModel(INF_AutorisierungsDienst i_edcAutorisierungsDienst, INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			m_edcCapabilityProvider = i_edcCapabilityProvider;
			PRO_colNichtGeloeschteWochenuhrZeilen = new ObservableCollection<INF_WochenuhrZeile>();
			SUB_FuerUpdatesDerAktuellenPcZeitRegistrieren();
			SUB_WochentageInitialisieren();
			PRO_cmdPcZeitInSpsUebernehmen = new DelegateCommand(SUB_AktuellePcZeitInSpsUebernehmen);
			PRO_cmdWochenuhrZeileLoeschen = new DelegateCommand<INF_WochenuhrZeile>(SUB_WochenuhrZeileLoeschen);
			PRO_cmdWochenuhrZeileHinzufuegen = new DelegateCommand(SUB_WochenuhrZeileHinzufuegen, FUN_blnKannWochenuhrZeileHinzufuegen);
		}

		public void SUB_ViewModelInitialisieren()
		{
			PRO_edcZeitschaltuhrCapability = m_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_ZeitschaltuhrCapability>();
			PRO_edcZeitschaltuhrCapability.m_evtAenderungStattgefunden += delegate
			{
				SUB_Aktualisieren();
			};
			PRO_colNichtGeloeschteWochenuhrZeilen.AddRange(FUN_enuNichtGeloeschteZeilenErmitteln());
			PRO_strUrlaubStart = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart.ToShortDateString();
			PRO_strUrlaubEnde = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde.ToShortDateString();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			PRO_colNichtGeloeschteWochenuhrZeilen.Clear();
			PRO_colNichtGeloeschteWochenuhrZeilen.AddRange(FUN_enuNichtGeloeschteZeilenErmitteln());
			if (!PRO_dtmUrlaubStart.HasValue || PRO_dtmUrlaubStart.Value.Equals(EDC_Konstanten.ms_dtmDefaultDatumUndZeit))
			{
				PRO_strUrlaubStart = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart.ToShortDateString();
			}
			if (!PRO_dtmUrlaubEnde.HasValue || PRO_dtmUrlaubEnde.Value.Equals(EDC_Konstanten.ms_dtmDefaultDatumUndZeit))
			{
				PRO_strUrlaubEnde = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde.ToShortDateString();
			}
			RaisePropertyChanged("PRO_edcZeitschaltuhrCapability");
		}

		public override Task FUN_fdcAenderungenSpeichernAsync()
		{
			if (PRO_dtmUrlaubStart.HasValue)
			{
				PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart = PRO_dtmUrlaubStart.Value;
				PRO_strUrlaubStart = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart.ToShortDateString();
			}
			if (PRO_dtmUrlaubEnde.HasValue)
			{
				PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde = PRO_dtmUrlaubEnde.Value;
				PRO_strUrlaubEnde = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde.ToShortDateString();
			}
			PRO_edcZeitschaltuhrCapability.SUB_AenderungenSpeichern();
			SUB_Aktualisieren();
			return base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			PRO_edcZeitschaltuhrCapability.SUB_AenderungenVerwerfen();
			PRO_colNichtGeloeschteWochenuhrZeilen.Clear();
			PRO_colNichtGeloeschteWochenuhrZeilen.AddRange(FUN_enuNichtGeloeschteZeilenErmitteln());
			PRO_strUrlaubStart = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart.ToShortDateString();
			PRO_strUrlaubEnde = PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde.ToShortDateString();
			SUB_Aktualisieren();
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfZeitschaltuhrEditieren");
		}

		private void SUB_Aktualisieren()
		{
			SUB_AktualisiereZustand();
			PRO_cmdWochenuhrZeileHinzufuegen.RaiseCanExecuteChanged();
			RaisePropertyChanged("PRO_blnDarfZeitschaltuhrEditieren");
		}

		private void SUB_FuerUpdatesDerAktuellenPcZeitRegistrieren()
		{
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
			dispatcherTimer.Tick += delegate
			{
				PRO_sttAktuelleZeit = DateTime.Now;
			};
			dispatcherTimer.Start();
		}

		private void SUB_AktuellePcZeitInSpsUebernehmen()
		{
			PRO_edcZeitschaltuhrCapability.SUB_ZeitAufSpsSetzen(PRO_sttAktuelleZeit);
		}

		private void SUB_WochenuhrZeileLoeschen(INF_WochenuhrZeile i_edcWochenUhrZeile)
		{
			i_edcWochenUhrZeile.PRO_enmZustand = ENUM_WochenuhrZeileZustaende.enmGeloescht;
			PRO_colNichtGeloeschteWochenuhrZeilen.Remove(i_edcWochenUhrZeile);
			SUB_Aktualisieren();
		}

		private bool FUN_blnKannWochenuhrZeileHinzufuegen()
		{
			if (PRO_edcZeitschaltuhrCapability == null)
			{
				return false;
			}
			return PRO_colNichtGeloeschteWochenuhrZeilen.Count < PRO_edcZeitschaltuhrCapability.PRO_enuWochenuhrZeilen.Count();
		}

		private void SUB_WochenuhrZeileHinzufuegen()
		{
			INF_WochenuhrZeile iNF_WochenuhrZeile = PRO_edcZeitschaltuhrCapability.PRO_enuWochenuhrZeilen.First((INF_WochenuhrZeile i_edcZeile) => i_edcZeile.PRO_enmZustand == ENUM_WochenuhrZeileZustaende.enmGeloescht);
			iNF_WochenuhrZeile.PRO_enmZustand = ENUM_WochenuhrZeileZustaende.enmAus;
			iNF_WochenuhrZeile.PRO_enmWochentagEin = DayOfWeek.Sunday;
			iNF_WochenuhrZeile.PRO_enmWochentagAus = DayOfWeek.Sunday;
			iNF_WochenuhrZeile.PRO_sttVon = TimeSpan.Zero;
			iNF_WochenuhrZeile.PRO_sttBis = TimeSpan.Zero;
			PRO_colNichtGeloeschteWochenuhrZeilen.Add(iNF_WochenuhrZeile);
			SUB_Aktualisieren();
		}

		private IEnumerable<INF_WochenuhrZeile> FUN_enuNichtGeloeschteZeilenErmitteln()
		{
			return from i_edcZeile in PRO_edcZeitschaltuhrCapability.PRO_enuWochenuhrZeilen
			where i_edcZeile.PRO_enmZustand != ENUM_WochenuhrZeileZustaende.enmGeloescht
			select i_edcZeile;
		}

		private void SUB_WochentageInitialisieren()
		{
			PRO_enuWochentage = new List<EDC_Wochentag>
			{
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Sunday,
					PRO_strLocKey = "6_300"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Monday,
					PRO_strLocKey = "6_301"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Tuesday,
					PRO_strLocKey = "6_302"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Wednesday,
					PRO_strLocKey = "6_303"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Thursday,
					PRO_strLocKey = "6_304"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Friday,
					PRO_strLocKey = "6_305"
				},
				new EDC_Wochentag
				{
					PRO_enmWochentag = DayOfWeek.Saturday,
					PRO_strLocKey = "6_306"
				}
			};
		}

		private bool FUN_blnDatumsAenderungVorhanden()
		{
			if (!PRO_dtmUrlaubStart.HasValue || !PRO_dtmUrlaubEnde.HasValue)
			{
				return true;
			}
			if (PRO_dtmUrlaubStart.Value != PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubStart)
			{
				return true;
			}
			if (PRO_dtmUrlaubEnde.Value != PRO_edcZeitschaltuhrCapability.PRO_sttUrlaubEnde)
			{
				return true;
			}
			return false;
		}
	}
}
