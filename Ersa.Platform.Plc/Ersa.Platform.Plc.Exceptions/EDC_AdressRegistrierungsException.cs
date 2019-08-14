using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ersa.Platform.Plc.Exceptions
{
    /// <summary>
    /// Address Registration Exception
    /// </summary>
	[Serializable]
	public class EDC_AdressRegistrierungsException : Exception
	{
		public IEnumerable<string> PRO_enuFehlerhafteAdressen
		{
			get;
			set;
		}

		public AggregateException PRO_fdcInnerExceptions
		{
			get;
			set;
		}

		public EDC_AdressRegistrierungsException()
		{
		}

		public EDC_AdressRegistrierungsException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_AdressRegistrierungsException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_AdressRegistrierungsException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
