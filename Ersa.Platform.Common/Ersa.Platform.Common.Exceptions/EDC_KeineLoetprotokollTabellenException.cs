using System;

namespace Ersa.Platform.Common.Exceptions
{
	public class EDC_KeineLoetprotokollTabellenException : Exception
	{
		public EDC_KeineLoetprotokollTabellenException()
		{
		}

		public EDC_KeineLoetprotokollTabellenException(string i_strMessage)
			: base(i_strMessage)
		{
		}
	}
}
