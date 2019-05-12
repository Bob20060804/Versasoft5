using System;
using System.Runtime.Serialization;

namespace Ersa.Global.Dienste.Exceptions
{
	[Serializable]
	public class EDC_ValidierungsException : Exception
	{
		public ENU_ValidierungsStatus PRO_enuValidierungsStatus
		{
			get;
			set;
		}

		public string PRO_strFehlerText
		{
			get;
			set;
		}

		public string PRO_strWarnungenText
		{
			get;
			set;
		}

		public EDC_ValidierungsException(string i_strFehlerText, string i_strWarnungenText, ENU_ValidierungsStatus i_enmStatus)
		{
			PRO_strFehlerText = i_strFehlerText;
			PRO_strWarnungenText = i_strWarnungenText;
			PRO_enuValidierungsStatus = i_enmStatus;
		}

		public EDC_ValidierungsException(string i_strMessage)
			: base(i_strMessage)
		{
			PRO_enuValidierungsStatus = ENU_ValidierungsStatus.enmAusnahmeFehler;
		}

		public EDC_ValidierungsException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
			PRO_enuValidierungsStatus = ENU_ValidierungsStatus.enmAusnahmeFehler;
		}

		protected EDC_ValidierungsException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
			PRO_enuValidierungsStatus = ENU_ValidierungsStatus.enmAusnahmeFehler;
		}
	}
}
