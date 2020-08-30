using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class additionalDataRequestType
	{
		[XmlElement("additionalData")]
		public valueRequestType[] additionalData
		{
			get;
			set;
		}
	}
}
