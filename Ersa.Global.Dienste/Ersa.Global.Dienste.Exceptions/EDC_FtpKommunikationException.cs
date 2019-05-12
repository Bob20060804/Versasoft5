using System;
using System.Runtime.Serialization;

namespace Ersa.Global.Dienste.Exceptions
{
	[Serializable]
	public class EDC_FtpKommunikationException : Exception
	{
		public string PRO_strQuellDatei
		{
			get;
			set;
		}

		public string PRO_strZielPfad
		{
			get;
			set;
		}

		public EDC_FtpKommunikationException()
		{
		}

		public EDC_FtpKommunikationException(string i_strMessage)
			: base(i_strMessage)
		{
		}

		public EDC_FtpKommunikationException(string i_strMessage, Exception i_fdcInner)
			: base(i_strMessage, i_fdcInner)
		{
		}

		protected EDC_FtpKommunikationException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}

		public override string ToString()
		{
			return $"An error happend during FTP transfer!  {Environment.NewLine}{Message} File: {PRO_strQuellDatei} destination path: {PRO_strZielPfad}";
		}
	}
}
