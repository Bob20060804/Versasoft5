using System;

namespace Ersa.Platform.Plc.Exceptions
{
	[Serializable]
	public class EDC_GruppeZugriffException : Exception
	{
		public EDC_GruppeZugriffException()
		{
		}

		public EDC_GruppeZugriffException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_GruppeZugriffException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}
	}
}
