using Ersa.AllgemeineEinstellungen.BetriebsmittelVerwaltung;
using Ersa.Global.Common;
using Ersa.Global.Common.Extensions;
using Ersa.Global.Controls.Extensions;
using Ersa.Platform.CapabilityContracts.MaschinenDatenVerwaltung;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.Infrastructure.Interfaces;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_RuestkomponentenVerwaltungViewModel : EDC_NavigationsViewModel
	{
		private const string mC_strPropertyFilter = "PRO_blnGeloescht";

		private readonly string[] ma_strPropertiesSortierung = new string[2]
		{
			"PRO_strName",
			"PRO_i32AnzahlEintraege"
		};

		private readonly Lazy<INF_MaschinenBasisDatenCapability> m_edcBasisDatenCapability;

		private ICollectionView m_fdcTabellenView;

		private IList<long> m_lstLetzteMaschinenGruppenIds = new List<long>();

		[Import]
		public EDC_AllgemeineEinstellungenController PRO_edcAllgemeineEinstellungenController
		{
			get;
			set;
		}

		[Import]
		public INF_IoDialogHelfer PRO_edcIoDialogHelfer
		{
			get;
			set;
		}

		[Import]
		public INF_RuestkomponentenVerwaltungsDienst PRO_edcRuestkomponentenVerwaltungsDienst
		{
			get;
			set;
		}

		[Import]
		public INF_AutorisierungsDienst PRO_edcAutorisierungsDienst
		{
			get;
			set;
		}

		public EDC_SmartObservableCollection<EDC_NiederhaltergruppeViewModel> PRO_lstNiederhaltergruppen
		{
			get;
		}

		public ICommand PRO_cmdNiederhaltergruppeHinzufuegen
		{
			get;
		}

		public ICommand PRO_cmdNiederhaltergruppeUmbenennen
		{
			get;
		}

		public ICommand PRO_cmdNiederhaltergruppeLoeschen
		{
			get;
		}

		public ICommand PRO_cmdNiederhalterHinzufuegen
		{
			get;
		}

		public ICommand PRO_cmdNiederhalterBearbeiten
		{
			get;
		}

		public ICommand PRO_cmdNiederhalterLoeschen
		{
			get;
		}

		public bool PRO_blnDarfEinstellungenEditieren => PRO_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungLoetprogramm");

		public override bool PRO_blnHatAenderung => PRO_lstNiederhaltergruppen.Any((EDC_NiederhaltergruppeViewModel i_edcGruppen) => i_edcGruppen.PRO_blnHatAenderung);

		public bool PRO_blnKannSpeichern => PRO_blnHatAenderung;

		private EDC_NiederhaltergruppeViewModel PRO_edcAusgewaehlteKomponente => m_fdcTabellenView.CurrentItem as EDC_NiederhaltergruppeViewModel;

		[ImportingConstructor]
		public EDC_RuestkomponentenVerwaltungViewModel(IEventAggregator i_fdcEventAggregator, INF_CapabilityProvider i_edcCapabilityProvider)
		{
			m_edcBasisDatenCapability = new Lazy<INF_MaschinenBasisDatenCapability>(i_edcCapabilityProvider.FUN_edcCapabilityHolen<INF_MaschinenBasisDatenCapability>);
			PRO_lstNiederhaltergruppen = new EDC_SmartObservableCollection<EDC_NiederhaltergruppeViewModel>();
			PRO_cmdNiederhaltergruppeHinzufuegen = new DelegateCommand(SUB_NiederhaltergruppeHinzufuegen);
			PRO_cmdNiederhaltergruppeUmbenennen = new DelegateCommand(delegate
			{
				SUB_NiederhaltergruppeUmbenennen(PRO_edcAusgewaehlteKomponente);
			});
			PRO_cmdNiederhaltergruppeLoeschen = new DelegateCommand(delegate
			{
				SUB_NiederhaltergruppeLoeschen(PRO_edcAusgewaehlteKomponente);
			});
			PRO_cmdNiederhalterHinzufuegen = new DelegateCommand(delegate
			{
				SUB_NiederhalterHinzufuegenAsync(PRO_edcAusgewaehlteKomponente);
			});
			PRO_cmdNiederhalterBearbeiten = new DelegateCommand<EDC_NiederhalterViewModel>(SUB_NiederhalterBearbeiten);
			PRO_cmdNiederhalterLoeschen = new DelegateCommand<EDC_NiederhalterViewModel>(SUB_NiederhalterLoeschen);
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			if (m_fdcTabellenView == null)
			{
				SUB_CollectionViewInitialisieren();
			}
			List<long> list = (await m_edcBasisDatenCapability.Value.FUN_fdcHoleZugewieseneGruppenIdsAsync().ConfigureAwait(continueOnCapturedContext: true)).ToList();
			bool num = !list.FUN_blnIdentisch(m_lstLetzteMaschinenGruppenIds);
			m_lstLetzteMaschinenGruppenIds = list;
			if (num)
			{
				await FUN_fdcRuestkomponentenListeFuellenAsync().ConfigureAwait(continueOnCapturedContext: true);
			}
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			using (FUN_fdcFortschrittsAnzeigeEinblenden("4_10104"))
			{
				try
				{
					List<EDC_Ruestkomponente> list = new List<EDC_Ruestkomponente>();
					foreach (EDC_NiederhaltergruppeViewModel item in PRO_lstNiederhaltergruppen)
					{
						List<EDC_Ruestwerkzeug> list2 = new List<EDC_Ruestwerkzeug>();
						EDC_Ruestkomponente eDC_Ruestkomponente = EDC_RuestkomponentenUtils.FUN_edcModellKonvertieren(item);
						foreach (EDC_NiederhalterViewModel item2 in item.PRO_lstEintraege)
						{
							list2.Add(EDC_RuestkomponentenUtils.FUN_edcModellKonvertieren(item2));
						}
						eDC_Ruestkomponente.PRO_enuRuestwerkzeuge = list2;
						list.Add(eDC_Ruestkomponente);
					}
					SUB_RuestkomponentenListeAktualisierenAsync(await PRO_edcRuestkomponentenVerwaltungsDienst.FUN_fdcAktualisiereRuestkomponentenAsync(list));
				}
				catch (Exception i_fdcEx)
				{
					SUB_FehlerDialogAnzeigen("13_250", i_fdcEx);
				}
			}
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override async Task FUN_fdcAenderungenVerwerfenAsync()
		{
			using (FUN_fdcFortschrittsAnzeigeEinblenden("13_609"))
			{
				try
				{
					foreach (EDC_NiederhaltergruppeViewModel item in (from i_edcTabelle in PRO_lstNiederhaltergruppen
					where i_edcTabelle.PRO_blnIstNeu
					select i_edcTabelle).ToList())
					{
						PRO_lstNiederhaltergruppen.Remove(item);
					}
					foreach (EDC_NiederhaltergruppeViewModel item2 in PRO_lstNiederhaltergruppen)
					{
						EDC_CollectionExtensions.RemoveRange(i_objListToAdd: from i_edcEintrag in item2.PRO_lstEintraege
						where i_edcEintrag.PRO_blnIstNeu
						select i_edcEintrag, i_objCollection: item2.PRO_lstEintraege);
					}
					foreach (EDC_NiederhaltergruppeViewModel item3 in (from i_edcTabelle in PRO_lstNiederhaltergruppen
					where i_edcTabelle.PRO_blnHatAenderung
					select i_edcTabelle).ToList())
					{
						item3.SUB_AenderungenVerwerfen();
					}
					foreach (EDC_NiederhalterViewModel item4 in PRO_lstNiederhaltergruppen.SelectMany((EDC_NiederhaltergruppeViewModel i_edcTabelle) => i_edcTabelle.PRO_lstEintraege))
					{
						item4.SUB_AenderungenVerwerfen();
					}
				}
				catch (Exception i_fdcEx)
				{
					SUB_FehlerDialogAnzeigen("5_5", i_fdcEx);
				}
				await base.FUN_fdcAenderungenVerwerfenAsync();
			}
		}

		public override void SUB_AktualisiereZustand()
		{
			RaisePropertyChanged("PRO_blnHatAenderung");
			RaisePropertyChanged("PRO_blnKannSpeichern");
			base.SUB_AktualisiereZustand();
		}

		public void SUB_NiederhaltergruppeHinzufuegen()
		{
			string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("13_1104"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_1107"), null, base.PRO_edcLokalisierungsDienst.FUN_strText("13_1104"), null, i_blnReturnErlaubt: false, FUN_strRuestkomponentenNameEingabeValidieren);
			if (!string.IsNullOrEmpty(text))
			{
				PRO_lstNiederhaltergruppen.Add(new EDC_NiederhaltergruppeViewModel(string.Empty)
				{
					PRO_strName = text,
					PRO_enmTyp = ENUM_RuestkomponentenTyp.Niederhalter
				});
				SUB_AktualisiereZustand();
			}
		}

		public void SUB_NiederhaltergruppeUmbenennen(EDC_NiederhaltergruppeViewModel i_edcRuestkomponente)
		{
			if (i_edcRuestkomponente != null)
			{
				string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("13_1105"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_318"), i_edcRuestkomponente.PRO_strName, base.PRO_edcLokalisierungsDienst.FUN_strText("13_1105"), null, i_blnReturnErlaubt: false, FUN_strRuestkomponentenNameEingabeValidieren);
				if (!string.IsNullOrEmpty(text))
				{
					i_edcRuestkomponente.PRO_strName = text;
					SUB_AktualisiereZustand();
				}
			}
		}

		public void SUB_NiederhaltergruppeLoeschen(EDC_NiederhaltergruppeViewModel i_edcRuestkomponente)
		{
			if (i_edcRuestkomponente != null)
			{
				i_edcRuestkomponente.PRO_blnGeloescht = true;
				foreach (EDC_NiederhalterViewModel item in i_edcRuestkomponente.PRO_lstEintraege)
				{
					item.PRO_blnGeloescht = true;
				}
				SUB_AktualisiereZustand();
			}
		}

		public void SUB_NiederhalterHinzufuegenAsync(EDC_NiederhaltergruppeViewModel i_edcRuestkomponente)
		{
			EDC_NiederhaltergruppeViewModel eDC_NiederhaltergruppeViewModel = i_edcRuestkomponente ?? PRO_lstNiederhaltergruppen.FirstOrDefault();
			if (eDC_NiederhaltergruppeViewModel != null)
			{
				EDC_NiederhalterViewModel eDC_NiederhalterViewModel = new EDC_NiederhalterViewModel
				{
					PRO_blnIstNeu = true
				};
				string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("13_1108"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_1111"), null, base.PRO_edcLokalisierungsDienst.FUN_strText("13_1108"), null, i_blnReturnErlaubt: false, FUN_strRuestwerkzeugIdentifikationEingabeValidieren);
				if (text != null)
				{
					EDC_RuestwerkzeugeData i_edcWerkzeugeintrag = new EDC_RuestwerkzeugeData
					{
						PRO_i64RuestkomponentenId = eDC_NiederhaltergruppeViewModel.PRO_i64RuestkomponentenId,
						PRO_strIdentifikation = text
					};
					eDC_NiederhalterViewModel.SUB_WerkzeugEintragSetzen(i_edcWerkzeugeintrag);
					eDC_NiederhaltergruppeViewModel = PRO_lstNiederhaltergruppen.Single((EDC_NiederhaltergruppeViewModel i_edcTabelle) => i_edcTabelle.PRO_blnIstAusgewaehlt);
					eDC_NiederhaltergruppeViewModel.PRO_lstEintraege.Add(eDC_NiederhalterViewModel);
					SUB_AktualisiereZustand();
				}
			}
		}

		public void SUB_NiederhalterBearbeiten(EDC_NiederhalterViewModel i_edcRuestwerkzeugEintrag)
		{
			if (i_edcRuestwerkzeugEintrag != null && PRO_lstNiederhaltergruppen.SingleOrDefault((EDC_NiederhaltergruppeViewModel i_edcTabelle) => i_edcTabelle.PRO_lstEintraege.Contains(i_edcRuestwerkzeugEintrag)) != null)
			{
				string text = base.PRO_edcInteractionController.FUN_strTextEingabeAbfrageAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText("13_1109"), base.PRO_edcLokalisierungsDienst.FUN_strText("13_1111"), i_edcRuestwerkzeugEintrag.PRO_strIdentifikation, base.PRO_edcLokalisierungsDienst.FUN_strText("13_1109"), null, i_blnReturnErlaubt: false, FUN_strRuestwerkzeugIdentifikationEingabeValidieren);
				if (!string.IsNullOrEmpty(text))
				{
					i_edcRuestwerkzeugEintrag.PRO_strIdentifikation = text;
					SUB_AktualisiereZustand();
				}
			}
		}

		public void SUB_NiederhalterLoeschen(EDC_NiederhalterViewModel i_edcRuestwerkzeugEintrag)
		{
			if (i_edcRuestwerkzeugEintrag != null)
			{
				i_edcRuestwerkzeugEintrag.PRO_blnGeloescht = true;
				SUB_AktualisiereZustand();
			}
		}

		public async Task FUN_fdcRuestkomponentenListeFuellenAsync()
		{
			using (FUN_fdcFortschrittsAnzeigeEinblenden("13_609"))
			{
				List<EDC_NiederhaltergruppeViewModel> i_enuElemente = EDC_RuestkomponentenUtils.FUN_enuModellKonvertieren(await PRO_edcRuestkomponentenVerwaltungsDienst.FUN_fdcLeseRuestkomponenteFuerTypAsync(ENUM_RuestkomponentenTyp.Niederhalter).ConfigureAwait(continueOnCapturedContext: true)).ToList();
				PRO_lstNiederhaltergruppen.SUB_Reset(i_enuElemente);
				SUB_AktualisiereZustand();
			}
		}

		public void SUB_RuestkomponentenListeAktualisierenAsync(IEnumerable<EDC_Ruestkomponente> i_enuKomponenten)
		{
			List<EDC_Ruestkomponente> source = i_enuKomponenten.ToList();
			List<EDC_NiederhaltergruppeViewModel> list = new List<EDC_NiederhaltergruppeViewModel>();
			foreach (EDC_NiederhaltergruppeViewModel edcNiederhaltergruppeViewModel in PRO_lstNiederhaltergruppen)
			{
				EDC_Ruestkomponente eDC_Ruestkomponente = source.FirstOrDefault((EDC_Ruestkomponente i_edcKomponente) => i_edcKomponente.PRO_edcRuestkomponenteData.PRO_strName == edcNiederhaltergruppeViewModel.PRO_strName);
				if (eDC_Ruestkomponente == null)
				{
					list.Add(edcNiederhaltergruppeViewModel);
				}
				else
				{
					edcNiederhaltergruppeViewModel.PRO_strOriginalName = eDC_Ruestkomponente.PRO_edcRuestkomponenteData.PRO_strName;
					if (edcNiederhaltergruppeViewModel.PRO_i64RuestkomponentenId == 0L)
					{
						edcNiederhaltergruppeViewModel.PRO_i64RuestkomponentenId = eDC_Ruestkomponente.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId;
						edcNiederhaltergruppeViewModel.PRO_i64MaschinenGruppenId = eDC_Ruestkomponente.PRO_edcRuestkomponenteData.PRO_i64MachinenGruppenId;
					}
					List<EDC_NiederhalterViewModel> list2 = new List<EDC_NiederhalterViewModel>();
					foreach (EDC_NiederhalterViewModel edcNiederhalterViewModel in edcNiederhaltergruppeViewModel.PRO_lstEintraege)
					{
						EDC_Ruestwerkzeug eDC_Ruestwerkzeug = eDC_Ruestkomponente.PRO_enuRuestwerkzeuge.FirstOrDefault((EDC_Ruestwerkzeug i_edcWerkzeug) => i_edcWerkzeug.PRO_edcRuestwerkzeugeData.PRO_strIdentifikation == edcNiederhalterViewModel.PRO_strIdentifikation);
						if (eDC_Ruestwerkzeug == null)
						{
							list2.Add(edcNiederhalterViewModel);
						}
						else
						{
							edcNiederhalterViewModel.PRO_strOriginalIdentifikation = eDC_Ruestwerkzeug.PRO_edcRuestwerkzeugeData.PRO_strIdentifikation;
							if (edcNiederhalterViewModel.PRO_i64RuestwerkzeugId == 0L)
							{
								edcNiederhalterViewModel.PRO_blnIstNeu = false;
								edcNiederhalterViewModel.PRO_i64RuestwerkzeugId = eDC_Ruestwerkzeug.PRO_edcRuestwerkzeugeData.PRO_i64RuestwerkzeugId;
								edcNiederhalterViewModel.PRO_i64RuestkomponentenId = eDC_Ruestkomponente.PRO_edcRuestkomponenteData.PRO_i64RuestkomponentenId;
							}
						}
					}
					if (list2.Any())
					{
						edcNiederhaltergruppeViewModel.PRO_lstEintraege.RemoveRange(list2);
					}
				}
			}
			if (list.Any())
			{
				PRO_lstNiederhaltergruppen.SUB_RemoveRange(list);
			}
			SUB_AktualisiereZustand();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfEinstellungenEditieren");
		}

		private void SUB_CollectionViewInitialisieren()
		{
			m_fdcTabellenView = CollectionViewSource.GetDefaultView(PRO_lstNiederhaltergruppen);
			m_fdcTabellenView.CurrentChanged += SUB_OnCurrentChanged;
			m_fdcTabellenView.SortDescriptions.Add(new SortDescription(ma_strPropertiesSortierung.FirstOrDefault(), ListSortDirection.Ascending));
			m_fdcTabellenView.SUB_LiveFilteringAktivieren("PRO_blnGeloescht");
			m_fdcTabellenView.SUB_LiveSortingAktivieren(ma_strPropertiesSortierung);
			m_fdcTabellenView.Filter = FUN_blnTabelleFilter;
		}

		private void SUB_OnCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			foreach (EDC_NiederhaltergruppeViewModel item in PRO_lstNiederhaltergruppen)
			{
				item.PRO_blnIstAusgewaehlt = (item == PRO_edcAusgewaehlteKomponente);
				if (!item.PRO_blnIstAusgewaehlt)
				{
					CollectionViewSource.GetDefaultView(item.PRO_lstEintraege).MoveCurrentToPosition(-1);
				}
			}
		}

		private bool FUN_blnTabelleFilter(object i_objElement)
		{
			EDC_NiederhaltergruppeViewModel eDC_NiederhaltergruppeViewModel = i_objElement as EDC_NiederhaltergruppeViewModel;
			if (eDC_NiederhaltergruppeViewModel != null)
			{
				return !eDC_NiederhaltergruppeViewModel.PRO_blnGeloescht;
			}
			return false;
		}

		private void SUB_FehlerDialogAnzeigen(string i_strTitelKey, Exception i_fdcEx)
		{
			base.PRO_edcInteractionController.SUB_OkHinweisAnzeigen(base.PRO_edcLokalisierungsDienst.FUN_strText(i_strTitelKey), base.PRO_edcLokalisierungsDienst.FUN_strText("13_252") + ": " + i_fdcEx.Message);
		}

		private string FUN_strRuestkomponentenNameEingabeValidieren(string i_strRuestkomponentenName)
		{
			if (PRO_lstNiederhaltergruppen.Any(delegate(EDC_NiederhaltergruppeViewModel i_edcGruppe)
			{
				if (!i_edcGruppe.PRO_blnGeloescht)
				{
					return i_edcGruppe.PRO_strName == i_strRuestkomponentenName;
				}
				return false;
			}))
			{
				return base.PRO_edcLokalisierungsDienst.FUN_strText("13_1112");
			}
			return string.Empty;
		}

		private string FUN_strRuestwerkzeugIdentifikationEingabeValidieren(string i_strIdentifikation)
		{
			if (PRO_lstNiederhaltergruppen.Any((EDC_NiederhaltergruppeViewModel i_edcGruppe) => i_edcGruppe.PRO_lstEintraege.Any((EDC_NiederhalterViewModel i_edcEintrag) => i_edcEintrag.PRO_strIdentifikation == i_strIdentifikation)))
			{
				return base.PRO_edcLokalisierungsDienst.FUN_strText("13_1113");
			}
			return string.Empty;
		}

		private IDisposable FUN_fdcFortschrittsAnzeigeEinblenden(string i_strTextKey)
		{
			return FUN_fdcFortschrittsAnzeigeEinblenden(i_blnUeberdeckendeAnzeige: true, base.PRO_edcLokalisierungsDienst.FUN_strText(i_strTextKey, "..."));
		}
	}
}
