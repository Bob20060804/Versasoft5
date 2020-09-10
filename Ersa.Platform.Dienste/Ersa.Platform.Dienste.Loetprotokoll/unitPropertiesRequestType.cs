using System;
using System.Xml.Serialization;

namespace Ersa.Platform.Dienste.Loetprotokoll
{
	[Serializable]
	public class unitPropertiesRequestType
	{
		[XmlElement("unitProperty")]
		public propertyRequestType[] unitProperty
		{
			get;
			set;
		}
	}
}
