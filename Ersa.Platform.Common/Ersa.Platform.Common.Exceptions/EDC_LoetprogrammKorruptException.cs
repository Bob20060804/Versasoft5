using System;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_LoetprogrammKorruptException : Exception
	{
		public string PRO_strDateiNameUndPfad
		{
			get;
			set;
		}

		public int PRO_intZeile
		{
			get;
			set;
		}

		public int PRO_intSpalte
		{
			get;
			set;
		}

		public EDC_LoetprogrammKorruptException()
		{
		}

		public EDC_LoetprogrammKorruptException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_LoetprogrammKorruptException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_LoetprogrammKorruptException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
