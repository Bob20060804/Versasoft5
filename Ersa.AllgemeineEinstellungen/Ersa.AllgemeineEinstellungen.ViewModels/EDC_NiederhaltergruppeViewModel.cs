using Ersa.Global.Controls.Extensions;
using Ersa.Global.Mvvm;
using Ersa.Platform.Common.Data.Betriebsmittel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_NiederhaltergruppeViewModel : BindableBase
	{
		private const string mC_strPropertyInitialeSortierung = "PRO_strIdentifikation";

		private const string mC_strPropertyFilter = "PRO_blnGeloescht";

		private readonly string[] ma_strPropertiesSortierung = new string[1]
		{
			"PRO_strIdentifikation"
		};

		private string m_strOriginalName;

		private string m_strName;

		private bool m_blnIstAusgewaehlt;

		private bool m_blnGeloescht;

		private ICollectionView m_fdcEintraegeView;

		private bool m_blnIstAusgeklappt;

		public ObservableCollection<EDC_NiederhalterViewModel> PRO_lstEintraege
		{
			get;
			set;
		}

		public long PRO_i64RuestkomponentenId
		{
			get;
			set;
		}

		public ENUM_RuestkomponentenTyp PRO_enmTyp
		{
			get;
			set;
		}

		public long PRO_i64MaschinenGruppenId
		{
			get;
			set;
		}

		public string PRO_strOriginalName
		{
			get
			{
				return m_strOriginalName;
			}
			set
			{
				m_strOriginalName = value;
			}
		}

		public string PRO_strName
		{
			get
			{
				return m_strName ?? m_strOriginalName;
			}
			set
			{
				SetProperty(ref m_strName, value, "PRO_strName");
			}
		}

		public bool PRO_blnIstAusgewaehlt
		{
			get
			{
				return m_blnIstAusgewaehlt;
			}
			set
			{
				SetProperty(ref m_blnIstAusgewaehlt, value, "PRO_blnIstAusgewaehlt");
			}
		}

		public int PRO_i32AnzahlEintraege => PRO_lstEintraege.Count((EDC_NiederhalterViewModel i_edcEintrag) => !i_edcEintrag.PRO_blnGeloescht);

		public bool PRO_blnIstNeu => string.IsNullOrEmpty(m_strOriginalName);

		public bool PRO_blnGeloescht
		{
			get
			{
				return m_blnGeloescht;
			}
			set
			{
				SetProperty(ref m_blnGeloescht, value, "PRO_blnGeloescht");
			}
		}

		public bool PRO_blnHatAenderung
		{
			get
			{
				if (!PRO_blnIstNeu && !(PRO_strName != m_strOriginalName) && !PRO_blnGeloescht)
				{
					return PRO_lstEintraege.Any((EDC_NiederhalterViewModel i_edcEintrag) => i_edcEintrag.PRO_blnHatAenderung);
				}
				return true;
			}
		}

		public bool PRO_blnIstAusgeklappt
		{
			get
			{
				return m_blnIstAusgeklappt;
			}
			set
			{
				SetProperty(ref m_blnIstAusgeklappt, value, "PRO_blnIstAusgeklappt");
			}
		}

		public EDC_NiederhaltergruppeViewModel(string i_strName)
		{
			PRO_lstEintraege = new ObservableCollection<EDC_NiederhalterViewModel>();
			SUB_CollectionViewInitialisieren();
			m_strOriginalName = i_strName;
		}

		public void SUB_AenderungenUebernehmen()
		{
			m_strOriginalName = PRO_strName;
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
		}

		public void SUB_AenderungenVerwerfen()
		{
			PRO_blnGeloescht = false;
			m_strName = null;
			RaisePropertyChanged("PRO_strName");
		}

		private void SUB_CollectionViewInitialisieren()
		{
			m_fdcEintraegeView = CollectionViewSource.GetDefaultView(PRO_lstEintraege);
			m_fdcEintraegeView.CurrentChanged += SUB_OnCurrentChanged;
			CollectionChangedEventManager.AddHandler(m_fdcEintraegeView, delegate
			{
				RaisePropertyChanged("PRO_i32AnzahlEintraege");
			});
			m_fdcEintraegeView.SUB_LiveFilteringAktivieren("PRO_blnGeloescht");
			m_fdcEintraegeView.SUB_LiveSortingAktivieren(ma_strPropertiesSortierung);
			m_fdcEintraegeView.Filter = FUN_blnVorlageFilter;
			m_fdcEintraegeView.SortDescriptions.Add(new SortDescription("PRO_strIdentifikation", ListSortDirection.Ascending));
		}

		private void SUB_OnCurrentChanged(object i_objSender, EventArgs i_fdcArgs)
		{
			EDC_NiederhalterViewModel eDC_NiederhalterViewModel = m_fdcEintraegeView.CurrentItem as EDC_NiederhalterViewModel;
			foreach (EDC_NiederhalterViewModel item in PRO_lstEintraege)
			{
				item.PRO_blnIstAusgewaehlt = (item == eDC_NiederhalterViewModel);
			}
		}

		private bool FUN_blnVorlageFilter(object i_objElement)
		{
			EDC_NiederhalterViewModel eDC_NiederhalterViewModel = i_objElement as EDC_NiederhalterViewModel;
			if (eDC_NiederhalterViewModel != null)
			{
				return !eDC_NiederhalterViewModel.PRO_blnGeloescht;
			}
			return false;
		}
	}
}
