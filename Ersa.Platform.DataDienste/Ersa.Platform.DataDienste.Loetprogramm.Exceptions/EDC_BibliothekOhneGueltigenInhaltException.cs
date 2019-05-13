using System;

namespace Ersa.Platform.DataDienste.Loetprogramm.Exceptions
{
	[Serializable]
	public class EDC_BibliothekOhneGueltigenInhaltException : Exception
	{
		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public EDC_BibliothekOhneGueltigenInhaltException()
		{
		}

		public EDC_BibliothekOhneGueltigenInhaltException(string i_strBibliotheksName)
		{
			PRO_strBibliotheksName = i_strBibliotheksName;
		}
	}
}
