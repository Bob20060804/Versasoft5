using System;

namespace Ersa.Platform.DataDienste.Loetprogramm.Exceptions
{
	[Serializable]
	public class EDC_LoetprogrammBereitsInBibliothekEnthaltenException : Exception
	{
		public string PRO_strLoetprogrammName
		{
			get;
			set;
		}

		public string PRO_strBibliotheksName
		{
			get;
			set;
		}

		public EDC_LoetprogrammBereitsInBibliothekEnthaltenException()
		{
		}

		public EDC_LoetprogrammBereitsInBibliothekEnthaltenException(string i_strLoetprogrammName, string i_strBibliotheksName)
		{
			PRO_strLoetprogrammName = i_strLoetprogrammName;
			PRO_strBibliotheksName = i_strBibliotheksName;
		}
	}
}
