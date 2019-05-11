using System;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	public class EDC_LoetprogrammImportMeldungException : Exception
	{
		public EDC_LoetprogrammImportMeldungException()
		{
		}

		public EDC_LoetprogrammImportMeldungException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_LoetprogrammImportMeldungException(string i_strMessage, Exception i_fdcInnerException)
			: base(i_strMessage, i_fdcInnerException)
		{
		}

		protected EDC_LoetprogrammImportMeldungException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
