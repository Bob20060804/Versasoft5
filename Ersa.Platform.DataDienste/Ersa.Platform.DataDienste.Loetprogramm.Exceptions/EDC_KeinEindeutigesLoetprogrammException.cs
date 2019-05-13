using System;

namespace Ersa.Platform.DataDienste.Loetprogramm.Exceptions
{
	[Serializable]
	public class EDC_KeinEindeutigesLoetprogrammException : Exception
	{
		public string PRO_strLoetprogrammName
		{
			get;
			set;
		}

		public EDC_KeinEindeutigesLoetprogrammException()
		{
		}

		public EDC_KeinEindeutigesLoetprogrammException(string i_strLoetprogrammName)
		{
			PRO_strLoetprogrammName = i_strLoetprogrammName;
		}
	}
}
