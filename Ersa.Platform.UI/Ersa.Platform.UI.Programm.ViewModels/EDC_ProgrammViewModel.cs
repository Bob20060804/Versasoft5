using Ersa.Global.Common;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ersa.Platform.UI.Programm.ViewModels
{
	public class EDC_ProgrammViewModel : EDC_ElementViewModel
	{
		private readonly Func<Task<IEnumerable<EDC_VersionViewModel>>> m_delVersionenLaden;

		private string m_strBenutzerName;

		private DateTime m_dtmDatum;

		private string m_strKommentar;

		private bool m_blnIstAusgeklappt;

		private bool m_blnDatenWerdenGeladen;

		private bool m_blnDatenWurdenGeladen;

		private ENUM_LoetprogrammFreigabeArt m_enmFreigabeArt;

		public EDC_SmartObservableCollection<EDC_VersionViewModel> PRO_lstVersionen
		{
			get;
		}

		public ENUM_LoetprogrammFreigabeArt PRO_enmFreigabeArt
		{
			get
			{
				return m_enmFreigabeArt;
			}
			set
			{
				SetProperty(ref m_enmFreigabeArt, value, "PRO_enmFreigabeArt");
			}
		}

		public ENUM_LoetprogrammFreigabeStatus[] PROa_enmFreigabeStatus
		{
			get;
			set;
		}

		public ENUM_LoetprogrammStatus[] PROa_enmStatus
		{
			get;
			set;
		}

		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public long PRO_i64BibliotheksId
		{
			get;
			set;
		}

		public string PRO_strBenutzername
		{
			get
			{
				return m_strBenutzerName;
			}
			set
			{
				SetProperty(ref m_strBenutzerName, value, "PRO_strBenutzername");
			}
		}

		public DateTime PRO_dtmDatum
		{
			get
			{
				return m_dtmDatum;
			}
			set
			{
				SetProperty(ref m_dtmDatum, value, "PRO_dtmDatum");
			}
		}

		public string PRO_strKommentar
		{
			get
			{
				return m_strKommentar;
			}
			set
			{
				SetProperty(ref m_strKommentar, value, "PRO_strKommentar");
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

		public bool PRO_blnDatenWerdenGeladen
		{
			get
			{
				return m_blnDatenWerdenGeladen;
			}
			private set
			{
				SetProperty(ref m_blnDatenWerdenGeladen, value, "PRO_blnDatenWerdenGeladen");
			}
		}

		public bool PRO_blnDatenWurdenGeladen
		{
			get
			{
				return m_blnDatenWurdenGeladen;
			}
			private set
			{
				SetProperty(ref m_blnDatenWurdenGeladen, value, "PRO_blnDatenWurdenGeladen");
			}
		}

		public EDC_ProgrammViewModel(long i_i64Id, string i_strName, long i_i64BibliotheksId, string i_strBibliotheksName, Func<Task<IEnumerable<EDC_VersionViewModel>>> i_delVersionenLaden)
			: base(i_i64Id, i_strName)
		{
			m_delVersionenLaden = i_delVersionenLaden;
			PRO_i64BibliotheksId = i_i64BibliotheksId;
			PRO_strBibliotheksName = i_strBibliotheksName;
			PRO_lstVersionen = new EDC_SmartObservableCollection<EDC_VersionViewModel>();
		}

		public Task FUN_fdcVersionenLadenAsync()
		{
			PRO_blnDatenWurdenGeladen = false;
			return FUN_fdcVersionenLadenWennNoetigAsync();
		}

		public async Task FUN_fdcVersionenLadenWennNoetigAsync()
		{
			if (!PRO_blnDatenWurdenGeladen)
			{
				using (FUN_fdcDatenWerdenGeladenSignalisieren())
				{
					await Task.Delay(10);
					IEnumerable<EDC_VersionViewModel> i_enuElemente = await m_delVersionenLaden();
					PRO_lstVersionen.SUB_Reset(i_enuElemente);
					ICollectionView defaultView = CollectionViewSource.GetDefaultView(PRO_lstVersionen);
					defaultView.SortDescriptions.Add(new SortDescription("PRO_dtmDatum", ListSortDirection.Descending));
					defaultView.MoveCurrentToFirst();
					PRO_blnDatenWurdenGeladen = true;
				}
			}
		}

		private IDisposable FUN_fdcDatenWerdenGeladenSignalisieren()
		{
			PRO_blnDatenWerdenGeladen = true;
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				PRO_blnDatenWerdenGeladen = false;
			});
		}
	}
}
