using Ersa.Global.Common;
using Ersa.Global.Mvvm;
using Ersa.Global.Mvvm.Commands;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Common.Loetprogramm;
using Ersa.Platform.Common.Produktionssteuerung;
using Ersa.Platform.DataDienste.Loetprogramm.Interfaces;
using Ersa.Platform.DataDienste.Produktionssteuerung.Interfaces;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.UI.Programm.Helfer;
using Ersa.Platform.UI.Programm.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ersa.Platform.UI.ViewModels
{
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class EDC_ProgrammAuswahlViewModel : BindableBase
	{
		protected readonly SemaphoreSlim ms_fdcSemaphore = new SemaphoreSlim(1);

		private readonly INF_LoetprogrammVerwaltungsDienst m_edcLoetprogrammVerwaltungsDienst;

		private readonly INF_ProduktionssteuerungsDienst m_edcProduktionssteuerungsDienst;

		private IEnumerable<EDC_BibliothekViewModel> m_enuBibliotheken;

		private ICollectionView m_fdcBibliothekenView;

		private EDC_ProgrammViewModel m_edcAusgewaehltesProgramm;

		private Action m_delAuswahlGeaendertAktion;

		private ENUM_LoetprogrammFreigabeArt m_enmFreigabeArt;

		public EDC_SmartObservableCollection<EDC_BibliothekViewModel> PRO_lstBibliotheken
		{
			get;
		}

		public DelegateCommand<EDC_ProgrammViewModel> PRO_cmdProgrammAuswahlGeaendert
		{
			get;
		}

		public AsyncCommand<object> PRO_cmdElementAusgeklapptGeaendert
		{
			get;
		}

		public EDC_ProgrammViewModel PRO_edcAusgewaehltesProgramm
		{
			get
			{
				return m_edcAusgewaehltesProgramm;
			}
			set
			{
				if (SetProperty(ref m_edcAusgewaehltesProgramm, value, "PRO_edcAusgewaehltesProgramm"))
				{
					RaisePropertyChanged("PRO_blnHatAenderung");
					m_delAuswahlGeaendertAktion?.Invoke();
				}
			}
		}

		public EDC_BibliothekViewModel PRO_edcAusgewaehlteBibliothek => m_fdcBibliothekenView.CurrentItem as EDC_BibliothekViewModel;

		public bool PRO_blnHatAenderung => PRO_edcAusgewaehltesProgramm != null;

		[ImportingConstructor]
		public EDC_ProgrammAuswahlViewModel(INF_LoetprogrammVerwaltungsDienst i_edcLoetprogrammVerwaltungsDienst, INF_ProduktionssteuerungsDienst i_edcProduktionssteuerungsDienst)
		{
			m_edcLoetprogrammVerwaltungsDienst = i_edcLoetprogrammVerwaltungsDienst;
			m_edcProduktionssteuerungsDienst = i_edcProduktionssteuerungsDienst;
			PRO_lstBibliotheken = new EDC_SmartObservableCollection<EDC_BibliothekViewModel>();
			PRO_edcAusgewaehltesProgramm = null;
			PRO_cmdProgrammAuswahlGeaendert = new DelegateCommand<EDC_ProgrammViewModel>(delegate(EDC_ProgrammViewModel i_edcPrg)
			{
				PRO_edcAusgewaehltesProgramm = i_edcPrg;
			});
			PRO_cmdElementAusgeklapptGeaendert = new AsyncCommand<object>(FUN_fdcElementAusgeklapptGeaendertAsync);
			SUB_ProgrammCollectionAufbauen();
		}

		public void SUB_AuswahlZuruecksetzen()
		{
			try
			{
				ms_fdcSemaphore.Wait();
				if (PRO_edcAusgewaehlteBibliothek != null)
				{
					CollectionViewSource.GetDefaultView(PRO_edcAusgewaehlteBibliothek.PRO_lstProgramme).MoveCurrentToPosition(-1);
					PRO_edcAusgewaehltesProgramm = null;
					m_fdcBibliothekenView.MoveCurrentToPosition(-1);
				}
			}
			finally
			{
				ms_fdcSemaphore.Release(1);
			}
		}

		public Task FUN_fdcProgrammSetzenAsync(long i_i64ProgrammId)
		{
			return FUN_fdcProgrammAuswahlSetzenAsync(i_i64ProgrammId);
		}

		public async Task FUN_fdcInitialisierenAsync(Action i_delAuswahlGeaendertAktion = null, bool i_blnArbeitsversionenVerwenden = false, string i_strProgrammeSuchbegriff = null)
		{
			m_delAuswahlGeaendertAktion = i_delAuswahlGeaendertAktion;
			EDC_BibliothekViewModel[] a_edcBibs = (await FUN_fdcLadeBibliothekenAsync(i_blnArbeitsversionenVerwenden, i_strProgrammeSuchbegriff).ConfigureAwait(continueOnCapturedContext: true)).ToArray();
			bool blnBibsAusklappen = !string.IsNullOrEmpty(i_strProgrammeSuchbegriff);
			if (blnBibsAusklappen)
			{
				EDC_BibliothekViewModel[] array = a_edcBibs;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].PRO_blnIstAusgeklappt = true;
				}
				await Task.WhenAll(from i_edcBib in a_edcBibs
				select i_edcBib.FUN_fdcProgrammeLadenWennNoetigAsync()).ConfigureAwait(continueOnCapturedContext: true);
			}
			await FUN_fdcProgrammCollectionNeuFuellenAsync(a_edcBibs, blnBibsAusklappen).ConfigureAwait(continueOnCapturedContext: true);
			EDC_ProduktionsEinstellungen eDC_ProduktionsEinstellungen = (await m_edcProduktionssteuerungsDienst.FUN_edcAktiveProduktionssteuerungsDatenLadenAsync())?.PRO_edcProduktionsEinstellungen;
			if (eDC_ProduktionsEinstellungen != null)
			{
				m_enmFreigabeArt = (eDC_ProduktionsEinstellungen.PRO_blnVierAugenFreigabeAktiv ? ENUM_LoetprogrammFreigabeArt.Zweistufig : ENUM_LoetprogrammFreigabeArt.Einstufig);
			}
			else
			{
				m_enmFreigabeArt = ENUM_LoetprogrammFreigabeArt.Einstufig;
			}
		}

		public async Task<IEnumerable<EDC_BibliothekViewModel>> FUN_fdcLadeBibliothekenAsync(bool i_blnArbeitsversionenVerwenden, string i_strProgrammeSuchbegriff = null)
		{
			IEnumerable<EDC_BibliothekInfo> source = from i_edcBib in await m_edcLoetprogrammVerwaltungsDienst.FUN_fdcBibliothekenAuslesenAsync(i_strProgrammeSuchbegriff).ConfigureAwait(continueOnCapturedContext: true)
			where i_edcBib.PRO_lstProgramme.Any(delegate(EDC_ProgrammInfo i_edcPrg)
			{
				if (!i_blnArbeitsversionenVerwenden)
				{
					return FUN_blnHatProgrammFehlerfreieFreigabe(i_edcPrg);
				}
				return FUN_blnHatProgrammFehlerfreieArbeitsversion(i_edcPrg);
			})
			select i_edcBib;
			m_enuBibliotheken = (from i_edcBib in source
			select EDC_ElementViewModelHelfer.FUN_edcBibliothekKonvertieren(i_edcBib, m_edcLoetprogrammVerwaltungsDienst, m_enmFreigabeArt, () => i_strProgrammeSuchbegriff, !i_blnArbeitsversionenVerwenden, i_blnArbeitsversionenVerwenden)).ToArray();
			return m_enuBibliotheken;
		}

		public Task FUN_fdcProgrammeFilternAsync(Action i_delAuswahlGeaendertAktion = null, bool i_blnArbeitsversionenVerwenden = false, string i_strProgrammeSuchbegriff = null)
		{
			m_delAuswahlGeaendertAktion = i_delAuswahlGeaendertAktion;
			return FUN_fdcInitialisierenAsync(i_delAuswahlGeaendertAktion, i_blnArbeitsversionenVerwenden, i_strProgrammeSuchbegriff);
		}

		public void SUB_ZuBibMitPrgScrollen(EDC_BibliothekViewModel i_edcBib, EDC_ProgrammViewModel i_edcPrg)
		{
			try
			{
				ms_fdcSemaphore.Wait();
				m_fdcBibliothekenView.MoveCurrentTo(i_edcBib);
				CollectionViewSource.GetDefaultView(i_edcBib.PRO_lstProgramme).MoveCurrentTo(i_edcPrg);
			}
			finally
			{
				ms_fdcSemaphore.Release(1);
			}
		}

		private void SUB_ProgrammCollectionAufbauen()
		{
			try
			{
				ms_fdcSemaphore.Wait();
				m_fdcBibliothekenView = CollectionViewSource.GetDefaultView(PRO_lstBibliotheken);
				m_fdcBibliothekenView.SortDescriptions.Add(new SortDescription("PRO_strName", ListSortDirection.Ascending));
				m_fdcBibliothekenView.MoveCurrentToPosition(-1);
			}
			finally
			{
				ms_fdcSemaphore.Release(1);
			}
		}

		private async Task FUN_fdcProgrammCollectionNeuFuellenAsync(IEnumerable<EDC_BibliothekViewModel> i_enuBibliotheken, bool i_blnBibsAusgeklapptLassen)
		{
			try
			{
				await ms_fdcSemaphore.WaitAsync();
				await EDC_Dispatch.FUN_fdcAwaitableAktion(delegate
				{
					EDC_BibliothekViewModel[] array = i_enuBibliotheken.ToArray();
					EDC_BibliothekViewModel[] array2 = array;
					foreach (EDC_BibliothekViewModel eDC_BibliothekViewModel in array2)
					{
						if (!i_blnBibsAusgeklapptLassen)
						{
							eDC_BibliothekViewModel.PRO_blnIstAusgeklappt = false;
						}
						eDC_BibliothekViewModel.PRO_blnIstAusgewaehlt = false;
						foreach (EDC_ProgrammViewModel item in eDC_BibliothekViewModel.PRO_lstProgramme)
						{
							item.PRO_blnIstAusgewaehlt = false;
						}
					}
					m_fdcBibliothekenView.MoveCurrentToPosition(-1);
					PRO_lstBibliotheken.SUB_Reset(array);
				}).ConfigureAwait(continueOnCapturedContext: true);
			}
			finally
			{
				ms_fdcSemaphore.Release(1);
			}
		}

		private bool FUN_blnHatProgrammFehlerfreieFreigabe(EDC_ProgrammInfo i_edcProgInfo)
		{
			if (!i_edcProgInfo.PRO_blnIstFehlerhaft)
			{
				return i_edcProgInfo.PROa_enmStatus.Contains(ENUM_LoetprogrammStatus.Freigegeben);
			}
			return false;
		}

		private bool FUN_blnHatProgrammFehlerfreieArbeitsversion(EDC_ProgrammInfo i_edcProgInfo)
		{
			if (!i_edcProgInfo.PROa_enmStatus.Contains(ENUM_LoetprogrammStatus.Arbeitsversion))
			{
				return false;
			}
			if (i_edcProgInfo.PROa_enmStatus.Length != i_edcProgInfo.PROa_enmFehlerhaft.Length)
			{
				return false;
			}
			int num = i_edcProgInfo.PROa_enmStatus.ToList().IndexOf(ENUM_LoetprogrammStatus.Arbeitsversion);
			return !i_edcProgInfo.PROa_enmFehlerhaft[num];
		}

		private async Task FUN_fdcProgrammAuswahlSetzenAsync(long i_i64ProgrammId)
		{
			if (i_i64ProgrammId == 0L)
			{
				return;
			}
			_003C_003Ec__DisplayClass35_0 _003C_003Ec__DisplayClass35_;
			EDC_ProgrammInfo edcProgrammInfo2 = _003C_003Ec__DisplayClass35_.edcProgrammInfo;
			EDC_ProgrammInfo edcProgrammInfo;
			EDC_ProgrammInfo eDC_ProgrammInfo = edcProgrammInfo = await m_edcLoetprogrammVerwaltungsDienst.FUN_fdcProgrammInfoLesenAsync(i_i64ProgrammId);
			if (edcProgrammInfo == null)
			{
				return;
			}
			PRO_edcAusgewaehltesProgramm = EDC_ElementViewModelHelfer.FUN_edcProgrammKonvertieren(edcProgrammInfo, m_edcLoetprogrammVerwaltungsDienst, m_enmFreigabeArt);
			EDC_BibliothekViewModel edcBibMitPrg = PRO_lstBibliotheken.FirstOrDefault((EDC_BibliothekViewModel i_edcBib) => i_edcBib.PRO_i64Id == edcProgrammInfo.PRO_i64BibId);
			if (edcBibMitPrg != null)
			{
				edcBibMitPrg.PRO_blnIstAusgewaehlt = true;
				edcBibMitPrg.PRO_blnIstAusgeklappt = true;
				await edcBibMitPrg.FUN_fdcProgrammeLadenWennNoetigAsync();
				EDC_ProgrammViewModel eDC_ProgrammViewModel = edcBibMitPrg.PRO_lstProgramme.FirstOrDefault((EDC_ProgrammViewModel i_edcPrg) => i_edcPrg.PRO_i64Id == i_i64ProgrammId);
				if (eDC_ProgrammViewModel != null)
				{
					eDC_ProgrammViewModel.PRO_blnIstAusgewaehlt = true;
				}
			}
		}

		private async Task FUN_fdcElementAusgeklapptGeaendertAsync(object i_objElement)
		{
			EDC_BibliothekViewModel eDC_BibliothekViewModel = i_objElement as EDC_BibliothekViewModel;
			if (eDC_BibliothekViewModel != null)
			{
				await FUN_fdcBibliothekAusgeklapptGeaendertBehandelnAsync(eDC_BibliothekViewModel);
			}
		}

		private async Task FUN_fdcBibliothekAusgeklapptGeaendertBehandelnAsync(EDC_BibliothekViewModel i_edcBib)
		{
			i_edcBib.PRO_blnIstAusgeklappt = !i_edcBib.PRO_blnIstAusgeklappt;
			if (i_edcBib.PRO_blnIstAusgeklappt)
			{
				await i_edcBib.FUN_fdcProgrammeLadenWennNoetigAsync();
			}
		}
	}
}
