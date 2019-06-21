using Ersa.AllgemeineEinstellungen.BetriebsmittelVerwaltung;
using Ersa.Global.Common;
using Ersa.Platform.Common.Data.Betriebsmittel;
using Ersa.Platform.DataDienste.BetriebsmittelVerwaltung.Interfaces;
using Ersa.Platform.Dienste.AuthentifizierungUndAutorisierung.Interfaces;
using Ersa.Platform.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	[Export]
	public class EDC_BetriebsmittelVerwaltungViewModel : EDC_NavigationsViewModel
	{
		private readonly EDC_AllgemeineEinstellungenController m_edcController;

		private readonly INF_BetriebsmittelVerwaltungsDienst m_edcBetriebsmittelVerwaltungsDienst;

		private readonly INF_AutorisierungsDienst m_edcAutorisierungsDienst;

		public DelegateCommand PRO_cmdFlussmittelHinzufuegen
		{
			get;
		}

		public DelegateCommand<EDC_Flussmittel> PRO_cmdFlussmittelLoeschen
		{
			get;
		}

		public DelegateCommand<EDC_Flussmittel> PRO_cmdFlussmittelAendern
		{
			get;
		}

		public IEnumerable<EDC_Flussmittel> PRO_lstAktiveFlussmittel => from i_edcFlussmittel in PRO_lstFlussmittel
		where !i_edcFlussmittel.PRO_blnIstGeloescht
		select i_edcFlussmittel;

		public override bool PRO_blnHatAenderung => PRO_lstFlussmittel.Any(delegate(EDC_Flussmittel i_edcFlussmittel)
		{
			if (!i_edcFlussmittel.PRO_blnHatAenderung && !i_edcFlussmittel.PRO_blnIstNeu)
			{
				return i_edcFlussmittel.PRO_blnIstGeloescht;
			}
			return true;
		});

		public bool PRO_blnKannSpeichern => PRO_blnHatAenderung;

		public bool PRO_blnDarfEinstellungenEditieren
		{
			get
			{
				if (!base.PRO_edcShellViewModel.PRO_blnIstMaschineInProduktion)
				{
					return m_edcAutorisierungsDienst.FUN_blnIstBenutzerAutorisiert("BerechtigungProduktionSteuern");
				}
				return false;
			}
		}

		public EDC_SmartObservableCollection<EDC_Flussmittel> PRO_lstFlussmittel
		{
			get;
		}

		[ImportingConstructor]
		public EDC_BetriebsmittelVerwaltungViewModel(EDC_AllgemeineEinstellungenController i_edcController, INF_BetriebsmittelVerwaltungsDienst i_edcBetriebsmittelVerwaltungsDienst, INF_AutorisierungsDienst i_edcAutorisierungsDienst)
		{
			m_edcController = i_edcController;
			m_edcBetriebsmittelVerwaltungsDienst = i_edcBetriebsmittelVerwaltungsDienst;
			m_edcAutorisierungsDienst = i_edcAutorisierungsDienst;
			PRO_cmdFlussmittelHinzufuegen = new DelegateCommand(SUB_FlussmittelHinzufuegen, () => PRO_blnDarfEinstellungenEditieren);
			PRO_cmdFlussmittelLoeschen = new DelegateCommand<EDC_Flussmittel>(SUB_FlussmittelLoeschen, FUN_blnKannFlussmittelEntfernenUndAendern);
			PRO_cmdFlussmittelAendern = new DelegateCommand<EDC_Flussmittel>(SUB_FlussmittelAendern, FUN_blnKannFlussmittelEntfernenUndAendern);
			PRO_lstFlussmittel = new EDC_SmartObservableCollection<EDC_Flussmittel>();
		}

		public override async Task FUN_fdcOnNavigatedToAsync(NavigationContext i_fdcNavigationContext)
		{
			await base.FUN_fdcOnNavigatedToAsync(i_fdcNavigationContext);
			IEnumerable<EDC_Flussmittel> i_enuElemente = await FUN_enuFlussmittelLadenAsync();
			PRO_lstFlussmittel.SUB_Reset(i_enuElemente);
			RaisePropertyChanged("PRO_lstAktiveFlussmittel");
		}

		public override async Task FUN_fdcAenderungenSpeichernAsync()
		{
			List<EDC_Flussmittel> list = (from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnIstNeu
			select i_edcFlussmittel).ToList();
			foreach (EDC_Flussmittel item in list)
			{
				item.SUB_AenderungenUebernehmen();
			}
			List<EDC_Flussmittel> lstGeaenderteFlussmittel = (from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnHatAenderung
			select i_edcFlussmittel).ToList();
			foreach (EDC_Flussmittel item2 in lstGeaenderteFlussmittel)
			{
				item2.SUB_AenderungenUebernehmen();
			}
			List<EDC_Flussmittel> lstGeloeschteFlussmittel = (from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnIstGeloescht
			select i_edcFlussmittel).ToList();
			if (list.Any())
			{
				await m_edcBetriebsmittelVerwaltungsDienst.FUN_fdcBetriebsmittelDatenSaetzeHinzufuegenAsync(FUN_enuKonvertiereNachBetriebsmittelData(list));
			}
			if (lstGeaenderteFlussmittel.Any())
			{
				await m_edcBetriebsmittelVerwaltungsDienst.FUN_fdcBetriebsmittelDatenSaetzeAendernAsync(FUN_enuKonvertiereNachBetriebsmittelData(lstGeaenderteFlussmittel));
			}
			if (lstGeloeschteFlussmittel.Any())
			{
				await m_edcBetriebsmittelVerwaltungsDienst.FUN_fdcBetriebsmittelDatenSaetzeLoeschenAsync(FUN_enuKonvertiereNachBetriebsmittelData(lstGeloeschteFlussmittel));
			}
			IEnumerable<EDC_Flussmittel> i_enuElemente = await FUN_enuFlussmittelLadenAsync();
			PRO_lstFlussmittel.SUB_Reset(i_enuElemente);
			SUB_SetzeOnPropertyChanged();
			await base.FUN_fdcAenderungenSpeichernAsync();
		}

		public override Task FUN_fdcAenderungenVerwerfenAsync()
		{
			foreach (EDC_Flussmittel item in (from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnIstNeu
			select i_edcFlussmittel).ToList())
			{
				PRO_lstFlussmittel.Remove(item);
			}
			foreach (EDC_Flussmittel item2 in from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnHatAenderung
			select i_edcFlussmittel)
			{
				item2.SUB_AenderungenVerwerfen();
			}
			foreach (EDC_Flussmittel item3 in from i_edcFlussmittel in PRO_lstFlussmittel
			where i_edcFlussmittel.PRO_blnIstGeloescht
			select i_edcFlussmittel)
			{
				item3.SUB_AenderungenVerwerfen();
			}
			SUB_SetzeOnPropertyChanged();
			return base.FUN_fdcAenderungenVerwerfenAsync();
		}

		protected override void SUB_BerechtigungenAuswerten()
		{
			base.SUB_BerechtigungenAuswerten();
			RaisePropertyChanged("PRO_blnDarfEinstellungenEditieren");
		}

		private void SUB_FlussmittelHinzufuegen()
		{
			EDC_Flussmittel edcFlussmittel = new EDC_Flussmittel();
			if (m_edcController.FUN_edcFlussmittelBearbeitenDialogAnzeigen(edcFlussmittel, (string i_strName, string i_strSpezifikation) => FUN_strNameundSpezifikationValidieren(i_strName, i_strSpezifikation, edcFlussmittel)))
			{
				PRO_lstFlussmittel.Add(edcFlussmittel);
				SUB_CommandsAktualisieren();
				SUB_SetzeOnPropertyChanged();
			}
		}

		private void SUB_FlussmittelLoeschen(EDC_Flussmittel i_edcFlussmittel)
		{
			if (i_edcFlussmittel != null)
			{
				if (i_edcFlussmittel.PRO_blnIstNeu)
				{
					PRO_lstFlussmittel.Remove(i_edcFlussmittel);
				}
				else
				{
					i_edcFlussmittel.PRO_blnIstGeloescht = true;
				}
				SUB_CommandsAktualisieren();
				SUB_SetzeOnPropertyChanged();
			}
		}

		private void SUB_FlussmittelAendern(EDC_Flussmittel i_edcFlussmittel)
		{
			if (m_edcController.FUN_edcFlussmittelBearbeitenDialogAnzeigen(i_edcFlussmittel, (string i_strName, string i_strSpezifikation) => FUN_strNameundSpezifikationValidieren(i_strName, i_strSpezifikation, i_edcFlussmittel)))
			{
				SUB_CommandsAktualisieren();
				SUB_SetzeOnPropertyChanged();
			}
			RaisePropertyChanged("PRO_blnHatAenderung");
			RaisePropertyChanged("PRO_blnKannSpeichern");
		}

		private void SUB_SetzeOnPropertyChanged()
		{
			RaisePropertyChanged("PRO_lstAktiveFlussmittel");
			RaisePropertyChanged("PRO_blnHatAenderung");
			RaisePropertyChanged("PRO_blnKannSpeichern");
		}

		private bool FUN_blnKannFlussmittelEntfernenUndAendern(EDC_Flussmittel i_edcFlussmittel)
		{
			if (i_edcFlussmittel == null)
			{
				return false;
			}
			return PRO_lstFlussmittel.Count > 0;
		}

		private void SUB_CommandsAktualisieren()
		{
			PRO_cmdFlussmittelLoeschen.RaiseCanExecuteChanged();
		}

		private async Task<IEnumerable<EDC_Flussmittel>> FUN_enuFlussmittelLadenAsync()
		{
			return (from i_edcFlussmittel in await m_edcBetriebsmittelVerwaltungsDienst.FUN_fdcLeseBetriebsmittelDatenFuerTypAsync(ENUM_BetriebsmittelTyp.Flussmittel)
			select new EDC_Flussmittel(i_edcFlussmittel.PRO_strName, i_edcFlussmittel.PRO_strSpezifikation)
			{
				PRO_i64Id = i_edcFlussmittel.PRO_i64BetriebsmittelId
			}).ToList();
		}

		private IEnumerable<EDC_BetriebsmittelData> FUN_enuKonvertiereNachBetriebsmittelData(IEnumerable<EDC_Flussmittel> i_enuFlussmittel)
		{
			return (from i_edcFlussmittelData in i_enuFlussmittel
			select new EDC_BetriebsmittelData
			{
				PRO_i64BetriebsmittelId = i_edcFlussmittelData.PRO_i64Id,
				PRO_enmTyp = ENUM_BetriebsmittelTyp.Flussmittel,
				PRO_strName = i_edcFlussmittelData.PRO_strName,
				PRO_strSpezifikation = i_edcFlussmittelData.PRO_strSpezifikation
			}).ToList();
		}

		private string FUN_strNameundSpezifikationValidieren(string i_strName, string i_strSpezifikation, EDC_Flussmittel i_edcFlussmittel)
		{
			string text = FUN_strTextValidieren(i_strName, 1, 100);
			if (!string.IsNullOrEmpty(i_strSpezifikation) && string.IsNullOrEmpty(text))
			{
				text = FUN_strTextValidieren(i_strSpezifikation, 0, 200);
			}
			if (string.IsNullOrEmpty(text) && PRO_lstAktiveFlussmittel.Any(delegate(EDC_Flussmittel i_edcEintrag)
			{
				if (i_edcEintrag.PRO_strName == i_strName)
				{
					return i_edcEintrag != i_edcFlussmittel;
				}
				return false;
			}))
			{
				text = base.PRO_edcLokalisierungsDienst.FUN_strText("11_1668");
			}
			return text;
		}

		private string FUN_strTextValidieren(string i_strText, int i_i32LaengeMin, int i_i32LaengeMax)
		{
			if (i_i32LaengeMin > 0 && (string.IsNullOrEmpty(i_strText) || i_strText.Length < i_i32LaengeMin))
			{
				return base.PRO_edcLokalisierungsDienst.FUN_strText("13_364");
			}
			if (i_strText.Length > i_i32LaengeMax)
			{
				return base.PRO_edcLokalisierungsDienst.FUN_strText("11_744");
			}
			return string.Empty;
		}
	}
}
