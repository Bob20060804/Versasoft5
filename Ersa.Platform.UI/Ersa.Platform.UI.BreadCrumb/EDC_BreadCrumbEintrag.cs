using System.Collections.ObjectModel;

namespace Ersa.Platform.UI.BreadCrumb
{
	public class EDC_BreadCrumbEintrag : EDC_BreadCrumbElement
	{
		private EDC_BreadCrumbUnterElement m_edcAuswahl;

		public int PRO_i32BreadCrumbEbene
		{
			private get;
			set;
		}

		public ObservableCollection<EDC_BreadCrumbUnterElement> PRO_lstUnterElemente
		{
			get;
			set;
		}

		public EDC_BreadCrumbUnterElement PRO_edcAuswahl
		{
			get
			{
				return m_edcAuswahl;
			}
			set
			{
				SetProperty(ref m_edcAuswahl, value, "PRO_edcAuswahl");
			}
		}

		public bool PRO_blnIstKlickbar
		{
			get;
			set;
		}

		public bool PRO_blnIstKlickbarMitAuswahl
		{
			get
			{
				if (PRO_blnIstKlickbar)
				{
					return PRO_blnSindUnterelementeAuswaehlbar;
				}
				return false;
			}
		}

		public bool PRO_blnIstNurAuswahl
		{
			get
			{
				if (!PRO_blnIstKlickbar)
				{
					return PRO_blnSindUnterelementeAuswaehlbar;
				}
				return false;
			}
		}

		public bool PRO_blnIstErstesElement => PRO_i32BreadCrumbEbene == 0;

		private bool PRO_blnSindUnterelementeAuswaehlbar
		{
			get
			{
				if (PRO_lstUnterElemente != null)
				{
					return PRO_lstUnterElemente.Count > 0;
				}
				return false;
			}
		}

		public EDC_BreadCrumbEintrag()
		{
			PRO_lstUnterElemente = new ObservableCollection<EDC_BreadCrumbUnterElement>();
		}
	}
}
