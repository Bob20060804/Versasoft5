using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class additionalResultCodesType
	{
		[XmlElement("additionalResultCode")]
		public valueType[] additionalResultCode
		{
			get;
			set;
		}
	}
}
