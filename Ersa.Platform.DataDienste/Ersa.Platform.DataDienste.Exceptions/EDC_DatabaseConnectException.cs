using System;

namespace Ersa.Platform.DataDienste.Exceptions
{
	public class EDC_DatabaseConnectException : Exception
	{
		public override string StackTrace => string.Empty;

		public EDC_DatabaseConnectException(string i_strMessage)
			: base(i_strMessage)
		{
		}
	}
}
