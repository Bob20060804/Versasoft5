using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_UnbekannterParameterException : Exception
	{
		public IEnumerable<string> PRO_enuUnbekannteParameter
		{
			get;
			set;
		}

		public EDC_UnbekannterParameterException()
		{
		}

		public EDC_UnbekannterParameterException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_UnbekannterParameterException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_UnbekannterParameterException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
