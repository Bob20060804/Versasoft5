using System;
using System.Runtime.Serialization;

namespace Ersa.Platform.Common.Exceptions
{
	[Serializable]
	public class EDC_KeinGeeigneterLoetProgrammInterpreterGefundenException : Exception
	{
		public string PRO_strDateiNameUndPfad
		{
			get;
			set;
		}

		public EDC_KeinGeeigneterLoetProgrammInterpreterGefundenException()
		{
		}

		public EDC_KeinGeeigneterLoetProgrammInterpreterGefundenException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_KeinGeeigneterLoetProgrammInterpreterGefundenException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_KeinGeeigneterLoetProgrammInterpreterGefundenException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
