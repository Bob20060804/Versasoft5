using System;

namespace Ersa.Platform.DataDienste.Loetprogramm.Exceptions
{
	[Serializable]
	public class EDC_BibliothekExistiertBereitsException : Exception
	{
		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public EDC_BibliothekExistiertBereitsException()
		{
		}

		public EDC_BibliothekExistiertBereitsException(string i_strBibliotheksName)
		{
			PRO_strBibliotheksName = i_strBibliotheksName;
		}
	}
}
