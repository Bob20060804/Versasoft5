using Ersa.Global.Common;
using Ersa.Platform.Common.Data.Loetprogrammverwaltung;
using Ersa.Platform.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ersa.Platform.UI.Programm.ViewModels
{
	public class EDC_BibliothekViewModel : EDC_ElementViewModel
	{
		private readonly Func<Task<IEnumerable<EDC_ProgrammViewModel>>> m_delProgrammeLaden;

		private bool m_blnIstAusgeklappt;

		private bool m_blnDatenWerdenGeladen;

		private bool m_blnDatenWurdenGeladen;

		private ENUM_LoetprogrammFreigabeArt m_enmFreigabeArt;

		public EDC_SmartObservableCollection<EDC_ProgrammViewModel> PRO_lstProgramme
		{
			get;
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

		public EDC_BibliothekViewModel(long i_i64Id, string i_strName, ENUM_LoetprogrammFreigabeArt i_enmFreigabeArt, Func<Task<IEnumerable<EDC_ProgrammViewModel>>> i_delProgrammeLaden)
			: base(i_i64Id, i_strName)
		{
			m_delProgrammeLaden = i_delProgrammeLaden;
			m_enmFreigabeArt = i_enmFreigabeArt;
			PRO_lstProgramme = new EDC_SmartObservableCollection<EDC_ProgrammViewModel>();
		}

		public async Task FUN_fdcProgrammeLadenWennNoetigAsync()
		{
			if (!PRO_blnDatenWurdenGeladen)
			{
				using (FUN_fdcDatenWerdenGeladenSignalisieren())
				{
					await Task.Delay(10);
					_003C_003Ec__DisplayClass18_0 _003C_003Ec__DisplayClass18_;
					List<EDC_ProgrammViewModel> lstProgramme2 = _003C_003Ec__DisplayClass18_.lstProgramme;
					List<EDC_ProgrammViewModel> lstProgramme = (await m_delProgrammeLaden()).ToList();
					foreach (EDC_ProgrammViewModel item in lstProgramme)
					{
						item.PRO_enmFreigabeArt = m_enmFreigabeArt;
					}
					EDC_Dispatch.SUB_AktionStarten(delegate
					{
						PRO_lstProgramme.SUB_Reset(lstProgramme);
					});
					ICollectionView defaultView = CollectionViewSource.GetDefaultView(PRO_lstProgramme);
					defaultView.SortDescriptions.Add(new SortDescription("PRO_strName", ListSortDirection.Ascending));
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
