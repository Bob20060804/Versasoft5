using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class orderPropertiesRequestType
	{
		[XmlElement("orderProperty")]
		public propertyRequestType[] orderProperty
		{
			get;
			set;
		}
	}
}
