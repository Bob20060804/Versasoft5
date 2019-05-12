using System;
using System.Runtime.Serialization;

namespace Ersa.Global.Dienste.Exceptions
{
	[Serializable]
	public class EDC_SerialisierungsException : Exception
	{
		public EDC_SerialisierungsException()
		{
		}

		public EDC_SerialisierungsException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_SerialisierungsException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_SerialisierungsException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
