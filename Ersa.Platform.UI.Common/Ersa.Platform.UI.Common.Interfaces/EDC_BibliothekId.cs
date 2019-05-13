using System;

namespace Ersa.Platform.UI.Common.Interfaces
{
	public class EDC_BibliothekId : IComparable
	{
		public long PRO_i64BibliothekId
		{
			get;
			set;
		}

		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public int CompareTo(object i_objBibliothekId)
		{
			EDC_BibliothekId eDC_BibliothekId = i_objBibliothekId as EDC_BibliothekId;
			if (eDC_BibliothekId == null)
			{
				return -1;
			}
			if (eDC_BibliothekId.PRO_i64BibliothekId.Equals(PRO_i64BibliothekId) && eDC_BibliothekId.PRO_strBibliotheksName.Equals(PRO_strBibliotheksName))
			{
				return 0;
			}
			return -1;
		}
	}
}
