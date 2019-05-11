using System;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_KeinGeeigneterSerialisiererGefundenException : Exception
	{
		public string PRO_strDateiName
		{
			get;
			set;
		}

		public EDC_KeinGeeigneterSerialisiererGefundenException()
		{
		}

		public EDC_KeinGeeigneterSerialisiererGefundenException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_KeinGeeigneterSerialisiererGefundenException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_KeinGeeigneterSerialisiererGefundenException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
