using Ersa.Global.Common;
using Ersa.Global.Common.Extensions;
using Ersa.Global.Controls.Extensions;
using Ersa.Platform.UI.Programm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ersa.Platform.UI.Programm
{
	public class EDU_ProgrammAuswahl : Control
	{
		public static readonly DependencyProperty PRO_lstBibliothekenProperty;

		public static readonly DependencyProperty PRO_strFilterTextProperty;

		public static readonly DependencyProperty PRO_blnBearbeitungErmoeglichenProperty;

		public static readonly DependencyProperty PRO_blnNurValideProgrammeAnzeigenProperty;

		public static readonly DependencyProperty PRO_blnProduktionsfreigabeAusblendenProperty;

		public static readonly DependencyProperty PRO_blnVersionenAnzeigenProperty;

		private ICollectionView m_fdcBibliothekenView;

		private ICollectionView m_fdcAktiveProgrammeView;

		private ICollectionView m_fdcAktiveVersionenView;

		public ObservableCollection<EDC_BibliothekViewModel> PRO_lstBibliotheken
		{
			get
			{
				return (ObservableCollection<EDC_BibliothekViewModel>)GetValue(PRO_lstBibliothekenProperty);
			}
			set
			{
				SetValue(PRO_lstBibliothekenProperty, value);
			}
		}

		public string PRO_strFilterText
		{
			get
			{
				return (string)GetValue(PRO_strFilterTextProperty);
			}
			set
			{
				SetValue(PRO_strFilterTextProperty, value);
			}
		}

		public bool PRO_blnBearbeitungErmoeglichen
		{
			get
			{
				return (bool)GetValue(PRO_blnBearbeitungErmoeglichenProperty);
			}
			set
			{
				SetValue(PRO_blnBearbeitungErmoeglichenProperty, value);
			}
		}

		public bool PRO_blnNurValideProgrammeAnzeigen
		{
			get
			{
				return (bool)GetValue(PRO_blnNurValideProgrammeAnzeigenProperty);
			}
			set
			{
				SetValue(PRO_blnNurValideProgrammeAnzeigenProperty, value);
			}
		}

		public bool PRO_blnProduktionsfreigabeAusblenden
		{
			get
			{
				return (bool)GetValue(PRO_blnProduktionsfreigabeAusblendenProperty);
			}
			set
			{
				SetValue(PRO_blnProduktionsfreigabeAusblendenProperty, value);
			}
		}

		public bool PRO_blnVersionenAnzeigen
		{
			get
			{
				return (bool)GetValue(PRO_blnVersionenAnzeigenProperty);
			}
			set
			{
				SetValue(PRO_blnVersionenAnzeigenProperty, value);
			}
		}

		static EDU_ProgrammAuswahl()
		{
			PRO_lstBibliothekenProperty = DependencyProperty.Register("PRO_lstBibliotheken", typeof(ObservableCollection<EDC_BibliothekViewModel>), typeof(EDU_ProgrammAuswahl), new FrameworkPropertyMetadata(SUB_OnBibliothekenChanged));
			PRO_strFilterTextProperty = DependencyProperty.Register("PRO_strFilterText", typeof(string), typeof(EDU_ProgrammAuswahl), new FrameworkPropertyMetadata(SUB_OnFilterTextChanged));
			PRO_blnBearbeitungErmoeglichenProperty = DependencyProperty.Register("PRO_blnBearbeitungErmoeglichen", typeof(bool), typeof(EDU_ProgrammAuswahl));
			PRO_blnNurValideProgrammeAnzeigenProperty = DependencyProperty.Register("PRO_blnNurValideProgrammeAnzeigen", typeof(bool), typeof(EDU_ProgrammAuswahl));
			PRO_blnProduktionsfreigabeAusblendenProperty = DependencyProperty.Register("PRO_blnProduktionsfreigabeAusblenden", typeof(bool), typeof(EDU_ProgrammAuswahl));
			PRO_blnVersionenAnzeigenProperty = DependencyProperty.Register("PRO_blnVersionenAnzeigen", typeof(bool), typeof(EDU_ProgrammAuswahl), new PropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_ProgrammAuswahl), new FrameworkPropertyMetadata(typeof(EDU_ProgrammAuswahl)));
		}

		private static void SUB_OnFilterTextChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_ProgrammAuswahl eDU_ProgrammAuswahl = i_objSender as EDU_ProgrammAuswahl;
			if (eDU_ProgrammAuswahl?.PRO_lstBibliotheken != null)
			{
				foreach (EDC_BibliothekViewModel item in eDU_ProgrammAuswahl.PRO_lstBibliotheken)
				{
					ListCollectionView listCollectionView = CollectionViewSource.GetDefaultView(item.PRO_lstProgramme) as ListCollectionView;
					if (listCollectionView == null)
					{
						break;
					}
					listCollectionView.Filter = eDU_ProgrammAuswahl.FUN_blnProgrammeFilter;
					if (!string.IsNullOrEmpty(eDU_ProgrammAuswahl.PRO_strFilterText))
					{
						EDC_BibliothekViewModel eDC_BibliothekViewModel = item;
						bool pRO_blnIstAusgeklappt = eDC_BibliothekViewModel.PRO_blnIstAusgeklappt;
						EDC_SmartObservableCollection<EDC_ProgrammViewModel> pRO_lstProgramme = item.PRO_lstProgramme;
						ListCollectionView listCollectionView2 = listCollectionView;
						eDC_BibliothekViewModel.PRO_blnIstAusgeklappt = (pRO_blnIstAusgeklappt | pRO_lstProgramme.Any(((CollectionView)listCollectionView2).PassesFilter));
					}
				}
			}
		}

		private static void SUB_OnBibliothekenChanged(DependencyObject i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_ProgrammAuswahl eDU_ProgrammAuswahl = i_objSender as EDU_ProgrammAuswahl;
			if (eDU_ProgrammAuswahl != null)
			{
				ObservableCollection<EDC_BibliothekViewModel> observableCollection = i_fdcArgs.NewValue as ObservableCollection<EDC_BibliothekViewModel>;
				if (observableCollection != null)
				{
					foreach (EDC_BibliothekViewModel item in observableCollection)
					{
						CollectionViewSource.GetDefaultView(item.PRO_lstProgramme).Filter = eDU_ProgrammAuswahl.FUN_blnProgrammeFilter;
					}
				}
				eDU_ProgrammAuswahl.SUB_BibliothekenViewErstellen();
			}
		}

		private void SUB_BibliothekenViewErstellen()
		{
			if (PRO_lstBibliotheken == null)
			{
				m_fdcBibliothekenView = CollectionViewSource.GetDefaultView(new List<EDC_BibliothekViewModel>());
				return;
			}
			m_fdcBibliothekenView = CollectionViewSource.GetDefaultView(PRO_lstBibliotheken);
			CurrentChangedEventManager.AddHandler(m_fdcBibliothekenView, SUB_OnBibliothekenCurrentChanged);
		}

		private void SUB_OnBibliothekenCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			SUB_VersionenViewFreigeben();
			SUB_ProgrammeViewFreigeben();
			EDC_BibliothekViewModel eDC_BibliothekViewModel = m_fdcBibliothekenView.CurrentItem as EDC_BibliothekViewModel;
			SUB_BibliothekStatusAktualisieren(eDC_BibliothekViewModel);
			if (eDC_BibliothekViewModel != null)
			{
				m_fdcAktiveProgrammeView = CollectionViewSource.GetDefaultView(eDC_BibliothekViewModel.PRO_lstProgramme);
				EDC_ProgrammViewModel eDC_ProgrammViewModel = m_fdcAktiveProgrammeView.CurrentItem as EDC_ProgrammViewModel;
				if (eDC_ProgrammViewModel != null)
				{
					m_fdcAktiveProgrammeView.MoveCurrentToPosition(-1);
					m_fdcAktiveProgrammeView.MoveCurrentTo(eDC_ProgrammViewModel);
				}
				else
				{
					m_fdcAktiveProgrammeView.MoveCurrentToFirst();
				}
				SUB_ProgrammeStatusAktualisieren(m_fdcAktiveProgrammeView.CurrentItem as EDC_ProgrammViewModel);
				CurrentChangedEventManager.AddHandler(m_fdcAktiveProgrammeView, SUB_OnProgrammeCurrentChanged);
			}
		}

		private void SUB_ProgrammeViewFreigeben()
		{
			if (m_fdcAktiveProgrammeView != null)
			{
				m_fdcAktiveProgrammeView.MoveCurrentToPosition(-1);
				CurrentChangedEventManager.RemoveHandler(m_fdcAktiveProgrammeView, SUB_OnProgrammeCurrentChanged);
			}
			m_fdcAktiveProgrammeView = null;
		}

		private void SUB_VersionenViewFreigeben()
		{
			if (m_fdcAktiveVersionenView != null)
			{
				m_fdcAktiveVersionenView.MoveCurrentToPosition(-1);
				CurrentChangedEventManager.RemoveHandler(m_fdcAktiveVersionenView, SUB_OnVersionenCurrentChanged);
			}
			m_fdcAktiveVersionenView = null;
		}

		private bool FUN_blnProgrammeFilter(object i_objListenObjekt)
		{
			EDC_ProgrammViewModel eDC_ProgrammViewModel = i_objListenObjekt as EDC_ProgrammViewModel;
			if (eDC_ProgrammViewModel == null)
			{
				return false;
			}
			if (PRO_blnNurValideProgrammeAnzeigen && eDC_ProgrammViewModel.PRO_blnIstFehlerhaft)
			{
				return false;
			}
			if (string.IsNullOrEmpty(PRO_strFilterText))
			{
				return true;
			}
			return FUN_blnFilterTextInProgrammInfoEnthalten(eDC_ProgrammViewModel, PRO_strFilterText);
		}

		private bool FUN_blnFilterTextInProgrammInfoEnthalten(EDC_ProgrammViewModel i_edcProgramm, string i_strFilterText)
		{
			if (!string.IsNullOrEmpty(i_edcProgramm.PRO_strName))
			{
				return i_edcProgramm.PRO_strName.FUN_blnContainsCaseInsensitive(i_strFilterText);
			}
			return false;
		}

		private void SUB_OnProgrammeCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			SUB_VersionenViewFreigeben();
			EDC_ProgrammViewModel eDC_ProgrammViewModel = m_fdcAktiveProgrammeView.CurrentItem as EDC_ProgrammViewModel;
			SUB_ProgrammeStatusAktualisieren(eDC_ProgrammViewModel);
			if (eDC_ProgrammViewModel != null)
			{
				m_fdcAktiveVersionenView = CollectionViewSource.GetDefaultView(eDC_ProgrammViewModel.PRO_lstVersionen);
				if (m_fdcAktiveVersionenView.CurrentItem == null)
				{
					m_fdcAktiveVersionenView.MoveCurrentToFirst();
				}
				SUB_VersionenStatusAktualisieren(m_fdcAktiveVersionenView.CurrentItem as EDC_VersionViewModel);
				CurrentChangedEventManager.AddHandler(m_fdcAktiveVersionenView, SUB_OnVersionenCurrentChanged);
			}
		}

		private void SUB_OnVersionenCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			SUB_VersionenStatusAktualisieren(m_fdcAktiveVersionenView.CurrentItem as EDC_VersionViewModel);
		}

		private void SUB_BibliothekStatusAktualisieren(EDC_BibliothekViewModel i_edcAusgewaehlteBibliothek)
		{
			if (PRO_lstBibliotheken != null)
			{
				foreach (EDC_BibliothekViewModel item in PRO_lstBibliotheken)
				{
					item.PRO_blnIstAusgewaehlt = (i_edcAusgewaehlteBibliothek != null && item.PRO_i64Id == i_edcAusgewaehlteBibliothek.PRO_i64Id);
				}
			}
		}

		private void SUB_ProgrammeStatusAktualisieren(EDC_ProgrammViewModel i_edcAusgewaehltesProgramm)
		{
			if (PRO_lstBibliotheken != null)
			{
				foreach (EDC_ProgrammViewModel item in PRO_lstBibliotheken.SelectMany((EDC_BibliothekViewModel i_edcBib) => i_edcBib.PRO_lstProgramme))
				{
					item.PRO_blnIstAusgewaehlt = (i_edcAusgewaehltesProgramm != null && item.PRO_i64Id == i_edcAusgewaehltesProgramm.PRO_i64Id);
				}
			}
			EDC_ProgrammCommands.ms_cmdProgrammAuswahlGeaendert.SUB_Execute(i_edcAusgewaehltesProgramm, this);
		}

		private void SUB_VersionenStatusAktualisieren(EDC_VersionViewModel i_edcAusgewaehlteVersion)
		{
			if (m_fdcAktiveProgrammeView != null)
			{
				foreach (EDC_VersionViewModel item in m_fdcAktiveProgrammeView.OfType<EDC_ProgrammViewModel>().SelectMany((EDC_ProgrammViewModel i_edcPrg) => i_edcPrg.PRO_lstVersionen))
				{
					item.PRO_blnIstAusgewaehlt = (i_edcAusgewaehlteVersion != null && item.PRO_i64Id == i_edcAusgewaehlteVersion.PRO_i64Id);
				}
			}
			EDC_ProgrammCommands.ms_cmdVersionAuswahlGeaendert.SUB_Execute(i_edcAusgewaehlteVersion, this);
		}
	}
}
