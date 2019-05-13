using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ersa.Platform.DataDienste.Loetprogramm.Exceptions
{
	[Serializable]
	public class EDC_MehrereBildDateienGefundenException : Exception
	{
		public IList<string> PRO_lstDateiNamen
		{
			get;
			set;
		}

		public EDC_MehrereBildDateienGefundenException()
		{
		}

		public EDC_MehrereBildDateienGefundenException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_MehrereBildDateienGefundenException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_MehrereBildDateienGefundenException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
