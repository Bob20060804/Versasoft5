using System;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_KritischeKommunikationsException : Exception
	{
		public EDC_KritischeKommunikationsException()
		{
		}

		public EDC_KritischeKommunikationsException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_KritischeKommunikationsException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_KritischeKommunikationsException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
