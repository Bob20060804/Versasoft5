using Ersa.Global.Mvvm;
using System;

namespace Ersa.Platform.UI.Benutzerabfrage
{
	public class EDC_BibliothekIdentifier : BindableBase, IComparable
	{
		private long m_i64BibliothekId;

		private string m_strBibliotheksName;

		public long PRO_i64BibliothekId
		{
			get
			{
				return m_i64BibliothekId;
			}
			set
			{
				SetProperty(ref m_i64BibliothekId, value, "PRO_i64BibliothekId");
			}
		}

		public string PRO_strBibliotheksName
		{
			get
			{
				return m_strBibliotheksName;
			}
			set
			{
				SetProperty(ref m_strBibliotheksName, value, "PRO_strBibliotheksName");
			}
		}

		public int CompareTo(object i_objBibliothekIdentifier)
		{
			EDC_BibliothekIdentifier eDC_BibliothekIdentifier = i_objBibliothekIdentifier as EDC_BibliothekIdentifier;
			if (eDC_BibliothekIdentifier == null)
			{
				return -1;
			}
			if (eDC_BibliothekIdentifier.PRO_i64BibliothekId.Equals(PRO_i64BibliothekId) && eDC_BibliothekIdentifier.PRO_strBibliotheksName.Equals(PRO_strBibliotheksName))
			{
				return 0;
			}
			return -1;
		}
	}
}
