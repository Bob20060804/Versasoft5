using System;

namespace Ersa.Platform.Plc.Exceptions
{
	[Serializable]
	public class EDC_AnfoQuittException : Exception
	{
		public EDC_AnfoQuittException(string i_strMessage)
			: base(i_strMessage)
		{
		}
	}
}
