using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ersa.Platform.Plc.Exceptions
{
	[Serializable]
	public class EDC_SpsVerbindungsAufbauFehlgeschlagenException : Exception
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

		public EDC_SpsVerbindungsAufbauFehlgeschlagenException()
		{
		}

		public EDC_SpsVerbindungsAufbauFehlgeschlagenException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_SpsVerbindungsAufbauFehlgeschlagenException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_SpsVerbindungsAufbauFehlgeschlagenException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
